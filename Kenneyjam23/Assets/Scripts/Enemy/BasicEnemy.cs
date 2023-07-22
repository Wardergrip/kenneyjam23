using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _visionRange = 5f;
    private NavMeshAgent _navMeshAgent;

    private IEnemyState _state;
    private EnemyWanderState _enemyWanderState;
    private EnemySeekState _enemySeekState;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (_navMeshAgent == null)
        {
            Debug.LogError($"Navmesh not found on object: {gameObject.name}");
        }

        _enemyWanderState = new EnemyWanderState();
        _enemyWanderState.EnterState(_navMeshAgent);

        _enemySeekState = new EnemySeekState
        {
            Target = _target
        };
        _enemySeekState.EnterState(_navMeshAgent);
    }

    // Update is called once per frame
    void Update()
    {
        if ((_target.position - transform.position).magnitude <= _visionRange)
        {
            if (_state != _enemySeekState)
            {
                _enemySeekState.EnterState(_navMeshAgent);
            }
            _state = _enemySeekState;
        }
        else
        {
            if (_state != _enemyWanderState)
            {
                _enemyWanderState.EnterState(_navMeshAgent);
            }
            _state = _enemyWanderState;
        }
        _state.UpdateState();
    }
}
