using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicTopBarItem : MonoBehaviour
{
    public Sprite relicImage;
    public string relicName;
    public string relicDescription;
    public GameObject relicInfoPrefab;
    public GameObject currRelicInfo = null;

    private Transform relicItemTransform;
    private RectTransform relicItemRectTransform;

    private void Start()
    {
        relicItemTransform = GetComponent<Transform>();
        relicItemRectTransform = GetComponent<RectTransform>();
    }

    public void RelicOnHover()
    {
        if (currRelicInfo != null)
        {
            Destroy(currRelicInfo.gameObject);
        }
        
        // Calculate the position for the tooltip based on the relic item's position and size
        Vector3 tooltipLocalPosition = new Vector3(relicItemRectTransform.sizeDelta.x * 2f, -relicItemRectTransform.sizeDelta.y * 2.5f, 0f);

        // Instantiate the tooltip as a child of the relic item with the correct local position
        currRelicInfo = Instantiate(relicInfoPrefab, relicItemTransform);
        currRelicInfo.transform.localPosition = tooltipLocalPosition;

        // Set up the tooltip content
        currRelicInfo.GetComponent<RelicInfo>().SetUp(relicName, relicDescription);
    }

    public void RelicOffHover()
    {
        // Hide the description text when not hovering
        if (currRelicInfo != null)
        {
            Destroy(currRelicInfo.gameObject);
        }
    }
}
