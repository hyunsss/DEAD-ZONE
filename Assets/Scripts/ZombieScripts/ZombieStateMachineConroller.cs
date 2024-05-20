using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum StateType { Idle, Walk, GotoNoise, FindEnemy, ChaseEnemy, Attack, Die }

public class ZombieStateMachineConroller : MonoBehaviour, INoiseWeight
{

    public int damage;

    public StateType state;
    public Animator animator;

    public Dictionary<StateType, BaseState> state_Dic = new Dictionary<StateType, BaseState>();

    public Coroutine currentState;

    public Transform headPosition;
    public Transform target;
    public Transform Target { get => target; set { target = value; if(target != null) ChangeState(StateType.ChaseEnemy);} }
    [HideInInspector] public Vector3 target_LastPos;

    public List<string> anim_params;

    [HideInInspector] public NavMeshAgent agent;

    #region Noise Properties
    private float noiseWeight;
    private Vector3 noisePos;
    public float NoiseWeight { get => noiseWeight; set => noiseWeight = value; }
    public Vector3 NoisePosition { get => noisePos; set => noisePos = value; }
    public Transform MyTransform { get => transform; }
    #endregion

    private RaycastHit wall_hit;
    private RaycastHit target_hit;

    public bool isDeath;

    [Space(20f)]
    [Header("Zombies Balance Properties")]
    [SerializeField] private float wallRayLength;  // 벽 감지 레이 길이
    [SerializeField] private float targetRayLength;  // 타겟 감지 레이 길이
    [SerializeField] private float attackRange;

    public ZombieAttack[] attack_skripts;

    private void Awake()
    {
        state_Dic = new Dictionary<StateType, BaseState>(){
            { StateType.Idle, new IdleState(this)},
            { StateType.Walk, new WalkState(this)},
            { StateType.GotoNoise, new GoToNoiseState(this)},
            { StateType.FindEnemy, new FindEnemyState(this)},
            { StateType.ChaseEnemy, new ChaseEnemyState(this, target)},
            { StateType.Attack, new AttackState(this)},
            { StateType.Die, new DieState(this)},
        };
        headPosition = transform.Find("headPos");
        anim_params = new List<string>() { "Idle", "Walk", "Chase", "Attack", "Die" };
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }

    private void Start() {
        Init();

        attack_skripts = GetComponentsInChildren<ZombieAttack>();
        DisableCollider();
    }

    public void EnableCollider() {
        foreach (var attack in attack_skripts) attack.collider.enabled = true;
    }

    public void DisableCollider() {
        foreach (var attack in attack_skripts) attack.collider.enabled = false;
    }


    public void Init()
    {
        NoiseWeightManager.Instance.AddNoise(gameObject, GetComponent<INoiseWeight>());
    }

    public void SetAnimState(string nextParam)
    {
        foreach (var param in anim_params)
        {
            animator.SetBool(param, false);

            if (param == nextParam)
            {
                animator.SetBool(param, true);
            }
        }
    }

    public void ChangeState(StateType nextState)
    {
        if (currentState != null)
        {
            state_Dic[state].ExitState();
        }
        state = nextState;
        state_Dic[state].StartState();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDeath == false) {
            //타겟이 없을 때
            if (target == null)
            {
                //소리의 크기가 5 이상일 때
                if (noiseWeight > 5f)
                {
                    ChangeState(StateType.GotoNoise);
                } else {
                    ChangeState(StateType.Walk);
                }
                //타겟이 존재 할 때
            } else {
                float distance = Vector3.Distance(target.position, transform.position);

                if(distance > 30f) {
                    target = null;
                    ChangeState(StateType.Walk);
                } else if(distance <= attackRange) {
                    ChangeState(StateType.Attack);
                } else if(distance >= 20f) {
                    ChangeState(StateType.FindEnemy);
                } else if(distance > attackRange && distance < 20f){
                    ChangeState(StateType.ChaseEnemy);
                }
            }

            
            if(target != null) SetTargetLastPos();
        }

    }

    void FixedUpdate()
    {
        if(!isDeath) {
            //벽 감지 레이캐스트
            if (Physics.Raycast(headPosition.position, transform.forward, out wall_hit, wallRayLength, ~(~0 & (1 << 3))))
            {
                if (wall_hit.collider != null)
                {
                    Vector3 direction = (Vector3.back - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);

                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
                }
            }

            if (target == null)
            {
                Vector3 rightDiagonal = transform.forward + transform.right * 0.5f;
                Vector3 rightDiagonal2 = transform.forward + transform.right * 0.25f;
                Vector3 leftDiagonal = transform.forward - transform.right * 0.5f;
                Vector3 leftDiagonal2 = transform.forward - transform.right * 0.25f;

                if (Physics.Raycast(headPosition.position, leftDiagonal.normalized, out target_hit, targetRayLength, 1 << 3)
                    || Physics.Raycast(headPosition.position, rightDiagonal.normalized, out target_hit, targetRayLength, 1 << 3)
                        || Physics.Raycast(headPosition.position, rightDiagonal2.normalized, out target_hit, targetRayLength, 1 << 3)
                            || Physics.Raycast(headPosition.position, leftDiagonal2.normalized, out target_hit, targetRayLength, 1 << 3))
                {
                    Target = target_hit.collider.transform;
                    Debug.Log("target set");
                }
            }
        }
    }

    public void Death() {
        isDeath = true;
        target = null;
        animator.SetLayerWeight(1, 0);
        ChangeState(StateType.Die);
        Debug.Log("죽음");
    }

    void SetTargetLastPos() {
        target_LastPos = target.transform.position;
    }

    // void OnDrawGizmos()
    // {
    //     // 벽 감지 레이 기즈모
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(headPosition.position, transform.position + transform.forward * wallRayLength);

    //     // 타겟 감지 레이 기즈모 1
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawLine(headPosition.position, (transform.forward + (transform.right * 0.5f)) * targetRayLength);

    //     // 타겟 감지 레이 기즈모 2
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawLine(headPosition.position, (transform.forward - transform.right * 0.5f) * targetRayLength);

    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawLine(headPosition.position, (transform.forward + (transform.right * 0.25f)) * targetRayLength);

    //     // 타겟 감지 레이 기즈모 2
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawLine(headPosition.position, (transform.forward - transform.right * 0.25f) * targetRayLength);
    // }

    public bool IsAnimationComplete(string stateName, int layer)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
        // 상태가 일치하고, normalizedTime이 1보다 크거나 같을 때 true 반환
        // normalizedTime은 0에서 1 사이의 값으로 애니메이션 진행률을 나타냅니다.
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1.0f;
    }
}

public abstract class BaseState
{

    public ZombieStateMachineConroller owner;

    public BaseState(ZombieStateMachineConroller owner)
    {
        this.owner = owner;
    }

    public abstract void StartState();

    public abstract void UpdateState();

    public abstract void ExitState();
}


public class IdleState : BaseState
{
    public IdleState(ZombieStateMachineConroller owner) : base(owner)
    {
    }

    IEnumerator Idle()
    {
        owner.SetAnimState("Idle");

        yield return new WaitForSeconds(Random.Range(0, 5f));
        owner.ChangeState(StateType.Walk);
    }

    public override void ExitState()
    {
        owner.StopCoroutine(owner.currentState);
    }

    public override void StartState()
    {
        owner.currentState = owner.StartCoroutine(Idle());
    }

    public override void UpdateState()
    {

    }
}

public class WalkState : BaseState
{
    public WalkState(ZombieStateMachineConroller owner) : base(owner)
    {

    }

    IEnumerator Walk()
    {
        owner.SetAnimState("Walk");

        yield return new WaitForSeconds(Random.Range(0f, 10f));
        owner.ChangeState(StateType.Idle);
    }

    public override void ExitState()
    {
        owner.StopCoroutine(owner.currentState);
    }

    public override void StartState()
    {
        owner.currentState = owner.StartCoroutine(Walk());
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}


public class GoToNoiseState : BaseState
{
    private Vector3 noisePos;

    public GoToNoiseState(ZombieStateMachineConroller owner) : base(owner)
    {
    }

    IEnumerator GoToNoise(Vector3 noisePos)
    {
        // float angleDifference = 360f;
        // // 목표 방향 계산
        // // 목표 방향을 향하는 회전 생성

        // // 부드러운 회전
        // while (angleDifference > 1f)
        // {
        //     Vector3 direction = (noisePos - owner.transform.position).normalized;
        //     Quaternion lookRotation = Quaternion.LookRotation(direction);

        //     owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, lookRotation, Time.deltaTime * 2f);
        //     angleDifference = Quaternion.Angle(owner.transform.rotation, lookRotation);
        //     yield return null;
        // }

        owner.agent.SetDestination(noisePos);

        owner.SetAnimState("Walk");
        yield return new WaitUntil(() => owner.agent.remainingDistance <= owner.agent.stoppingDistance);

        NoiseWeightManager.Instance.ResetNoiseProperties(owner.gameObject.GetInstanceID());
        if (owner.NoiseWeight > 20f)
        {
            owner.SetAnimState("Walk");
        }
        else
        {
            owner.SetAnimState("Idle");
        }
    }

    public override void ExitState()
    {
        owner.StopCoroutine(owner.currentState);
    }

    public override void StartState()
    {
        noisePos = owner.NoisePosition;
        owner.currentState = owner.StartCoroutine(GoToNoise(owner.NoisePosition));
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}

public class FindEnemyState : BaseState
{
    private Vector3 target_lastPos;
    private Stack<Vector3> expected_Poses;
    public FindEnemyState(ZombieStateMachineConroller owner) : base(owner)
    {

    }

    void InitializeSearchLocate()
    {
        expected_Poses = new Stack<Vector3>();

        float boxWidth = 50f;  // 박스의 너비
        float boxDepth = 50f;  // 박스의 깊이
        Vector3 boxCenter = target_lastPos;  // 박스 중심을 적의 위치로 설정

        for (int i = 0; i < 5; i++)
        {
            float randomX = Random.Range(-boxWidth / 2, boxWidth / 2);
            float randomZ = Random.Range(-boxDepth / 2, boxDepth / 2);
            Vector3 randomPoint = new Vector3(boxCenter.x + randomX, boxCenter.y, boxCenter.z + randomZ);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) // 1.0f는 최대 거리
            {
                expected_Poses.Push(hit.position);
            }
        }

    }

    IEnumerator FindEnemy()
    {
        int length = expected_Poses.Count;
        owner.SetAnimState("Walk");

        for (int i = 0; i < length; i++)
        {
            Debug.Log("Setting destination: " + expected_Poses.Peek());
            owner.agent.SetDestination(expected_Poses.Pop());

            yield return new WaitUntil(() => !owner.agent.pathPending && owner.agent.remainingDistance <= owner.agent.stoppingDistance);
        }
        //적이 있을 만한 예상 위치를 리스트로 저장하고 그 리스트 들을 순회한다
        //리스트 순회가 끝나면 그땐 Walk 스테이트로 변경하자


        yield return null;
    }

    public override void ExitState()
    {
        owner.StopCoroutine(owner.currentState);
        owner.agent.isStopped = true;
    }

    public override void StartState()
    {
        owner.agent.isStopped = false;
        target_lastPos = owner.target_LastPos;
        owner.target = null;

        InitializeSearchLocate();
        owner.currentState = owner.StartCoroutine(FindEnemy());
    }

    public override void UpdateState()
    {

    }
}

public class ChaseEnemyState : BaseState
{
    private Transform target;

    public ChaseEnemyState(ZombieStateMachineConroller owner, Transform target) : base(owner)
    {
        this.target = target;
    }

    IEnumerator ChaseEnemy()
    {
        owner.SetAnimState("Chase");

        owner.agent.SetDestination(target.position);
        yield return new WaitForSeconds(0.5f);

    }

    public override void ExitState()
    {
        owner.StopCoroutine(owner.currentState);
        owner.agent.ResetPath();
    }

    public override void StartState()
    {
        target = owner.target;
        owner.currentState = owner.StartCoroutine(ChaseEnemy());
    }

    public override void UpdateState()
    {

    }
}

public class AttackState : BaseState
{
    private Transform target;

    public AttackState(ZombieStateMachineConroller owner) : base(owner)
    {
    }

    IEnumerator Attack()
    {
        owner.SetAnimState("Attack");

        owner.transform.LookAt(target);
        owner.EnableCollider();
        yield return new WaitUntil(() => owner.IsAnimationComplete("Attack", 0));
        Debug.Log("Compelete Animation");
    }

    public override void ExitState()
    {
        owner.StopCoroutine(owner.currentState);
        owner.DisableCollider();
    }

    public override void StartState()
    {
        target = owner.target;
        owner.currentState = owner.StartCoroutine(Attack());
    }

    public override void UpdateState()
    {

    }
}

public class DieState : BaseState
{
    public DieState(ZombieStateMachineConroller owner) : base(owner)
    {
    }

    public override void ExitState()
    {
        owner.Init();
    }

    public override void StartState()
    {
        owner.SetAnimState("Die");
    }

    public override void UpdateState()
    {

    }
}


