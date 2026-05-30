using System;
using System.Collections.Generic;
using UnityEngine;

namespace KahuInteractive.VisualFX
{

    public static class VFX
    {
        private const string IMPORT_PATH = "Visual Effects/";
        private static Dictionary<string, OneShotEffectType> _effectDict;
        private static Dictionary<string, List<Action<float>>> _rtpcListenerDict;


        private static bool _Initialised;

        private static Transform _instanceContainer;
        public static Transform instanceContainer
        {
            get
            {
                return _instanceContainer;
            }
        }


        #region Initialisation
        public static void Initialise()
        {
            if (_Initialised)
            {
                Debug.LogError("KahuVFX: Don't initialise this system twice.");
                return;
            }

            _Initialised = true;

            _instanceContainer = new GameObject("[KahuVFX] Effect Holder").transform;
            GameObject.DontDestroyOnLoad(_instanceContainer);


            SetupEffectDictionary();
        }

        private static void SetupEffectDictionary()
        {
            _effectDict = new ();
            _rtpcListenerDict = new Dictionary<string, List<Action<float>>>();

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
                    if (effect.useCase == OneShotEffectType.UseCase.Reuse)
                    {
                        SetupEffectWithSceneInstance(effect);                    
                    }
                }
                else if (loadedObject is RtpcType)
                {
                    AddRtpcToDictionary(loadedObject as RtpcType);
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
            GameObject newInstance = GameObject.Instantiate(effect.prefab, _instanceContainer);
            newInstance.name = $"{effect.name} Scene Instance"; 
            
            effect.SetReusableInstance(newInstance.GetComponent<VFX_OneShotHandler>());        
        }

        private static void AddRtpcToDictionary(RtpcType rtpc)
        {
            if (!_rtpcListenerDict.ContainsKey(rtpc.rtpcName))
            {
                _rtpcListenerDict.Add(rtpc.rtpcName, new List<Action<float>>());
            }   
        }

        #endregion


        public static VFX_OneShotHandler Play(OneShotEffectData data)
        {
            OneShotEffectType effect = GetEffectFromString(data.name);
            VFX_OneShotHandler handler = effect.Play(data);

            return handler;
        }

        public static void UpdateRTPC(string name, float value)
        {
            foreach (Action<float> action in _rtpcListenerDict[name])
            {
                action.Invoke(value);
            }
        }

        public static void UpdateRTPC(RtpcType rtpc, float value)
        {
            UpdateRTPC(rtpc.rtpcName, value);
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
    

        public static void RegisterRtpcListener(string name, Action<float> callback)
        {
            _rtpcListenerDict[name].Add(callback);
        }  

        public static void DeregisterRptcLisener(string name, Action<float> callback)
        {
            _rtpcListenerDict[name].Remove(callback);
        } 
    }


}

