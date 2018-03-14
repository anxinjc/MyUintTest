using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigidbody2DTest : MonoBehaviour {

    Rigidbody2D m_kRigidbody2D;
    bool m_bAddForce = false;

    // Use this for initialization
    void Start () {
        m_kRigidbody2D = gameObject.GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (m_bAddForce)
        {
            m_kRigidbody2D.AddForce(Vector3.right * 10, ForceMode2D.Force);
        }


        
    }

    void OnGUI()
    {
        if (GUILayout.Button("hit"))
        {
            //m_bAddForce = true;
            m_kRigidbody2D.AddForce(Vector3.right * 10, ForceMode2D.Impulse);
            m_kRigidbody2D.gravityScale = 1;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        m_kRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        //Destroy(m_kRigidbody2D);
    }
}
