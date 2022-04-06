using System.Collections;
using DG.Tweening;
using DunnGSunn;
using GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class GameplayScreen : BaseScreen
    {
        #region Fields

        public Button pauseButton;

        public SunTween outOfMoveTween;

        public TextMeshProUGUI scoreText;

        public EventSystem eventSystem;

        private int _currentScore;
        private int _scoreAdd;

        #endregion

        public override void Initialize()
        {
            pauseButton.onClick.AddListener(OnPauseButtonClick);
            
            SunEventManager.StartListening(EventID.ScoreInGameplay, OnScoreChanged);
            SunEventManager.StartListening(EventID.ShowEndGame, OnShowEndGame);

            outOfMoveTween.finishedEventWhen = EventWhen.Reverse;
            outOfMoveTween.AddListenerToEnd(OnOutOfMoveFinish);
        }

        private void OnOutOfMoveFinish()
        {
            eventSystem.enabled = true;
            SunUIController.PushScreen<EndGameScreen>(hideCurrentScreen: false);
        }

        private void OnShowEndGame()
        {
            StartCoroutine(nameof(ShowEndGameCoroutine));
        }

        private void OnPauseButtonClick()
        {
            if (!CanClick) return;

            SunUIController.PushScreen<PauseScreen>(hideCurrentScreen: false);
        }

        public override void Destroy()
        {
            SunEventManager.StopListening(EventID.ScoreInGameplay, OnScoreChanged);
            SunEventManager.StopListening(EventID.ShowEndGame, OnShowEndGame);
        }

        private void OnScoreChanged()
        {
            _scoreAdd = (int) SunEventManager.GetSender(EventID.ScoreInGameplay);
            var to = _currentScore + _scoreAdd;
            DOTween.To(() => _currentScore, x => _currentScore = x, to, .5f).OnUpdate(() =>
            {
                scoreText.text = _currentScore.ToString();
            });
        }

        public void InitializeGameplay()
        {
            Gameplay.InitializeGameplay();
            outOfMoveTween.SetStartToCurrentValue();
            scoreText.text = "0";
            _currentScore = 0;
        }

        private IEnumerator ShowEndGameCoroutine()
        {
            eventSystem.enabled = false;
            outOfMoveTween.PlayForward();
            yield return new WaitForSeconds(outOfMoveTween.duration + 1.5f);
            outOfMoveTween.PlayReverse();
        }
    }
}