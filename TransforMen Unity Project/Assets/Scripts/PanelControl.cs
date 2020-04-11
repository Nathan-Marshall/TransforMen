using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{

    public bool displayInfoPanel = false;

    private GameObject infoPanel;
    private GameObject qText, tText, cText, fText, actionButton;

    private System.Action buttonAction = null;

    private System.Func<string> queueFunc = null;
    private System.Func<string> timeFunc = null;

    // Start is called before the first frame update
    void Start()
    {
        infoPanel = GameObject.Find("Canvas").transform.Find("Info Panel").gameObject;
        infoPanel.SetActive(false);

        qText = infoPanel.transform.Find("Queue").gameObject;
        tText = infoPanel.transform.Find("Time").gameObject;
        cText = infoPanel.transform.Find("Cost").gameObject;
        fText = infoPanel.transform.Find("Flavour Text").gameObject;
        actionButton = infoPanel.transform.Find("Action Button").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (queueFunc != null)
        {
            qText.GetComponent<TMPro.TextMeshProUGUI>().SetText(queueFunc());
        }

        if (timeFunc != null)
        {
            tText.GetComponent<TMPro.TextMeshProUGUI>().SetText(timeFunc());
        }

        // If left click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.name != "Terrain")
            {
                if ((hit.collider.name.Contains("Training") || hit.collider.name.Contains("Upgrade") || hit.collider.name.Contains("Crawler")) && !hit.collider.name.Contains("Placement"))
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

    public void SetInfo(System.Func<string> queueFunc, System.Func<string> timeFunc, string costText, string flavourText, string buttonText, System.Action buttonAction = null)
    {
        qText.GetComponent<TMPro.TextMeshProUGUI>().SetText(queueFunc());
        tText.GetComponent<TMPro.TextMeshProUGUI>().SetText(timeFunc());
        cText.GetComponent<TMPro.TextMeshProUGUI>().SetText(costText);
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
