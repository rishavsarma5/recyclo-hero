using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public Dictionary<string, GameObject> topBarRelics = new Dictionary<string, GameObject>();
    public Transform relicContainer;
    public GameObject relicItem;

    public void DisplayRelics()
    {
        foreach (Transform relicItem in relicContainer)
            Destroy(relicItem.gameObject);

        foreach (GameObject relicInfo in topBarRelics.Values)
            Instantiate(relicInfo, relicContainer);
            
    }

    public void AddRelicItem(Relic relic)
    {
        GameObject newRelic = Instantiate(relicItem);
        RelicTopBarItem relicItemInfo = newRelic.GetComponent<RelicTopBarItem>();
        relicItemInfo.relicImage = relic.relicImage;
        relicItemInfo.relicDescription = relic.relicDescription;
        topBarRelics[relic.relicDescription] = newRelic;
        DisplayRelics();
    }

    public void RemoveRelicItem(Relic relic)
    {
        topBarRelics.Remove(relic.relicDescription);
        DisplayRelics();
    }
}
