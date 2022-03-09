using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUI : MonoBehaviour
{
    
    public Text QuestionLabel;

    public Text Answer1Label;

    public Text Answer2Label;

    public Text Answer3Label;

    public static bool perder=false;

    private Dice dice;

    public GameObject gameControl;

    private GameControl gm3;

    [SerializeField] private QuestionManager questionManager;

    [SerializeField] private List<Image> lifeImageList;

    [SerializeField] public GameObject restCoin, noCoinDiag;

    [SerializeField] public Text scoreText,scoreText2, timerText, coinText,finalizadoText, finalizadoText2;

    [SerializeField] private GameObject gameOverPanel;

    public Text TimerText { get => timerText; }

    public Text ScoreText { get => scoreText; }
    public GameObject GameOverPanel { get => gameOverPanel; }

    void Start()
    {
        noCoinDiag.gameObject.SetActive(false);
        restCoin.gameObject.SetActive(false);

    }

    public void Update()
    {
         
        if(gm3.dadoFallido1==false &&  questionManager.ganar==false){
            
            perder=true;
            finalizadoText.text="Perdiste";
            finalizadoText2.text="Se descontarán 50 puntos y 1 moneda";
            Debug.Log("Perder: "+perder);
            restCoin.gameObject.SetActive(true);       
            
        }else if(gm3.dadoFallido1==false && questionManager.isCorrect==true)
        {
            perder=false;
            //GameControl.gc.whosTurn=1;           
            restCoin.gameObject.SetActive(false);
            finalizadoText.text="Felicidades";
            finalizadoText2.text="Recompesa de 1 moneda";
            
        }
    }

    

     void Awake()
    {
        gm3 = gameControl.GetComponent<GameControl>();
        
    }

     

    public void ReduceLife(int remainingLife)
    {
        lifeImageList[remainingLife].color = Color.red;
        
        Debug.Log("Life Remaining: "+remainingLife);
    }


    public void PopulateQuestion(QuestionModel questionModel)
    {
        QuestionLabel.text=questionModel.Question;
        Answer1Label.text=questionModel.Answer1;
        Answer2Label.text=questionModel.Answer2;
        Answer3Label.text=questionModel.Answer3;


    }

    public void UpdateScore(int score){

        scoreText2.text="Puntos: "+score;
//     
    }

    public void UpdateCoins(int coin){
        
        coinText.text=""+coin;
        
    }

    public void RButton()
    {
   

            //gameOverPanel.SetActive(false);
            if (perder==true && questionManager.coinCount>=0)
            {

               questionManager.coinCount= questionManager.coinCount-1;
               coinText.text=questionManager.coinCount.ToString();
            }            

            if (perder==true && questionManager.coinCount<0)
            {
               noCoinDiag.gameObject.SetActive(true); 
               
            }else if (perder==false && questionManager.coinCount>=0)
            {
                questionManager.coinCount= questionManager.coinCount+1;
                coinText.text=questionManager.coinCount.ToString();
                Debug.Log("Moneda +1");
                
            }

            questionManager.siguientePregunta=true;

            
            
        
            Debug.Log("MoverDado: ");
    }

}
