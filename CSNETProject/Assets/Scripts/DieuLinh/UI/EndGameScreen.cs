using DunnGSunn;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DieuLinh
{
    public class EndGameScreen : BaseScreen
    {
        #region Fields

        [Header("===== Button =====")]
        public Button replayButton;
        public Button homeButton;

        [Header("===== Text =====")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI bestScoreText;

        #endregion

        #region Override functions

        public override void Initialize()
        {
            // Thêm sự kiện click cho button
            replayButton.onClick.AddListener(OnReplayButtonClick);
            homeButton.onClick.AddListener(OnHomeButtonClick);
        }

        #endregion
        
        #region Events

        // Sự kiện replay button click
        private void OnReplayButtonClick()
        {
            if (!CanClick) return;
             
            // Ẩn màn hình pause
            SunUIController.PopScreen();
            
            // Chuẩn bị gameplay
            SunUIController.GetScreen<GameplayScreen>().InitializeGameplay();
        }

        // Sự kiện home button click
        private void OnHomeButtonClick()
        {
            if (!CanClick) return;
             
            // Ẩn tất cả và về màn hình chính
            SunUIController.PopAllScreen();
        }

        #endregion

        #region Other

        //
        public void ShowScore(int currentScore)
        {
            // Set score text
            scoreText.text = currentScore.ToString();

            // Kiểm tra điểm hiện tại có lớn hơn best score trước đó không
            if (DataManager.BestScore < currentScore)
            {
                DataManager.BestScore = currentScore;
            }

            bestScoreText.text = DataManager.BestScore.ToString();
        }

        #endregion
    }
}