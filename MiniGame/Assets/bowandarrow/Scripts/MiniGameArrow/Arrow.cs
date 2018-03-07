using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGameArrow
{
    public class Arrow : MonoBehaviour
    {

        public enum ArrowState
        {
            preparing,
            flying,
            hitted
        }

        public ArrowState arrowState = ArrowState.preparing;
        
        public float arrowStartX = 0.7f;

        public bool hasHitted = false;

        public GameObject arrowHead;

        public bool addAcceleration = false;

        public Rigidbody rigidbody;

        // Use this for initialization
        void Start()
        {
            arrowHead = gameObject.transform.Find("arrowHead").gameObject;
        }
        
        /// <summary>
        /// 拉弓
        /// </summary>
        public void DrawTheBow(float powerValue)
        {
            Vector3 arrowPosition = transform.localPosition;
            arrowPosition.x = arrowStartX - powerValue;
            transform.localPosition = arrowPosition;
        }

        public void Shoot(float power)
        {
            if (gameObject.GetComponent<Rigidbody>() == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.AddForce(Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)) * new Vector3(25f * power, 0, 0), ForceMode.VelocityChange);
                //Debug.Log("power = " + power);
            }

            lastFramePosition = transform.position;

            arrowState = ArrowState.flying;

            Global.flyTime = 0f;

            GameObject.Find("UI Root").transform.Find("Control - Colored Slider").gameObject.GetComponent<BoxCollider>().enabled = false;

            

            //addAcceleration = true;
        }

        Vector3 lastFramePosition = new Vector3(0, 0, 0);
        Vector3 curFramePosition = new Vector3(0, 0, 0);
        float deltaY = 0f;
        bool listeningDeltaY = true;
        
        // Update is called once per frame
        void FixedUpdate()
        {
            if (arrowState == ArrowState.flying)
            {
                rotateArrow();


                
                if (rigidbody != null && addAcceleration)
                {
                    rigidbody.AddForce(Vector3.right * 15f, ForceMode.Acceleration);
                }

                if (listeningDeltaY)
                {
                    curFramePosition = transform.position;

                    deltaY = curFramePosition.y - lastFramePosition.y;

                    //Debug.Log(deltaY);

                    if (deltaY < 0f)
                    {
                        addAcceleration = true;
                        listeningDeltaY = false;
                        //Debug.Log("addAcceleration");
                    }

                    lastFramePosition = curFramePosition;
                }

                Global.flyTime += 0.02f;
            }



        }

        void rotateArrow()
        {
            if (transform.GetComponent<Rigidbody>() != null)
            {
                // do we fly actually?
                if (GetComponent<Rigidbody>().velocity != Vector3.zero)
                {
                    // get the actual velocity
                    Vector3 vel = GetComponent<Rigidbody>().velocity;
                    // calc the rotation from x and y velocity via a simple atan2
                    float angleZ = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
                    float angleY = Mathf.Atan2(vel.z, vel.x) * Mathf.Rad2Deg;
                    // rotate the arrow according to the trajectory
                    transform.eulerAngles = new Vector3(0, -angleY, angleZ);
                }
            }
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.transform.name == "Cube")
            {
                if (hasHitted)
                {
                    return;
                }
                hasHitted = true;


                if (Global.autoShoot)
                {
                    ShootData shootData = new ShootData { flyTime = Global.flyTime, hitObjectName = other.gameObject.name, hitPoint = other.contacts[0].point, power = Global.power };
                    Global.dataList.Add(shootData);
                    Debug.Log(shootData.ToString());
                }
                
                OnDestroyArrow();
            }

            if (other.transform.name == "level1" || other.transform.name == "level2" || other.transform.name == "level3")
            {
                if (hasHitted)
                {
                    return;
                }
                hasHitted = true;

                if (Global.autoShoot)
                {
                    ShootData shootData = new ShootData { flyTime = Global.flyTime, hitObjectName = other.gameObject.name, hitPoint = other.contacts[0].point, power = Global.power };
                    Global.dataList.Add(shootData);
                    Debug.Log(shootData.ToString());

                    OnDestroyArrow();

                    return;
                }




                arrowHead.SetActive(false);

                GameObject.Find("targetRoot").GetComponent<TargetGenerator>().OnBeHitted();

                //GetComponent<AudioSource>().PlayOneShot(targetHit);
                // set velocity to zero
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                // disable the rigidbody
                GetComponent<Rigidbody>().isKinematic = true;
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                //Debug.Log(gameObject.transform.position);
                //Debug.Log(other.contacts[0].point);
                Debug.Log(other.transform.name);

                hasHitted = true;
            }
        }

        public void OnDestroyArrow()
        {
            Destroy(gameObject);
            MiniGameArrowMain.Instance.arrowManager.GenerateArrow();
        }
        
    }
    

    public class ShootData
    {
        public float power;
        public float flyTime;
        public Vector3 hitPoint;
        public string hitObjectName;

        public override string ToString()
        {
            return string.Format("power = {0};  flyTime = {1};  hitObjectName = {2};  hitPoint = {3};  hitPointX = {4};  hitPointY = {5};  hitPointZ = {6}", power, flyTime, hitObjectName, hitPoint, hitPoint.x, hitPoint.y, hitPoint.z);
        }
    }
}

