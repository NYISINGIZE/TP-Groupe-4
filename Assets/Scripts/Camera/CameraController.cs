using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]

public class CameraController : MonoBehaviour
{
    // This handle camera settings
    [System.Serializable] //The variables can be edited in the inspector

    public class CameraSettings
    {
        [Header("Camera Move Settings")]   // Unity attributes that header label in the inspector.
        public float zoomSpeed = 5;
        public float moveSpeed = 5;
        public float rotationSpeed = 5;
        public float originalFieldofView = 70;
        public float zoomFieldofView = 20;
        public float MouseX_Sensitivity = 5;
        public float MouseY_Sensitivity = 5;
        public float MaxClampAngle = 90;
        public float MinClampAngle = -30;
    }
    [SerializeField]
    public CameraSettings cameraSettings;

    [System.Serializable]
    public class CameraInputSettings //The name the input axes that control camera rotation and zoom, as well as the input axis for aiming
    {
        public string MouseXAxis = "Mouse X";
        public string MouseYAxis = "Mouse Y";
        public string AimingInput = "Fire2";
    }
    [SerializeField]
    public CameraInputSettings inputSettings; //Store the input settings data for this controller

    Transform center;
    Transform target;

    Camera mainCam;

    float cameraXrotation = 0;
    float cameraYrotation = 0;




    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        center = transform.GetChild(0);
        FindPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(!target)
            return;
        
        //if(!Application.isPlaying)
           // return;

        RotateCamera();
        ZoomCamera();

    }

    void LateUpdate()
    {
        if (target)
        {
            FollowPlayer();
        }
        else
        {
            FindPlayer();
        }      
    }



    void FindPlayer()
    {
         target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void FollowPlayer()
    {
        Vector3 moveVector = Vector3.Lerp(transform.position, target.transform.position, cameraSettings.moveSpeed * Time.deltaTime);
        transform.position = moveVector;
    }

    void RotateCamera()
    {
        cameraXrotation += Input.GetAxis(inputSettings.MouseYAxis) * cameraSettings.MouseY_Sensitivity;
        cameraYrotation += Input.GetAxis(inputSettings.MouseXAxis) * cameraSettings.MouseX_Sensitivity;

        cameraXrotation = Mathf.Clamp(cameraXrotation, cameraSettings.MinClampAngle, cameraSettings.MaxClampAngle);

        cameraYrotation = Mathf.Repeat(cameraYrotation, 360);
        //cameraYrotation = Mathf.Clamp(cameraYrotation, 360);

        Vector3 rotatingAngle = new Vector3 (cameraXrotation, cameraYrotation, 0);

        Quaternion rotation = Quaternion.Slerp(center.transform.localRotation, Quaternion.Euler(rotatingAngle), cameraSettings.rotationSpeed * Time.deltaTime);
        
        center.transform.localRotation = rotation;

    }


    void ZoomCamera()
    {
        if(Input.GetButton(inputSettings.AimingInput))
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, cameraSettings.zoomFieldofView, cameraSettings.zoomSpeed * Time.deltaTime);
        }
        else
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, cameraSettings.originalFieldofView, cameraSettings.zoomSpeed * Time.deltaTime);
        }


    }



}
