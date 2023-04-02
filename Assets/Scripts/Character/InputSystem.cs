using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]

public class InputSystem : MonoBehaviour
{

    Movement moveScript;

    [System.Serializable]
    public class InputSettings
    {
        public string forwardInput = "Vertical";
        public string strafeInput = "Horizontal";
        public string sprintInput = "Sprint";
        public string aim = "Fire2";
        public string fire = "Fire1";
    }
    [SerializeField]
    public InputSettings input;

    public float lookDistance = 5;
    public float lookSpeed = 5;

    [Header("Aiming Settings")]
    RaycastHit hit;
    public LayerMask aimLayers;
    Ray ray;

    [Header("Spine Settings")]
    public Transform spine;
    public Vector3 spineOffset;

     [Header("Head Rotation Settings")]
     public float lookAtPoint = 2.8f;


    Transform camCenter;
    Transform mainCam;

    public Bow bowScript;
    bool isAiming;
    public bool testAim;


    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<Movement>();
        camCenter = Camera.main.transform.parent;
        mainCam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis(input.forwardInput) != 0 || Input.GetAxis(input.strafeInput) != 0)
            RotateToCamView();
      
        isAiming = Input.GetButton(input.aim);

        if (testAim)
            isAiming = true;

        moveScript.AnimateCharacter(Input.GetAxis(input.forwardInput), Input.GetAxis(input.strafeInput));
        moveScript.SprintCharacter(Input.GetButton(input.sprintInput));
        moveScript.CharacterAim(isAiming);

        if (isAiming)
        {
            Aim();
            moveScript.CharacterPullString(Input.GetButton(input.fire));

        }
            


    }

    void LateUpdate()
    {
        if(isAiming)
            RotateCharacterSpine();
    }

    void RotateToCamView()
    {
        Vector3 camCenterPos = camCenter.position;
        Vector3 lookPoint = camCenterPos + (camCenter.forward * lookDistance);
        Vector3 direction = lookPoint - transform.position;


        Quaternion lookRotation = Quaternion.LookRotation(direction);
        lookRotation.x = 0;
        lookRotation.z = 0;

        Quaternion finalRotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
        transform.rotation = finalRotation;
    }

    //Aiming and sends a ray cast to a target
    void Aim()
    {
        Vector3 camPosition = mainCam.position;
        Vector3 dir = mainCam.forward;
        
        ray = new Ray(camPosition, dir);
        if(Physics.Raycast(ray, out hit, 500f, aimLayers))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            bowScript.ShowCrosshair(hit.point);
        } 
        else
        {
            bowScript.RemoveCrosshair();
        }

    }
    
    void RotateCharacterSpine()
    {
        spine.LookAt(ray.GetPoint(50));
        spine.Rotate(spineOffset);

    }
}
