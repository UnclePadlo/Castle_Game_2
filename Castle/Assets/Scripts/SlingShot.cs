using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    static private SlingShot S;
    
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocity = 8f;


    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigitbody;

    static public Vector3 LAUNCH_POS 
    {
        get 
        {
            if(S == null)
            {
                return Vector3.zero;
            }
            return S.launchPos;
                } 
    }

    private void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    private void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }
    private void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }
    private void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile);
        projectile.transform.position = launchPos;
        projectileRigitbody = projectile.GetComponent<Rigidbody>();
        projectileRigitbody.isKinematic= true;
    }
    private void Update() 
    {
        if (!aimingMode) { return; }
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMeghitude = this.GetComponent<SphereCollider>().radius;

        if(mouseDelta.magnitude > maxMeghitude) 
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMeghitude;
        }
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if(Input.GetMouseButtonUp(0)) 
        {
            aimingMode= false;
            projectileRigitbody.isKinematic = false;
            projectileRigitbody.velocity = -mouseDelta * velocity;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }
}
