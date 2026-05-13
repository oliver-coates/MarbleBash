using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KahuInteractive.HassleFreeConfig
{
 
    public class ConfigDataFile : ScriptableObject
    {
        [SerializeField, ReadOnly] private string _name;
        [SerializeField] private List<ConfigValue> _values; 
        
        private ConfigValueGroup _root;
        public ConfigValueGroup root
        {
            get
            {
                return _root;
            }
        }
        
        private Dictionary<string, ConfigValue> _dict;

        /// <summary>
        /// Call when first creating this object.
        /// </summary>
        public void FirstTimeSetup()
        {
            _name = "Production";
            _values = new List<ConfigValue>();
            _dict = new Dictionary<string, ConfigValue>();

            // AddValue(new ConfigValue("My Config Value 1", 23f, "ExampleGroup"));
            AddValue(new ConfigValue("A", 23f, ""));
            AddValue(new ConfigValue("B", 12f, "Test Group"));
            AddValue(new ConfigValue("C", 200f, "Test Group"));

            _root = ReconstructValuesTree(_values);
        }

        /// <summary>
        /// Call everytime this object is loaded into memory
        /// </summary>
        public void Initialise()
        {
            _dict = ReconstructDictionary(_values);
            _root = ReconstructValuesTree(_values);
        }

        private Dictionary<string, ConfigValue> ReconstructDictionary(List<ConfigValue> configValues)
        {
            Dictionary<string, ConfigValue> output = new ();

            foreach (ConfigValue value in configValues)
            {
                output.Add(value.name, value);
            }
            
            return output;
        }

        private ConfigValueGroup ReconstructValuesTree(List<ConfigValue> configValues)
        {
            ConfigValueGroup root = new ConfigValueGroup(_name, null);

            foreach (ConfigValue value in configValues)
            {
                string[] pathElements = value.path.Split('/'); 

                // Start path navigating from the root:
                ConfigValueGroup currentGroup = root;
                foreach (string pathElement in pathElements)
                {
                    if (pathElement == "")
                    {
                        continue;
                    }
                    
                    if (currentGroup.ChildGroupExists(pathElement, out ConfigValueGroup groupAtPath))
                    {
                        currentGroup = groupAtPath;
                    }
                    else
                    {
                        currentGroup = currentGroup.AddChildGroup(pathElement);
                    }
                }

                currentGroup.AddValue(value);
            }

            return root;
        }

        public void AddValue(ConfigValue newValue)
        {
            _values.Add(newValue);
            _dict.Add(newValue.name, newValue);

            _root = ReconstructValuesTree(_values);
        }

        public ConfigValue GetValue(string name)
        {
            return _dict[name];
        }

        public float Read(string name)
        {
            return _dict[name].value;
        }

        public void SetValue(string name, float value)
        {
            _dict[name].value = value;
        }

        public void ChangeName(string oldName, string newName)
        {
            ConfigValue toRename = _dict[oldName];
            _dict.Remove(oldName);

            toRename.name = newName;
            _dict.Add(newName, toRename);
        }

        public List<ConfigValue> GetAllConfigValues()
        {
            return _values;
        }

        public bool IsNameAlreadyInUse(string name)
        {
            return _dict.ContainsKey(name);
        }

        public void ChangePath(string name, string newPath)
        {
            GetValue(name).path = newPath;
            _root = ReconstructValuesTree(_values);
        }

        public void RemoveValue(ConfigValue value)
        {
            _dict.Remove(value.name);
            _values.Remove(value);

            _root = ReconstructValuesTree(_values);
        }
    }

    [System.Serializable]
    public class ConfigValue : IConfigValueOrGroup
    {
        public string name;
        public float value;
        public string path;

        public ConfigValue(string name, float value, string path)
        {
            this.name = name;
            this.value = value;
            this.path = path;
        }

        public string GetName()
        {
            return name;
        }
    
    }

    public class ConfigValueGroup : IConfigValueOrGroup
    {
        public string name;

        public ConfigValueGroup parent;
        private List<ConfigValueGroup> _children; 
        private List<ConfigValue> _values;

        public ConfigValueGroup(string name, ConfigValueGroup parent)
        {
            this.name = name;
            this.parent = parent;

            _children = new List<ConfigValueGroup>();
            _values = new List<ConfigValue>();
        }
    
        public bool ChildGroupExists(string name, out ConfigValueGroup outGroup)
        {
            foreach (ConfigValueGroup group in _children)
            {
                if (group.name == name)
                {
                    outGroup = group;
                    return true;
                }
            }

            outGroup = null;
            return false;
        }
    
        public ConfigValueGroup AddChildGroup(string name)
        {
            ConfigValueGroup newGroup = new ConfigValueGroup(name, this); 
            _children.Add(newGroup);
            return newGroup;
        }

        public void AddValue(ConfigValue v)
        {
            _values.Add(v);
        }
    
        public List<ConfigValue> GetValues()
        {
            return _values;
        }

        public List<ConfigValueGroup> GetSubGroups()
        {
            return _children;
        }

        public string GetName()
        {
            return name;
        }
    }

    public interface IConfigValueOrGroup
    {
        public string GetName();
    }
}

