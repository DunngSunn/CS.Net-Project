using DieuLinh;
using DunnGSunn;
using NgocAnh;
using UnityEngine;
using UnityEngine.UI;

namespace LeVy
{
    public class PauseScreen : BaseScreen
    {
        #region Fields

        [Header("===== Button =====")]
        public Button closeButton;
        public Button replayButton;
        public Button homeButton;

        [Header("===== Slider =====")]
        public Slider musicSlider;
        public Slider sfxSlider;

        #endregion
        
        #region Override functions

        public override void Initialize()
        {
            // Thêm sự kiện click cho button
            closeButton.onClick.AddListener(OnCloseButtonClick);
            replayButton.onClick.AddListener(OnReplayButtonClick);
            homeButton.onClick.AddListener(OnHomeButtonClick);
            
            // Thêm sự kiện đổi giá trị cho slider
            musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
        }
        
        public override void Show()
        {
            base.Show();
            
            // Thay đổi giá trị của slider
            musicSlider.value = AudioManager.MusicValue;
            sfxSlider.value = AudioManager.SfxValue;
        }

        #endregion

        #region Events

        // Sự kiện close button click
        private void OnCloseButtonClick()
        {
            if (!CanClick) return;
            
            // Ẩn màn hình pause
            SunUIController.PopScreen();
        }

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
        
        // Sự kiện sfx thay đổi giá trị
        private void OnSFXValueChanged(float value)
        {
            AudioManager.SfxValue = value;
            SunEventManager.EmitEvent(EventID.SfxValueChanged, sender: value);
        }

        // Sự kiện music thay đổi giá trị
        private void OnMusicValueChanged(float value)
        {
            AudioManager.MusicValue = value;
            SunEventManager.EmitEvent(EventID.MusicValueChanged, sender: value);
        }

        #endregion
    }
}