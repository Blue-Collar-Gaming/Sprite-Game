using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentRiders : MonoBehaviour {
    public Transform platformParent;

	// Use this for initialization
	void Start () {
        platformParent = transform.parent;
	}

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = platformParent;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
