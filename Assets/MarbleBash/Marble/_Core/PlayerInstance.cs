using UnityEngine;

namespace MarbleBash
{
    [System.Serializable]
    public class PlayerInstance : Marble
    {
        
        #region Initialisation & Destruction

        private void Awake()
        {
            GameController.OnInitialisePlayer += Initialise;
        }

        private void OnDestroy()
        {
            GameController.OnInitialisePlayer -= Initialise;
        } 

        #endregion

        internal void Initialise()
        {
            _level = new MarbleLevel();
            InitialiseInternal();
        }
    }
}