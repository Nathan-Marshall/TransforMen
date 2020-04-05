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

    //These private functions change the corresponding numbers to red 
    void populationInsufficient()
    {
        populationDisplay.color = new Color32(255, 20, 20, 255);
    }

    void scrapInsufficient()
    {
        scrapDisplay.color = new Color32(255, 20, 20, 255);
    }

    void spikesInsufficient()
    {
        spikesDisplay.color = new Color32(255, 20, 20, 255);
    }

    void crawlbitsInsufficient()
    {
        crawlbitsDisplay.color = new Color32(255, 20, 20, 255);
    }

    //Given a label, change it to be red 
    void showLabelInsufficienct(GameObject panel, string labelName)
    {
        //Whatever is passed in, change its text to red. 
        TMPro.TextMeshProUGUI label = panel.transform.Find(labelName).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        label.color = new Color32(255, 0, 0, 255);
    }

    //Displays all values that are insufficient as red (labels, too) 
    public void showInsufficiency(int popCost, int scrapCost, int spikeCost, int crawlCost)
    {

       if (resources.GetPopulationResource() < popCost)
        {
            showLabelInsufficienct(gameObject, "Population Label");
            gameObject.GetComponent<ResourcePanel>().populationInsufficient();
        }
        if (resources.GetScrapResource() < scrapCost)
        {
            showLabelInsufficienct(gameObject, "Scrap Label");
            gameObject.GetComponent<ResourcePanel>().scrapInsufficient();
        }
        if (resources.GetSpikeResource() < spikeCost)
        {
            showLabelInsufficienct(gameObject, "Spikes Label");
            gameObject.GetComponent<ResourcePanel>().spikesInsufficient();
        }
        if (resources.GetCrawlbitResource() < crawlCost)
        {
            showLabelInsufficienct(gameObject, "Crawlbits Label");
            gameObject.GetComponent<ResourcePanel>().crawlbitsInsufficient();
        }

        StartCoroutine(returnTextToWhite(gameObject));
    }

    //Returns the menu text back to white 
    IEnumerator returnTextToWhite(GameObject panel)
    {
        yield return new WaitForSeconds(1.5f);

        float t = 1;
        Color32 defaultColour = new Color32(255, 255, 255, 255);

        TMPro.TextMeshProUGUI popLabel = panel.transform.Find("Population Label").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI scrapLabel = panel.transform.Find("Scrap Label").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI spikeLabel = panel.transform.Find("Spikes Label").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI crawlLabel = panel.transform.Find("Crawlbits Label").gameObject.GetComponent<TMPro.TextMeshProUGUI>();

        TMPro.TextMeshProUGUI popValue = panel.GetComponent<ResourcePanel>().populationDisplay;
        TMPro.TextMeshProUGUI scrapValue = panel.GetComponent<ResourcePanel>().scrapDisplay;
        TMPro.TextMeshProUGUI spikeValue = panel.GetComponent<ResourcePanel>().spikesDisplay;
        TMPro.TextMeshProUGUI crawlValue = panel.GetComponent<ResourcePanel>().crawlbitsDisplay;

        List<TMPro.TextMeshProUGUI> allText = new List<TMPro.TextMeshProUGUI>();
        allText.Add(popLabel);
        allText.Add(scrapLabel);
        allText.Add(spikeLabel);
        allText.Add(crawlLabel);
        allText.Add(popValue);
        allText.Add(scrapValue);
        allText.Add(spikeValue);
        allText.Add(crawlValue);

        t += (Time.deltaTime * 1.5f);

        //Not a smooth transition at the moment, could be nicer 
        foreach (TMPro.TextMeshProUGUI text in allText)
        {
            text.color = Color32.Lerp(text.color, defaultColour, t);
        }

    }
}
