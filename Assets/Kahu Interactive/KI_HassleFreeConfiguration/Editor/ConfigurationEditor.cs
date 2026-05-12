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
    private ConfigValue _currentlySelectedValue;


    // UI References:
    private TreeView _treeView;
    private VisualElement _valueEditorHolder;
    private TextField _valueNameField;
    private FloatField _valueFloatField;
    private TextField _valuePathField;


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

        List<ConfigValue> values = _data.GetAllConfigValues();
        ConfigValueGroup root = _data.root;

        ConfigTree viewTree = new ConfigTree(root);
        _treeView.SetRootItems(viewTree.Get());

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
        AssetDatabase.SaveAssets();

        _treeView.SetRootItems(new ConfigTree(_data.root).Get());

        _treeView.Rebuild();
    }

    private void UpdateValueSelection(IEnumerable<object> enumerable)
    {
        IConfigValueOrGroup i = _treeView.selectedItem as IConfigValueOrGroup;

        if (i is null or ConfigValueGroup)
        {
            _currentlySelectedValue = null;
            DisableValueEditor();
        }
        else
        {
            ConfigValue configValue = i as ConfigValue;
            _currentlySelectedValue = configValue;

            _valueEditorHolder.SetEnabled(true);
            _valueEditorHolder.style.opacity = 1f;
            
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
