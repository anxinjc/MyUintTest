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

        //蓄力条
        public UISlider powerSlider;
        
        public ArrowManager(GameObject bow)
        {
            this.bow = bow;

            init();
        }

        void init()
        {
            arrowPrefab = Resources.Load("Prefabs/arrow") as GameObject;
            powerSlider = GameObject.Find("UI Root/Control - Colored Slider").GetComponent<UISlider>();
            powerSlider.onChange.Add(new EventDelegate(onPowerSliderChangeValue));
            //powerSlider.onDragFinished += onPowerSliderDragFinished;
            powerSlider.onDragFinished += onAutoShoot;

            if (arrow == null)
            {
                GenerateArrow();
            }
        }

        void onPowerSliderChangeValue()
        {
            arrow.GetComponent<Arrow>().DrawTheBow(powerSlider.value);
        }

        void onPowerSliderDragFinished()
        {
            arrow.GetComponent<Arrow>().Shoot(powerSlider.value);
        }


        float autoPower = 0f;
        void onAutoShoot()
        {
            powerSlider.value = autoPower;
            arrow.GetComponent<Arrow>().Shoot(autoPower);
            autoPower += 0.01f;
        }

        public bool isAutoShoot = false;

        public void GenerateArrow()
        {
            arrow = GameObject.Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            arrow.name = "arrow";
            arrow.transform.parent = bow.transform;

            arrow.transform.localScale = new Vector3(1, 1, 1);
            arrow.transform.localPosition = new Vector3(0.7f, 0, 0);
            arrow.transform.localEulerAngles = new Vector3(0, 0, 0);

            if (isAutoShoot && autoPower < 0.2f)
            {
                powerSlider.onDragFinished();
            }
        }
    }

}
