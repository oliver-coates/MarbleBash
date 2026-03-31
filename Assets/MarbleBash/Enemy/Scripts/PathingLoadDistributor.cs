using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace MarbleBash.Enemy
{
    

    
    public class PathingLoadDistributor : MonoBehaviour
    {

        private static PathingLoadDistributor _this;

        private LinkedList<PathingRequester> _requesters;
        LinkedListNode<PathingRequester> _currentRequesterNode;

        [Header("Settings:")]
        [SerializeField] private int _maxRequestsAllowedPerFrame;

        [Header("Status (READ ONLY):")]
        [SerializeField, ReadOnly] private int _numRequesters;
        [SerializeField,ReadOnly] private int _numRequestsAllowedPerFrame;

        private void Awake()
        {
            _requesters = new LinkedList<PathingRequester>();
            _this = this;
        }


        public static void SubscribeTo(IDistributedPathingAgent pathingAgent)
        {
            _this._SubscribeTo(pathingAgent);            
        }
        private void _SubscribeTo(IDistributedPathingAgent pathingAgent)
        {
            PathingRequester newRequester = new PathingRequester(_numRequesters, pathingAgent);
            _requesters.AddLast(new LinkedListNode<PathingRequester>(newRequester));
            _numRequesters++;

            if (_currentRequesterNode == null)
            {
                _currentRequesterNode = _requesters.Last;
            }

            if (_numRequesters > _maxRequestsAllowedPerFrame)
            {
                _numRequestsAllowedPerFrame = _maxRequestsAllowedPerFrame;
            }
            else
            {
                _numRequestsAllowedPerFrame = _numRequesters;
            }
        }

        private void Update()
        {            
            int numRequestsProcessedThisFrame = 0;
            while (numRequestsProcessedThisFrame < _numRequestsAllowedPerFrame)
            {            
                // Advance along linked list
                // Wrap back around if needed
                if (_currentRequesterNode.Next == null)
                {
                    _currentRequesterNode = _requesters.First;
                }
                else
                {
                    _currentRequesterNode = _currentRequesterNode.Next;                
                }

                CalculatePathFor(_currentRequesterNode.Value);
                numRequestsProcessedThisFrame++;
            }
        }

        private void CalculatePathFor(PathingRequester requester)
        {
            Vector3 targetPosition = requester.agent.GetPathingTargetPosition();
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(requester.agent.GetCurrentPosition(), targetPosition, NavMesh.AllAreas, path))
            {
                requester.agent.SetPath(path);
                // Debug.Log($"[TIME: {Time.time:0.00}] Calculated path for requester : {requester.id}");
            }
            else
            {
                // Debug.LogWarning($"[Pathing Load Distributor] Could not calculate path for requester : {requester.id}, TIME: {Time.time:0.00}");            
            }

        }

        private class PathingRequester
        {
            public int id;
            public IDistributedPathingAgent agent;


            public PathingRequester(int id, IDistributedPathingAgent agent)
            {
                this.agent = agent;
                this.id = id;
            }
        }

    }


    public interface IDistributedPathingAgent
    {
        public void SetPath(NavMeshPath path);

        public Vector3 GetPathingTargetPosition();
        public Vector3 GetCurrentPosition();
    }

}
