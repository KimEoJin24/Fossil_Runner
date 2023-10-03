using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VileageEnter : MonoBehaviour
{
    // Inspector 영역에 표시할 배경음악 이름
    public string bgmName = "";

    public GameObject CamObject;

    void Start()
    {
        //CamObject = GameObject.Find("Main Camera");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            CamObject.GetComponent<PlayMusicOperator>().PlayBGM(bgmName);
    }
}
