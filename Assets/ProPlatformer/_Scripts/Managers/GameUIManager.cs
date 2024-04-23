using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Myd.Platform
{
    public class GameUIManager : UnitySingleton<GameUIManager>
    {
        private void Start()
        {
            timeCountDown=timeMax;
            
            YTEventManager.Instance.AddEventListener(EventStrings.GAME_OVER, GameOver);
        }

        private void OnDestroy()
        {
            YTEventManager.Instance.RemoveEventListener(EventStrings.GAME_OVER, GameOver);
        }

        public GameObject pausePanel;
        public GameObject settingsPanel;

        public void TimePause()
        {
            YTEventManager.Instance.TriggerEvent(EventStrings.TIME_PAUSE);
            pausePanel.SetActive(true);
        }

        public void TimeContinue()
        {
            YTEventManager.Instance.TriggerEvent(EventStrings.TIME_CONTINUE);
            pausePanel.SetActive(false);
        }

        public void OpenSettings()
        {
            settingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            settingsPanel.SetActive(false);
        }

        public void BackToMenu()
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
        }


        // 倒计时
        public int timeMax=10;
        public RectTransform timeCountDownMask;
        public Text timeCountDownText;
        private float _timeCountDown;

        public float timeCountDown
        {
            get => _timeCountDown;
            set
            {
                _timeCountDown = value;
                timeCountDownText.text = ((int)_timeCountDown).ToString();
                //如果改变了倒计时UI大小，这里要跟着改
                timeCountDownMask.sizeDelta= new Vector2(1300*_timeCountDown/timeMax, 30);
                if (_timeCountDown<0f)
                {
                    YTEventManager.Instance.TriggerEvent(EventStrings.GAME_OVER);
                }
            }
        }
        
        //游戏结束
        public GameObject gameOverPanel;
        public void GameOver()
        {
            gameOverPanel.SetActive(true);
        }
        public void GameRestart()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }
}