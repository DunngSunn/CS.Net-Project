using DunnGSunn;
using NgocAnh;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseScreen : BaseScreen
    {
        #region Fields

        public Button closeButton;
        public Button replayButton;
        public Button homeButton;

        public Slider musicSlider;
        public Slider sfxSlider;

        #endregion
        
        public override void Initialize()
        {
            closeButton.onClick.AddListener(OnCloseButtonClick);
            replayButton.onClick.AddListener(OnReplayButtonClick);
            homeButton.onClick.AddListener(OnHomeButtonClick);
            
            musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
        }

        private void OnCloseButtonClick()
        {
            if (!CanClick) return;
         
            SunUIController.PopScreen();
        }

        private void OnReplayButtonClick()
        {
             if (!CanClick) return;
             
             SunUIController.PopScreen();
             SunUIController.GetScreen<GameplayScreen>().InitializeGameplay();
        }

        private void OnHomeButtonClick()
        {
             if (!CanClick) return;
             
             SunUIController.PopAllScreen();
        }

        public override void Show()
        {
            base.Show();
            
            musicSlider.value = AudioManager.MusicValue;
            sfxSlider.value = AudioManager.SfxValue;
        }
        
        private void OnSFXValueChanged(float value)
        {
            AudioManager.SfxValue = value;
            SunEventManager.EmitEvent(EventID.SfxValueChanged, sender: value);
        }

        private void OnMusicValueChanged(float value)
        {
            AudioManager.MusicValue = value;
            SunEventManager.EmitEvent(EventID.MusicValueChanged, sender: value);
        }
    }
}