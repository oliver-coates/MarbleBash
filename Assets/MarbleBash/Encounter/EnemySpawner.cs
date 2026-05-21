using KahuInteractive.HassleFreeConfig;
using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash.Encounters
{

    public class EnemySpawner
    {
        private readonly float _MinimumSpawnDistance = Configuration.Read("minimum_distance_to_spawn_enemy_to_player");
        private readonly GameObject _EnemyPrefab = Configuration.Get<CombatConfig>().enemyPrefab;

        private class EnemySpawnData
        {
            public EnemyType type;     
            public int level;

            public Vector3 postion;
        }
        
        internal void SpawnEncounter(Encounter encounter)
        {
            foreach (Encounter.ElementTEMP enemyElement in encounter.elements)
            {
                SpawnEncounterElement(encounter, enemyElement);
            }
        }

        private void SpawnEncounterElement(Encounter encounter, Encounter.ElementTEMP enemyElement)
        {
            EnemyClass @class = enemyElement.enemyClass;
            for (int enemyIndex = 0; enemyIndex < enemyElement.num; enemyIndex++)
            {
                EnemySpawnData spawnData = GenerateEnemyData(encounter, enemyElement, @class);

                SpawnEnemy(spawnData);
            }
        }

        private EnemySpawnData GenerateEnemyData(Encounter encounter, Encounter.ElementTEMP enemyElement, EnemyClass @class)
        {
            int level = enemyElement.level;
            float marbleRadius = 1f; // For now we will just assume a radius of 1, this should be reviewed later.
            Vector3 randomPosition = FindSpawnLocation(encounter.position, encounter.radius, marbleRadius);

            EnemySpawnData data = new()
            {
                type = @class.GetTypeAtLevel(level),
                level = level,
                postion = randomPosition
            };

            return data;
        }

        private void SpawnEnemy(EnemySpawnData data)
        {
            EnemyInstance newInstance = GameObject.Instantiate(_EnemyPrefab).GetComponent<EnemyInstance>();
        
            newInstance.transform.position = data.postion;
            
            // ToDo: Implement level & type  here
        }


        #region Finding Spawn Location
        private Vector3 FindSpawnLocation(Vector3 position, float spawnRadius, float marbleRadius)
        {
            int attempts = 0;
            while (attempts < 128)
            {
                attempts++;

                Vector3 pos = GetRandomPosition(position, spawnRadius);

                if (IsPositionTooCloseToPlayer(pos))
                {
                    continue;
                }

                if (IsPositionWithinGeometry(position, marbleRadius))
                {
                    continue;
                }

                return pos;
            }


            Debug.LogError("Could not find suitable position!");
            return Vector3.zero;
        }

        private static Vector3 GetRandomPosition(Vector3 position, float radius)
        {
            Vector2 randomUnitCircle = UnityEngine.Random.insideUnitCircle * radius;

            Vector3 pos = position + new Vector3(randomUnitCircle.x, 0, randomUnitCircle.y);
            return pos;
        }

        private bool IsPositionTooCloseToPlayer(Vector3 pos)
        {
            float distanceToPlayer = Vector3.Distance(pos, Player.transform.position);

            return distanceToPlayer < _MinimumSpawnDistance;
        }

        private static bool IsPositionWithinGeometry(Vector3 position, float marbleRadius)
        {
            Collider[] collidersHit = Physics.OverlapSphere(position, marbleRadius, LayerMask.NameToLayer("Default")); 
            return collidersHit.Length > 0;
        }
        #endregion



    }


}

