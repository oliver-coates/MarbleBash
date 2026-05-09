using UnityEngine;

namespace KahuInteractive.VisualFX
{

    public class VFX_ContinuousEffectHandler : MonoBehaviour
    {
        [Header("RTPC:")]
        [SerializeField] private RtpcType _rtpc;
    
        protected float _value;

        #region Initialisation & Destruction
        
        protected virtual void Start()
        {
            VFX.RegisterRtpcListener(_rtpc.rtpcName, UpdateRTPC);
        }
        
        protected virtual void OnDestroy()
        {
            VFX.DeregisterRptcLisener(_rtpc.rtpcName, UpdateRTPC);
        }
        
        #endregion
    
        private void UpdateRTPC(float value)
        {
            _value = value;
        }

    }


}

