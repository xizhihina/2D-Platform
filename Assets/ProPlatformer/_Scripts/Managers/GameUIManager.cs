using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Myd.Platform
{
    public class GameUIManager : UnitySingleton<GameUIManager>
    {
        public GameObject pausePanel;
        public GameObject settingsPanel;

        public void PauseGame()
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
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
                timeCountDownMask.sizeDelta= new Vector2(1300*_timeCountDown/60, 30);
            }
        }
    }
}