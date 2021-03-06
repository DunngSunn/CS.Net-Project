using DunnGSunn;
using NgocAnh;
using UnityEngine;

namespace DunnGSunn
{
    public abstract class BaseScreen : SunUI
    {
        // Override can click fields
        public override bool CanClick
        {
            get
            {
                var result = Time.unscaledTime > _lastTimeClick + 0.5f;
                if (result)
                {
                    _lastTimeClick = Time.unscaledTime;
                    
                    // Play audio
                    AudioManager.Instance.PlaySound("Click");
                }
                return result;
            }
        }
    }
}