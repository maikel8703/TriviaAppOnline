using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPiece : MonoBehaviour
{
    public static int ruta=0;

    public Route currentRoute;

    private PlayerMove playerMove2;

    [SerializeField] public GameObject loadScene;

    

    [SerializeField] public GameObject main;

    public int routePosition=0;

    public int routePosition2=0;

    // Remove unnecessary variables
    public bool isMoving;

    public bool moviendose=false;

    //GameControl gameControl;

    public int control=0;

    public QuestionManager questionManager;

    private GameControl gameControl;

    private GameControl gameControl1;

    public GameObject game2Control;

    private Vector3 screenPoint;  

    private Vector3 offset;


    private Vector3 desiredPosition;
    private Vector3 desiredScale=Vector3.one;

    private float yOffset=0.2f;


    public int rutasP1=11;

    public int rutasP2=11;

    public int myroute1,myroute2;

    void Start()
    {
        //targetPosition=this.targetPosition;
        
    }


    // Update is called once per frame
    void Update()
    {     

        //transform.position=Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime*10);
        //transform.localScale=Vector3.Lerp(transform.localScale, desiredPosition, Time.deltaTime*10);
              

        if(moviendose==true && isMoving==false && control==1 && routePosition<=9){

            //SceneManager.LoadSceneAsync("Quiz", LoadSceneMode.Additive);           
            loadScene.SetActive(true);
            //main.SetActive(true);
            questionManager.correrTiempo=true;
            questionManager.siguientePregunta=true;
            Debug.Log("isMoving: "+ruta);
            ruta=0;
            control=0;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //SceneManager.UnloadSceneAsync("Quiz");
            moviendose=false;
        }

       
        
    }

    void Awake(){

        gameControl1= game2Control.GetComponent<GameControl>();
        

        //DontDestroyOnLoad(transform.root.gameObject);

    }



    // Make this public so we can call it from GameControl
    // Add number of steps to move as parameter
    public IEnumerator Move(int steps)
    {
        if (isMoving)
        {
            yield break;

        }
        isMoving = true;

        //Jump();

        while (steps > 0)
        {
            Debug.Log("Route position: "+routePosition);

    
            routePosition++;

            routePosition %= currentRoute.childNodeList.Count;      

            Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
            while (MoveToNextNode(nextPos)) { yield return null; }



            yield return new WaitForSeconds(0.1f);

            
            steps--;
            moviendose=true;

            
                      

             if(routePosition>0)
            {
               Debug.Log("Ruta Mayor que 7: "+ruta);

               ruta=1;
            }

            if(routePosition>9)
            {
               
               Debug.Log("Ganaste!!! "); 
            }

            if(gameControl1.moveplayer1==true){

                control=1;
            }

            
            //SceneManager.LoadScene("Quiz");
            
        }

        /*while (QuizUI.perder==true && GameControl.moveplayer1==true)
        {
            Debug.Log("Route position: "+routePosition);
            
            routePosition--;
            routePosition %= currentRoute.childNodeList.Count;

            Vector3 backPos = currentRoute.childNodeList[routePosition].position;
            while (MoveToBackNode(backPos)) { yield return null; }

            yield return new WaitForSeconds(0.1f);
            steps--;
            //QuizUI.perder=false;
            //moviendose=true;

             /*if(routePosition>1)
            {
               Debug.Log("Ruta Mayor que 7: "+ruta); 

               ruta=1;
            }

            
            //SceneManager.LoadScene("Quiz");
            
        }*/

        isMoving = false;
       
    }

    bool MoveToNextNode(Vector3 goal)
    {
      
        this.transform.Translate(Vector3.up*0.1f);
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 8f * Time.deltaTime));
        
    }

     bool MoveToBackNode(Vector3 goal)
    {
        this.transform.Translate(Vector3.up*0.1f);
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 8f * Time.deltaTime));        
        
        
    }

    public virtual void SetPosition(Vector3 position, bool force=false)
    {
        desiredPosition=position;
        if (force)
        
            transform.position=desiredPosition;
        
    }

    public virtual void SetScale(Vector3 scale, bool force=false)
    {
        desiredScale=scale;
        if (force)
        
            transform.localScale=desiredScale;
        
    }

    private void OnTriggerEnter(Collider other)
	{

		if (other.CompareTag("preguntas"))
		{
            SceneManager.LoadScene("Quiz");
			
		}
    }

		

}
