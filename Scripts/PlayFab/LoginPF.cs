using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.Networking;
using GleyInternetAvailability;

public class LoginPF : MonoBehaviour
{
    public InputField nameInput;
    public InputField emailInput;
    public InputField passwordInput;

    public InputField emailRegisterInput;
    public InputField passwordRegisterInput;

    public InputField passwordRegisterCheckInput;

    public Text messageText;

    private bool registroPress=false;

    private bool internetAccess;

    public void Awake()
    {
         if(Application.internetReachability == NetworkReachability.NotReachable){
             Debug.Log("Error. Check internet connection!");
       
         }

    }

    public void CheckIConnection()
    {
        GleyInternetAvailability.Network.IsAvailable(CompleteMethod);
    }

    private void CompleteMethod(ConnectionResult connectionResult)
    {
        if(connectionResult==ConnectionResult.Working)
        {
            internetAccess=true;
            Debug.Log("Estás Conectado a internet");

        }else
        {
            messageText.text="No estás Conectado a internet";
            Debug.Log("No estás Conectado a internet");
            internetAccess=false;
        }

    }

    

    /*IEnumerator CheckInternetConnection()
    {

        UnityWebRequest request=new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();
        if(request.error!=null)
        {
            messageText.text="No estás Conectado a internet";
            Debug.Log("No estás Conectado a internet");
            internetAccess=false;
        }else
        {            
            internetAccess=true;
            Debug.Log("Estás Conectado a internet");
        }
    }*/

    public void CheckName()
    {
        if (registroPress==true)
        {
            UpdateNameButton();
            registroPress=false;
        }
        
    }
    public void RegisterButton()
    {
        //StartCoroutine(CheckInternetConnection());
        CheckIConnection();

       

        if(internetAccess==true){

             if (nameInput.text=="")
        {
            messageText.text="Necesitas un Nombre";
        }

        if (passwordRegisterInput.text!="" && passwordRegisterInput.text.Length<6)
        {
            messageText.text="Contraseña muy Corta";
            return;
        }

            if (emailRegisterInput.text!="" && passwordRegisterInput.text==passwordRegisterCheckInput.text && nameInput.text!="")
        {
            var request=new RegisterPlayFabUserRequest{
            Email=emailRegisterInput.text,
            Password=passwordRegisterInput.text,
            RequireBothUsernameAndEmail=false, 
                 
        

        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
       
            
        }else
        {
           messageText.text="llene todos los Campos"; 
        } 

        
        }   
           

    }


    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Update Display Name");
    }

    public void UpdateNameButton()
    {
        
        var request=new UpdateUserTitleDisplayNameRequest{
             
            DisplayName=nameInput.text,
            
        

        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);

        
            
        
       
    }


    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        
        messageText.text="Registro Exitoso";
        registroPress=true;
         

    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void LoginButton()
    {

        //StartCoroutine(CheckInternetConnection());
        CheckIConnection();       

        if (internetAccess==true)
        {
             if (nameInput.text!="")
        {
            UpdateNameButton();
        }        

        var request=new LoginWithEmailAddressRequest{
            Email=emailInput.text,
            Password=passwordInput.text,
            InfoRequestParameters=new GetPlayerCombinedInfoRequestParams{
                GetPlayerProfile=true
            } 
                      

        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
            
        }   
        
    }

    void OnLoginSuccess(LoginResult result)
    {        
              
        string name=null;
        if(result.InfoResultPayload.PlayerProfile!=null)
        name=result.InfoResultPayload.PlayerProfile.DisplayName;
        
        if(name==null)
        {
            messageText.text="No tiene Asignado un Nombre";
        }else
        {
            Gamemanager.instance.ChangeScene(1);
            Debug.Log("Logueado Correctamente");
        }
    }

     public void ResetPasswordButton()
    {
        
    }

    public void ClearText()
    {
       messageText.text="";
       emailInput.text="";
       passwordInput.text="";
       emailRegisterInput.text="";
       passwordRegisterInput.text="";
       passwordRegisterCheckInput.text="";
    }
    
}
