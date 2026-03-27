using UnityEngine;
using UnityEngine.AI;

namespace MarbleBash.Enemy
{

    
    public class Enemy : MonoBehaviour, IDistributedPathingAgent
    {
        private NavMeshPath _path;

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

        private void Start()
        {
            PathingLoadDistributor.SubscribeTo(this);
        }

        private void Update()
        {
            Vector3 prev = transform.position;
            foreach (Vector3 point in _path.corners)
            {
                Debug.DrawLine(prev, point, Color.blue);
                prev = point;
            }
        }


    }


}
