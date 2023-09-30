using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpwanManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bear;
    public int bearNum;  //°õµ¹ÀÌ ¼ö
    public Transform bearRespwanPos;

    public GameObject fox;
    public int foxNum;  //°õµ¹ÀÌ ¼ö
    public Transform foxRespwanPos;



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

        //bearNum = 2;
    }

    private void Start()
    {
        StartSetBearNum();
        StartSetFoxNum();

    }

    // Update is called once per frame
    void Update()
    {

        UpdateBearNum();
        UpdateFoxNum();
    }

    void BearRespwan()
    {
        scale = Random.Range(0.2f,1F);
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

    void FoxRespwan()
    {
        scale = Random.Range(0.2f, 1F);
        GameObject foxs = Instantiate(fox, foxRespwanPos.position, Quaternion.identity);
        foxs.GetComponent<NPC>().player = player;
        foxs.transform.localScale = new Vector3(scale, scale, scale);
        

    }
    public void FoxDie()
    {

        foxNum--;
    }
    public void StartSetFoxNum()
    {
        for (int i = 0; i < foxNum; i++)
        {
            FoxRespwan();
        }
    }
    public void UpdateFoxNum()
    {
        if (foxNum < 2)
        {
            foxNum++;
            Invoke("FoxRespwan", 4);


        }
    }
}
