using System.Collections.Generic;
using KahuInteractive.HassleFreeConfig;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ConfigurationEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset root_uxml = default;

    [SerializeField]
    private VisualTreeAsset configValueField_uxml = default;


    private ConfigDataFile _data;


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
        
        var treeView = rootVisualElement.Q<TreeView>("config_tree_view");

        List<ConfigValue> values = _data.GetAllConfigValues();
        ConfigValueGroup root = _data.root;

        ConfigTree viewTree = new ConfigTree(root);
        treeView.SetRootItems(viewTree.Get());
        // treeView.itemsSource = values;

        // treeView.makeItem = CreateConfigValueField;
        treeView.makeItem = () => new Label();

        treeView.bindItem = (VisualElement element, int index ) => BindConfigValueToListIndex(element, index, treeView);
        // treeView.bindItem = (VisualElement element, int index) =>
        //     (element as Label).text = treeView.GetItemDataForId<IConfigValueOrGroup>(index).GetName();
    }

    private VisualElement CreateConfigValueField()
    {
        VisualElement root = configValueField_uxml.CloneTree();

        return root;
    }

    private void BindConfigValueToListIndex(VisualElement element, int index, TreeView treeView)
    {
        IConfigValueOrGroup treeElement = treeView.GetItemDataForIndex<IConfigValueOrGroup>(index);

        if (treeElement is ConfigValue)
        {
            ConfigValue configValue = treeElement as ConfigValue;

            (element as Label).text = $"[{index} Value] {configValue.name} : {configValue.value}";
            // TextField nameField = element.Q<TextField>("name");
            // nameField.value = v.name;
            // nameField.isDelayed = true;
            // nameField.RegisterValueChangedCallback(evt => 
            // {
            //     // Prevent naming to an existing name.
            //     if (_data.IsNameAlreadyInUse(evt.newValue))
            //     {
            //         nameField.value = evt.previousValue;
            //         return;
            //     }

            //     _data.ChangeName(evt.previousValue, evt.newValue);
            //     AssetDatabase.SaveAssets();
            // });
            

            // FloatField valueField = element.Q<FloatField>("value");
            // valueField.value = v.value;
            // valueField.isDelayed = true;
            // valueField.RegisterValueChangedCallback(evt => 
            // {
            //     _data.SetValue(v.name, evt.newValue);
            //     AssetDatabase.SaveAssets();
            // });

        }
        else
        {
            ConfigValueGroup group = treeElement as ConfigValueGroup;
        
            (element as Label).text = $"[{index} Group] {group.name}";
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

    #region Backup
    #if false

    public void CreateGUI()
    {
        _data = GetOrCreateDataFile(); 

        root_uxml.CloneTree(rootVisualElement);
        
        var listView = rootVisualElement.Q<ListView>();

        List<ConfigValue> values = _data.GetAllConfigValues(); 
        listView.itemsSource = values;

        listView.makeItem = CreateConfigValueField;

        listView.bindItem = BindConfigValueToListIndex;
    
    }

    private VisualElement CreateConfigValueField()
    {
        VisualElement root = configValueField_uxml.CloneTree();

        return root;
    }

    private void BindConfigValueToListIndex(VisualElement element, int index)
    {
        ConfigValue v = _data.GetAllConfigValues()[index];

        TextField nameField = element.Q<TextField>("name");
        nameField.value = v.name;
        nameField.isDelayed = true;
        nameField.RegisterValueChangedCallback(evt => 
        {
            // Prevent naming to an existing name.
            if (_data.IsNameAlreadyInUse(evt.newValue))
            {
                nameField.value = evt.previousValue;
                return;
            }

            _data.ChangeName(evt.previousValue, evt.newValue);
            AssetDatabase.SaveAssets();
        });
        

        FloatField valueField = element.Q<FloatField>("value");
        valueField.value = v.value;
        valueField.isDelayed = true;
        valueField.RegisterValueChangedCallback(evt => 
        {
            _data.SetValue(v.name, evt.newValue);
            AssetDatabase.SaveAssets();
        });
    }

    #endif
    #endregion
}
