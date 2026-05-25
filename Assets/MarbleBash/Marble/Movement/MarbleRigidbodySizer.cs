using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{
    public class MarbleRigidbodySizer : MarbleSubComponent
    {
        private float _sizeBase;
        private float _massBase;
        // private float _dragBase;

        protected override void Initialise()
        {
            _sizeBase = Configuration.Read("marble_size_base");
            _massBase = Configuration.Read("marble_mass_base");
            // _dragBase = Configuration.Read("marble_")

            _marble.stats.marbleSize.OnChange += UpdateSize;
            _marble.stats.rigidbodyMass.OnChange += UpdateMass;
            _marble.stats.rigidbodyDrag.OnChange += UpdateDrag;
        }

        private void UpdateSize(float sizeMultiplier)
        {
            float size = _sizeBase * sizeMultiplier;

            _marble.transform.localScale = new Vector3(size, size, size);
        }

        private void UpdateMass(float massMultiplier)
        {
            float mass = _massBase * massMultiplier;

            _marble.rigidbody.mass = mass;
        }

        private void UpdateDrag(float drag)
        {
            _marble.rigidbody.linearDamping = drag;
            _marble.rigidbody.angularDamping = drag / 2f;
        }
    }


}
