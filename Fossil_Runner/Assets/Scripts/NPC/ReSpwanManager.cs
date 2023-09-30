using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpwanManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bear;
    public int bearNum;  //°õµ¹ÀÌ ¼ö
    public Transform bearRespwanPos;



    public PlayerController player;
    float scale;

    public static ReSpwanManager Instance;  //½Ì±ÛÅæ ÁøÇà


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        bearNum = 2;
    }

    private void Start()
    {
        StartSetBearNum();

    }

    // Update is called once per frame
    void Update()
    {

        UpdateBearNum();
    }

    void BearRespwan()
    {
        scale = Random.Range(1F, 2F);
        GameObject bears = Instantiate(bear, bearRespwanPos.position, Quaternion.identity);
        bears.GetComponent<NPC>().player = player;
        bears.transform.localScale = new Vector3(scale, scale, scale);
        // bearNum++;

    }

    public void BearDie()
    {

        bearNum--;
    }
    public void StartSetBearNum()
    {
        for (int i = 0; i < bearNum; i++)
        {
            BearRespwan();
        }
    }
    public void UpdateBearNum()
    {
        if (bearNum < 2)
        {
            bearNum++;
            Invoke("BearRespwan", 4);


        }
    }
}
