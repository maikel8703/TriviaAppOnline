using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizUI : MonoBehaviour
{

   
    [SerializeField] private QuizManager quizManager;

    [SerializeField] public GameObject loadScene2,restCoin, noCoinDiag;
    [SerializeField] public Text questionInfoText, scoreText,scoreText2, timerText, finalizadoText, finalizadoText2,coinText;
    [SerializeField] private List<Image> lifeImageList;

    [SerializeField] private GameObject gameOverPanel, mainMenuPanel, gameMenuPanel;
    [SerializeField] private Image questionImage;

    [SerializeField] private AudioSource questionAudio;
    [SerializeField] private List<Button> options, uiButtons; 

    [SerializeField] private Color correctCol, wrongCol, normalCol;
   

    private Question question;
    private bool answered = false;

    //AuthManager authManager;

    private float audioLength;

    public static bool perder=false;

    private Dice dice;

    public GameObject gameControl;

    private GameControl gm3;

    public Text TimerText { get => timerText; }                     //getter
    public Text ScoreText { get => scoreText; }                     //getter
    public GameObject GameOverPanel { get => gameOverPanel; }
    // Start is called before the first frame update
    void Start()
    {
        restCoin.gameObject.SetActive(false);
        noCoinDiag.gameObject.SetActive(false);
        
         for (int i = 0; i < options.Count; i++)
        {
            Button localBtn = options[i];
            localBtn.onClick.AddListener(() => Onclick(localBtn));
        }

          for (int i = 0; i < uiButtons.Count; i++)
        {
            Button localBtn = uiButtons[i];
            localBtn.onClick.AddListener(() => Onclick(localBtn));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        

        //quizManager.Save();
        if(gm3.dadoFallido1==false &&  quizManager.lifeRemaining<2){

            perder=true;
            finalizadoText.text="Perdiste";
            finalizadoText2.text="Se descontarán 50 puntos y 1 moneda";
            Debug.Log("Perder: "+perder);
            restCoin.gameObject.SetActive(true);
            /*if(quizManager.scoreCount>=50){

            //quizManager.scoreCount=quizManager.scoreCount-50;
            } */          
            
        }else if(gm3.dadoFallido1==false && quizManager.lifeRemaining>=2)
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
        gm3 = gameControl.GetComponent<GameControl>();;
    }

    public void SetQuestion(Question question){

        this.question=question;

       switch(question.questionType){

           case QuestionType.TEXT:

          questionImage.transform.parent.gameObject.SetActive(false);
           break;

           case QuestionType.IMAGE:
           ImageHolder();
           questionImage.transform.parent.gameObject.SetActive(true);
           questionImage.sprite=question.qustionImg;
           break;

           case QuestionType.AUDIO:
           ImageHolder();
           questionAudio.transform.parent.gameObject.SetActive(true);
           audioLength=question.qustionClip.length;
           StartCoroutine(PlayAudio());
           break;
       }

   questionInfoText.text = question.questionInfo;                      //set the question text

        //suffle the list of options
        List<string> ansOptions = ShuffleList.ShuffleListItems<string>(question.options);

        //assign options to respective option buttons
        for (int i = 0; i < options.Count; i++)
        {
            //set the child text
            options[i].GetComponentInChildren<Text>().text = ansOptions[i];
            options[i].name = ansOptions[i];    //set the name of button
            options[i].image.color=Color.white;
        }

        answered = false; 
    }

    IEnumerator PlayAudio()
    {

        if(question.questionType==QuestionType.AUDIO)
        {
            questionAudio.PlayOneShot(question.qustionClip);

            yield return new WaitForSeconds(audioLength+0.5f);
            StartCoroutine(PlayAudio());
        }else
        {
            StopCoroutine(PlayAudio());
            yield return null;
        }
    }

    void ImageHolder(){

        questionImage.transform.parent.gameObject.SetActive(true);
        //questionImage.transform.gameObject.SetActive(false);
        questionAudio.transform.gameObject.SetActive(false);
    }

      /// <summary>
    /// Method assigned to the buttons
    /// </summary>
    /// <param name="btn">ref to the button object</param>

    void Onclick(Button btn){

        if(quizManager.GameStatus==GameStatus.PLAYING){


        

        if(!answered){

            answered=true;
            bool val=quizManager.Answer(btn.name);

            if(val)
        {
            btn.image.color=Color.green;
        }
        else
        {
            btn.image.color=Color.red;
        }
        }
        }

        switch (btn.name)
        {
            case "Animal":
            quizManager.lifeRemaining=3;
            quizManager.StartGame(0);
            lifeImageList[0].color=Color.green;
            lifeImageList[1].color=Color.green;
            lifeImageList[2].color=Color.green;
            mainMenuPanel.SetActive(false);
            gameMenuPanel.SetActive(true);
            break;
            case "Bird":
            quizManager.lifeRemaining=3;
            lifeImageList[0].color=Color.green;
            lifeImageList[1].color=Color.green;
            lifeImageList[2].color=Color.green;
            quizManager.StartGame(1);
            mainMenuPanel.SetActive(false);
            gameMenuPanel.SetActive(true);
            break;
        }

        
    }

     public void ReduceLife(int remainingLife)
    {
        lifeImageList[remainingLife].color = Color.red;
        
        Debug.Log("Life Remaining: "+remainingLife);
    }


    public void RButton()
    {
   

            //gameOverPanel.SetActive(false);
            if (perder==true && quizManager.coinCount>=0)
            {

               quizManager.coinCount= quizManager.coinCount-1;
               coinText.text=quizManager.coinCount.ToString();
               //quizManager.lifeRemaining=3;
            }            

            if (perder==true && quizManager.coinCount<0)
            {
               noCoinDiag.gameObject.SetActive(true); 
               //gameOverPanel.gameObject.SetActive(false);
               
            }else if (perder==false && quizManager.coinCount>=0)
            {
                quizManager.coinCount= quizManager.coinCount+1;
                coinText.text=quizManager.coinCount.ToString();
                Debug.Log("Moneda +1");
                //loadScene2.gameObject.SetActive(false);
                
            }

            
            
        
            Debug.Log("MoverDado: ");
            //dice.moverDado=true;         

        

       
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene("SampleScene");
        //loadScene2.SetActive(false);
        //SceneManager.UnloadSceneAsync(SampleScene);
    }

    public void UpdateScore(int score){

        scoreText2.text="Puntos: "+score;
//        LobbyDatabase.instance.puntuacionData=score;
    }

    public void UpdateCoins(int coin){
        
        coinText.text=""+coin;
        //Debug.Log("Monedas"+coin);
        
    }

}
