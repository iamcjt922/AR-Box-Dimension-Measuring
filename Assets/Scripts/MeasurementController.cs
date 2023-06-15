using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

using UnityEngine.UI;
using RengeGames.HealthBars;
using TMPro;

[RequireComponent(typeof(ARRaycastManager))]
public class MeasurementController : MonoBehaviour
{
    
    [SerializeField]
    private GameObject measurementPointPrefab;

    [SerializeField]
    private float measurementFactor = 39.37f;

    [SerializeField]
    private Vector3 offsetMeasurement = Vector3.zero;

    //[SerializeField]
    //private GameObject welcomePanel;

    //[SerializeField]
    //private Button dismissButton;

    [SerializeField]
    private TextMeshPro distanceText;

    [SerializeField]
    private ARCameraManager arCameraManager;

    [SerializeField]
    private Text description;

    [SerializeField]
    private GameObject cubit;
    
    private LineRenderer measureLine;

    private ARRaycastManager arRaycastManager;
   
    private GameObject startPoint;

    private GameObject endPoint;

    private Vector2 touchPosition = default;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private List<GameObject> listPoints = new List<GameObject>();
    private List<LineRenderer> lines = new List<LineRenderer>();

    private float initDistance;
    private Vector3 initScale;
    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private Slider lengthSlider;
    [SerializeField]
    private Slider widthSlider;
    [SerializeField]
    private Slider heightSlider;
    [SerializeField]
    private Text lengthText, widthText, heightText,volumeText;
    private Vector3 originalSize;
    private string unit = "Inch";

    public Text Dimension;
    private bool onTouchHold = false;
    private bool isMenuOpen = false;
    public float maxScale = 100f;
    public GameObject panel;

    public InputField xInputField, yInputField, zInputField, xyzInputField;
    public Slider xyzSlider;

    public TextMeshPro xEdgeText,yEdgeText,zEdgeText, yEdgeText2;

    private ARPointCloudManager arPointCloudManager;
    private PointCloudParser pointCloudParser;
    public ARCusor aRCusor;
    public GameObject sphere;
    public LockScript lockScript;

    public TextMeshProUGUI percent;
    public UltimateCircularHealthBar hb;
    public GameObject LoadingScreen;
    public Material BoxMaterial;
    private PlaneAreaBehaviour planeAreaBehaviour;

    void Awake() 
    {

        //var engine = UnityEditor.Scripting.Python.;
        //ICollection<string> searchPaths = engine.GetSearchPaths();
        ////Path to the folder of test.py
        //searchPaths.Add(Application.dataPath);
        ////Path to the Python standard library
        //searchPaths.Add(Application.dataPath + @"\StreamingAssets" + @"\Lib\");
        //engine.SetSearchPaths(searchPaths);
        //dynamic py = engine.ExecuteFile(Application.dataPath + @"\StreamingAssets" + @"\Python\test.py");
        //dynamic test = py.Test("Codemaker");
        planeAreaBehaviour = GetComponent<PlaneAreaBehaviour>();
        arRaycastManager = GetComponent<ARRaycastManager>();
     
        startPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);
        endPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);
   
        measureLine = GetComponent<LineRenderer>();
        
        startPoint.SetActive(false);
        endPoint.SetActive(false);
        //GameObject.Instantiate(cubit);

        //dismissButton.onClick.AddListener(Dismiss);
     
       
    }
    private void Start()
    {
       
        panel.SetActive(isMenuOpen);
        lengthSlider.minValue = 0;
        widthSlider.minValue = 0;
        heightSlider.minValue = 0;
        xyzSlider.minValue = 0;
        lengthSlider.maxValue = maxScale;
        widthSlider.maxValue = maxScale;
        heightSlider.maxValue = maxScale;
        xyzSlider.maxValue = maxScale;

        lengthSlider.value = cubit.transform.localScale.z;
        widthSlider.value = cubit.transform.localScale.x;
        heightSlider.value = cubit.transform.localScale.y;
        xyzSlider.value = cubit.transform.localScale.x;

        //slider
        lengthSlider.onValueChanged.AddListener(delegate { lenghtSliderChanged(); });
        widthSlider.onValueChanged.AddListener(delegate { widthSliderChanged(); });
        heightSlider.onValueChanged.AddListener(delegate { hightSliderChanged(); });
        xyzSlider.onValueChanged.AddListener(delegate { OnXyzChanged(); });

        //input field
        //xInputField= GameObject.FindGameObjectWithTag("xInputField").GetComponent<InputField>();
        //yInputField = GameObject.FindGameObjectWithTag("yInputField").GetComponent<InputField>();
        //zInputField = GameObject.FindGameObjectWithTag("zInputField").GetComponent<InputField>();
        //xyzInputField = GameObject.FindGameObjectWithTag("xyzInputField").GetComponent<InputField>();

        xInputField.onValueChanged.AddListener(delegate { OnXInputFieldChanged(); });
        yInputField.onValueChanged.AddListener(delegate { OnYInputFieldChanged(); });
        zInputField.onValueChanged.AddListener(delegate { OnZInputFieldChanged(); });
        xyzInputField.onValueChanged.AddListener(delegate { OnXYZInputFieldChanged(); });


        originalSize = cubit.transform.localScale;
        cubit.SetActive(false);
        UpdateText();
       
        arPointCloudManager = GetComponent<ARPointCloudManager>();
        percent.SetText("");
        pointCloudParser = new PointCloudParser(arPointCloudManager,cubit,aRCusor,sphere,lockScript,hb,percent,planeAreaBehaviour);
        
        pointCloudParser.OnEnable();

        StartCoroutine(ExampleCoroutine());
       


    }
    IEnumerator ExampleCoroutine()
    {
        LoadingScreen.SetActive(true);
        yield return new WaitForSeconds(3);
        LoadingScreen.SetActive(false);
    }
    private void OnXInputFieldChanged()
    {
        var value = float.Parse(xInputField.text);
        if (unit == "Cm")
        {
            value = value / 100;
        }
        else
        {
            value = 0.0254f * value;
        }
        Vector3 scale = cubit.transform.localScale;
        scale.x = value;
        cubit.transform.localScale = scale;

    }
    private void OnYInputFieldChanged()
    {

        var value = float.Parse(yInputField.text);
        if (unit == "Cm")
        {
            value = value / 100;
        }
        else
        {
            value = 0.0254f * value;
        }
        Vector3 scale = cubit.transform.localScale;
        scale.y = value;
        cubit.transform.localScale = scale;
    }
    private void OnZInputFieldChanged()
    {
        var value = float.Parse(zInputField.text);
        if (unit == "Cm")
        {
            value = value / 100;
        }
        else
        {
            value = 0.0254f * value;
        }
        Vector3 scale = cubit.transform.localScale;
        scale.z= value;
        cubit.transform.localScale = scale;
    }
    private void OnXYZInputFieldChanged()
    {
        var value = float.Parse(xyzInputField.text);
        if (unit == "Cm")
        {
            value = value / 100;
        }
        else
        {
            value = 0.0254f * value;
        }
        cubit.transform.localScale = new Vector3(value, value, value);

    }

    private void OnXyzChanged()
    {

        Vector3 scale = cubit.transform.localScale;
        var value = xyzSlider.value;
        
        cubit.transform.localScale = new Vector3(value,value,value);

    }
    public void lenghtSliderChanged()
    {
       
        Vector3 scale = cubit.transform.localScale;
        var value= lengthSlider.value;
        var z = value;
        scale.z = z;
        cubit.transform.localScale = scale;
      




    }
    public void widthSliderChanged()
    {
        Vector3 scale = cubit.transform.localScale;
        var value = widthSlider.value;
        var x = value;
        scale.x = x;
        cubit.transform.localScale = scale;
      
    }
    public void hightSliderChanged()
    {
        Vector3 scale = cubit.transform.localScale;
        var value = heightSlider.value;
        var y = value;
        scale.y = y;
        cubit.transform.localScale = scale;


    }

    private void OnEnable() 
    {
        if(measurementPointPrefab == null)
        {
            Debug.LogError("measurementPointPrefab must be set");
            enabled = false;
        }    
    }
   
    void Update()
    {
        if (arPointCloudManager.trackables.count > 0)
        {
          
            arPointCloudManager.SetTrackablesActive(lockScript.isClock);
        }
        Debug.Log($"isClock {lockScript.isClock}");

       
        pointCloudParser.stopScan = !lockScript.isClock;


        if (isMenuOpen == false)
        {

            if (Input.touchCount > 0)
            {

                Touch touch = Input.GetTouch(0);
                //OnDragCube();
                
                if (touch.phase == TouchPhase.Began)
                {
                    touchPosition = touch.position;
                    if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                    {
                        //GameObject point = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);
                        //point.SetActive(true);

                        Pose hitPose = hits[0].pose;
                        //point.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                        //listPoints.Add(point);
                        //cubit.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                    }
                }


                if (listPoints.Count > 1)
                {

                    measureLine.positionCount = listPoints.Count;
                    var distance = "";
                    for (int i = 0; i < listPoints.Count; i++)
                    {
                        measureLine.SetPosition(i, listPoints[i].transform.position);
                        if (i < listPoints.Count - 1)
                        {
                            distance += $"A{i}->A{i + 1}= {(Vector3.Distance(listPoints[i].transform.position, listPoints[i + 1].transform.position) * measurementFactor).ToString("F2")} in\n";
                        }

                    }
                    description.text = distance;


                }


            }

            if (Input.touchCount == 2)
            {
                var touchZero = Input.GetTouch(0);
                var touchOne = Input.GetTouch(1);
                if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled || touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
                {
                    return;
                }
                if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
                {
                    initDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    initScale = cubit.transform.localScale;


                }
                else
                {
                    var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    if (Mathf.Approximately(initDistance, 0))
                    {
                        return;
                    }
                    var factor = currentDistance / initDistance;
                    var scale = initScale * factor;
                    if (scale.x <= maxScale && scale.y <= maxScale && scale.z <= maxScale)
                    {
                        cubit.transform.localScale = scale;

                    }
                }

            }
        }
      
        
        
        UpdateText();

        var golbalPoints = pointCloudParser.golbalPoints;
        var finalPoints = golbalPoints.FindAll((x) => x.weight >= 3);
        var percent = ((float)finalPoints.Count / (float)golbalPoints.Count);
        if (finalPoints.Count > 0) {
            hb.SetPercent(percent);
            if (percent >= 0.7 && percent <= 0.8)
            {
                BoxMaterial.SetColor("_Color", Color.red);
            }
            else if (percent >= 0.8 && percent <= 0.9)
            {
                BoxMaterial.SetColor("_Color", Color.yellow);
            }
            else if (percent >= 0.9)
            {
                BoxMaterial.SetColor("_Color", Color.green);
            }
           

            this.percent.SetText($"{((int)(percent * 100))}");
        }
        if (lockScript.isClock==false)
        {
            pointCloudParser.golbalPoints.Clear();
            this.percent.SetText("");
            
            BoxMaterial.SetColor("_Color", Color.grey);

            //BoxMaterial.color = Color.HSVToRGB();
        }

      
    }
    private void UpdateText()
    {
        Vector3 scale = cubit.transform.localScale;
        widthSlider.value = scale.x;
        heightSlider.value = scale.y;
        lengthSlider.value = scale.z;
        

        var x = (scale.x*100).ToString("0.00");
        var y = (scale.y*100).ToString("0.00");
        var z = (scale.z*100).ToString("0.00");


     
        if (unit != "Cm")
        {
            x =  (scale.x * 39.3701f).ToString("0.00");
            y = (scale.y * 39.3701f).ToString("0.00");
            z = (scale.z * 39.3701f).ToString("0.00");
            //xInputField.text = x;
            //yInputField.text = y;
            //zInputField.text = z;

        }
        //xInputField.text = x;
        //yInputField.text = y;
        //zInputField.text = z;
        if (x == y && y == z)
        {
            //xyzInputField.text = x;
            xyzSlider.value = scale.x;
           
        }
        volumeText.text = (double.Parse(x) * double.Parse(y) * double.Parse(z)).ToString("0,00");



        widthText.text = $"Width= {x} {unit}";
        heightText.text = $"Height= {y} {unit}";
        lengthText.text = $"Length= {z} {unit}";
        Dimension.text = $"Dimension: {x}*{y}*{z} {unit}";
        xEdgeText.text =$"{x} {unit}";
        yEdgeText.text = $"{y} {unit}";
        yEdgeText2.text = $"{y} {unit}";
        zEdgeText.text = $"{z} {unit}";
      
    }

    
    public void OnReset() {
     var scale=this.cubit.transform.localScale;
        scale.x = 0.3f;
        scale.y = 0.3f;
        scale.z = 0.3f;
        this.cubit.transform.localScale = scale;
        this.cubit.SetActive(false);
        pointCloudParser.golbalPoints.Clear();
    }
    public void SetUnit(string _unit)
    {
        this.unit = _unit;
        Debug.Log($"SetUnit {_unit}");
    }
    private void OnDragCube() {


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    onTouchHold = true;


                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                onTouchHold = false;
            }

           
        }
        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            //if (placedObject == null)
            //{
            //    if (defaultRotation > 0)
            //    {
            //        placedObject = Instantiate(placedPrefab, hitPose.position, Quaternion.identity);
            //        placedObject.transform.Rotate(Vector3.up, defaultRotation);
            //    }
            //    else
            //    {
            //        placedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
            //    }
            //}
            //else
            //{
            //}

            if (onTouchHold)
            {
                cubit.transform.position = hitPose.position;
                cubit.transform.rotation = hitPose.rotation;
            }
        }


    }
    public void OnMenuChange()
    {
        isMenuOpen = !isMenuOpen;
        panel.SetActive(isMenuOpen);
       
    }
    public void PlaceBox()
    {
        if (lockScript.isClock)
        {
            pointCloudParser.placeBox();
            aRCusor.cursorChildObject.SetActive(false);
        }
      
    }

}
