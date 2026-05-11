using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        var listView = rootVisualElement.Q<ListView>();

        List<ConfigValue> values = _data.GetAllConfigValues(); 
        listView.itemsSource = values;

        listView.makeItem = CreateConfigValueField;

        listView.bindItem = BindConfigValueToListIndex;
    
    }

    private VisualElement CreateConfigValueField()
    {
        // VisualElement root = new VisualElement();

        // Label label = new Label();  
        
        // FloatField field = new FloatField();
        
        // var layout = field.layout;
        // layout.position = new Vector2(100, 0);

        // root.Add(label);
        // root.Add(field);

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
            // _data.SetValue(v.name, )
        });
        

        FloatField valueField = element.Q<FloatField>("value");
        valueField.value = v.value;
        valueField.isDelayed = true;
        valueField.RegisterValueChangedCallback(evt => 
        {
            _data.SetValue(v.name, evt.newValue);
        });
    }

    // private void OnFieldValueChanged(ChangeEvent<float> evt)
    // {
    //     evt.
    // }

    protected IList<TreeViewItemData<ConfigValue>> GetListData(ConfigDataFile f)
    {
        var list = new List<TreeViewItemData<ConfigValue>>();
        int id = 0;

        foreach (ConfigValue configValue in f.GetAllConfigValues())
        {
            list.Add(new TreeViewItemData<ConfigValue>(id, configValue));

            id++;
        }

        return list;
    }

    private ConfigDataFile GetOrCreateDataFile()
    {
        string path = "Kahu Interactive/Configuration/main.asset";
        if (AssetDatabase.AssetPathExists(path))
        {
            return Resources.Load<ConfigDataFile>(path);;            
        }
        else
        {
            ConfigDataFile file = ScriptableObject.CreateInstance<ConfigDataFile>();

            ValidateStoragePathExists();

            AssetDatabase.CreateAsset(file, "Assets/Resources/Kahu Interactive/Configuration/main.asset");
            AssetDatabase.SaveAssets();

            file.Initialise();

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
}
