using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score{

    //public string name;
    public int scoreData;
    public int monedasData;

    public Score (int scoreData, int monedasData )
    {
        //this.name=name;        
        this.scoreData=scoreData;
        this.monedasData=monedasData;
    }
}

public class ScoreBox : MonoBehaviour
{

    public bool isBegin=true;

    public int scoreFinal=0;

    public int monedasFinal=0;

    public QuestionManager questionManager;

   public void Update()
    {

        if(isBegin==true && scoreFinal>Scoring.totalScore)
        {          

            Scoring.totalScore=scoreFinal;
            questionManager.coinCount=monedasFinal;
            isBegin=false;
            Debug.Log("isBegin true");                  
            
        }else
        {
            scoreFinal=Scoring.totalScore;
            monedasFinal=questionManager.coinCount;
            Debug.Log("isBegin false"); 

        } 
       
        
    }
    
    public Score ReturnClass(){

                
        return new Score(scoreFinal,monedasFinal);
    }

    public void SetUi(string score, string monedas)
    {
        scoreFinal = System.Convert.ToInt32(score);
        monedasFinal = System.Convert.ToInt32(monedas);
        //scoreFinal=score.scoreData;
        //monedasFinal=monedas.monedasData;
    }
}
