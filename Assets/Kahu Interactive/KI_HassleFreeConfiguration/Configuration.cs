using System;
using System.Collections.Generic;
using UnityEngine;

namespace KahuInteractive.HassleFreeConfig
{

    public static class Configuration
    {
        private const string CONFIG_PATH = "Configuration/";
        private static Dictionary<Type, ConfigBase> _configDictionary;
        private static ConfigDataFile _currentDataFile;

        public static void Initialise()
        {
            _configDictionary = new Dictionary<Type, ConfigBase>();

            // Load all:
            UnityEngine.Object[] objects = Resources.LoadAll(CONFIG_PATH);
        
            foreach (UnityEngine.Object loadedObject in objects)
            {
                // Ensure this object is a subclass of the configbase
                if (loadedObject.GetType().IsSubclassOf(typeof(ConfigBase)))
                {
                    Type actualType = loadedObject.GetType();

                    AddToDictionary(loadedObject as ConfigBase, actualType);
                }
            }
        
            _currentDataFile = GetCurrentDataFile();
            _currentDataFile.Initialise();
        }

        private static void AddToDictionary(ConfigBase config, Type type)
        {
            if (_configDictionary.ContainsKey(type))
            {
                Debug.LogError($"Hassle Free Config: Loaded multiple config files of type '{type}'");
                return;
            }

            _configDictionary.Add(type, config);
        }

        public static T Get<T>() where T : ConfigBase
        {
            Type requestedType = typeof(T);

            if (_configDictionary.ContainsKey(requestedType) == false)
            {
                Debug.LogError($"Hassle Free Config: The requested config type '{requestedType}' has not been found.");
                return null;
            }

            return _configDictionary[requestedType] as T;
        } 

        public static float Read(string valueName)
        {
            return _currentDataFile.Read(valueName);
        }

        private static ConfigDataFile GetCurrentDataFile()
        {
            ConfigDataFile retrievedFile = Resources.Load<ConfigDataFile>("Kahu Interactive/Configuration/main");

            return retrievedFile;            
        }
    }

public class ConfigBase : ScriptableObject
{

}

}