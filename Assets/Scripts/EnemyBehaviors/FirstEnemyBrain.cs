using UnityEngine;

public class FirstEnemyBrain : MonoBehaviour
{
    EnemyData data;

    EnemyBehaviorBase currentBehavior;

    [SerializeField] EnemyBehaviorBase patrol;
    [SerializeField] EnemyBehaviorBase chase;
    [SerializeField] EnemyBehaviorBase melee;
    [SerializeField] EnemyBehaviorBase vision;

    private basicMeleeBehavior meleeScript;

    private EnemyHealth enemyHealthScript;

    void Awake()
    {
        data = GetComponent<EnemyData>();
        enemyHealthScript = GetComponentInChildren<EnemyHealth>();
        
        meleeScript = melee as basicMeleeBehavior;
    }

    private void Start()
    {
        setBehavior(patrol);
    }

    private void Update()
    {
        if (enemyHealthScript.isKnockedBack) return;

        vision.Execute();

        currentBehavior?.Execute();

        checkTransitions();
    }

    void checkTransitions()
    {
        if (!data.hasSeenPlayer) return;

        if (meleeScript.shouldAttack())
        {
            setBehavior(melee);
        }
        else if (!enemyHealthScript.isKnockedBack)
        {
            setBehavior(chase);
        }
    }

    void setBehavior (EnemyBehaviorBase newBehavior)
    {
        if (enemyHealthScript.isKnockedBack) return;
        if (currentBehavior  == newBehavior) return;
        currentBehavior?.Exit();
        currentBehavior = newBehavior;
        currentBehavior?.Enter();
    }
}
