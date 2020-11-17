using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.Events;
using MGG.DailyReward.Scripts;

public class DailyReward : MonoBehaviour
{
    public UnityEvent notInternetConnection;
    public UnityEvent httpError;
    public UnityEvent rewardEarnedToday;
    public List<UnityEvent> eventsDays = new List<UnityEvent>();

    private DailyRewardComponent dailyReward;

    private void Start()
    {
        dailyReward = gameObject.GetComponent<DailyRewardComponent>();
        StartCoroutine(dailyReward.CheckDailyReward(day =>
        {
            if (day == -1) //Not Internet Connection
                notInternetConnection.Invoke();

            else if (day == -2) //Http Error!
                httpError.Invoke();

            else if (day == 0) //Reward Earned Today
                rewardEarnedToday.Invoke();

            else if (day >= 1) //Every day's event!
            {
                var x = day;
                if (day > eventsDays.Count) x = day % eventsDays.Count;

                if (x == 0) x = eventsDays.Count;

                for (int i = 0; i < eventsDays.Count; i++)
                {
                    if (i == x - 1) eventsDays[i].Invoke();
                }
            }
        }));
    }
}