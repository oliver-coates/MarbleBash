using UnityEngine;

public class EnemiesBenchmark : MonoBehaviour
{
    [SerializeField, Min(0)] private float _spawnRadius = 5f;
    
    [SerializeField] private float _timeToSpawnEnemy = 0.025f;
    [SerializeField] private int _numEnemies;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _rotator;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private Transform _enemyHolder;
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
            Instantiate(_prefab, spawnPos, Quaternion.identity, _enemyHolder);
            _spawnTimer = _timeToSpawnEnemy;
        }
    }

}
