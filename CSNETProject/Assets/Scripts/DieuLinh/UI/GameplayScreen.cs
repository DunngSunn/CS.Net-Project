using System.Collections;
using DG.Tweening;
using DunnGSunn;
using LeVy;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DieuLinh
{
    public class GameplayScreen : BaseScreen
    {
        #region Fields

        [Header("===== Button =====")]
        public Button pauseButton;

        [Header("===== Tween =====")]
        public SunTween outOfMoveTween;

        [Header("===== Text =====")]
        public TextMeshProUGUI scoreText;

        [Header("===== Event system =====")]
        public EventSystem eventSystem;

        private int _currentScore;
        private int _scoreAdd;

        #endregion

        #region Override functions

        public override void Initialize()
        {
            // Thêm sự kiện click cho button
            pauseButton.onClick.AddListener(OnPauseButtonClick);
            
            // Bắt đầu nghe sự kiện ingame
            SunEventManager.StartListening(EventID.ScoreInGameplay, OnScoreChanged);
            SunEventManager.StartListening(EventID.ShowEndGame, OnShowEndGame);

            // Điều khiển bảng hiện nguyên nhân endgame
            outOfMoveTween.finishedEventWhen = EventWhen.Reverse;
            outOfMoveTween.AddListenerToEnd(OnOutOfMoveFinish);
        }
        
        public override void Destroy()
        {
            // Kết thúc nghe sự kiện ingame
            SunEventManager.StopListening(EventID.ScoreInGameplay, OnScoreChanged);
            SunEventManager.StopListening(EventID.ShowEndGame, OnShowEndGame);
        }

        #endregion

        #region Event

        // Sự kiện bảng out of move sau khi hiển thị xong
        private void OnOutOfMoveFinish()
        {
            // Cho phép bắt sự kiện chạm vào màn hình
            eventSystem.enabled = true;
            
            // Hiển thị màn hình end game
            SunUIController.PushScreen<EndGameScreen>(hideCurrentScreen: false);
            SunUIController.GetScreen<EndGameScreen>().ShowScore(_currentScore);
        }

        // Sự kiện show endgame
        private void OnShowEndGame()
        {
            StartCoroutine(nameof(ShowEndGameCoroutine));
        }
        
        // Sự kiện pause button click
        private void OnPauseButtonClick()
        {
            if (!CanClick) return;

            // Hiển thj màn hình pause
            SunUIController.PushScreen<PauseScreen>(hideCurrentScreen: false);
        }
        
        // Sự kiện điểm số thay đổi
        private void OnScoreChanged()
        {
            // Nhận số điểm được thêm vào
            _scoreAdd = (int) SunEventManager.GetSender(EventID.ScoreInGameplay);
            
            // Thay đổi điểm trên màn hình - Dũng làm 
            var to = _currentScore + _scoreAdd;
            DOTween.To(() => _currentScore, x => _currentScore = x, to, .5f).OnUpdate(() =>
            {
                scoreText.text = _currentScore.ToString();
            });
        }

        // Chuẩn bị cho gameplay - Dũng làm
        public void InitializeGameplay()
        {
            Gameplay.InitializeGameplay();
            outOfMoveTween.SetStartToCurrentValue();
            scoreText.text = "0";
            _currentScore = 0;
        }
        
        #endregion

        #region Coroutine

        private IEnumerator ShowEndGameCoroutine()
        {
            // Tắt cho phép bắt sự kiện chạm vào màn hình 
            eventSystem.enabled = false;
            
            // Hiển thị bảng nguyên nhân endgame
            outOfMoveTween.PlayForward();
            
            // Đợi 1 thời gian ngắn
            yield return new WaitForSeconds(outOfMoveTween.duration + 1.5f);
            
            // Ẩn bảng nguyên nhân endgame
            outOfMoveTween.PlayReverse();
        }

        #endregion
    }
}