using UnityEngine;

namespace KahuInteractive.VisualFX
{

    [CreateAssetMenu(fileName = "Unnamed Effect", menuName = "Kahu Interactive/VFX/One Shot")]
    public class OneShotEffectType : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        public GameObject prefab
        {
            get
            {
                return _prefab;
            }
        }
    
        private VisualEffectHandler _instance;

        public void SetSceneInstance(VisualEffectHandler instance)
        {
            _instance = instance;
        }

        public void Play(OneShotEffectData data)
        {
            _instance.Play(data);
        }
    }

}