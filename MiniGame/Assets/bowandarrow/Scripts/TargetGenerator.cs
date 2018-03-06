using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGameArrow
{
    public class TargetGenerator : MonoBehaviour
    {

        public GameObject targetTemplate;
        public GameObject curTarget;

        public GameObject startPoint;
        public GameObject endPoint;

        public GameObject autoShootTarget;

        bool gameStarted = false;


        enum TargetState
        {
            beHitted,
            notBeHitted
        }

        TargetState m_state = TargetState.notBeHitted;

        public float speed = 2f;

        // Use this for initialization
        void Start()
        {
            startPoint = Resources.Load("Prefabs/startPoint") as GameObject;
            endPoint = Resources.Load("Prefabs/endPoint") as GameObject;

            autoShootTarget = transform.Find("target 1").gameObject;
            
            MiniGameArrowMain.Instance.startGame += startGame;
        }

        void startGame()
        {
            init();

            gameStarted = true;
        }

        void init()
        {
            if (Global.autoShoot)
            {
                autoShootTarget.SetActive(true);
            }
            else
            {
                autoShootTarget.SetActive(false);
            }


        }

        void FixedUpdate()
        {
            if (!gameStarted)
            {
                return;
            }

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


        Vector3 startPosition;
        Vector3 endPosition;
        public Vector3 SetOverTarget(float time)
        {
            startPosition = curTarget.transform.position;
            endPosition = curTarget.transform.position - time / 0.02f * new Vector3(0, speed * 0.02f, 0);

            //Instantiate(startPoint, startPosition, Quaternion.identity);
            //Instantiate(endPoint, endPosition, Quaternion.identity);

            return endPosition;
        }

        public void HideAllCollider()
        {
            curTarget.transform.Find("level1").gameObject.GetComponent<BoxCollider>().enabled = false;
            curTarget.transform.Find("level2").gameObject.GetComponent<BoxCollider>().enabled = false;
            curTarget.transform.Find("level3").gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

}
