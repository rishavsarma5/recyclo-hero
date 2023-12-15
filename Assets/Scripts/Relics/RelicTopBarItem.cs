using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicTopBarItem : MonoBehaviour
{
    public Sprite relicImage;
    public string relicDescription;
    public TMP_Text descriptionText;

    private void Start()
    {
        descriptionText.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        // Show the description text on hover
        descriptionText.text = relicDescription;
        descriptionText.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        // Hide the description text when not hovering
        descriptionText.gameObject.SetActive(false);
    }
}
