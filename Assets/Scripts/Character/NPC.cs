using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPC : MonoBehaviour
    {
        private NavMeshAgent _navAgent;
        [SerializeField] private Transform target;

        private void OnEnable()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            StartCoroutine(UpdatePath());
        }

        IEnumerator UpdatePath()
        {
            while (target!=null)
            {
                _navAgent.SetDestination(target.position);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}