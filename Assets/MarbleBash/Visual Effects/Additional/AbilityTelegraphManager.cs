using UnityEngine;

namespace MarbleBash
{
    public class AbilityTelegraphManager : MarbleSubComponent
    {
        private float _intensity;

        protected override void Initialise()
        {
        }

        private void Update()
        {
            _marble.materials.baseMat.SetFloat("_FlashAmount", _intensity);
            _intensity = Mathf.MoveTowards(_intensity, 0f, Time.deltaTime * 4f);
        }

        public void Flash()
        {
            _intensity = 1f;
        }    
    }
}