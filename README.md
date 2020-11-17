# Daily-rewards-Unity


 * A small system of daily rewards based on https://worldtimeapi.org API
 
 
Decided to share its code with everyone for free. Since I am a novice indie developer and self-taught programmer, I do not think that I have the right to try to sell, but for the sake of experience I want to share with you what I was able to implement for my game in Google Play.

How do I add and configure my small plugin in your project?

Download the scripts and place them in your project. After creating a new empty game object and putting on it such 2 scripts as
 * DailyRewardComponent.cs
 * DailyReward.cs

And proceed to their settings.

<br>DailyRewardComponent.cs
![enter image description here](https://mggstudio.ru/GitHub/DailyReward/Screenshot_1.png)

1. Fields localLastReceiveBonusTimeKey - this is the name of the key for PlayerPrefs, which will store the time of the last call.
2. Fields localDayInRowKey - this is the day of the last award received
3. Fields serverUri - in this field, you must put a link to the API worldtimeapi.org
    * For example https://worldtimeapi.org/api/timezone/Europe/Moscow this request returns a JSON string with Moscow time. The script works with it. 
 
 <br><br>DailyReward.cs
![enter image description here](https://mggstudio.ru/GitHub/DailyReward/Screenshot_2.png)

This script is made on UnityEvent and is very easy to configure.

1. Not Internet Connection - executed when the player logged in to the game without Internet access.
2. Http Error - executed if for some reason the server returns an error on request.
3. Reward Earned Today - executed if the player has already received a reward for entering today.
4. Every day's event - is a reward for every day . Click the "Add Day" button and fill in the event.

This system was a good fit for my project. You can see an example of using this system in my game. [Google Play](https://play.google.com/store/apps/details?id=com.MGG.ClassicCase)

    Thank you all for your attention. And sorry for my bad English.