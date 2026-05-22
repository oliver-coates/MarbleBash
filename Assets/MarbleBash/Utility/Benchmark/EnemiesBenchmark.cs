using MarbleBash;
using MarbleBash.Encounters;
using MarbleBash.Enemy;
using UnityEngine;

public class EnemiesBenchmark : MonoBehaviour
{
    [Header("References:")]
    
    [SerializeField] private Transform _rotator;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private Transform _enemyHolder;

    [Header("Settings:")]
    [SerializeField, Min(0)] private float _spawnRadius = 5f;
    [SerializeField] private float _timeToSpawnEnemy = 0.025f;
    [SerializeField] private int _numEnemies;

    [Header("Enemy Info:")]
    [SerializeField] private GameObject _prefab;
    [SerializeField] private EnemyClass _enemyClass;
    [SerializeField, Min(1)] private int _enemyLevel = 1;

    private int _numEnemiesSpawned;
    private float _spawnTimer;

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        _rotator.Rotate(0, Time.deltaTime * 120f, 0);

        if (_spawnTimer < 0 && _numEnemiesSpawned < _numEnemies)
        {
            Vector3 spawnPos = _spawnPos.position + (Random.insideUnitSphere * _spawnRadius);
            _numEnemiesSpawned++;
            
            EnemyInstance spawnedEnemy = Instantiate(_prefab, spawnPos, Quaternion.identity, _enemyHolder).GetComponent<EnemyInstance>();
            EnemySpawnData spawnData = new EnemySpawnData(
                _enemyClass,
                _enemyLevel,
                spawnPos
            );
            spawnedEnemy.Initialise(spawnData);

            _spawnTimer = _timeToSpawnEnemy;
        }
    }

}
