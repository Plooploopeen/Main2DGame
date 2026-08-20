using UnityEngine;

public abstract class EnemyBehaviorBase : MonoBehaviour
{
    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();
}