using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpwanManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bear;
    public int bearNum;  //������ ��
    public Transform bearRespwanPos;

    public GameObject fox;
    public int foxNum;  //���� ��
    public Transform foxRespwanPos;

    public GameObject eagle;
    public int eagleNum;  //������ ��
    public Transform eagleRespwanPos;

    public GameObject dino;
    public int dinoNum;  //���� ��
    public Transform dinoRespwanPos;

    public GameObject BossDino;
    public int BossDinoNum;  //���� ��
    public Transform BossDinoRespwanPos;

    public GameObject playerPos;  
    public Transform PlayerRespwanPos;




    public PlayerController player;
    public PlayerConditions playerConditions;
    float scale;

    public static ReSpwanManager Instance;  //�̱��� ����


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
        StartPlayerRespwan();
        StartSetBearNum();
        StartSetFoxNum();
        StartSetEagleNum();
        StartSetDinoNum();
        StartSetBossDinoNum();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerRespwan();
        UpdateBearNum();
        UpdateFoxNum();
        UpdateEagleNum();
        UpdateDinoNum();
    }

    void BearRespwan()
    {
        scale = Random.Range(0.2f,1F);
        GameObject bears = Instantiate(bear, bearRespwanPos.position, Quaternion.identity);
        bears.GetComponent<NPC>().player = player;
        bears.GetComponent<NPC>().playerConditions = playerConditions;
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
        foxs.GetComponent<NPC>().playerConditions = playerConditions;
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

    void EagleRespwan()
    {
        scale = Random.Range(0.2f, 1F);
        GameObject eagles = Instantiate(eagle, eagleRespwanPos.position, Quaternion.identity);
        eagles.GetComponent<NPC>().player = player;
        eagles.GetComponent<NPC>().playerConditions = playerConditions;
        eagles.transform.localScale = new Vector3(scale, scale, scale);
    }
    public void EagleDie()
    {
        eagleNum--;
    }
    public void StartSetEagleNum()
    {
        for (int i = 0; i < eagleNum; i++)
        {
            EagleRespwan();
        }
    }
    public void UpdateEagleNum()
    {
        if (eagleNum < 2)
        {
            eagleNum++;
            Invoke("EagleRespwan", 4);
        }
    }

    void DinoRespwan()
    {
        scale = Random.Range(0.2f, 1F);
        GameObject dinos = Instantiate(dino, dinoRespwanPos.position, Quaternion.identity);
        dinos.GetComponent<NPC>().player = player;
        dinos.GetComponent<NPC>().playerConditions = playerConditions;
        dinos.transform.localScale = new Vector3(scale, scale, scale);
    }
    public void DinoDie()
    {
        dinoNum--;
    }
    public void StartSetDinoNum()
    {
        for (int i = 0; i < dinoNum; i++)
        {
            DinoRespwan();
        }
    }
    public void UpdateDinoNum()
    {
        if (dinoNum < 2)
        {
            dinoNum++;
            Invoke("DinoRespwan", 4);
        }
    }

    void BossDinoRespwan()
    {
        scale = Random.Range(0.1f, 0.3F);
        GameObject dinos = Instantiate(BossDino, BossDinoRespwanPos.position, Quaternion.identity);
        dinos.GetComponent<NPC>().player = player;
        dinos.GetComponent<NPC>().playerConditions = playerConditions;
        dinos.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void StartSetBossDinoNum()
    {
        for (int i = 0; i < BossDinoNum; i++)
        {
            BossDinoRespwan();
        }
    }

    void PlayerRespwan()
    {
        if(playerConditions.health.curValue <= 0)
        {
            Invoke("StartPlayerRespwan",4);
            Invoke("ResPwanHealth", 4);

        }
    }

    public void StartPlayerRespwan()
    {
        playerPos.transform.position = PlayerRespwanPos.position;
    }
    public void ResPwanHealth()
    {
        playerConditions.health.curValue = 50;
        playerConditions.animator.SetBool("Dead", false);
    }
}
