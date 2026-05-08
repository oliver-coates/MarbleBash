using UnityEngine;

namespace KahuInteractive.VisualFX
{

    [CreateAssetMenu(fileName = "Unnamed Effect", menuName = "Kahu Interactive/VFX/One Shot")]
    public class OneShotEffectType : ScriptableObject
    {
        public enum UseCase
        {
            Reuse,
            CreateNewEachTime
        }

        [SerializeField] private GameObject _prefab;
        public GameObject prefab
        {
            get
            {
                return _prefab;
            }
        }

        [SerializeField] private UseCase _useCase;
        public UseCase useCase
        {
            get
            {
                return _useCase;
            }	
        }


        private VisualEffectHandler _reusableInstance;

        public void SetReusableInstance(VisualEffectHandler instance)
        {
            _reusableInstance = instance;
        }

        public void Play(OneShotEffectData data)
        {
            switch (_useCase)
            {
                case UseCase.Reuse:
                    _reusableInstance.Play(data);
                    break;

                case UseCase.CreateNewEachTime:
                    VisualEffectHandler effectInstance = Instantiate(_prefab, VFX.instanceContainer).GetComponent<VisualEffectHandler>();
                    effectInstance.Play(data);
                    break;
            }
            
        }
    }

}