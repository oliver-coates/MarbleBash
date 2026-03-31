using MarbleBash.Abilities;
using UnityEngine;

namespace MarbleBash
{

    public class Player : MonoBehaviour
    {
        private static Player _this;

        private void Awake()
        {
            _this = this;
        }

        [SerializeField] private Rigidbody _rigidbody;
        public static new Rigidbody rigidbody
        {
            get
            {
                return _this._rigidbody;
            }
        }


        [SerializeField] private Abilities.Abilities _abilities;
        public static Abilities.Abilities abilities
        {
            get
            {
                return _this._abilities;
            }
        }

        [SerializeField] private PlayerLook _look;
        public static PlayerLook look
        {
            get
            {
                return _this._look;
            }
        }

        [SerializeField] private PlayerMovement _movement;
        public static PlayerMovement movement
        {
            get
            {
                return _this._movement;
            }
        }
        
        [SerializeField] private Transform _transform;
        public static new Transform transform
        {
            get
            {
                return _this._transform;
            }
        }
    }

}

