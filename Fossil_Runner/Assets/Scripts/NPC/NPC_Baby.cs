using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Baby : MonoBehaviour
{
    public PlayerController player;
    public GameObject mySelf; //������ ����Ʈ�� ���� ��ġ
    public GameObject fireBall;
    public Transform fireBallPos; //���� ������ ��ġ
    public List<GameObject> dropItem = new List<GameObject>();
    Vector3 itemPosition;
    //public GameObject bearDropItem;

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
        player = ReSpwanManager.Instance.player;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();// meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

    }

    private void Start()
    {
        SetState(AIState.Wandering);  //ó���� ��Ȳ���� ����
    }

    private void Update()
    {
        // �þư� �ذ� ���� ���� ��ü�� ������ ���ٷ� ���־���.  Debug.Log(IsPlaterInFireldOfView());
        playerDistance = Vector3.Distance(transform.position, player.transform.position); //�÷��̾�� �ڽŻ����� �Ÿ�
                                                                                          // ����� �������� Debug.Log(playerDistance);
                                                                                          // animator.SetBool("Moving", aiState != AIState.Idle);//������ �ִ°��� �ƴϸ� �����̱�

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
                Instantiate(fireBall, fireBallPos.position, Quaternion.identity);//Quaternion.identity�̰� ���� �ѹ� �˾ƺ���
                                                                                 // Player.health -= 10;//����κ� ����ƽ���� �ϱ�

                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
    }

    private void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f) //��Ȳ�ϴ� ���̰�, �����Ÿ��� 0.1���� �۴�
        {

            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime)); //���ο� �����̼��ϴ� ���� ������Ű�� ��
        }



        if (playerDistance < detectDistance)  //�Ÿ��ȿ� ����Ҵٸ�
        {

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
           // Weapon weapon = other.GetComponent<Weapon>();
           // health -= weapon.damage;
            Debug.Log("���� ü�� : " + health);
        }

        if (other.tag == "Bullet")
        {
          //  Bullet bullet = other.GetComponent<Bullet>();
          //  health -= bullet.damage;
            Debug.Log("���� ü�� : " + health);
        }
        if (health <= 0)
        {

            //Invoke("Respwan",4);
            DropItem();
            Die();

        }
        StartCoroutine(DamageFlash());
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

        Destroy(gameObject);
    }

    IEnumerator DamageFlash() //�����̴� ����.
    {
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f);
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = Color.white;
    }


    public void DropItem()
    {
        for (int i = 0; i < dropItem.Count; i++)
        {
            itemPosition = mySelf.transform.position + new Vector3(0, 5, 0);
            //dropItem.Count;
            int num = Random.Range(0, dropItem.Count);
            GameObject go = Instantiate(dropItem[num], itemPosition, Quaternion.identity);
            //Debug.Log(itemPosition);
            go.GetComponent<Rigidbody>().AddForce(transform.up * 20, ForceMode.Impulse); //�߷��� ���ٰ� Ű�� ������ ����?

            //�������� �ҷ��� for�� �������ϰų� ȿ���� �ָ� �ɵ�
        }

        //switch (type)
        //{
        //    case AnimalType.bear:           
        //        for (int i = 0; i < dropItem.Count; i++)
        //        {
        //            itemPosition = mySelf.transform.position + new Vector3(0, 5, 0);
        //            //dropItem.Count;
        //            int num = Random.Range(0, dropItem.Count);
        //            GameObject go = Instantiate(dropItem[num], itemPosition, Quaternion.identity);
        //            //Debug.Log(itemPosition);
        //            go.GetComponent<Rigidbody>().AddForce(transform.up * 20, ForceMode.Impulse); //�߷��� ���ٰ� Ű�� ������ ����?

        //            //�������� �ҷ��� for�� �������ϰų� ȿ���� �ָ� �ɵ�
        //        }
        //        break;
        //    case NPC.AnimalType.fox:
        //        break;
        //    case NPC.AnimalType.dinosaur:
        //        break;
        //    case NPC.AnimalType.eagle:
        //        break;
        //    default: break;
        //}
    }

    IEnumerator DestroyDropItem() //�����̴� ����.
    {

        int num = Random.Range(0, dropItem.Count);
        GameObject go = Instantiate(dropItem[num], this.gameObject.transform.up * 5, Quaternion.identity);
        Debug.Log(this.gameObject.transform.up * 5);
        go.GetComponent<Rigidbody>().AddForce(transform.up * 80, ForceMode.Impulse); //�߷��� ���ٰ� Ű�� ������ ����?          

        yield return new WaitForSeconds(4f);
        Destroy(go);

    }
}