// Copy and paste this file to create your own configuration type 

#if false // <-- Ensures the compiler ignores this. Don't include this in your config files.

using System.Collections.Generic;
using UnityEngine;

namespace KahuInteractive.HassleFreeConfig
{

// [CreateAssetMenu(fileName = "ExampleConfig", menuName = "Configuration/Example")]
public class ExampleConfig : ConfigBase
{
    [SerializeField] private int _playerHealth;
    public int playerHealth
    {
        get
        {
            return _playerHealth;
        }	
    }


    [SerializeField] private float _reloadTime;
    public float reloadTime
    {
        get
        {
            return _reloadTime;
        }	
    }

    [SerializeField] private string _levelUpNotification;
    public string levelUpNotification
    {
        get
        {
            return _levelUpNotification;
        }	
    }

}

}


#endif