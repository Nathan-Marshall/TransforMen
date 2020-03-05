using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePanel : MonoBehaviour
{
    public PlayerResources resources;
    public TMPro.TextMeshProUGUI populationDisplay;
    public TMPro.TextMeshProUGUI scrapDisplay;
    public TMPro.TextMeshProUGUI spikesDisplay;
    public TMPro.TextMeshProUGUI crawlbitsDisplay;

    void OnGUI() {
        populationDisplay.SetText(resources.GetPopulationResource().ToString());
        scrapDisplay.SetText(resources.GetScrapResource().ToString());
        spikesDisplay.SetText(resources.GetSpikeResource().ToString());
        crawlbitsDisplay.SetText(resources.GetCrawlbitResource().ToString());
    }
}
