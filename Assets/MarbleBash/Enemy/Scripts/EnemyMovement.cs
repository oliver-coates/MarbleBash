#define DEBUG_DRAW_PATH

using System;
using System.Linq;
using MarbleBash.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace MarbleBash.Enemy
{
    
    public class EnemyMovement : MonoBehaviour, IDistributedPathingAgent
    {
        private NavMeshPath _path;
        private Rigidbody _rb;
        private EnemyInstance _enemy;

        private void Start()
        {
            _rb = this.GetComponentSafe<Rigidbody>();
            _enemy = this.GetComponentSafe<EnemyInstance>();

            PathingLoadDistributor.SubscribeTo(this);
        }

        private void Update()
        {
            if (_path != null)
            {
                // This method only has a body if the DEBUG_DRAW_PATH precompiler bool is true.
                // If not, the compiler will remove it
                DebugDrawPath();
            
                ChasePlayer();            
            }
        }

        private void ChasePlayer()
        {
            Vector3 targetPositon = _path.corners[1].ShearTo2D();
            Vector3 myPosition = transform.position.ShearTo2D();

            Vector3 direction = (targetPositon - myPosition).normalized;
            float speed = 350f;

            Vector3 force = direction * speed * Time.deltaTime;

            _rb.AddForce(force);
            Debug.DrawLine(transform.position, transform.position + direction * 2, Color.hotPink);
        }

        #region Public Methods
        public Vector3 GetCurrentPosition()
        {
            return transform.position;
        }

        public Vector3 GetPathingTargetPosition()
        {
            return Player.rigidbody.transform.position;
        }

        public void SetPath(NavMeshPath path)
        {
            _path = path;
        }
        #endregion

        #region Debug Methods
        private void DebugDrawPath()
        {
            #if DEBUG_DRAW_PATH
            Vector3 prev = transform.position;
            foreach (Vector3 point in _path.corners)
            {
                Debug.DrawLine(prev, point, Color.blue);
                prev = point;
            }  
            #endif
        }
        #endregion
    
    }
}