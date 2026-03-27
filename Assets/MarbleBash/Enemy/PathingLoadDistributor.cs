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
        private int _numRequestsAllowedPerFrame;

        [Header("Status:")]
        [SerializeField] private int _numRequesters;

        private void Awake()
        {
            _requesters = new LinkedList<PathingRequester>();
            _this = this;
        }


        public static void SubscribeTo(Action<NavMeshPath> loadRequester)
        {
            _this._SubscribeTo(loadRequester);            
        }
        private void _SubscribeTo(Action<NavMeshPath> loadRequester)
        {
            PathingRequester newRequester = new PathingRequester(_numRequesters, loadRequester);
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
            requester.callback.Invoke(new NavMeshPath());
            Debug.Log($"[TIME: {Time.time:0.00}] Calculated path for requester : {requester.id}");
        }


        private class PathingRequester
        {
            public int id;
            public Action<NavMeshPath> callback;

            public PathingRequester(int id, Action<NavMeshPath> callback)
            {
                this.id = id;
                this.callback = callback;
            }
        }
    }


}
