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


        private VFX_OneShotHandler _reusableInstance;

        public void SetReusableInstance(VFX_OneShotHandler instance)
        {
            _reusableInstance = instance;
        }

        public VFX_OneShotHandler Play(OneShotEffectData data)
        {
            switch (_useCase)
            {
                case UseCase.Reuse:
                    _reusableInstance.Play(data);
                    return null;

                case UseCase.CreateNewEachTime:
                    VFX_OneShotHandler effectInstance = Instantiate(_prefab, VFX.instanceContainer).GetComponent<VFX_OneShotHandler>();
                    effectInstance.Play(data);
                    return effectInstance;
                
                default:
                    Debug.LogError("Unhandled effect UseCase");
                    return null;
            }
            
        }
    }

}