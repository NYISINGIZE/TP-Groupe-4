using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    [System.Serializable]
    public class BowSettings
    {
        [Header("Arrow Settings")]
        public float arrowCount;
        public GameObject arrowPrefab;
        public Transform arrowPos;

        [Header("Bow Equip and UnEquip Settings")]
        public Transform EquipPos;
        public Transform UnEquipPos;
        public Transform UnEquipParent;
        public Transform EquipParent;

        [Header("Bow String Settings")]
        public Transform bowSettings;
        public Transform stringInitialPos;
        public Transform stringHandPullPos;
        public Transform stringInitialParent;
    }
    [SerializeField]
    public BowSettings bowSettings;

    [Header("Crosshair Settings")]

    public GameObject crossHairPrefab;
    GameObject currentCrossHair;
    bool canPullString = false;
    bool canFireArrow = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void EquipBow()
    {
        this.transform.position = bowSettings.EquipPos.position;
        this.transform.rotation = bowSettings.EquipPos.rotation;
        this.transform.parent = bowSettings.EquipParent;
    }

    void UnEquipBow()
    {
        this.transform.position = bowSettings.UnEquipPos.position;
        this.transform.rotation = bowSettings.UnEquipPos.rotation;
        this.transform.parent = bowSettings.UnEquipParent;
    }

    public void ShowCrosshair(Vector3 crosshairPos)

    {
        if(!currentCrossHair)
        currentCrossHair = Instantiate(crossHairPrefab) as GameObject;
        currentCrossHair.transform.position = crosshairPos;
        currentCrossHair.transform.LookAt(Camera.main.transform);

    }
    
    public void RemoveCrosshair()
    {
        if(currentCrossHair)
            Destroy(currentCrossHair);

    }

}
