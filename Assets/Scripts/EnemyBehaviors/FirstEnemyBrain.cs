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
        if (data.isKnockedBack) return;

        vision.Execute();

        currentBehavior?.Execute();

        checkTransitions();
    }

    void checkTransitions()
    {
        if (!data.hasSeenPlayer) return;

        //core transitions
        if (meleeScript.shouldAttack())
        {
            setBehavior(melee);
        }
        else if (!data.isKnockedBack)
        {
            setBehavior(chase);
        }

        //finicky fixes
        if (currentBehavior == melee && data.isKnockedBack)
        {
            meleeScript.disableHitbox();
            currentBehavior?.Exit();
            currentBehavior = null;
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
