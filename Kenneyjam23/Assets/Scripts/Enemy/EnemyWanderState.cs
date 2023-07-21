using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class EnemyWanderState : IEnemyState
{
    public float Range { get; set; } = 10f;
    public float WaitingTime { get; set; } = 3f;

    private NavMeshAgent _agent;
    private Vector3 _gotoPosition;
    private float _waitingTime = 0f;

    public void EnterState(NavMeshAgent agent)
    {
        _agent = agent;
        SetNewTargetPosition();
    }

    public void ExitState()
    {
        _agent.isStopped = true;
    }

    public void UpdateState()
    {
        _waitingTime -= Time.deltaTime;
        if (_waitingTime <= 0f && (_agent.transform.position - _gotoPosition).magnitude <= 1f)
        {
            SetNewTargetPosition();
        }
    }

    private void SetNewTargetPosition()
    {
        _gotoPosition = RandomNavSphere(_agent.transform.position, Range);
        _agent.SetDestination(_gotoPosition);
        _waitingTime = WaitingTime;
    }

    private Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        const int maxTries = 50;
        for (int i = 0; i < maxTries; ++i)
        {
            Vector3 randDirection = Random.insideUnitSphere * distance;
            randDirection += origin;
            NavMeshPath path = new NavMeshPath();

            NavMesh.CalculatePath(origin, randDirection, -1, path);

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                if (NavMesh.SamplePosition(randDirection, out var navHit, distance, -1))
                {
                    return navHit.position;
                }
            }
        }
        
        return origin;
    }
}
