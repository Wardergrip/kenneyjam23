using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _visionRange = 7.5f;
    [SerializeField] private float _damageRange = 3f;
    [SerializeField] private GameObject _damageObject;
    [SerializeField] private float _maxAttackCooldown = 1f;
    private NavMeshAgent _navMeshAgent;

    private IEnemyState _state;
    private float _attackCooldown;
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
        _attackCooldown -= Time.deltaTime;
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

        if (_attackCooldown <= 0f && (_target.position - transform.position).magnitude <= _damageRange)
        {
            _attackCooldown = _maxAttackCooldown;
            Instantiate(_damageObject, transform.position, Quaternion.identity);
        }
    }

    public void Died()
    {
        Destroy(gameObject);
    }
}