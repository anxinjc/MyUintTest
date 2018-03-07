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

        public Action fixedUpdate;
        public Action startGame;

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
            readConfig();
        }

        void readConfig()
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
        void FixedUpdate()
        {
            if (fixedUpdate != null)
            {
                fixedUpdate();
            }
        }

        string autoShoot = Global.autoShoot.ToString();
        string needControlResult = Global.needControlResult.ToString();

        void OnGUI()
        {
            if (GUI.Button(new Rect(100, 100, 150, 100), "自动射击: " + autoShoot))
            {
                Global.autoShoot = !Global.autoShoot;
                autoShoot = Global.autoShoot.ToString();
            }

            if (!Global.autoShoot)
            {
                if (GUI.Button(new Rect(420, 100, 150, 100), "不可击中: " + needControlResult))
                {
                    Global.needControlResult = !Global.needControlResult;
                    needControlResult = Global.needControlResult.ToString();
                }
            }

            if (GUI.Button(new Rect(260, 100, 150, 100), "start game"))
            {
                if (startGame != null)
                {
                    startGame();
                }
            }

            if (GUI.Button(new Rect(100, 210, 150, 100), "save data"))
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

            if (GUI.Button(new Rect(260, 210, 150, 100), "mark all hitPoints"))
            {
                GameObject hitPointPrefab = Resources.Load("Prefabs/hitPoint") as GameObject;
                //for (int i = 0, length = Global.dataList.Count; i < length; i++)
                //{
                //    Instantiate(hitPointPrefab, Global.dataList[i].hitPoint, Quaternion.identity);
                //}

                for (int i = 0, length = Global.dataListFromConfig.Count; i < length; i++)
                {
                    Instantiate(hitPointPrefab, Global.dataListFromConfig[i].hitPoint, Quaternion.identity);
                }
            }
        }

        //void autoShoot()
        //{
        //    arrowManager.isAutoShoot = true;

        //    if (arrowManager.powerSlider.onDragFinished != null)
        //    {
        //        arrowManager.powerSlider.onDragFinished();
        //    }
            
        //}
    }
}


