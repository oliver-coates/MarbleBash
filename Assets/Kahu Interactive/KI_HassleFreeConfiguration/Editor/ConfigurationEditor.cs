using System;
using System.Collections.Generic;
using KahuInteractive.HassleFreeConfig;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ConfigurationEditor : EditorWindow
{


    [SerializeField]
    private VisualTreeAsset root_uxml = default;


    private ConfigDataFile _data;
    private ConfigTree _tree;
    private ConfigValue _currentlySelectedValue;


    // UI References:
    private TreeView _treeView;
    private VisualElement _valueEditorHolder;
    private TextField _valueNameField;
    private FloatField _valueFloatField;
    private TextField _valuePathField;

    public const string NEW_VALUE_NAME = "Unnamed config value";
    private string _lastSelectedPath;

    [MenuItem("Window/Kahu Interactive/Hassle Free Config/Editor")]
    public static void Summon()
    {
        ConfigurationEditor wnd = GetWindow<ConfigurationEditor>();
        wnd.titleContent = new GUIContent("Configuration Editor");
    }

    public void CreateGUI()
    {
        _data = GetOrCreateDataFile(); 

        root_uxml.CloneTree(rootVisualElement);
        
        _treeView = rootVisualElement.Q<TreeView>("config_tree_view");
        _valueEditorHolder = rootVisualElement.Q<VisualElement>("value_editor_panel");

        _valueNameField = rootVisualElement.Q<TextField>("selected_value_name_field");
        _valueNameField.isDelayed = true;
        _valueNameField.RegisterValueChangedCallback(OnNameChanged);

        _valueFloatField = rootVisualElement.Q<FloatField>("selected_value_value_field");
        _valueFloatField.isDelayed = true;
        _valueFloatField.RegisterValueChangedCallback(OnValueChanged);

        _valuePathField = rootVisualElement.Q<TextField>("selected_value_path_field");
        _valuePathField.isDelayed = true;
        _valuePathField.RegisterValueChangedCallback(OnPathChanged);

        rootVisualElement.Q<Button>("refresh_tree_button").RegisterCallback<ClickEvent>((e) => RefreshTree());
        rootVisualElement.Q<Button>("new_value_button").RegisterCallback<ClickEvent>((e) => AttemptCreateNewValue());
        rootVisualElement.Q<Button>("delete_value_button").RegisterCallback<ClickEvent>((e) => AttemptDeleteValue());

        List<ConfigValue> values = _data.GetAllConfigValues();
        ConfigValueGroup root = _data.root;

        RefreshTree();

        _treeView.makeItem = () => new Label();
        _treeView.bindItem = (VisualElement element, int index ) => BindConfigValueToListIndex(element, index, _treeView);
    
        _treeView.selectionChanged += UpdateValueSelection;
        
        DisableValueEditor();
    }

    private void OnValueChanged(ChangeEvent<float> evt)
    {
        _data.SetValue(_currentlySelectedValue.name, evt.newValue);
        AssetDatabase.SaveAssets();

        _treeView.RefreshItems();
    }

    private void OnNameChanged(ChangeEvent<string> evt)
    {
        // Prevent naming to an existing name.
        if (_data.IsNameAlreadyInUse(evt.newValue))
        {
            _valueNameField.SetValueWithoutNotify(evt.previousValue);
            Debug.Log("Config names must be unique.");
            return;
        }

        _data.ChangeName(evt.previousValue, evt.newValue);
        AssetDatabase.SaveAssets();

        _treeView.RefreshItems();
    }

    private void OnPathChanged(ChangeEvent<string> evt)
    {
        _data.ChangePath(_currentlySelectedValue.name, evt.newValue);
        _lastSelectedPath = evt.newValue;
        AssetDatabase.SaveAssets();

        _treeView.RefreshItems();
    }

    private void UpdateValueSelection(IEnumerable<object> enumerable)
    {
        IConfigValueOrGroup i = _treeView.selectedItem as IConfigValueOrGroup;

        if (i is null or ConfigValueGroup)
        {
            _currentlySelectedValue = null;
            _lastSelectedPath = "";
            DisableValueEditor();
        }
        else
        {
            ConfigValue configValue = i as ConfigValue;
            _currentlySelectedValue = configValue;

            _valueEditorHolder.SetEnabled(true);
            _valueEditorHolder.style.opacity = 1f;
            _lastSelectedPath = configValue.path;

            _valueNameField.SetValueWithoutNotify(configValue.name);
            _valueFloatField.SetValueWithoutNotify(configValue.value);
            _valuePathField.SetValueWithoutNotify(configValue.path);
        }
    }

    private void DisableValueEditor()
    {
        _valueEditorHolder.SetEnabled(false);
        _valueEditorHolder.style.opacity = 0.1f;
    }

    private void RefreshTree()
    {
        ConfigValue prevSelection = _currentlySelectedValue;

        _tree = new ConfigTree(_data.root);
        _treeView.SetRootItems(_tree.Get());
        
        _treeView.Rebuild();

        if (prevSelection != null)
        {
            if (_data.IsNameAlreadyInUse(prevSelection.name))
            {
                FocusTreeOnValue(prevSelection);            
            }
        }

    }

    private void AttemptCreateNewValue()
    {
        // Ensure a newly created value isn't currently sitting within the dictionary...
        if (_data.IsNameAlreadyInUse(NEW_VALUE_NAME))
        {
            FocusTreeOnValue(_data.GetValue(NEW_VALUE_NAME));
            return;
        }

        ConfigValue newValue = new ConfigValue(NEW_VALUE_NAME, 0f, _lastSelectedPath);
        _currentlySelectedValue = newValue;

        _data.AddValue(newValue);

        RefreshTree();     
    }

    private void FocusTreeOnValue(ConfigValue configValue)
    {
        if (_data.IsNameAlreadyInUse(configValue.name) == false)
        {
            return;
        }

        int newValueId = _tree.GetElementId(configValue);
        _treeView.SetSelectionById(newValueId);   
        // _treeView.SetSelectionByIdWithoutNotify(new int[1] {newValueId});   
    }

    private void AttemptDeleteValue()
    {
        if (_currentlySelectedValue == null)
        {
            return;
        }
        
        _data.RemoveValue(_currentlySelectedValue);

        RefreshTree();
    }

    private void BindConfigValueToListIndex(VisualElement element, int index, TreeView treeView)
    {
        IConfigValueOrGroup treeElement = treeView.GetItemDataForIndex<IConfigValueOrGroup>(index);

        if (treeElement is ConfigValue)
        {
            ConfigValue configValue = treeElement as ConfigValue;

            (element as Label).text = $"{configValue.name}  :  <i>{configValue.value}</i>";
        }
        else
        {
            ConfigValueGroup group = treeElement as ConfigValueGroup;
            Label label = element as Label;

            label.text = $"<b>• {group.name}</b>";
        }
    }

    #region File Loading & Selection
    private ConfigDataFile GetOrCreateDataFile()
    {
        string path = "Kahu Interactive/Configuration/main";
        if (AssetDatabase.AssetPathExists("Assets/Resources/Kahu Interactive/Configuration/main.asset"))
        {
            ConfigDataFile dataFile = Resources.Load<ConfigDataFile>(path);
            dataFile.Initialise();

            return dataFile;
        }
        else
        {
            ConfigDataFile file = ScriptableObject.CreateInstance<ConfigDataFile>();
            file.FirstTimeSetup();

            ValidateStoragePathExists();

            AssetDatabase.CreateAsset(file, "Assets/Resources/Kahu Interactive/Configuration/main.asset");
            AssetDatabase.SaveAssets();


            return file;
        }
    }

    private void ValidateStoragePathExists()
    {
        if (!AssetDatabase.AssetPathExists("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");        
        }

        if (!AssetDatabase.AssetPathExists("Assets/Resources/Kahu Interactive"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Kahu Interactive");            
        }

        if (!AssetDatabase.AssetPathExists("Assets/Resources/Kahu Interactive/Configuration"))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Kahu Interactive", "Configuration");            
        }

    }
    #endregion

}
