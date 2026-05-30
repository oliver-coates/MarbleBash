using UnityEngine;


namespace KahuInteractive.VisualFX
{
    public abstract class VFX_OneShotHandler : MonoBehaviour
    {   
        public abstract void Play(OneShotEffectData data);

        public abstract void Finish();
    }

 
}