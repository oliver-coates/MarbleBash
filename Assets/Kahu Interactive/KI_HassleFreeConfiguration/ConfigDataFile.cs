using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KahuInteractive.HassleFreeConfig
{
 
    public class ConfigDataFile : ScriptableObject
    {
        [SerializeField] private List<ConfigValue> _values; 
        private Dictionary<string, ConfigValue> _dict;

        /// <summary>
        /// Call when first creating this object.
        /// </summary>
        public void FirstTimeSetup()
        {
            _values = new List<ConfigValue>();
            _dict = new Dictionary<string, ConfigValue>();

            AddValue(new ConfigValue("My Config Value 1", 23f));
        }

        /// <summary>
        /// Call everytime this object is loaded into memory
        /// </summary>
        public void Initialise()
        {
            _dict = ReconstructDictionary(_values);
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

        public void AddValue(ConfigValue newValue)
        {
            _values = new List<ConfigValue>();
            _dict.Add(newValue.name, newValue);
        }

        public ConfigValue GetValue(string name)
        {
            return _dict[name];
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
    }

    [System.Serializable]
    public class ConfigValue
    {
        public string name;
        public float value;

        public ConfigValue(string name, float value)
        {
            this.name = name;
            this.value = value;
        }
    }
}

