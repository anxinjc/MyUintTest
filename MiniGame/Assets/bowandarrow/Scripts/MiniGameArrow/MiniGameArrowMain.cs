using System;
using System.Collections;
using System.Collections.Generic;
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
            bow = GameObject.Find("bow");
            arrowManager = new ArrowManager(bow);
        }

        void init()
        {
            
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
                startGame();
            }
        }

        void startGame()
        {
            arrowManager.isAutoShoot = true;

            if (arrowManager.powerSlider.onDragFinished != null)
            {
                arrowManager.powerSlider.onDragFinished();
            }
            
        }
    }
}


