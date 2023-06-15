using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject item;
    public GameObject content;
    private List<GameObject> listItems=new List<GameObject>();
    //private List<Vector3> listSizes =new List<Vector3>();
    private List<BoxItem> boxItems = new List<BoxItem>();
    [System.Obsolete]
    void Start()
    {
        boxItems.Add(new BoxItem("box", "Box", new Vector3(12f, 12f, 12f),Vector3.zero));
        boxItems.Add(new BoxItem("Kelloggs Corn", "Kelloggs Corn",new Vector3(7.5f,13.5f,3),Vector3.zero));
        boxItems.Add(new BoxItem("Kleenex Lotion", "Kleenex Lotion", new Vector3(8.5f, 15f, 8.75f), Vector3.zero));
        boxItems.Add(new BoxItem("Solo Heavyweight", "Solo Heavyweight", new Vector3(6.25f, 4.75f, 18), Vector3.zero));
        
        for(int i = 0; i < boxItems.Count; i++)
        {
            var position = item.transform.position;
            if (i > 0)
            {
                position = listItems[i - 1].transform.position;
                position.y = listItems[i - 1].transform.position.y - 120;
            }
            var scale = boxItems[i].scale;
            var box = Instantiate(item, position, Quaternion.identity);
            box.name = $"{scale.x}*{scale.y}*{scale.z}";
            box.transform.parent = content.transform;
            var sizeText = box.transform.FindChild("Size").gameObject.GetComponent<Text>();
         var boxImage=  box.transform.FindChild("Image").gameObject.GetComponent<Image>();
            boxImage.sprite= Resources.Load<Sprite>(boxItems[i].image);
            sizeText.text = boxItems[i].name;
            listItems.Add(box);



        }
        item.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
  

}
public class BoxItem
{
    public string image;
    public string name;
    public Vector3 scale;
    public Vector3 position;
    public BoxItem(string image,string name,Vector3 scale,Vector3 position)
    {
        this.image = image;
        this.name = name;
        this.scale = scale;
        this.position = position;

    }
}