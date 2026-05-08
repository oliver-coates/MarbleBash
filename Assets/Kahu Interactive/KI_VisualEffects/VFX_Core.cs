using System;
using System.Collections.Generic;
using UnityEngine;

namespace KahuInteractive.VisualFX
{

    public static class VFX
    {
        private const string IMPORT_PATH = "Visual Effects/";
        private static Dictionary<string, OneShotEffectType> _effectDict;

        private static bool _Initialised;

        private static Transform _playbackHolder;

        #region Initialisation
        public static void Initialise()
        {
            if (_Initialised)
            {
                Debug.LogError("KahuVFX: Don't initialise this system twice.");
                return;
            }

            _Initialised = true;

            _playbackHolder = new GameObject("[KahuVFX] Effect Holder").transform;
            GameObject.DontDestroyOnLoad(_playbackHolder);

            SetupEffectDictionary();
        }

        private static void SetupEffectDictionary()
        {
            _effectDict = new ();

            // Load all from the specified path:
            UnityEngine.Object[] objects = Resources.LoadAll(IMPORT_PATH);
            
            foreach (UnityEngine.Object loadedObject in objects)
            {
                // Ensure this object is an effect type
                // if (loadedObject.GetType().IsSubclassOf(typeof(OneShotEffectType)))
                if (loadedObject is OneShotEffectType)
                {
                    OneShotEffectType effect = loadedObject as OneShotEffectType; 
                    
                    AddEffectToDictionary(effect);
                    SetupEffectWithSceneInstance(effect);
                }
            }
        }

        private static void AddEffectToDictionary(OneShotEffectType effect)
        {
            if (_effectDict.ContainsKey(effect.name))
            {
                Debug.LogError($"KahuVFX: Loaded multiple effect types with name '{effect.name}'");
                return;
            }

            _effectDict.Add(effect.name, effect);
        }        
        
        private static void SetupEffectWithSceneInstance(OneShotEffectType effect)
        {
            GameObject newInstance = GameObject.Instantiate(effect.prefab, _playbackHolder);
            newInstance.name = $"{effect.name} Scene Instance"; 
            
            effect.SetSceneInstance(newInstance.GetComponent<VisualEffectHandler>());        
        }

        #endregion

        public static void Play(OneShotEffectData data)
        {
            OneShotEffectType effect = GetEffectFromString(data.name);
            
            effect.Play(data);
        }

        private static OneShotEffectType GetEffectFromString(string name)
        {
            if (_effectDict.ContainsKey(name))
            {
                return _effectDict[name];
            }
            else
            {
                Debug.LogError($"Could not find effect with name '{name}'");
                return null;
            }
        }
    }


}

