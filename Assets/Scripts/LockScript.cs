using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class LockScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isClock=false;
    public Sprite toggleOn;
    public Sprite toggleOff;
    public GameObject toggle;
    public ARPointCloudManager aRPointCloudManager;
    void Start()
    {
        toggle.GetComponent<Image>().sprite = isClock ? toggleOn : toggleOff;
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnLock()
    {
        isClock = !isClock;
        toggle.GetComponent<Image>().sprite = isClock ? toggleOn : toggleOff;
        aRPointCloudManager.SetTrackablesActive(isClock);
    }
}
