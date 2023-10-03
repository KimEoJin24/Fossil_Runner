using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum AIState  //ai����
{
    Idle,
    Wandering,
    Attacking,
    Fleeing
}


public class NPC : MonoBehaviour//, IDamagable
{
    public enum AnimalType
    {
        bear,
        fox,
        eagle,
        dinosaur
    }
    public Slider HPbar;

    public PlayerController player;
    public PlayerConditions playerConditions;
    public AnimalType type;
    public GameObject mySelf;  //�ڱ� �ڽ������ ����
    public List<GameObject> dropItem = new List<GameObject>();  //����Ʈ�� �������� ��� ����
    Vector3 itemPosition; //�������� ����Ʈ���� ���� ��ġ

    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    // public ItemData[] dropOnDeath;  ������ ����߸��� ��

    [Header("AI")]
    private AIState aiState;
    public float detectDistance;  //Ž���Ÿ�
    public float safeDistance;   //�����Ÿ�

    [Header("Wandering")]
    public float minWanderDistance;  //��Ȳ �ּҰŸ� 
    public float maxWanderDistance;  // ��Ȳ �ִ�Ÿ�
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
        HPbar.maxValue = health;
        HPbar.minValue = 0;
        SetState(AIState.Wandering);  //ó���� ��Ȳ���� ����
    }

    private void Update()
    {
        HPbar.value = health;
        // �þư� �ذ� ���� ���� ��ü�� ������ ���ٷ� ���־���.  Debug.Log(IsPlaterInFireldOfView());
        playerDistance = Vector3.Distance(transform.position, player.transform.position); //�÷��̾�� �ڽŻ����� �Ÿ�
        // ����� �������� Debug.Log(playerDistance);
        animator.SetBool("Moving", aiState != AIState.Idle);//������ �ִ°��� �ƴϸ� �����̱�

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
        if (agent.remainingDistance < 0.1f)  //�̵��Ÿ��� ������
        {
            agent.SetDestination(GetFleeLocation()); //�������� ã��
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
            if (agent.CalculatePath(player.transform.position, path)) //��θ� ���ΰ˻��Ѵ�.
            {
                agent.SetDestination(player.transform.position); //�����⸦ ã��
            }
            else
            {
                SetState(AIState.Fleeing);
            }
        }
        else
        {
            //Debug.Log("������");
            agent.isStopped = true;  //�������� ���ϴ� �κ� 
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                playerConditions.health.curValue -= 3f;
                Debug.Log("일반스킬로의 체력" + playerConditions.health.curValue);
                //Player.health -= 10;//����κ� ����ƽ���� �ϱ�
                // Debug.Log("ü�� : " + Player.health);
                // PlayerController.instance.GetComponent<IDamagable>().TakePhysicalDamage(damage);
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
    }

    private void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f) //��Ȳ�ϴ� ���̰�, �����Ÿ��� 0.1���� �۴�
        {
            //Debug.Log("��������" + agent.remainingDistance);
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime)); //���ο� �����̼��ϴ� ���� ������Ű�� ��
        }
        // Debug.Log("�÷��̾���� �Ÿ� : " + playerDistance);
        // Debug.Log("Ž�� �Ÿ� : " + detectDistance);
        // Debug.Log(playerDistance < detectDistance);


        if (playerDistance < detectDistance)  //�Ÿ��ȿ� ����Ҵٸ�
        {
            // Debug.Log("���ݹ����ȿ� �Ծ�");
            SetState(AIState.Attacking);
        }
    }

    bool IsPlaterInFireldOfView() //�þư��� ��������
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;//�Ÿ����ϱ�
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    private void SetState(AIState newState) //������ ���¿� ���� ��ȭ ���ϱ�
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

    void WanderToNewLocation()  //���ο� �Ÿ��� ���ϴ� ���
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
        while (Vector3.Distance(transform.position, hit.position) < detectDistance) //hit�� �ش���ġ�� �Ÿ��� Ž���Ÿ����� �۴ٸ�
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
       
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            health -= weapon.damage;
            Debug.Log("���� ü�� : " + health);
            StartCoroutine(DamageFlash());
        }

        //if (other.tag == "Bullet")
        //{
        //    Bullet bullet = other.GetComponent<Bullet>();
        //    health -= bullet.damage;
        //    Debug.Log("���� ü�� : " + health);
        //}
        if (health <= 0)
        {
            DropItem();
            Die();
        }

        //StartCoroutine(DamageFlash());
    }
    //public void TakePhysicalDamage(int damageAmount)
    //{
    //    health -= damageAmount;
    //    if (health <= 0)
    //        Die();

    //    StartCoroutine(DamageFlash());
    //}

    void Die()
    {
        //for (int x = 0; x < dropOnDeath.Length; x++)
        //{
        //    Instantiate(dropOnDeath[x].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        //}
        if (type == AnimalType.bear)
        {

            ReSpwanManager.Instance.bearNum--;

        }

        Destroy(gameObject);
    }
    public void DropItem()
    {
        for (int i = 0; i < dropItem.Count; i++)
        {
            itemPosition = mySelf.transform.position + new Vector3(0, 5, 0);
            //dropItem.Count;
            int num = Random.Range(0, dropItem.Count);
            GameObject go = Instantiate(dropItem[num], itemPosition, Quaternion.identity);
            go.transform.localScale = new Vector3(7, 7, 7);
            //Debug.Log(go.transform.localScale);
            go.GetComponent<Rigidbody>().AddForce(transform.up * 5, ForceMode.Impulse); //�߷��� ���ٰ� Ű�� ������ ����?

            //�������� �ҷ��� for�� �������ϰų� ȿ���� �ָ� �ɵ�
        }
    }

        IEnumerator DamageFlash() //����̴� ����.
    {
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f);
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = Color.white;
    }
}
