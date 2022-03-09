using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }

    // Update is called once per frame
    void Login()
    {
        var request=new LoginWithCustomIDRequest{
            CustomId=SystemInfo.deviceUniqueIdentifier,
            CreateAccount=true

        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess,OnError);
        
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Sucsefull Login Account Create");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while Loggin in Creating Account");
        Debug.Log(error.GenerateErrorReport());
    }
}
