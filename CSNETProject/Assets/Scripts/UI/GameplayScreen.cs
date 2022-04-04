using DunnGSunn;
using GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameplayScreen : BaseScreen
    {
        #region Fields

        public Button pauseButton;

        public SunTween outOfMoveTween;

        public TextMeshProUGUI scoreText;

        #endregion

        public void InitializeGameplay()
        {
            Gameplay.Instance.InitializeGameplay();
            outOfMoveTween.SetStartToCurrentValue();
            scoreText.text = "0";
        }
        
    }
}