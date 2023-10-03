using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VileageEnter : MonoBehaviour
{
    // Inspector ������ ǥ���� ������� �̸�
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
