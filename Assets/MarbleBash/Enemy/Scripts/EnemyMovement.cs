#define DEBUG_DRAW_PATH

using UnityEngine;
using UnityEngine.AI;

namespace MarbleBash.Enemy
{
    public class EnemyMovement : MarbleMovement, IDistributedPathingAgent
    {
        private NavMeshPath _path;

        #region Initialisation & Destruction
        protected override void Initialise()
        {
            base.Initialise();

            PathingLoadDistributor.SubscribeTo(this);
        }

        private void OnDestroy()
        {
            PathingLoadDistributor.UnsubscribeFrom(this);
        }
        #endregion

        protected override void Update()
        {
            base.Update();

            if (_path != null)
            {
                // This method only has a body if the DEBUG_DRAW_PATH precompiler bool is true.
                // If not, the compiler will remove it
                DebugDrawPath();
            
                ChasePlayer();            
            }
        }

        protected override bool CheckIsGrounded(out float distanceToGround)
        {
            float halfScale = transform.localScale.x / 2f;
            
            // Find the position at the very bottom of our marble
            Vector3 floorPosition = transform.position + (Vector3.down * halfScale);

            // Update our grounded position:
            Ray downRay = new Ray(floorPosition + (Vector3.up * 0.05f), Vector3.down);
            if (Physics.Raycast(downRay, out RaycastHit hit, 100f, _config.groundedLayerMask))
            {
                distanceToGround = hit.distance;
            }
            else
            {
                distanceToGround = 100f;
            }

            return distanceToGround < 0.1f;    
        }

        private void ChasePlayer()
        {
            Vector3 targetPositon;
            if (_path.corners.Length == 1)
            {
                targetPositon = _path.corners[0].ShearTo2D(); 
            }
            else
            {
                targetPositon = _path.corners[1].ShearTo2D(); 
            }
            Vector3 myPosition = transform.position.ShearTo2D();

            Vector3 direction = (targetPositon - myPosition).normalized;

            float speed = 350f;

            Vector3 force = direction * speed * Time.deltaTime;

            _marble.rigidbody.AddForce(force);
            Debug.DrawLine(transform.position, transform.position + direction * 2, Color.hotPink);
        }


        #region Public Methods
        public Vector3 GetCurrentPosition()
        {
            return transform.position;
        }

        public Vector3 GetPathingTargetPosition()
        {
            return Player.movement.groundedPosition;
        }

        public void SetPath(NavMeshPath path)
        {
            _path = path;
        }

        public bool IsRequestingPath()
        {
            return isGrounded;
        }
        #endregion


        #region Debug Methods

        #if DEBUG_DRAW_PATH
        private readonly Color[] debugColours = {Color.red, Color.blue, Color.green, Color.yellow, Color.pink, Color.brown, Color.turquoise, Color.purple};
        #endif
        
        private void DebugDrawPath()
        {
            #if DEBUG_DRAW_PATH
            Vector3 prev = transform.position;
            int i = 0;
            foreach (Vector3 point in _path.corners)
            {
                Debug.DrawLine(prev, point, debugColours[i]);
                prev = point;
                i++;
                if (i >= debugColours.Length)
                {
                    i = 0;
                }
            }  
            #endif
        }

        #endregion

    }
}