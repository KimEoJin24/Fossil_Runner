using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Tree : MonoBehaviour
{
    public int health;
    private MeshRenderer[] meshRenderers;
    public GameObject mySelf;
    Vector3 itemPosition;
    public List<GameObject> dropItem = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        meshRenderers = GetComponents<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            health -= weapon.damage;
            Debug.Log("몬스터 체력 : " + health);
            StartCoroutine(DamageFlash());
        }

        //if (other.tag == "Bullet")
        //{
        //    Bullet bullet = other.GetComponent<Bullet>();
        //    health -= bullet.damage;
        //    Debug.Log("몬스터 체력 : " + health);
        //}
        if (health <= 0)
        {

            //Invoke("Respwan",4);
            // Die();
            DropItem();
            Destroy(gameObject);

        }
        
    }

    IEnumerator DamageFlash() //깜빡이는 것임.
    {
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.4f);
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
            go.transform.localScale = new Vector3(7, 7, 7);
            //Debug.Log(go.transform.localScale);
            go.GetComponent<Rigidbody>().AddForce(transform.up * 5, ForceMode.Impulse); 

            
        }
    }
}
