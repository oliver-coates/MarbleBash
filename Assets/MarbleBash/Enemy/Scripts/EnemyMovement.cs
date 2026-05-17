// #define DEBUG_DRAW_PATH

using KahuInteractive.HassleFreeConfig;
using UnityEngine;
using UnityEngine.AI;

namespace MarbleBash.Enemy
{
    public class EnemyMovement : MarbleMovement, IDistributedPathingAgent
    {
        private NavMeshPath _path;
        private Vector3 _pathingTarget;

        #region Initialisation & Destruction
        protected override void Initialise()
        {
            base.Initialise();

            _moveSpeedMultiplier = Configuration.Read("enemy_movement_speed");
            _jumpHeightMultiplier = Configuration.Read("enemy_jump_height");

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

            // This method only has a body if the DEBUG_DRAW_PATH precompiler bool is true.
            // If not, the compiler will remove it
            DebugDrawPath();
        }

        protected override bool CheckIsGrounded(out float distanceToGround)
        {
            float halfScale = transform.localScale.x / 2f;
            
            // Since we draw our distance raycast from the center of our marble, we need to subtract the radius of our marble from the found distance to ground.
            float heightOffset = halfScale;

            // Update our grounded position:
            Ray downRay = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(downRay, out RaycastHit hit, GROUNDED_RAYCAST_DOWN_MAXIMUM_DISTANCE, _config.groundedLayerMask))
            {
                distanceToGround = hit.distance - heightOffset;
                _groundedPosition = hit.point;
            }
            else
            {
                distanceToGround = GROUNDED_RAYCAST_DOWN_MAXIMUM_DISTANCE;
            }

            return distanceToGround < 0.1f;    
        }

        protected override Vector3 GetMovementDirection()
        {
            if (_path == null)
            {
                return Vector3.zero;
            }

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

            return (targetPositon - myPosition).normalized;
        }

        #region Public Methods
        public void SetPathingTarget(Vector3 target)
        {
            _pathingTarget = target;
        }

        public Vector3 GetCurrentPosition()
        {
            return transform.position;
        }

        public Vector3 GetPathingTargetPosition()
        {
            return _pathingTarget;
        }

        public void SetPath(NavMeshPath path)
        {
            _path = path;
        }

        public bool IsRequestingPath()
        {
            return isGrounded;
        }

        protected override Vector3 GetLookDirection()
        {
            return (Player.transform.position - transform.position).normalized;
        }
        #endregion

        #region Debug Methods

        #if DEBUG_DRAW_PATH
        private readonly Color[] debugColours = {Color.red, Color.blue, Color.green, Color.yellow, Color.pink, Color.brown, Color.turquoise, Color.purple};
        #endif
        
        private void DebugDrawPath()
        {
            #if DEBUG_DRAW_PATH
            if (_path == null)
            {
                return;     
            }

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