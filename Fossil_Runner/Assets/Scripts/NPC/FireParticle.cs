using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticle : MonoBehaviour
{
    ParticleSystem ps;
    PlayerConditions playerConditions;
    private void Awake()
    {
        playerConditions = ReSpwanManager.Instance.playerConditions;
        ps = GetComponent<ParticleSystem>();
    }
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            playerConditions.health.curValue -= 2f;
            Debug.Log("불" + playerConditions.health.curValue);
        }
        //Debug.Log("파티클 충돌");

    }
}
