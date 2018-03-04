using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {

    LineRenderer bowStringLinerenderer;
    UISlider powerSlider;
    List<Vector3> bowStringPosition;

    // Use this for initialization
    void Start () {
        bowStringLinerenderer = gameObject.transform.Find("bowString").gameObject.AddComponent<LineRenderer>();
        bowStringLinerenderer.SetVertexCount(3);
        bowStringLinerenderer.SetWidth(0.05F, 0.05F);
        bowStringLinerenderer.useWorldSpace = false;
        bowStringLinerenderer.material = Resources.Load("Materials/bowStringMaterial") as Material;
        bowStringPosition = new List<Vector3>();
        bowStringPosition.Add(new Vector3(-0.44f, 1.43f, 2f));
        bowStringPosition.Add(new Vector3(-0.44f, -0.06f, 2f));
        bowStringPosition.Add(new Vector3(-0.43f, -1.32f, 2f));
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, bowStringPosition[1]);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);

        powerSlider = GameObject.Find("UI Root/Control - Colored Slider").GetComponent<UISlider>();
        powerSlider.onChange.Add(new EventDelegate(onPowerSliderChangeValue));
        powerSlider.onDragFinished += onPowerSliderDragFinished;
    }

    void onPowerSliderChangeValue()
    {
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, new Vector3(-(0.44f + powerSlider.value), -0.06f, 2f));
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);
    }

    void onPowerSliderDragFinished()
    {
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, bowStringPosition[1]);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
