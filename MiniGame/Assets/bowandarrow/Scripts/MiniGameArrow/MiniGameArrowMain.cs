using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MiniGameArrow
{
    public class MiniGameArrowMain : MonoBehaviour
    {
        static MiniGameArrowMain instance;
        public static MiniGameArrowMain Instance
        {
            get {
                if (instance ==null)
                {
                    instance = GameObject.Find("Main").GetComponent<MiniGameArrowMain>();
                }

                return instance;
            }
        }

        Action update;

        public enum GameState
        {
            Menu,
            Game
        }

        public GameState gameState = GameState.Menu;

        public GameObject bow;

        public ArrowManager arrowManager;
        
        // Use this for initialization
        void Start()
        {
            init();

            bow = GameObject.Find("bow");
            arrowManager = new ArrowManager(bow);
        }

        void init()
        {
            TextAsset txtFile = Resources.Load("Configs/shootdata") as TextAsset;
            if (txtFile != null)
            {
                string[] dataArr = txtFile.text.Split('^');
                foreach (string data in dataArr)
                {
                    if (data.Length < 6)
                    {
                        continue;
                    }
                    string[] itemArr = data.Split('&');
                    float flyTime = float.Parse(itemArr[0]);
                    float power = float.Parse(itemArr[1]);
                    string hitObjectName = itemArr[2];
                    float x = float.Parse(itemArr[3]);
                    float y = float.Parse(itemArr[4]);
                    float z = float.Parse(itemArr[5]);
                    Global.dataListFromConfig.Add(new ShootData { flyTime = flyTime, power = power, hitObjectName = hitObjectName, hitPoint = new Vector3(x, y, z) });
                }
            }

            foreach (ShootData item in Global.dataListFromConfig)
            {
                Debug.Log(item.ToString());
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (update != null)
            {
                update();
            }
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(100, 100, 150, 150), "auto shoot"))
            {
                autoShoot();
            }

            if (GUI.Button(new Rect(260, 100, 150, 150), "save data"))
            {
                string content = "";
                for (int i = 0, length = Global.dataList.Count; i < length; i++)
                {

                    content += 
                        Global.dataList[i].flyTime + "&" + 
                        Global.dataList[i].power + "&" + 
                        Global.dataList[i].hitObjectName + "&" + 
                        Global.dataList[i].hitPoint.x + "&" +
                        Global.dataList[i].hitPoint.y + "&" +
                        Global.dataList[i].hitPoint.z + "&" +
                        "^";

                    StreamWriter sw = new StreamWriter(@"E:\GitSpace\MyUintTest\MiniGame\Assets\bowandarrow\Resources\Configs\shootdata.txt");
                    sw.Write(content);
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        void autoShoot()
        {
            arrowManager.isAutoShoot = true;

            if (arrowManager.powerSlider.onDragFinished != null)
            {
                arrowManager.powerSlider.onDragFinished();
            }
            
        }
    }
}


