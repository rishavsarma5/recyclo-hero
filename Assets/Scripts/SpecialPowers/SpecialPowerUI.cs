using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialPowerUI : MonoBehaviour
{
    public SpecialPowerOption specialPower;
    public TMP_Text specialPowerDescription;
    public Image specialPowerImage;

    //public GameObject discardEffect;
    BattleSceneManager battleSceneManager;

    // Animator animator;
    private void Awake()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        //animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //animator.Play("HoverOffCard");
    }

    public void LoadSpecialPowerUI(SpecialPowerOption _specialPower)
    {
        specialPower = _specialPower;
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        specialPowerDescription.text = specialPower.description;
        specialPowerImage.sprite = specialPower.image;
    }

    public void SelectSpecialPowerOption()
    {
        specialPower.PerformAction();
        TurnOffUI();
    }

    public void DeselectSpecialPowerOption()
    {
        //animator.Play("HoverOffCard");
    }

    public void TurnOffUI()
    {
        foreach(SpecialPowerUI sp in battleSceneManager.playerSpecialPowerCardsDisplayed)
        {
            sp.gameObject.SetActive(false);
        }
        battleSceneManager.specialPowerMenu.SetActive(false);
        battleSceneManager.specialPowerOptionChosen = true;
    }
}
