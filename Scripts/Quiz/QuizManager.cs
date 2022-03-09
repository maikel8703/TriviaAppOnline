using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

[Serializable]
public class PlayerData
{

    public int puntuacion;
    public int monedas;
}

public class QuizManager : MonoBehaviour

{
    public static QuizManager gm;
    //public int puntuacion;
    private string filePath;
    #pragma warning disable 649
    [SerializeField] private QuizUI quizUI;
    [SerializeField] private List<QuizDataScriptable> quizData;
    [SerializeField] private float timeInSeconds;
    #pragma warning disable 649

    //[SerializeField] private float timeLimit=30f;
private List<Question> questions;

private Question selectedQuestion;

public int scoreCount=0;

public int coinCount=0;

private int currentScoreCount=0;
private float currentTimer;
public int lifeRemaining =3;

public int puntos;
private int coins;

public int vidas;

public ScoreBox scoreBoxes;

//AuthManager authManager;

private GameStatus gameStatus=GameStatus.NEXT;

public GameStatus GameStatus{get{return gameStatus;}}
    // Start is called before the first frame update
    public void StartGame(int index)
    {
        //currentScoreCount=currentScoreCount+scoreCount;
        
        currentTimer=timeInSeconds;
        lifeRemaining=3;
        vidas=lifeRemaining;
        questions=new List<Question>();

        for (int i = 0; i < quizData[index].questions.Count; i++)
        {

            questions.Add(quizData[index].questions[i]);
            
        }        

        SelectQuestion();
        gameStatus=GameStatus.PLAYING;
        
    }

    // Update is called once per fram

    private void SelectQuestion()
    {
        //get the random number
        int val = UnityEngine.Random.Range(0, questions.Count);
        //set the selectedQuetion
        selectedQuestion = questions[val];
        //send the question to quizGameUI
        quizUI.SetQuestion(selectedQuestion);

        questions.RemoveAt(val);
    }

    

    private void Update(){

      
        puntos=Scoring.totalScore;
        quizUI.UpdateScore(puntos);

        
       

        
        
        PlayFabLobby.instance.SendLeaderBoard(puntos);
        
        

        //authManager.UpdateScoreFB(puntos);

        //coins=coinCount;
        //coins=50;
        quizUI.UpdateCoins(coinCount);
        

        if (gameStatus == GameStatus.PLAYING)
        {
            currentTimer -= Time.deltaTime;
            SetTime(currentTimer);
        }

       
    }

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
        coinCount=0;

    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private void Awake()
    {

        GetPuntos();

        

        /*filePath = Application.persistentDataPath + "/save.sav";

        if (File.Exists(filePath))
		{
			Load();
        }*/
    }

    /*public void Save(){

        BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(filePath);

		PlayerData data = new PlayerData();
        //data.puntuacion=scoreCount;
        data.puntuacion=Scoring.totalScore;
        data.monedas=coinCount;

        bf.Serialize(file, data);
		file.Close();
    }

    void Load()
    {

        BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(filePath, FileMode.Open);

		PlayerData data = (PlayerData)bf.Deserialize(file);
		file.Close();

        //scoreCount=data.puntuacion;
        Scoring.totalScore=data.puntuacion;
        coinCount=data.monedas;
    }*/

    private void SetTime(float value)
    {
      
        TimeSpan time = TimeSpan.FromSeconds(currentTimer);                       //set the time value
        quizUI.TimerText.text = "Tiempo:"+time.ToString("mm':'ss");   //convert time to Time format

        if (currentTimer <= 0)
        {
            //Game Over
            gameStatus=GameStatus.NEXT;
            quizUI.GameOverPanel.SetActive(true);
        }
    }

    public void OnDestroy()
    {
      SavePuntos();
      
      Debug.Log("On Destroy");
        
    }

    public void UpdateScore()
	{
		//scoreText.text = "Puntos: " + score + "m";
		
		lifeRemaining=3;
        
	}

    
    public void ReiniciarGame(){

        coinCount=0;
        scoreCount=0;

        
    }

    public bool Answer(string answered){

        bool correctAns = false;

        if (answered==selectedQuestion.correctAns)
        {
            //Yes, Ans is correct

            correctAns=true;
            //scoreCount+=50;
            Scoring.totalScore+=50;
            //quizUI.ScoreText.text="Score:"+scoreCount;
            quizUI.ScoreText.text="Score:"+Scoring.totalScore;

            
         
        }
        else
        {
            lifeRemaining--;
            quizUI.ReduceLife(lifeRemaining);

            if(lifeRemaining<=0){

                //lifeRemaining=3;
                gameStatus=GameStatus.NEXT;
                quizUI.GameOverPanel.SetActive(true);
            }
           
        }

        if(gameStatus==GameStatus.PLAYING){

            if(questions.Count>0){
        Invoke("SelectQuestion",0.4f);
            }else{

                gameStatus=GameStatus.NEXT;
                quizUI.GameOverPanel.SetActive(true);

            }
        }

        return correctAns;

    }
}

[System.Serializable]
public class Question
{
    public string questionInfo;         //question text
    public QuestionType questionType;   //type
    public Sprite qustionImg;        //image for Image Type
    public AudioClip qustionClip;         //audio for audio type
    public UnityEngine.Video.VideoClip videoClip;   //video for video type
    public List<string> options;        //options to select
    public string correctAns;           //correct option
}

[System.Serializable]
public enum QuestionType
{
    TEXT,
    IMAGE,
    AUDIO,
    VIDEO
}

[System.Serializable]
public enum GameStatus
{
    PLAYING,
    NEXT
}
