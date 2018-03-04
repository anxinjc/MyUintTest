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

        public Arrow arrow;


        public Canvas menuCanvas;
        public Canvas instructionsCanvas;
        public Canvas highscoreCanvas;
        public Canvas gameCanvas;
        public Canvas gameOverCanvas;

        // Use this for initialization
        void Start()
        {

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
            //if (GUI.Button(new Rect(100, 260, 150, 150), "play the game"))
            //{
            //    startGame();
            //}
        }

        void startGame()
        {
            menuCanvas.enabled = false;
            highscoreCanvas.enabled = false;
            instructionsCanvas.enabled = false;
            gameCanvas.enabled = true;

            //arrowPrepared = false;

            //gameState = GameStates.game;
        }
    }
}


