using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour {

    Rigidbody rigidbody;
    bool addForce = false;

    // Use this for initialization
    void Start () {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (addForce)
        {
            rigidbody.AddForce(Vector3.right * 100, ForceMode.Acceleration);
        }
        
    }

    void OnGUI()
    {
        if (GUILayout.Button("add"))
        {
            addForce = true;
        }
    }
}
