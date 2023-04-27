using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyAgent : MonoBehaviour
    {
        public static OnPatrol OnPatrol;
        public static OnFollow OnFollow;

        private NavMeshAgent _agent;
        [SerializeField] private float _speedPatrol;
        [SerializeField] private float _speedFollow;
        
        private void Awake()
        {
            _agent = TryGetComponent<NavMeshAgent>(out var agent) ? agent : gameObject.AddComponent<NavMeshAgent>();

            OnPatrol += Patrol;
            OnFollow += Follow;
        }

        private void Follow(Transform followTransform)
        {
            _agent.speed = _speedFollow;
            _agent.stoppingDistance = 2f;
            StopCoroutine(PatrolTerrain());
            StartCoroutine(FollowPosition(followTransform));
        }

        private void Patrol()
        {
            _agent.speed = _speedPatrol;
            _agent.stoppingDistance = 0f;
            StopCoroutine(FollowPosition(null));
            StartCoroutine(PatrolTerrain());
        }

        private IEnumerator FollowPosition(Transform followTransform)
        {
            while (true)
            {
                if (followTransform == null)
                {
                    yield return new WaitForSeconds(5f);
                    continue;
                }

                if (_agent.SetDestination(followTransform.position))
                    yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator PatrolTerrain()
        {
            while (true)
            {
                if (_agent.remainingDistance < 2.5f)
                {
                    if (_agent.SetDestination(SetNewPositionPatrol(transform)))
                    {
                        yield return new WaitForSeconds(2);
                    }
                }
                else
                {
                    yield return new WaitForSeconds(2);
                }
            }
        }

        private static Vector3 SetNewPositionPatrol(Transform transform)
        {
            return transform.position +
                   transform.forward * Random.Range(10f, 30f) +
                   new Vector3(Random.Range(5f, 15f) * Random.Range(-1, 2), 0, 0);
        }
    }
}