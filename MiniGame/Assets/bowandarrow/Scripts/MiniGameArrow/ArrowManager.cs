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
            int index = 0;
            float pValue = powerSlider.value;
            for (int i = 0, length = Global.dataListFromConfig.Count; i < length; i++)
            {
                if (Mathf.Abs(pValue - Global.dataListFromConfig[i].power) < 0.01f)
                {
                    shootData = Global.dataListFromConfig[i];
                    index = i;
                    break;
                }
            }

            //hitPoint = GameObject.Instantiate(hitPointPrefab, shootData.hitPoint, Quaternion.identity) as GameObject;
            //Debug.Log(shootData.ToString());

            //计算靶子位移
            Vector3 targetEndPosition = GameObject.Find("targetRoot").GetComponent<TargetGenerator>().SetOverTarget(shootData.flyTime);

            //计算delta y
            float deltaY = shootData.hitPoint.y - targetEndPosition.y;

            //判断是否命中
            float realPower = powerSlider.value;

            if (Global.needControlResult)
            {
                realPower = findExactlyMissPower(targetEndPosition, index);

                //if (deltaY >= 0 && deltaY <= 1.2f)
                //{
                //    realPower = findExactlyMissPower_top(targetEndPosition, index);
                //    Debug.Log("可击中上半部！;;deltaY=" + deltaY + ";;powerSlider.value=" + powerSlider.value + ";;realPower=" + realPower);
                //    GameObject.Find("targetRoot").GetComponent<TargetGenerator>().HideAllCollider();
                //}
                //else if (deltaY < 0 && deltaY >= -1.2f)
                //{
                //    realPower -= 0.04f;
                //    Debug.Log("可击中下半部！;;deltaY=" + deltaY + ";;powerSlider.value=" + powerSlider.value + ";;realPower=" + realPower);
                //    GameObject.Find("targetRoot").GetComponent<TargetGenerator>().HideAllCollider();
                //}
                //else
                //{
                //    Debug.Log("deltaY=" + deltaY);
                //}
            }
            
            arrow.GetComponent<Arrow>().Shoot(realPower);
        }

        
        /// <summary>
        /// 如果提前预知可命中，则由近及远依次计算其临近索引的射击数据，找到刚好脱靶的数值
        /// </summary>
        float findExactlyMissPower(Vector3 targetEndPosition, int shootIndex)
        {
            float finalPower = powerSlider.value;
            
            float curDeltaY = calculateDeltaY(targetEndPosition, shootIndex);

            if (curDeltaY >= 0 && curDeltaY <= 1.3f)        //可命中上半部
            {
                //Debug.Log("可击中上半部！;;deltaY=" + curDeltaY + ";;  powerSlider.value=" + powerSlider.value);
                for (int i = shootIndex, length = Global.dataListFromConfig.Count; i < length; i++)
                {
                    //Debug.Log("try power: " + Global.dataListFromConfig[i].power);
                    float deltaY = calculateDeltaY(targetEndPosition, i);
                    if (deltaY > 1.3f)
                    {
                        finalPower = Global.dataListFromConfig[i].power;
                        //Debug.Log("final power: " + finalPower + ";; final deltaY: " + deltaY);
                        break;
                    }
                }
            }
            else if (curDeltaY < 0 && curDeltaY >= -1.3f)       //可命中下半部
            {
                //Debug.Log("可击中下半部！;;deltaY=" + curDeltaY + ";;  powerSlider.value=" + powerSlider.value);
                for (int i = shootIndex; i >= 0; i--)
                {
                    //Debug.Log("try power: " + Global.dataListFromConfig[i].power);
                    float deltaY = calculateDeltaY(targetEndPosition, i);
                    if (deltaY < -1.3f)
                    {
                        finalPower = Global.dataListFromConfig[i].power;
                        //Debug.Log("final power: " + finalPower + ";; final deltaY: " + deltaY);
                        break;
                    }
                }
            }

            return finalPower;
        }

        float findExactlyMissPower_top(Vector3 targetEndPosition, int shootIndex)
        {
            float finalPower = powerSlider.value;

            for (int i = shootIndex, length = Global.dataListFromConfig.Count; i < length; i++)
            {
                Debug.Log("try power: " + Global.dataListFromConfig[i].power);
                float deltaY = calculateDeltaY(targetEndPosition, i);
                if (deltaY > 1.2f)
                {
                    finalPower = Global.dataListFromConfig[i].power;
                    break;
                }
            }

            return finalPower;
        }

        float calculateDeltaY(Vector3 targetEndPosition, int shootIndex)
        {
            Vector3 hitPoint = Global.dataListFromConfig[shootIndex].hitPoint;
            return hitPoint.y - targetEndPosition.y;
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
