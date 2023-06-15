using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExistAppScript : MonoBehaviour
{
    public GameObject ExistPanel;
    // Start is called before the first frame update
    void Start()
    {
        ExistPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnExistApp()
    {
        ExistPanel.SetActive(true);

    }
    public void OnNoPress() {
        ExistPanel.SetActive(false);
    }
    public void OnYesPress()
    {
        ExistPanel.SetActive(false);
        Application.Quit();
    }

}
