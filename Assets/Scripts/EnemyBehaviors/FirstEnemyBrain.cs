using UnityEngine;

public class FirstEnemyBrain : MonoBehaviour
{
    EnemyData data;

    EnemyBehaviorBase currentBehavior;

    [SerializeField] EnemyBehaviorBase patrol;
    [SerializeField] EnemyBehaviorBase chase;
    [SerializeField] EnemyBehaviorBase attack;
    [SerializeField] EnemyBehaviorBase vision;

    void Awake()
    {
        data = GetComponent<EnemyData>();
    }

    private void Start()
    {
        setBehavior(patrol);
    }

    private void Update()
    {
        if (data.isKnockedBack)
        {
            if (currentBehavior != null)
            {
                currentBehavior.Exit();
                currentBehavior = null;
                data.animator.Play("Idle");
            }
            return;
        }

        vision.Execute();

        currentBehavior?.Execute();

        checkTransitions();
    }

    void checkTransitions()
    {
        if (!data.hasSeenPlayer) return;

        //core transitions
        if (attack.shouldAttack())
        {
            setBehavior(attack);
        }
        else if (!data.isKnockedBack)
        {
            setBehavior(chase);
        }
    }

    void setBehavior (EnemyBehaviorBase newBehavior)
    {
        if (data.isKnockedBack) return;
        if (currentBehavior  == newBehavior) return;
        currentBehavior?.Exit();
        currentBehavior = newBehavior;
        currentBehavior?.Enter();
    }
}
