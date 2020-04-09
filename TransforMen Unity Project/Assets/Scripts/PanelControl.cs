using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{

    public bool displayInfoPanel = false;

    private GameObject infoPanel;

    private System.Action buttonAction = null;

    // Start is called before the first frame update
    void Start()
    {
        infoPanel = GameObject.Find("Canvas").transform.Find("Info Panel").gameObject;
        infoPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //If left click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.name != "Terrain")
            {
                if ((hit.collider.name.Contains("Training") || hit.collider.name.Contains("Upgrade")) && !hit.collider.name.Contains("Placement"))
                {
                    displayInfoPanel = true;
                }
                else
                {
                    displayInfoPanel = false;
                }
            }
        }


        if (displayInfoPanel)
        {
            infoPanel.SetActive(true);
        }
        else
        {
           if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
           {
                infoPanel.SetActive(false);
           }  
        }
    }

    public void SetInfo(string queueText, int queueLength, string trainText, float trainTime, string costText, string costValue, string flavourText, string buttonText, System.Action buttonAction = null)
    {
        GameObject qText, qLength, tText, tTime, cText, cValue, fText, actionButton;

        qText = infoPanel.transform.Find("Queue").gameObject;
        qLength = infoPanel.transform.Find("Queue Value").gameObject;
        tText = infoPanel.transform.Find("Time").gameObject;
        tTime = infoPanel.transform.Find("Time Value").gameObject;
        cText = infoPanel.transform.Find("Cost").gameObject;
        cValue = infoPanel.transform.Find("Cost Value").gameObject;
        fText = infoPanel.transform.Find("Flavour Text").gameObject;
        actionButton = infoPanel.transform.Find("Action Button").gameObject;

        qText.GetComponent<TMPro.TextMeshProUGUI>().SetText(queueText);
        qLength.GetComponent<TMPro.TextMeshProUGUI>().SetText(queueLength.ToString());
        tText.GetComponent<TMPro.TextMeshProUGUI>().SetText(trainText);
        tTime.GetComponent<TMPro.TextMeshProUGUI>().SetText(trainTime.ToString());
        cText.GetComponent<TMPro.TextMeshProUGUI>().SetText(costText);
        cValue.GetComponent<TMPro.TextMeshProUGUI>().SetText(costValue.ToString());
        fText.GetComponent<TMPro.TextMeshProUGUI>().SetText(flavourText);

        this.buttonAction = buttonAction;

        //Hide the action button if there is no action
        if (this.buttonAction == null)
        {
            actionButton.SetActive(false);
        }
        else
        {
            actionButton.SetActive(true);
        }
    }

    public void ExecuteButtonAction()
    {
        buttonAction();
    }
}
