using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayFabLobby : MonoBehaviour
{
    public static PlayFabLobby instance;
    public Text nameUser;
    public string user;

    public GameObject rowPrefab;
    public Transform rowsParent;

     public void Awake()
    {
        GetPlayerProfile(user);

        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            
            Destroy(instance.gameObject);
            instance=this;
        }

       
      
    }

    
     
   public void GetPlayerProfile(string playFabId) {

       nameUser.text="";

    PlayFabClientAPI.GetPlayerProfile( new GetPlayerProfileRequest() {
        
        PlayFabId = playFabId,
        ProfileConstraints = new PlayerProfileViewConstraints() {
            ShowDisplayName = true
        }
    },
    result => nameUser.text="" + result.PlayerProfile.DisplayName,
    error => Debug.LogError(error.GenerateErrorReport()));
}

public void LogOutButton()
{
    PlayFabClientAPI.ForgetAllCredentials();
    SceneManager.LoadSceneAsync(0);
}

public void SendLeaderBoard(int score)
{
    var request=new UpdatePlayerStatisticsRequest
    {
        Statistics=new List<StatisticUpdate>
        {
            new StatisticUpdate{
                StatisticName="ScoreGame",
                Value=score
            }
        }
    };
    PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);

}

void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
{
    Debug.Log("LeaderBoard Score enviado");
}

public void GetLeaderBoard()
{
     var request=new GetLeaderboardRequest
    {
       StatisticName="ScoreGame",
       StartPosition=0,
       MaxResultsCount=10
    };
    PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
}

void OnLeaderBoardGet(GetLeaderboardResult result)
{
    foreach(Transform item in rowsParent)
    {
        Destroy(item.gameObject);
    }
    foreach(var item in result.Leaderboard)
    {
        GameObject newGO=Instantiate(rowPrefab, rowsParent);
        Text[] texts=newGO.GetComponentsInChildren<Text>();
        texts[0].text=(item.Position+1).ToString();
        texts[1].text=item.DisplayName;
        texts[2].text=item.StatValue.ToString();

        Debug.Log(item.Position+""+item.DisplayName+""+item.StatValue);
    }

}

void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
}
