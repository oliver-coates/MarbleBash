using UnityEngine;

namespace MarbleBash
{
    public class PlayerInstance : Marble
    {
        private PlayerMovement _playerMovement;
        public new PlayerMovement movement => _playerMovement;

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

            _playerMovement = this.GetComponentSafe<PlayerMovement>();

            InitialiseInternal();
        }
    }
}