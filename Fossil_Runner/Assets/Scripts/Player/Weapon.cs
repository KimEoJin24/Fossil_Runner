using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public enum AttackType
{
    Melee,
    Range
};

public class Weapon : MonoBehaviour
{
    public AttackType AttackType;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Use()
    {
        if (AttackType == AttackType.Melee)
        {
            StopCoroutine(nameof(Swing));
            StartCoroutine(nameof(Swing));
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.4f);
        trailEffect.enabled = false;
    }
}
