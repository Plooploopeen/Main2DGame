using UnityEngine;

public class FirstEnemyBrain : MonoBehaviour
{
    EnemyData data;

    IEnemyBehavior currentBehavior;

    private patrolBehavior patrol;
    private chasePlayerBehavior chase;
    private basicMeleeBehavior melee;
    private coneOfVisionBehavior vision;

    private EnemyHealth enemyHealthScript;

    void Awake()
    {
        data = GetComponent<EnemyData>();
        patrol = GetComponent<patrolBehavior>();
        chase = GetComponent<chasePlayerBehavior>();
        melee = GetComponent<basicMeleeBehavior>();
        vision = GetComponent<coneOfVisionBehavior>();

        enemyHealthScript = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        setBehavior(patrol);
    }

    private void Update()
    {
        vision.Execute();

        currentBehavior?.Execute();

        checkTransitions();
    }

    void checkTransitions()
    {
        if (!data.hasSeenPlayer) return;

        if (melee.shouldAttack())
        {
            setBehavior(melee);
        }
        else if (!enemyHealthScript.isKnockedBack)
        {
            setBehavior(chase);
        }
    }

    void setBehavior (IEnemyBehavior newBehavior)
    {
        if (currentBehavior  == newBehavior) return;
        currentBehavior?.Exit();
        currentBehavior = newBehavior;
        currentBehavior?.Enter();
    }
}
