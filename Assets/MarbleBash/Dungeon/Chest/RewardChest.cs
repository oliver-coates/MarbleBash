using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash
{

    public class RewardChest : MonoBehaviour
    {
        public enum ChestState
        {
            Closed = 0,
            Opening = 1,
            Opened = 2
        }

        [SerializeField] private TriggerHandle _collisionTrigger;
        [SerializeField] private Transform _spawnPoint;

        private Stack<ChestLoot> _contents;

        private ChestState _state;
        private float _openingTimer;

        private readonly float _SpawnContentsInterval;

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
            _collisionTrigger.onTriggerEnter += TriggerEnter;
        }

        private void TriggerEnter(Collider collider)
        {
            if (_state == ChestState.Closed)
            {
                if (collider.CompareTag("Player"))
                {
                    Open();
                }
            }
        }

        private void Open()
        {
            _state = ChestState.Opening;
        }

        private void Update()
        {
            if (_state == ChestState.Opening)
            {
                UpdateOpeningState();
            }
        }

        private void UpdateOpeningState()
        {
            _openingTimer += Time.deltaTime;

            if (_openingTimer > _SpawnContentsInterval)
            {
                _spawnPoint.Rotate(0, Time.deltaTime * 3f, 0);
                if (_contents.TryPop(out ChestLoot contents))
                {
                    SpawnNextContents(contents);
                    _openingTimer = 0;
                }
                else
                {
                    FinishedOpening();
                }
            }
        }

        private void FinishedOpening()
        {
            _state = ChestState.Opened;
        }

        private void SpawnNextContents(ChestLoot contents)
        {
            contents.Spawn(_spawnPoint.position, _spawnPoint.rotation);
        }
    }


}

