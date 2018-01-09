using UnityEngine;
using UnityEngine.AI;

namespace UniLife.GOAP {
    public class Navigator {
        NavMeshAgent mNavMeshAgent;

        public Navigator(GameObject pAgentGameObject) {
            mNavMeshAgent = pAgentGameObject.GetComponent<NavMeshAgent>();
        }

        public void SetDestination(Vector3 pDestination) {
            mNavMeshAgent.destination = pDestination;
        }
    }
}
