using MarbleBash.Enemy;
using UnityEngine;


namespace MarbleBash.Encounters
{

    public class EncounterManager : MonoBehaviour
    {
        public const string ENEMY_CLASS_PATH = "Marble Bash/Enemy/Classes";
        private EnemyClass[] _classes;

        #region Initialisation & Destruction
        
        private void Awake()
        {
            GameController.OnInitialiseLevel += Initialise;
        }
        
        private void OnDestroy()
        {
            GameController.OnInitialiseLevel -= Initialise;
        }
        
        #endregion

        private void Initialise()
        {
            LoadAndInitialiseEnemyClassObjects();
        }

        private void LoadAndInitialiseEnemyClassObjects()
        {
            _classes = Resources.LoadAll<EnemyClass>(ENEMY_CLASS_PATH);

            foreach (EnemyClass @class in _classes)
            {
                @class.Initialise();
            }
        }
    }
}
