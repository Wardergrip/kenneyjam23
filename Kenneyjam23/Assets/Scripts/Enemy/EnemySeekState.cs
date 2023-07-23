using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySeekState : IEnemyState
{
    public Transform Target { get; set; }
    private NavMeshAgent _agent;
    public void EnterState(NavMeshAgent agent)
    {
        _agent = agent;
    }

    public void ExitState()
    {
        
    }

    public void UpdateState()
    {
        if (Target == null)
        {
            Debug.LogWarning("No target set in seek mode");
            return;
        }
        _agent.SetDestination(Target.position);
    }
}
