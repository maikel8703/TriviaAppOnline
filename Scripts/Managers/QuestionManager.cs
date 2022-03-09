using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class QuestionManager : Singleton<QuestionManager>
{
    public static Action OnNewQuestionLoading;

    public Transform CorrectText;

    public Transform IncorrectText;

    public QuestionUI questionUI;
    private TriviaManager triviaManager;

    private QuestionModel currentQuestion;

    public string CategoryName;

    public int scoreCount=0;

    public int coinCount=0;

    private int currentScoreCount=0;

    private float currentTimer;

    public int lifeRemaining =3;

    public int puntos;

    private int coins;

    public int vidas;

    public bool siguientePregunta;
    public bool correrTiempo;

    public float timeInSeconds=30;

    public bool isCorrect=true;

    public bool dadoAutomatico=false;

    public bool ganar=true;


    private void Start()
    {
         currentTimer=timeInSeconds;
       
        triviaManager=TriviaManager.Instance;

        LoadNextQuestion();
                
        lifeRemaining=3;
        vidas=lifeRemaining;
    }

    

    public void Update()
    {
        
        LoadNextQuestion();
        puntos=Scoring.totalScore;
        questionUI.UpdateScore(puntos);
        
        PlayFabLobby.instance.SendLeaderBoard(puntos);     

        

        questionUI.UpdateCoins(coinCount);

        if (correrTiempo==true)
        {
            
            currentTimer-= Time.deltaTime;
            SetTime(currentTimer);
        
        }else if(correrTiempo==false && currentTimer > 0)
        {
            currentTimer=timeInSeconds;
            SetTime(currentTimer);       
            
        }

    }
    private void SetTime(float value)
    {
      
        TimeSpan time = TimeSpan.FromSeconds(value);                       //set the time value
        questionUI.TimerText.text = "Tiempo:"+time.ToString("mm':'ss");   //convert time to Time format

        if (currentTimer <= 0)
        {
            //Game Over
            //gameStatus=GameStatus.NEXT;
            questionUI.GameOverPanel.SetActive(true);
        }
    }

    void LoadNextQuestion()
    {
        if(siguientePregunta==true){
            siguientePregunta=false;
            questionUI.GameOverPanel.SetActive(false);
        currentQuestion=triviaManager.GetQuestionForCategory(CategoryName);

        if(currentQuestion!=null)
        {
            questionUI.PopulateQuestion(currentQuestion);

        }

        OnNewQuestionLoading?.Invoke();
        }
    }

    public void ResetTime()
    {
        timeInSeconds=30;
    }


    public void UpdateScore()
	{		
		lifeRemaining=3;
        
	}

    public bool AnswerQuestion(int answerIndex)
    {
        isCorrect= currentQuestion.CorrectAnswerIndex==answerIndex;

        if(isCorrect)
        {
            dadoAutomatico=false;
            TweeningResult(CorrectText);
            Scoring.totalScore+=50;
            questionUI.ScoreText.text="Score:"+Scoring.totalScore;

        }else
        {
            ganar=false;
            dadoAutomatico=true;
            TweeningResult(IncorrectText);
            vidas--;
            questionUI.ReduceLife(vidas);

            if(lifeRemaining<=0){

                //lifeRemaining=3;
                questionUI.GameOverPanel.SetActive(true);
            }

        }

        return isCorrect;

    }

    void TweeningResult(Transform resutTransform)
    {
        Sequence result=DOTween.Sequence();
        result.Append(resutTransform.DOScale(1,.5f).SetEase(Ease.OutBack));
        result.AppendInterval(1f);
        result.Append(resutTransform.DOScale(0,.2f).SetEase(Ease.Linear));
        result.AppendCallback(LoadPanel);

    }

    void LoadPanel()
    {
      questionUI.GameOverPanel.SetActive(true);
      siguientePregunta=false;
      correrTiempo=false;
      LoadNextQuestion();
      //currentTimer=30; 
      
            
       
    }

}
