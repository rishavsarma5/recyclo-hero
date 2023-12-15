using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyStatsUI : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthSliderText;
    public Slider lightShieldSlider;
    public TMP_Text lightShieldSliderText;
    public Slider heavyArmorSlider;
    public TMP_Text heavyArmorSliderText;
    public Slider spSlider;
    public TMP_Text spSliderText;
    public TMP_Text enemyName;
    public TMP_Text enemyAttack;
    public Image enemyImage;
    public Image enemyBackground;

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

    public void DisplayLightShield(int healthAmount)
    {
        lightShieldSliderText.text = $"{healthAmount}/{lightShieldSlider.maxValue}";
        lightShieldSlider.value = healthAmount;
    }

    public void DisplayUpdatedLightShield(int healthAmount, int newMaxValue)
    {
        lightShieldSlider.maxValue = newMaxValue;
        lightShieldSliderText.text = $"{healthAmount}/{lightShieldSlider.maxValue}";
        lightShieldSlider.value = healthAmount;
    }

    public void DisplayHeavyArmor(int healthAmount)
    {
        heavyArmorSliderText.text = $"{healthAmount}/{heavyArmorSlider.maxValue}";
        heavyArmorSlider.value = healthAmount;
    }

    public void DisplayUpdatedHeavyArmor(int healthAmount, int newMaxValue)
    {
        heavyArmorSlider.maxValue = newMaxValue;
        heavyArmorSliderText.text = $"{healthAmount}/{heavyArmorSlider.maxValue}";
        heavyArmorSlider.value = healthAmount;
    }

    public void DisplaySpecialPower(int powerLevel)
    {
        spSliderText.text = $"{powerLevel}/{spSlider.maxValue}";
        spSlider.value = powerLevel;
    }

    public void DisplayUpdatedSpecialPower(int currValue, int newMaxValue)
    {
        spSlider.maxValue = newMaxValue;
        spSliderText.text = $"{currValue}/{spSlider.maxValue}";
        spSlider.value = currValue;
    }
}
