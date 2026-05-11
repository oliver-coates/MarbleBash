using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KahuInteractive.HassleFreeConfig
{
 
    public class ConfigDataFile : ScriptableObject
    {
        private Dictionary<string, ConfigValue> _dict;

        public void Initialise()
        {
            _dict = new();

            AddValue(new ConfigValue("Hey", 3));
            AddValue(new ConfigValue("Ho", 213));
            AddValue(new ConfigValue("Hi", Mathf.PI));
        }

        public void AddValue(ConfigValue newValue)
        {
            _dict.Add(newValue.name, newValue);
        }

        public ConfigValue GetValue(string name)
        {
            return _dict[name];
        }

        public List<ConfigValue> GetAllConfigValues()
        {
            return _dict.Values.ToList();
        }
    }

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

