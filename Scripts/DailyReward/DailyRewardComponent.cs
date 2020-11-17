using MGG.DailyReward.Scripts.Structs;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace MGG.DailyReward.Scripts
{
    public class DailyRewardComponent : MonoBehaviour
    {
        [Header("Properties")]
        public int serverTime;
        public int lastReceiveBonusTime;
        public int dayInRow;
        [Space]
        public bool isNetworkError;
        public bool isHttpError;
        public bool isLoaded;
        public bool isCompleteLoaded;
        public bool isLocalDataFounded;

        [Header("Settings")]
        [SerializeField] private string localLastReceiveBonusTimeKey;
        [SerializeField] private string localDayInRowKey;
        [Space]
        [SerializeField] private string serverUri;
        [Space]
        [SerializeField] private bool useLocalData;
        [SerializeField] private int localTime;

        private void Awake()
        {
            if (this.useLocalData)
            {
                LoadLocalData();
                this.serverTime = this.localTime;
                this.isCompleteLoaded = true;
                this.isLoaded = true;
            }
            else
            {
                LoadLocalData();
                StartCoroutine(SendRequest());
            }
        }

        private IEnumerator SendRequest()
        {
            UnityWebRequest request = UnityWebRequest.Get(this.serverUri);
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                this.isNetworkError = true;
                this.isLoaded = true;
#if UNITY_EDITOR
                Debug.Log("{GameLog} => [DailyRewardComponent] - (<color=yellow>Network Error</color>) -> " + request.error);
#endif
                yield break;
            }

            if (request.isHttpError)
            {
                this.isHttpError = true;
                this.isLoaded = true;
#if UNITY_EDITOR
                Debug.Log("{GameLog} => [DailyRewardComponent] - (<color=yellow>Http Error</color>) -> " + request.error);
#endif
            }

            string json = request.downloadHandler.text;
            ServerTimeResponse response = JsonUtility.FromJson<ServerTimeResponse>(json);

            this.serverTime = response.unixtime;
            this.isCompleteLoaded = true;
            this.isLoaded = true;
        }

        public IEnumerator CheckDailyReward(Action<int> callback)
        {
            while (!this.isLoaded)
            {
                yield return new WaitForSeconds(0.25f);
            }

            if (this.isNetworkError)
            {
                callback(-1);
                yield break;
            }

            if (this.isHttpError)
            {
                callback(-2);
                yield break;
            }

            if (this.isCompleteLoaded)
            {
                CalculateDay();
                callback(this.dayInRow);
            }
        }

        private void LoadLocalData()
        {
            if (PlayerPrefs.HasKey(localLastReceiveBonusTimeKey) && PlayerPrefs.HasKey(localDayInRowKey))
            {
                this.lastReceiveBonusTime = PlayerPrefs.GetInt(localLastReceiveBonusTimeKey);
                this.dayInRow = PlayerPrefs.GetInt(localDayInRowKey);
                this.isLocalDataFounded = true;
            }
        }

        private void SetLastReceiveBonusTime()
        {
            this.lastReceiveBonusTime = this.serverTime;
            PlayerPrefs.SetInt(localLastReceiveBonusTimeKey, this.lastReceiveBonusTime);
            PlayerPrefs.SetInt(localDayInRowKey, this.dayInRow);
        }

        private void CalculateDay()
        {
            if (!this.isLocalDataFounded)
            {
                this.dayInRow = 1;
                SetLastReceiveBonusTime();
            }

            if (this.isLocalDataFounded)
            {
                int timeDifference = this.serverTime - this.lastReceiveBonusTime;

                if (timeDifference <= 86400)
                {
                    this.dayInRow = 0;
                }

                if (timeDifference > 86400 && timeDifference < (86400 * 2))
                {
                    this.dayInRow += 1;
                    SetLastReceiveBonusTime();
                }

                if (timeDifference > (86400 * 2))
                {
                    this.dayInRow = 1;
                    SetLastReceiveBonusTime();
                }
            }
        }
    }
}
