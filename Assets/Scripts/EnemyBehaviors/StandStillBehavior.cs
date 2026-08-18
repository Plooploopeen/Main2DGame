using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

public class StandStillBehavior : MonoBehaviour, IEnemyBehavior
{
    EnemyData data;

    [SerializeField] bool startFacingRight;

    private void Awake()
    {
        data = GetComponent<EnemyData>();
    }
    public void Enter()
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

    public void Execute()
    {
        data.rb.linearVelocity = Vector3.zero;
    }
    public void Exit()
    {

    }
}
