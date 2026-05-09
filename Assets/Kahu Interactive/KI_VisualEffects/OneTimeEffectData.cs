using UnityEngine;

namespace KahuInteractive.VisualFX
{

    public class OneShotEffectData
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
        public float strength;

        public OneShotEffectData(string name, Vector3 position, Quaternion rotation, float strength = 1f)
        {
            this.name = name;
            this.position = position;
            this.rotation = rotation;
            this.strength = strength;
        }
    }

}

