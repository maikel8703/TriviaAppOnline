using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabNivelData : MonoBehaviour
{
    public ScoreBox scoreBoxes;

    [SerializeField] private QuestionManager questionManager;
     public void GetPuntos()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
         
    }

    void OnDataReceived(GetUserDataResult result)
    {
        if(result.Data !=null && result.Data.ContainsKey("scoreFinal") && result.Data.ContainsKey("monedasFinal") )
        {
            scoreBoxes.SetUi(result.Data["scoreFinal"].Value, result.Data["monedasFinal"].Value);
        }else
        {
            Debug.Log("Player Data no cargado");
        }

    }

    public void SavePuntos()
    {
        var request=new UpdateUserDataRequest{

            Data=new Dictionary<string, string>{
                {"scoreFinal", scoreBoxes.scoreFinal.ToString()},
                {"monedasFinal", scoreBoxes.monedasFinal.ToString()}


            }
        };

        PlayFabClientAPI.UpdateUserData(request, OndataSend, OnError);    
       
        
    }

    void OndataSend(UpdateUserDataResult result )
    {
        Debug.Log("Dato Enviado");
        Scoring.totalScore=0;
        questionManager.coinCount=0;

    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void Awake()
    {
        GetPuntos();
    }

    public void OnDestroy()
    {
      SavePuntos();
      
      Debug.Log("On Destroy");
        
    }
}
