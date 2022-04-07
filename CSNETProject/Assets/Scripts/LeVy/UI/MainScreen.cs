using DieuLinh;
using DunnGSunn;
using NgocAnh;
using UnityEngine;
using UnityEngine.UI;

namespace LeVy
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

        // Sự kiện sfx thay đổi giá trị
        private void OnSFXValueChanged(float value)
        {
            AudioManager.SfxValue = value;
            
            // Bắn sự kiện sfx thay đổi
            SunEventManager.EmitEvent(EventID.SfxValueChanged, sender: value);
        }

        // Sự kiện music thay đổi giá trị
        private void OnMusicValueChanged(float value)
        {
            AudioManager.MusicValue = value;
            
            // Bắn sự kiện music thay đổi
            SunEventManager.EmitEvent(EventID.MusicValueChanged, sender: value);
        }

        #endregion

        #region Button event

        // Sự kiện nút play được click
        private void OnPlayButtonClick()
        {
            if (!CanClick) return;
            
            // Hiển thị màn hình gameplay
            SunUIController.PushScreen<GameplayScreen>(hideCurrentScreen: true);
            
            // Chuẩn bị gameplay
            SunUIController.GetScreen<GameplayScreen>().InitializeGameplay();
        }

        #endregion
    }
}