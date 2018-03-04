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
                gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().AddForce(Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)) * new Vector3(25f * power, 0, 0), ForceMode.VelocityChange);
                Debug.Log("power = " + power);
            }

            arrowState = ArrowState.flying;
        }

        // Update is called once per frame
        void Update()
        {
            if (arrowState == ArrowState.flying)
            {
                rotateArrow();
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
                Debug.Log(other.contacts[0].point);
                OnDestroyArrow();
            }

            if (other.transform.name == "level1" || other.transform.name == "level2" || other.transform.name == "level3")
            {
                if (hasHitted)
                {
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

                Debug.Log(gameObject.transform.position);
                Debug.Log(other.contacts[0].point);
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
}

