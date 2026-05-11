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
    private VisualTreeAsset m_VisualTreeAsset = default;

    private ConfigDataFile _data;


    [MenuItem("Window/Kahu Interactive/Hassle Free Config/Editor")]
    public static void Summon()
    {
        ConfigurationEditor wnd = GetWindow<ConfigurationEditor>();
        wnd.titleContent = new GUIContent("Configuration Editor");
    }

    public void CreateGUI()
    {
        // // Each editor window contains a root VisualElement object
        // VisualElement root = rootVisualElement;

        // _data = GetOrCreateDataFile(); 

        // ListView listView = root.Q<ListView>();
        // Debug.Log($"GOT: {listView}");

        // // Instantiate UXML
        // VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        // root.Add(labelFromUXML);

        _data = GetOrCreateDataFile(); 

        m_VisualTreeAsset.CloneTree(rootVisualElement);
        var listView = rootVisualElement.Q<ListView>();

        List<ConfigValue> values = _data.GetAllConfigValues(); 
        listView.itemsSource = values;

        listView.makeItem = CreateConfigValueField;

        listView.bindItem = BindConfigValueToListIndex;
    
    }

    private VisualElement CreateConfigValueField()
    {
        VisualElement root = new VisualElement();

        Label label = new Label();  
        
        FloatField field = new FloatField();
        
        var layout = field.layout;
        layout.position = new Vector2(100, 0);

        root.Add(label);
        root.Add(field);

        return root;
    }

    private void BindConfigValueToListIndex(VisualElement element, int index)
    {
        ConfigValue v = _data.GetAllConfigValues()[index];

        var children = element.Children().ToArray();

        Label label = (children[0] as Label); 
        label.text = v.name;
        
        FloatField field = (children[1] as FloatField);
        field.value = v.value;
        field.isDelayed = true;
        field.RegisterValueChangedCallback(evt => v.value = evt.newValue); 
    }

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
