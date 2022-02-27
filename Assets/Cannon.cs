using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject CannonBallPrefab;
    public Transform FirePoint;
    public float FieldOfViewDegrees;
    public float VisibilityDistance;
    public float FireRate = 1.0f;

    private float _lastShot;

    // Update is called once per frame
    void Update()
    {
        var closestTarget = FindClosestTarget();
        
        Vector3 rayDirectionFromCannon = closestTarget.transform.position - transform.position;
        Vector3 rayDirectionFromFirePoint = closestTarget.transform.position - FirePoint.transform.position;
 
        if ((Vector3.Angle(rayDirectionFromCannon, transform.forward)) <= FieldOfViewDegrees * 0.5f && Time.time > FireRate + _lastShot)
        {
            RaycastHit hit;
            if (Physics.Raycast(FirePoint.transform.position, rayDirectionFromFirePoint, out hit, VisibilityDistance))
            {
                Debug.Log("Can see target");
 
                Vector3 targetInitialVelocity = CalculateVelocity(hit.point,FirePoint.position, 1f);

                transform.rotation = Quaternion.LookRotation(targetInitialVelocity);
                
                GameObject cannonBall = Instantiate(
                    CannonBallPrefab,
                    FirePoint.position,
                    Quaternion.identity);
                Rigidbody target = cannonBall.GetComponent<Rigidbody>();
                target.velocity = targetInitialVelocity;
                
                _lastShot = Time.time;
            }
        }
        
    }
    private GameObject FindClosestTarget()
    {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Target");
        GameObject closestObject = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gameObjects)
        {
            Vector3 difference = go.transform.position - position;
            float currentDistance = difference.sqrMagnitude;
            if (currentDistance < distance)
            {
                closestObject = go;
                distance = currentDistance;
            }
        }
        return closestObject;
    }
    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        //define the distance x and y
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.Normalize();
        distanceXZ.y = 0;
        
        //creating a float that represents distance 
        float sY = distance.y;
        float sXZ = distance.magnitude;
        
        //calculating initial x velocity Vx = x / t
        float vXZ = sXZ / time;
        
        //calculating initial y velocity vY0 = y/t + 1/2 * g * t
        float vY = sY / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;
        Vector3 result = distanceXZ * vXZ;
        result.y = vY;
        return result;
    }   
}
