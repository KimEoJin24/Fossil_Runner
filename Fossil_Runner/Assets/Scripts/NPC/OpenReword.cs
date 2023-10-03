using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenReword : MonoBehaviour
{

    private Animator animator;
    public GameObject Dragon;

    public GameObject mySelf;
    Vector3 itemPosition;
    public List<GameObject> dropItem = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Open() 
    {
        Invoke("OpenAni", 12);
        Invoke("DropItem", 18);
    }

    public void OpenAni()
    {
        animator.SetTrigger("Open");
    }
    public void DropItem()
    {
        for (int i = 0; i < dropItem.Count; i++)
        {
            itemPosition = mySelf.transform.position + new Vector3(0, 5, 0);
            //dropItem.Count;
            int num = Random.Range(0, dropItem.Count);
            GameObject go = Instantiate(dropItem[num], itemPosition, Quaternion.identity);
            go.transform.localScale = new Vector3(15, 15, 15);
            //Debug.Log(go.transform.localScale);
            go.GetComponent<Rigidbody>().AddForce(transform.up * 5, ForceMode.Impulse);


        }
    }


}
