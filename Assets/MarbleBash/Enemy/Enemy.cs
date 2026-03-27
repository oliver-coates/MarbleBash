using UnityEngine;
using UnityEngine.AI;

namespace MarbleBash.Enemy
{

    
    public class Enemy : MonoBehaviour
    {
        private NavMeshPath newPath;

        private void SetNewPath(NavMeshPath newPath)
        {
            
        }

        private void Start()
        {
            PathingLoadDistributor.SubscribeTo(SetNewPath);
        }

        private void Update()
        {
            
        }
    }


}
