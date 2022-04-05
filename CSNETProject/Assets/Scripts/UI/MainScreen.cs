using DunnGSunn;
using NgocAnh;
using UnityEngine.UI;

namespace UI
{
    public class MainScreen : BaseScreen
    {
        #region Fields

        public Button playButton;

        public Slider musicSlider;
        public Slider sfxSlider;

        #endregion

        public override void Initialize()
        {
            playButton.onClick.AddListener(OnPlayButtonClick);
            
            musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
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

        private void OnPlayButtonClick()
        {
            if (!CanClick) return;
            SunUIController.PushScreen<GameplayScreen>(hideCurrentScreen: true);
            SunUIController.GetScreen<GameplayScreen>().InitializeGameplay();
        }
    }
}