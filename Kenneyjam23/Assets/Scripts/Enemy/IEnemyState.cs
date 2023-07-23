using UnityEngine.AI;

public interface IEnemyState
{
    void EnterState(NavMeshAgent agent);
    void UpdateState();
    void ExitState();
}