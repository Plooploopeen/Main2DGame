using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

public class StandStillBehavior : EnemyBehaviorBase
{
    EnemyData data;

    [SerializeField] bool startFacingRight;

    private void Awake()
    {
        data = GetComponent<EnemyData>();
    }
    public override void Enter()
    {
        float absScale = Mathf.Abs(transform.localScale.x);

        if (startFacingRight)
        {
            transform.localScale = new Vector3(absScale, absScale, absScale);
        }
        else
        {
            transform.localScale = new Vector3(-absScale, absScale, absScale);
        }
    }

    public override void Execute()
    {
        data.rb.linearVelocity = new Vector2(0f, data.rb.linearVelocity.y);
    }
    public override void Exit()
    {

    }
}
