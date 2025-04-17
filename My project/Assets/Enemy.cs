using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _viewDistanse = 15;

    [SerializeField] public float _viewAngel = 120;

    [SerializeField] private Transform[] _directionPoint;
    private int _currentPointIndex = 0;

    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        ActionSelection();
    }

    private void ActionSelection()
    {
        if (CanSeePlayer())
        {
            _agent.SetDestination(_player.transform.position);
        }
        else
        {
            if (_directionPoint.Length > 0)
                _agent.SetDestination(_directionPoint[_currentPointIndex].position);

            Patrol();
        }
    }

    private void Patrol()
    {
        if (_directionPoint.Length == 0)
            return;

        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _currentPointIndex = (_currentPointIndex + 1) % _directionPoint.Length;
            _agent.SetDestination(_directionPoint[_currentPointIndex].position);
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer > _viewDistanse)
            return false;

        float angelBetween = Vector3.Angle(transform.forward, directionToPlayer);

        if (angelBetween > _viewAngel * 0.5f)
            return false;

        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hitInfo , distanceToPlayer, ~0))
        {
            if(hitInfo.transform != _player.transform)
                return false;
        }

        return true;
    }
}
