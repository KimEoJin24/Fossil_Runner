using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MeterController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Slider thirstySlider;

    public MeterController(Slider staminaSlider)
    {
        this.staminaSlider = staminaSlider;
    }

    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }
    
    public void SetMaxStamina(float stamina)
    {
        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;
    }

    public void SetStamina(float stamina)
    {
        staminaSlider.value = stamina;
    }
    
    public void SetMaxThirsty(float thirsty)
    {
        thirstySlider.maxValue = thirsty;
        thirstySlider.value = thirsty;
    }

    public void SetThirsty(float thirsty)
    {
        thirstySlider.value = thirsty;
    }
}
