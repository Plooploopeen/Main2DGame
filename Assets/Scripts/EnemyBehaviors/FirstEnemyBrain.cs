using UnityEngine;

public class FirstEnemyBrain : MonoBehaviour
{
    IEnemyBehavior currentBehavior;



    void setBehavior (IEnemyBehavior newBehavior)
    {
        if (currentBehavior  == newBehavior) return;
        currentBehavior.Exit();
        currentBehavior = newBehavior;
        currentBehavior.Enter();
    }
}
