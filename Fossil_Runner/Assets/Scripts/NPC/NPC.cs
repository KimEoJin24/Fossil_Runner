using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum AIState  //ai상태
{
    Idle,
    Wandering,
    Attacking,
    Fleeing
}


public class NPC : MonoBehaviour//, IDamagable
{
    public PlayerController player;

    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    // public ItemData[] dropOnDeath;  죽으면 떨어뜨리는 것

    [Header("AI")]
    private AIState aiState;
    public float detectDistance;  //탐지거리
    public float safeDistance;   //안전거리

    [Header("Wandering")]
    public float minWanderDistance;  //방황 최소거리 
    public float maxWanderDistance;  // 방황 최대거리
    public float minWanderWaitTime;
    public float maxWanderWaitTime;


    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;

    public float fieldOfView = 120f;

    private NavMeshAgent agent;
    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();// meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

    }

    private void Start()
    {
        SetState(AIState.Wandering);  //처음에 방황으로 시작
    }

    private void Update()
    {
        // 시아각 해결 문제 없음 본체랑 꼬리랑 꺼꾸로 되있었다.  Debug.Log(IsPlaterInFireldOfView());
        playerDistance = Vector3.Distance(transform.position, player.transform.position); //플레이어와 자신사이의 거리
        // 여기는 문젱없음 Debug.Log(playerDistance);
        animator.SetBool("Moving", aiState != AIState.Idle);//가만히 있는것이 아니면 움직이기

        switch (aiState)
        {
            case AIState.Idle: PassiveUpdate(); break;
            case AIState.Wandering: PassiveUpdate(); break;
            case AIState.Attacking: AttackingUpdate(); break;
            case AIState.Fleeing: FleeingUpdate(); break;
        }

    }

    private void FleeingUpdate()
    {
        if (agent.remainingDistance < 0.1f)  //이동거리가 가까우면
        {
            agent.SetDestination(GetFleeLocation()); //목적지를 찾기
        }
        else
        {
            SetState(AIState.Wandering);
        }
    }

    private void AttackingUpdate()
    {
        if (playerDistance > attackDistance || !IsPlaterInFireldOfView())
        {
            agent.isStopped = false;
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(player.transform.position, path)) //경로를 새로검색한다.
            {
                agent.SetDestination(player.transform.position); //목적기를 찾기
            }
            else
            {
                SetState(AIState.Fleeing);
            }
        }
        else
        {
            //Debug.Log("나공격");
            agent.isStopped = true;  //데미지를 입하는 부분 
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                //Player.health -= 10;//여기부분 스태틱으로 하기
                                    // Debug.Log("체력 : " + Player.health);
                                    // PlayerController.instance.GetComponent<IDamagable>().TakePhysicalDamage(damage);
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
    }

    private void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f) //방황하는 중이고, 남은거리가 0.1보다 작다
        {
            //Debug.Log("독수리꺼" + agent.remainingDistance);
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime)); //새로운 로케이션하는 것이 지연시키는 것
        }
        // Debug.Log("플레이어와의 거리 : " + playerDistance);
        // Debug.Log("탐지 거리 : " + detectDistance);
        // Debug.Log(playerDistance < detectDistance);


        if (playerDistance < detectDistance)  //거리안에 들오았다면
        {
            // Debug.Log("공격범위안에 왔어");
            SetState(AIState.Attacking);
        }
    }

    bool IsPlaterInFireldOfView() //시아각에 들어오는지
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;//거리구하기
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    private void SetState(AIState newState) //곰돌이 상태에 따라 변화 구하기
    {
        aiState = newState;
        switch (aiState)
        {
            case AIState.Idle:
                {
                    agent.speed = walkSpeed;
                    agent.isStopped = true;
                }
                break;
            case AIState.Wandering:
                {
                    agent.speed = walkSpeed;
                    agent.isStopped = false;
                }
                break;

            case AIState.Attacking:
                {
                    agent.speed = runSpeed;
                    agent.isStopped = false;
                }
                break;
            case AIState.Fleeing:
                {
                    agent.speed = runSpeed;
                    agent.isStopped = false;
                }
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    void WanderToNewLocation()  //새로운 거리를 구하는 방법
    {
        if (aiState != AIState.Idle)
        {
            return;
        }
        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }


    Vector3 GetWanderLocation() //
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance) //hit와 해당위치의 거리가 탐지거리보다 작다면
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    Vector3 GetFleeLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (GetDestinationAngle(hit.position) > 90 || playerDistance < safeDistance)
        {

            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - player.transform.position, transform.position + targetPos);
    }



    public void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Melee")
        //{
        //    Weapon weapon = other.GetComponent<Weapon>();
        //    health -= weapon.damage;
        //    Debug.Log("몬스터 체력 : " + health);
        //}

        //if (other.tag == "Bullet")
        //{
        //    Bullet bullet = other.GetComponent<Bullet>();
        //    health -= bullet.damage;
        //    Debug.Log("몬스터 체력 : " + health);
        //}
        //if (health <= 0)
            Die();

        StartCoroutine(DamageFlash());
    }
    public void TakePhysicalDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
            Die();

        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        //for (int x = 0; x < dropOnDeath.Length; x++)
        //{
        //    Instantiate(dropOnDeath[x].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        //}

        Destroy(gameObject);
    }

    IEnumerator DamageFlash() //깜까이는 것임.
    {
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f);
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = Color.white;
    }
}
