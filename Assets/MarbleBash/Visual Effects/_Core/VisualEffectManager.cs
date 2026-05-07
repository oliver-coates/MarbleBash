using UnityEngine;


namespace MarbleBash
{

    public class VisualEffectManager : MonoBehaviour
    {
        private static VisualEffectManager _instance;

        [Header("Effect managers:")]
        [SerializeField] private GroundPoundEffectManager _groundPound;

        private void Awake()
        {
            _instance = this;
        }

        public static void CreateEffect(string name, Vector3 position, float strength)
        {
            IVisualEffectManager manager = _instance.GetManagerFromString(name);
            manager.CreateEffect(position, strength);
        }

        private IVisualEffectManager GetManagerFromString(string name)
        {
            switch (name)
            {
                case "Ground Pound":
                    return _groundPound;
                
                default:
                    Debug.Log($"Could not find vfx manager with name '{name}'");
                    return null;
            }
        }
    }


}

