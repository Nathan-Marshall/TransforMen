using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{

    public bool displayInfoPanel = false;

    private GameObject infoPanel;
    private GameObject qText, qLength, tText, tTime, cText, cValue, fText, actionButton;

    private System.Action buttonAction = null;

    private System.Func<int> queueFunc = null;
    private System.Func<float> timeFunc = null;

    // Start is called before the first frame update
    void Start()
    {
        infoPanel = GameObject.Find("Canvas").transform.Find("Info Panel").gameObject;
        infoPanel.SetActive(false);

        qText = infoPanel.transform.Find("Queue").gameObject;
        qLength = infoPanel.transform.Find("Queue Value").gameObject;
        tText = infoPanel.transform.Find("Time").gameObject;
        tTime = infoPanel.transform.Find("Time Value").gameObject;
        cText = infoPanel.transform.Find("Cost").gameObject;
        cValue = infoPanel.transform.Find("Cost Value").gameObject;
        fText = infoPanel.transform.Find("Flavour Text").gameObject;
        actionButton = infoPanel.transform.Find("Action Button").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (queueFunc != null)
        {
            qLength.GetComponent<TMPro.TextMeshProUGUI>().SetText(queueFunc().ToString());
        }

        if (timeFunc != null)
        {
            tTime.GetComponent<TMPro.TextMeshProUGUI>().SetText(timeFunc().ToString("0.00"));
        }

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

    public void SetInfo(string queueText, System.Func<int> queueFunc, string trainText, System.Func<float> timeFunc, string costText, string costValue, string flavourText, string buttonText, System.Action buttonAction = null)
    {
        qText.GetComponent<TMPro.TextMeshProUGUI>().SetText(queueText);
        qLength.GetComponent<TMPro.TextMeshProUGUI>().SetText(queueFunc().ToString());
        tText.GetComponent<TMPro.TextMeshProUGUI>().SetText(trainText);
        tTime.GetComponent<TMPro.TextMeshProUGUI>().SetText(timeFunc().ToString("0.00"));
        cText.GetComponent<TMPro.TextMeshProUGUI>().SetText(costText);
        cValue.GetComponent<TMPro.TextMeshProUGUI>().SetText(costValue.ToString());
        fText.GetComponent<TMPro.TextMeshProUGUI>().SetText(flavourText);

        this.buttonAction = buttonAction;
        this.queueFunc = queueFunc;
        this.timeFunc = timeFunc;

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
