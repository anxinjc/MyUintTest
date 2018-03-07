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

        //箭靶移动范围
        Vector3 m_startPosition = new Vector3(0, 0, 0);
        Vector3 m_endPosition = new Vector3(0, 0, 0);


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

            findTargetMoveRange();
        }

        void findTargetMoveRange()
        {
            GameObject g = new GameObject("g_start");
            g.transform.parent = GameObject.Find("targetRoot").transform;

            g.transform.localPosition = new Vector3(0, 6.07f, 0);      //6.07    ~    -6.12
            m_startPosition = g.transform.position;
            
            g.transform.localPosition = new Vector3(0, -6.12f, 0);
            m_endPosition = g.transform.position;

            Destroy(g);
        }

        void FixedUpdate()
        {
            if (!gameStarted)
            {
                return;
            }

            if (Global.autoShoot)
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
            if (curTarget == null)
            {
                startPosition = m_startPosition;
            }
            else
            {
                startPosition = curTarget.transform.position;
            }

            endPosition = startPosition - time / 0.02f * new Vector3(0, speed * 0.02f, 0);

            //如果结束点超出下限，则从起点重新累加
            if (endPosition.y <= m_endPosition.y)
            {
                Vector3 delta = m_endPosition - endPosition;
                endPosition = m_startPosition - delta;
            }

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
