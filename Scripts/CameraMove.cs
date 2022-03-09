using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    #region Singleton

    public static CameraMove Instance {get; private set;}

    private void Awake()
    {

        if(Instance==null)
        {
            Instance=this;
        }

        else if(Instance !=this)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    [SerializeField] private float minDistanceForZoom=10f;
    [SerializeField] private float maxPossibleDistance=50f;
    [SerializeField] private float smoothing=0.5f;
    [SerializeField] private float minY=10f;
    [SerializeField] private float maxY=50f;

    [SerializeField] private List<Transform> targets=new List<Transform>();

    private Vector3 velocity;

    private void LateUpdate()
    {

        if (targets.Count==0)
        {
            return;            
        }

        Move();
        Zoom();
    }

    private void Move()
    {

        Vector3 centerPoint=GetCenterPoint();
        centerPoint.y=transform.position.y;

        transform.position=Vector3.SmoothDamp(transform.position, centerPoint, ref velocity, smoothing);

    }

    private void Zoom()
    {

        float greatestDistance=GetGreatestDistance();

        if(greatestDistance<minDistanceForZoom)
        {
            greatestDistance=0f;
        }

        float newY=Mathf.Lerp(minY, maxY, greatestDistance/maxPossibleDistance);

        transform.position=new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, newY, Time.deltaTime), transform.position.z);

    }

    private float GetGreatestDistance()
    {
        Bounds bounds = EncapsulateTargets();

        return bounds.size.x>bounds.size.z?bounds.size.x:bounds.size.z;
    }

    private Vector3 GetCenterPoint()
    {
        if (targets.Count==1)
        {
            return targets[0].position;
        }

        Bounds bounds=EncapsulateTargets();

        Vector3 center=bounds.center;

        return center;
    } 

    private Bounds EncapsulateTargets() 
    {

        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);

        foreach(Transform target in targets)
        {
            bounds.Encapsulate(target.position);
        }

        return bounds;
    }

}
