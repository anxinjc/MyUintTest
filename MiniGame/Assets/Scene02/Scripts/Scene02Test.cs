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

    int i = 0;
    float time = 0;
    bool hasShowY = false;

	// Update is called once per frame
	void FixedUpdate () {

        //i++;
        //time = Time.deltaTime;
        //Debug.Log("这是第" + i + "帧");
        //Debug.Log("时间为:" + time);

        if (isUseG)
        {
            arrow.transform.localPosition = new Vector3(sr, h, 0);
            v0 += g * Time.deltaTime;
            h += v0 * Time.deltaTime + 0.5f * g * Time.deltaTime * Time.deltaTime;

            //sr += vr * Time.deltaTime;
            //speed += -0.981f * Time.deltaTime;
            
        }

        i++;
        if (i >= 250 && !hasShowY)
        {
            Debug.Log(arrow.transform.localPosition.y);
            hasShowY = true;
        }
	}

    IEnumerator showPosition()
    {
        yield return new WaitForSeconds(5);
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
            
        }
    }

    void calculateResult()
    {
        float s = v0 * 5f + 0.5f * g * 25f;
        Debug.Log("s = " + s);
    }
}
