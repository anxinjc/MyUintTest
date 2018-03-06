using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGameArrow
{
    public class ArrowManager
    {
        GameObject bow;
        GameObject arrowPrefab;
        GameObject arrow;
        GameObject hitPointPrefab;
        GameObject hitPoint;

        //蓄力条
        public UISlider powerSlider;

        public Action onGenerateArrow;

        float autoPower = 0f;       //自动射击时的power值

        public ArrowManager(GameObject bow)
        {
            this.bow = bow;

            init();

            MiniGameArrowMain.Instance.startGame += startGame;
        }

        void init()
        {
            hitPointPrefab = Resources.Load("Prefabs/hitPoint") as GameObject;

            arrowPrefab = Resources.Load("Prefabs/arrow") as GameObject;
            powerSlider = GameObject.Find("UI Root/Control - Colored Slider").GetComponent<UISlider>();
            powerSlider.onChange.Add(new EventDelegate(onPowerSliderChangeValue));
            
            if (arrow == null)
            {
                GenerateArrow();
            }

            powerSlider.gameObject.SetActive(false);
        }

        void startGame()
        {
            powerSlider.gameObject.SetActive(true);

            //自动射击
            if (Global.autoShoot)
            {
                powerSlider.onDragFinished += onAutoShoot;

                onGenerateArrow += () => {
                    if (autoPower < 1f) powerSlider.onDragFinished();
                };

                powerSlider.onDragFinished();
            }
            else
            {
                powerSlider.onDragFinished += onPowerSliderDragFinished;
            }
        }

        void onPowerSliderChangeValue()
        {
            arrow.GetComponent<Arrow>().DrawTheBow(powerSlider.value);
        }

        void onPowerSliderDragFinished()
        {
            //arrow.GetComponent<Arrow>().Shoot(powerSlider.value);

            Global.power = powerSlider.value;

            //提前生成落点
            ShootData shootData = new ShootData();
            float pValue = powerSlider.value;
            foreach (ShootData item in Global.dataListFromConfig)
            {
                if (Mathf.Abs(pValue - item.power) < 0.01f)
                {
                    shootData = item;
                }
            }

            Debug.Log(hitPointPrefab == null);
            //hitPoint = GameObject.Instantiate(hitPointPrefab, shootData.hitPoint, Quaternion.identity) as GameObject;
            Debug.Log(shootData.ToString());

            //计算靶子位移
            Vector3 targetEndPosition = GameObject.Find("targetRoot").GetComponent<TargetGenerator>().SetOverTarget(shootData.flyTime);

            //计算delta y
            float deltaY = shootData.hitPoint.y - targetEndPosition.y;

            //判断是否命中
            float realPower = powerSlider.value;

            if (Global.needControlResult)
            {
                if (deltaY >= 0 && deltaY <= 1.2f)
                {
                    realPower += 0.04f;
                    //realPower += 1.13f - deltaY + 0.01f;
                    Debug.Log("可击中上半部！;;deltaY=" + deltaY + ";;powerSlider.value=" + powerSlider.value + ";;realPower=" + realPower);
                    GameObject.Find("targetRoot").GetComponent<TargetGenerator>().HideAllCollider();
                }
                else if (deltaY < 0 && deltaY >= -1.2f)
                {
                    realPower -= 0.04f;
                    //realPower -= (1.13f + deltaY + 0.01f);
                    Debug.Log("可击中下半部！;;deltaY=" + deltaY + ";;powerSlider.value=" + powerSlider.value + ";;realPower=" + realPower);
                    GameObject.Find("targetRoot").GetComponent<TargetGenerator>().HideAllCollider();
                }
                else
                {
                    Debug.Log("deltaY=" + deltaY);
                }
            }
            
            arrow.GetComponent<Arrow>().Shoot(realPower);
        }
        
        
        void onAutoShoot()
        {
            powerSlider.value = autoPower;
            arrow.GetComponent<Arrow>().Shoot(autoPower);
            Global.power = autoPower;
            autoPower += 0.01f;
        }
        
        public void GenerateArrow()
        {
            arrow = GameObject.Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            arrow.name = "arrow";
            arrow.transform.parent = bow.transform;

            arrow.transform.localScale = new Vector3(1, 1, 1);
            arrow.transform.localPosition = new Vector3(0.7f, 0, 0);
            arrow.transform.localEulerAngles = new Vector3(0, 0, 0);

            if (onGenerateArrow != null)
            {
                onGenerateArrow();
            }

            powerSlider.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

}
