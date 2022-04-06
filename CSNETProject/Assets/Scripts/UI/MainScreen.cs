using DunnGSunn;
using NgocAnh;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainScreen : BaseScreen
    {
        #region Fields

        [Header("===== Button =====")]
        public Button playButton;

        [Header("===== Slider =====")]
        public Slider musicSlider;
        public Slider sfxSlider;

        #endregion

        #region Override functions

        public override void Initialize()
        {
            // Thêm sự kiện click cho button
            playButton.onClick.AddListener(OnPlayButtonClick);
            
            // Thêm sự kiện đổi giá trị cho slider
            musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
        }

        public override void Show()
        {
            base.Show();
            
            // Thay đổi giá trị của slider theo âm lượng của music và sfx
            musicSlider.value = AudioManager.MusicValue;
            sfxSlider.value = AudioManager.SfxValue;
        }

        #endregion

        #region Slider event

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

        #endregion

        #region Button event

        private void OnPlayButtonClick()
        {
            if (!CanClick) return;
            SunUIController.PushScreen<GameplayScreen>(hideCurrentScreen: true);
            SunUIController.GetScreen<GameplayScreen>().InitializeGameplay();
        }

        #endregion
    }
}