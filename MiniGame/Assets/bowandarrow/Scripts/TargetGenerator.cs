using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGenerator : MonoBehaviour {

    public GameObject targetTemplate;
    public GameObject curTarget;

    enum TargetState
    {
        beHitted,
        notBeHitted
    }

    TargetState m_state = TargetState.notBeHitted;

    public float speed = 2f;
    
	// Use this for initialization
	void Start () {


    }
	
    void Update()
    {
        switch (m_state)
        {
            case TargetState.notBeHitted:
                if (curTarget == null)
                {
                    generateTarget();
                }
                else
                {
                    if (curTarget.transform.localPosition.y >= -6.12f)
                    {
                        curTarget.transform.localPosition -= new Vector3(0, speed * Time.deltaTime, 0);
                    }
                    else
                    {
                        Destroy(curTarget);
                    }
                }
                break;

            case TargetState.beHitted:

                break;
        }
        
    }

    void generateTarget()
    {
        curTarget = Instantiate(targetTemplate);
        curTarget.name = "curTarget";
        curTarget.transform.parent = GameObject.Find("targetRoot").transform;
        curTarget.transform.localPosition = new Vector3(0, 6.07f, 0);      //6.07    ~    -6.12
    }

    public void OnBeHitted()
    {
        m_state = TargetState.beHitted;
    }

    public void Reset()
    {
        if (curTarget != null)
        {
            Destroy(curTarget);
        }

        m_state = TargetState.notBeHitted;
    }
}
