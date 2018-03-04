using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene02Test : MonoBehaviour {

    public GameObject arrow;

	// Use this for initialization
	void Start () {
        arrow = GameObject.Find("arrow");
	}


    bool isUseG = false;
    float speed = 0;
    float v0 = 100;
    float h = 0;
    float g = -9.81f * 30;

    float vr = 100;
    float sr = 0;
	// Update is called once per frame
	void Update () {
        if (isUseG)
        {
            arrow.transform.localPosition = new Vector3(sr, h, 0);
            v0 += g * Time.deltaTime;
            h += v0 * Time.deltaTime + 0.5f * g * Time.deltaTime * Time.deltaTime;

            sr += vr * Time.deltaTime;
            //speed += -0.981f * Time.deltaTime;


        }
	}

    IEnumerator showPosition()
    {
        yield return new WaitForSeconds(3);
        Debug.Log(arrow.transform.localPosition.y);
        
    }

    void OnGUI()
    {
        if (GUILayout.Button("shoot"))
        {
            isUseG = true;
            calculateResult();

            StartCoroutine(showPosition());
            //Invoke("showPosition", 3);
            //shoot();
        }
    }

    void shoot()
    {
        if (arrow.GetComponent<Rigidbody>() == null)
        {
            arrow.AddComponent<Rigidbody>();
            //arrow.transform.GetComponent<Rigidbody>().useGravity = false;
            arrow.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
            //arrow.transform.parent = gameManager.transform;
            //arrow.GetComponent<Rigidbody>().AddForce(Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)) * new Vector3(25f * 0.1f, 0, 0), ForceMode.VelocityChange);

            calculateResult();

            Invoke("showPosition", 1);
        }
    }

    void calculateResult()
    {
        float s = v0 * 3 + 0.5f * g * 9f;
        Debug.Log("s = " + s);
    }
}
