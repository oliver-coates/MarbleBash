using UnityEngine;


namespace KahuInteractive.VisualFX
{
    public abstract class VisualEffectHandler : MonoBehaviour
    {   
        public abstract void Play(OneShotEffectData data);
    }

 
}