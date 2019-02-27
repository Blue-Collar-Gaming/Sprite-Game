using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideablePlatform : MonoBehaviour {
    public float zTravelDistance = 5.5f;
    public float traversalTime = 10.0f;
    float traversedZdistance = 0.0f;

    public float traversalSpeedModifier = -1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if((zTravelDistance - traversedZdistance) < (Time.deltaTime / traversalTime))
        {
            transform.Translate(0, 0, (zTravelDistance - traversedZdistance) * traversalSpeedModifier);
            traversalSpeedModifier *= -1;
            traversedZdistance = 0;
        } else
        {
            transform.Translate(0, 0, (Time.deltaTime / traversalTime) * traversalSpeedModifier);
            traversedZdistance += (Time.deltaTime/traversalTime);
        }
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.parent = gameObject.transform;
        Debug.Log("Entered");
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.parent = null;
    }
}
