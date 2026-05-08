using UnityEngine;

namespace KahuInteractive.VisualFX
{

    public class OneShotEffectData
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
        public float strength;

        public OneShotEffectData(string name, Vector3 position)
        {
            this.name = name;
            this.position = position;
            rotation = Quaternion.identity;
            strength = 1f;
        }
    }

}

