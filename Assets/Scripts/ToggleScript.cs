using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isOn=true;
    public Sprite toggleOn;
    public Sprite toggleOff;
    public GameObject gameObject;
    public GameObject gameObject2;
    public MeasurementController measurementController;
    public string unit = "cm";
    void Start()
    {
        gameObject.GetComponent<Image>().sprite = isOn ?  toggleOn:toggleOff;
    }

    // Update is called once per frame
    void Update()
    {
        //isOn = !isOn;
        //gameObject.GetComponent<Image>().sprite = isOn ? toggleOff : toggleOn;
        //gameObject2.GetComponent<Image>().sprite = isOn ? toggleOn : toggleOff;
    }
    public void OnToggle()
    {
        isOn = !isOn;
        gameObject.GetComponent<Image>().sprite = isOn? toggleOn : toggleOff;
        gameObject2.GetComponent<Image>().sprite = isOn ? toggleOff : toggleOn;

        if (isOn&&unit=="cm")
        {
            measurementController.SetUnit("cm");
        }
        else
        {
            measurementController.SetUnit("inch");
        }
       
    }
}
