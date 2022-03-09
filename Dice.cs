using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
   Rigidbody rb;

    bool hasLanded;
    bool thrown;

    Vector3 initPosition;
    
    public int diceValue;

    public DiceSide[] diceSides;

    public bool IsDoneRolling;
    // Assign game object through editor
    public GameObject gameControllerGameObject; 

    public GameObject quizMg; 

    public GameObject player1; 
    public GameObject player2;
    private GameControl gameControl;

    public static bool continuar=true;

    public int rutasMover1=11;
    public int rutasMover2=11;

    public bool moverDado=false;

    private QuestionUI quizUI;

    private QuestionManager questionManager;

    private PlayerPiece playerPiece1;
    private PlayerPiece playerPiece2;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPosition = transform.position;
        rb.useGravity = false;
    }
    void Awake()
    {
        gameControl = gameControllerGameObject.GetComponent<GameControl>();
        playerPiece1= player1.GetComponent<PlayerPiece>();
        playerPiece2= player2.GetComponent<PlayerPiece>();

        questionManager = quizMg.GetComponent<QuestionManager>();
    }

    // Update is called once per frame
    void Update()
    { 

        if(gameControl.dadoFallido1==true && gameControl.gameOver==false && questionManager.coinCount>=0)
        {
            //gameControl.dadoFallido1=false;
            RollDice();
        }
        
    
        if (rb.IsSleeping() && !hasLanded && thrown)
        {
            hasLanded = true;
            //rb.useGravity = false;
            rb.isKinematic = true;

            SideValueCheck();        
            
            gameControl.MovePlayer(diceValue);
           
        
            
        }
        else if (rb.IsSleeping() && hasLanded && diceValue == 0)
        {
            RollAgain();
        }
    }

    private void OnMouseDown(){

        if(playerPiece1.isMoving==false && playerPiece2.isMoving==false && gameControl.gameOver==false && questionManager.lifeRemaining>=2){        

        RollDice();  
        }
              

    }

    public void RollDiceAuto()
    {
        if(QuestionUI.perder==true && questionManager.dadoAutomatico==true && questionManager.coinCount>0){

        RollDice();
        questionManager.dadoAutomatico=false;
        }   
    }

    public void RollDice() 
    {
        if (!thrown && !hasLanded)
        {
            IsDoneRolling = false;
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0,250), Random.Range(0,250), Random.Range(0,250));           
        }
        else if (thrown && hasLanded)
        {
            RollAgain();
        }
    }

    void Reset()
    {
        transform.position = initPosition;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
        rb.isKinematic = false;
        IsDoneRolling = true;
    }

    void RollAgain()
    {
        Reset();
        IsDoneRolling = false;
        thrown = true;
        rb.useGravity = true;
        rb.AddTorque(Random.Range(0,250), Random.Range(0,250), Random.Range(0,250));
    }

    void SideValueCheck()
    {
        diceValue = 0;
        foreach (DiceSide side in diceSides)
        {
            if (side.OnGround())
            {
                diceValue = side.sideValue;
                Debug.Log(diceValue + " has been rolled!");
            }
        }
    }
}
