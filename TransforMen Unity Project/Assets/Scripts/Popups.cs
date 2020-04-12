using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Popups : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameObject.Find("Canvas").transform.Find("Quit Prompt").gameObject.SetActive(true);
        }
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("Menu");
    }
}
