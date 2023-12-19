using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RelicInfo : MonoBehaviour
{
    public TMP_Text relicTitle;
    public TMP_Text relicDescription;

    public void SetUp(string title, string description)
    {
        relicTitle.text = title;
        relicDescription.text = description;
    }

}
