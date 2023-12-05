using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthSliderText;
    public Slider tempShieldSlider;
    public TMP_Text tempShieldSliderText;
    public Slider permShieldSlider;
    public TMP_Text permShieldSliderText;
    public Slider spSlider;
    public TMP_Text spSliderText;
    public Image tempShieldImage;
    public Image permShieldImage;

    public void DisplayHealth(int healthAmount)
    {
        healthSliderText.text = $"{healthAmount}/{healthSlider.maxValue}";
        healthSlider.value = healthAmount;
    }

    public void DisplayUpdatedHealth(int healthAmount, int newMaxValue)
    {
        healthSlider.maxValue = newMaxValue;
        healthSliderText.text = $"{healthAmount}/{healthSlider.maxValue}";
        healthSlider.value = healthAmount;
    }

    public void DisplayTempShield(int healthAmount)
    {
        tempShieldSliderText.text = $"{healthAmount}/{tempShieldSlider.maxValue}";
        tempShieldSlider.value = healthAmount;
    }

    public void DisplayUpdatedTempShield(int healthAmount, int newMaxValue)
    {
        tempShieldSlider.maxValue = newMaxValue;
        tempShieldSliderText.text = $"{healthAmount}/{tempShieldSlider.maxValue}";
        tempShieldSlider.value = healthAmount;
    }

    public void DisplayPermShield(int healthAmount)
    {
        permShieldSliderText.text = $"{healthAmount}/{permShieldSlider.maxValue}";
        permShieldSlider.value = healthAmount;
    }

    public void DisplayUpdatedPermShield(int healthAmount, int newMaxValue)
    {
        permShieldSlider.maxValue = newMaxValue;
        permShieldSliderText.text = $"{healthAmount}/{permShieldSlider.maxValue}";
        permShieldSlider.value = healthAmount;
    }

    public void DisplaySpecialPower(int powerLevel)
    {
        spSliderText.text = $"{powerLevel}/{healthSlider.maxValue}";
        spSlider.value = powerLevel;
    }
}
