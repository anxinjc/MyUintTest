using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public GameObject progressBar;
    UISlider slider;
    bool isSliderGrow = true;
    
    public GameObject stayArrow;
    public GameObject flyArrow;

    public GameObject diamond;

    public bool isAllowHit = false;

    void Start () {
        progressBar = GameObject.Find("UI Root/Progress Bar");
        slider = progressBar.GetComponent<UISlider>();
        slider.value = 0;

        stayArrow = GameObject.Find("UI Root/stayArrow");
        stayArrow.SetActive(true);
        flyArrow = GameObject.Find("UI Root/flyArrow");
        flyArrow.SetActive(false);

        diamond = GameObject.Find("UI Root/target/diamond");

        //refreshAward();
    }
	
	void Update () {
        if (Input.GetMouseButton(0))
        {
            if (!stayArrow.GetComponent<Animation>().isPlaying)
            {
                stayArrow.GetComponent<Animation>().Play();
            }

            if (isSliderGrow)
            {
                slider.value += Time.deltaTime * 1.5f;
                if (slider.value >= 1.0f)
                {
                    isSliderGrow = false;
                }
            }
            else
            {
                slider.value -= Time.deltaTime * 1.5f;
                if (slider.value <= 0.0f)
                {
                    isSliderGrow = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (stayArrow.GetComponent<Animation>().isPlaying)
            {
                stayArrow.GetComponent<Animation>().Stop();
                stayArrow.transform.localPosition = new Vector3(0, -243, 0);
                shoot();
            }

            Debug.Log("Shoot!");
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            StartCoroutine(refreshAwardInCoroutine());
        }
        
	}

    void shoot()
    {
        flyArrow.SetActive(true);
        TweenPosition tp = flyArrow.GetComponent<TweenPosition>();
        tp.from = new Vector3(0, -243, 0);
        tp.to = new Vector3(0, 257, 0);
        Vector3 from = tp.from;
        Vector3 to = tp.to;

        HitTimeRange hitTimeRange = getCurHitTimeRange();

        //set speed
        //Debug.Log(slider.value);
        float duration = 2.0f - (2.0f - 0.5f) * slider.value;

        if (!isAllowHit)
        {
            if (diamond.transform.localPosition.x < 0)
            {
                hitTimeRange = getCurHitTimeRange();
                if (duration >= hitTimeRange.min && duration <=hitTimeRange.max)
                {
                    duration = hitTimeRange.min - 0.1f;

                    Debug.Log("duration : " + duration);
                }
            }
        }

        tp.duration = duration;

        //StartCoroutine(executeAfterSecs(duration, ()=> {
        //    tp.from = to;
        //    tp.to = to + (to - from);

        //    tp.ResetToBeginning();
        //    tp.PlayForward();
        //}));

        EventDelegate.Add(tp.onFinished, () =>
        {

            flyArrow.SetActive(false);

        });
        tp.ResetToBeginning();
        tp.PlayForward();
    }

    IEnumerator executeAfterSecs(float sec, Action callBack)
    {
        yield return new WaitForSeconds(sec);

        if (callBack != null)
        {
            callBack();
        }
    }



    void refreshAward()
    {

        StartCoroutine(refreshAwardInCoroutine());
        
    }

    IEnumerator refreshAwardInCoroutine()
    {
        TweenPosition tp = diamond.GetComponent<TweenPosition>();

        while (true)
        {
            float duration = UnityEngine.Random.Range(0.2f, 2f);
            tp.duration = duration * 2;
            tp.ResetToBeginning();
            tp.PlayForward();

            yield return new WaitForSeconds(duration * 2);
        }
    }

    HitTimeRange getCurHitTimeRange()
    {
        float awardCurX = Mathf.Abs(diamond.transform.localPosition.x);
        float awardCurSpeed = 438f / diamond.GetComponent<TweenPosition>().duration / 2;

        Debug.Log("awardCurX : " + awardCurX);
        Debug.Log("awardCurSpeed : " + awardCurSpeed);

        HitTimeRange hitTimeRange = new HitTimeRange();
        hitTimeRange.min = (awardCurX - 50) / awardCurSpeed;
        hitTimeRange.max = (awardCurX + 50) / awardCurSpeed;

        //float hitTimeMin = (awardCurX - 50) / awardCurSpeed;
        //float hitTimeMax = (awardCurX + 50) / awardCurSpeed;

        Debug.Log("min : " + hitTimeRange.min + "    max : " + hitTimeRange.max);

        return hitTimeRange;
    }


    void OnGUI()
    {
        

    }
}

struct HitTimeRange
{
    public float min;
    public float max;
}
