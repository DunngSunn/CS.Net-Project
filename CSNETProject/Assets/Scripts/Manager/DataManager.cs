using DunnGSunn;
using UnityEngine;

namespace Manager
{
    public class DataManager : SunMonoSingleton<DataManager>
    {
        // ======================== //
        #region Fields

        public static int BestScore
        {
            get => PlayerPrefs.GetInt("BestScore", 0);
            set => PlayerPrefs.SetInt("BestScore", value);
        }

        #endregion
    }
}