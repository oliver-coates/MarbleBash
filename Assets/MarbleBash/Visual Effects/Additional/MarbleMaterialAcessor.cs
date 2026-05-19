using UnityEngine;
namespace MarbleBash
{
    public class MarbleMaterialAcessor : MonoBehaviour
    {
        [Header("References:")]
        [SerializeField] private MeshRenderer _damageFlashRenderer;
        public MeshRenderer damageFlashRenderer
        {
            get
            {
                return _damageFlashRenderer;
            }
        }

        private Material _damageFlashMaterial;
        public Material damageFlash
        {
            get
            {
                return _damageFlashMaterial;
            }
        }


        [SerializeField] private MeshRenderer _baseRenderer;
        public MeshRenderer baseRenderer
        {
            get
            {
                return _baseRenderer;
            }
        }

        private Material _baseMaterial;
        public Material baseMat
        {
            get
            {
                return _baseMaterial;
            }
        }


        [SerializeField] private MeshRenderer _outlineRenderer;
        public MeshRenderer outlineRenderer
        {
            get
            {
                return _outlineRenderer;
            }
        }
        
        private Material _outlineMaterial;
        public Material outline
        {
            get
            {
                return _outlineMaterial;
            }
        }


        private void Awake()
        {
             _damageFlashMaterial = new Material(_damageFlashRenderer.material);
            _damageFlashRenderer.material = _damageFlashMaterial;
        
            _baseMaterial = new Material(_baseRenderer.material);
            _baseRenderer.material = _baseMaterial;

            _outlineMaterial = new Material(_outlineRenderer.material);
            _outlineRenderer.material = _outlineMaterial;
        }
    }
}
