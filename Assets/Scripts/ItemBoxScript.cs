using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Button button;
    public GameObject cube;
    public LockScript lockScript;

    //private Game
    void Start()
    {
        button = this.GetComponent<Button>();
        this.button.onClick.AddListener(delegate { onClick(); });
        //button;
    }
    private void onClick()
    {
       var list= this.transform.name.Split('*');
       var x= float.Parse(list[0])* 0.0254f;
        var y = float.Parse(list[1]) * 0.0254f;
        var z = float.Parse(list[2]) * 0.0254f;
        Debug.Log($"onClick {x}, {y}, {z}");
        cube.transform.localScale = new Vector3(x, y, z);
        lockScript.isClock = false;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
