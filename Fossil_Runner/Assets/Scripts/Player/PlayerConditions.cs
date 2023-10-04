using System;
using UnityEngine;
using UnityEngine.Events;

public interface IDamagable
{
    void TakePhysicalDamage(float amount);
}

[System.Serializable]
public class Condition
{
    // [HideInInspector]
    public float curValue;
    public float maxValue;
    public float startValue;
    public float regenRate;
    public float decayRage;

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }
}

public class PlayerConditions : MonoBehaviour, IDamagable
{
    public MeterController meter;
    public Condition health;
    public Condition thirsty;
    public Condition stamina;
    public float noThirstyHealthDecay;
    public bool useRunStamina;
    public float attackStamina;

    public Animator animator;
    private PlayerController _controller;

    public UnityEvent onTakeDamage;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<PlayerController>();
    }

    void Start()
    {
        health.curValue = health.startValue;
        thirsty.curValue = thirsty.startValue;
        stamina.curValue = stamina.startValue;
        meter.SetMaxHealth(health.maxValue);
        meter.SetMaxStamina(stamina.maxValue);
        meter.SetMaxThirsty(thirsty.maxValue);
    }

    void Update()
    {
        thirsty.Subtract(thirsty.decayRage * Time.deltaTime);
        stamina.Add(stamina.regenRate * Time.deltaTime);

        if (thirsty.curValue == 0.0f)
        {
            health.Subtract(noThirstyHealthDecay * Time.deltaTime);
        }
        if (health.curValue <= 0.0f)
        {
            Die();
        }
        if (useRunStamina)
        {
            UseStamina(stamina.decayRage * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        meter.SetHealth(health.curValue);
        meter.SetStamina(stamina.curValue);
        meter.SetThirsty(thirsty.curValue);
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Drink(float amount)
    {
        thirsty.Add(amount);
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
        {
            animator.SetBool("Run", false);
            _controller.moveSpeed /= _controller.runSpeedRate;
            useRunStamina = false;
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
        animator.SetBool("Dead", true);
    }

    public void TakePhysicalDamage(float amount)
    {
        health.Subtract(amount);
        onTakeDamage?.Invoke();
        animator.SetTrigger("Pain");
    }
}
