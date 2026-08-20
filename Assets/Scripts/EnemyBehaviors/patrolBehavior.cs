using UnityEngine;

public class patrolBehavior : EnemyBehaviorBase
{
    private float patrolTime;
    [SerializeField] float speed;
    [SerializeField] float moveTimerLimit;
    [SerializeField] float waitTimerLimit;
    private enum PatrolState { moveLeft, moveRight, standStill }
    private PatrolState patrolState = PatrolState.moveRight;
    private PatrolState nextState;

    private EnemyData data;

    void Awake()
    {
        data = GetComponent<EnemyData>();
    }


    public override void Enter()
    {

    }

    public override void Execute()
    {
        patrolTime += Time.deltaTime;

        if (patrolState == PatrolState.moveRight)
        {
            data.rb.linearVelocity = new Vector2(speed, data.rb.linearVelocity.y);

            if (patrolTime > moveTimerLimit)
            {
                patrolTime = 0;
                patrolState = PatrolState.standStill;
                nextState = PatrolState.moveLeft;
            }
        }
        else if (patrolState == PatrolState.moveLeft)
        {
            data.rb.linearVelocity = new Vector2(-speed, data.rb.linearVelocity.y);

            if (patrolTime > moveTimerLimit)
            {
                patrolTime = 0;
                patrolState = PatrolState.standStill;
                nextState = PatrolState.moveRight;
            }
        }
        else if (patrolState == PatrolState.standStill)
        {
            data.rb.linearVelocity = new Vector2(0, data.rb.linearVelocity.y);

            if (patrolTime > waitTimerLimit)
            {
                patrolTime = 0;

                if (nextState == PatrolState.moveRight)
                {
                    patrolState = PatrolState.moveRight;
                }
                else
                {
                    patrolState = PatrolState.moveLeft;
                }
            }
        }
    }

    public override void Exit()
    {
        
    }


}
