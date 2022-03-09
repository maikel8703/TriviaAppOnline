using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOutPoint : MonoBehaviour
{
    //[SerializeField] EnemyEntry[] enemyList;

	public bool AreaCleared {get; private set;}
	private bool activePoint;
	public PlayerMove playerMove;
    //private int enemyKilled, totalEnemy;

    private GameControl gameControl;

    public GameObject gameCon;

	public void Initialize(PlayerMove value)

	{

		playerMove=value;
	}

    private void Start()
    {
        playerMove.SetPlayerMovement(false);
        //activePoint=true;

        /*foreach (var enemy in enemyList)
        {

            enemy.enemy.gameObject.SetActive(false);
            totalEnemy=!(enemy.enemy is HostageScript) ? totalEnemy+1:totalEnemy+0;
        }*/
    }
    
    // Update is called once per frame
    void Update()
    {      

    	/*if(Input.GetKeyDown(KeyCode.Space))
    	{

    		playerMove.SetPlayerMovement(false);


    	}*/

        /*if(gameControl.player1Piece.routePosition>5 && activePoint)
    	{

    		playerMove.SetPlayerMovement(true);
    		AreaCleared=true;
    		activePoint=false;


    	}*/
        
    }

    void Awake()
    {

        gameControl= gameCon.GetComponent<GameControl>();
    }

    public void StartShootOut(float timer)
    {

    	activePoint=true;

    	playerMove.SetPlayerMovement(false);
        //StartCoroutine(SendEnemies());
//        this.DelayedAction(SetAreaCleared, timer);
     //   ManagerGame.Instance.StartTimer(timer);


    }

    /*IEnumerator SendEnemies()
    {
        foreach(var enemy in enemyList)
        {

            yield return new WaitForSeconds(enemy.delay);

            enemy.enemy.gameObject.SetActive(true);
            enemy.enemy.Init(this);

            Debug.Log(enemy.enemy.gameObject.name+"Spawned");
        }

    }

    public void EnemyKilled()
    {

        enemyKilled++;

        if(enemyKilled==totalEnemy)
        {
            playerMove.AreaCleared();
            AreaCleared=true;
            activePoint=false;
            ManagerGame.Instance.StopTimer();

        }


    }

   public void SetAreaCleared()
   {

    if(AreaCleared||ManagerGame.Instance.PlayerDead)
    return;

    AreaCleared=true;
    playerMove.SetPlayerMovement(true);

    foreach (var enemy in enemyList)
    {

        enemy.enemy.StopShooting();

    }
   } */


}

/*[System.Serializable]
public class EnemyEntry
{

    public EnemyScript enemy;
    public float delay;

}*/
