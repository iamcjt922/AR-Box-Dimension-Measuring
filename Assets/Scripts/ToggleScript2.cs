using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript2 : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isOn = false;
    public Sprite toggleOn;
    public Sprite toggleOff;
    public GameObject toggle;
    public MeasurementController measurementController;
    void Start()
    {
        toggle.GetComponent<Image>().sprite = isOn ? toggleOn : toggleOff;
        //if (isOn)
        //{
        //    text.text = "Cm";
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnToggle()
    {
        isOn = !isOn;
        toggle.GetComponent<Image>().sprite = isOn ? toggleOn : toggleOff;
      

        if (isOn)
        {
            measurementController.SetUnit("Cm");
          
        }
        else
        {
            measurementController.SetUnit("Inch");
          
        }

    }
}
