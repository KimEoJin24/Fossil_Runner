using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoPotal : MonoBehaviour
{
    ParticleSystem ps;
    public Transform potalPos;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = potalPos.position;
        }
        Debug.Log("��ƼŬ �浹");
    }
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = potalPos.position;
        }
        Debug.Log("��ƼŬ �浹");

    }
}
