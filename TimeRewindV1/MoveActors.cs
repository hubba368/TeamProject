using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveActors : MonoBehaviour
{

	public List<Vector3> Movements = new List<Vector3>(); //list of player vector coords every frame
    public List<Vector3> enemyMovements = new List<Vector3>();
    public int PlayerMovementIndex = 0;   //index of current player position
    public int EnemyMovementIndex = 0;
    public bool rewinding;  		//bool to check if rewinding time
    public bool isListEmpty;		//bool to check if the list is empty
    public bool hasPlayerMoved;           //bool to check if the player is moving
    public bool hasEnemyMoved;           //bool to check if the player is moving
                                          // public float speed = 2f;  		//player speeed

    public GameObject player;
    public GameObject[] enemies;
    public GameObject playerTransformObj;
    public GameObject enemyTransformObj;

    public Vector3 playerLastPosition;    //the last position of the player
    public List<Vector3> enemylastPosition = new List<Vector3>();    //the last position of the enemy(ies)

    private Rigidbody charRigidbody; //player rigid body
    private Animator charAnimator;
    private int enemyInitialIndex = 0;

    void Start()
    {
        playerLastPosition = player.transform.position;
        hasPlayerMoved = false;
        hasEnemyMoved = false;
        isListEmpty = false;
        charRigidbody = player.GetComponent<Rigidbody>();

        playerTransformObj = Instantiate(playerTransformObj);
        enemyTransformObj = Instantiate(enemyTransformObj);

        player = GameObject.FindGameObjectWithTag("Player");
        playerTransformObj.transform.position = player.transform.position;
        playerTransformObj.SetActive(false);

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemies)
        {
            enemylastPosition.Add(enemies[enemyInitialIndex].transform.position);
            enemyInitialIndex++;
        }

    }

    void FixedUpdate()
    {
        HasActorMoved();


        

        if (hasPlayerMoved == true)
        {
            //hasMoved = false;
            if (!rewinding)
            {
                //if not rewinding, add players position to list
                Movements.Add(player.transform.position);
                PlayerMovementIndex++;


            }
        }


        if (hasEnemyMoved == true)
        {
            //hasMoved = false;
            if (!rewinding)
            {
                //if not rewinding, add players position to list
                foreach (GameObject enemy in enemies)
                {

                    enemyMovements.Add(enemy.transform.position);
                    EnemyMovementIndex++;
                }
            }

        }

        if (rewinding)
        {
            if (enemies.Length > 0)
            {
                //rewind
                foreach (GameObject enemy in enemies)
                {
                    // List<Vector3> enemyMov = EnemyMovements;
                    RewindTime();
                }
            }
            else
            {
                RewindTime();
            }
            
            
        }

        if (PlayerMovementIndex > Movements.Count - 1)
        {
            PlayerMovementIndex = Movements.Count;
        }
        if (EnemyMovementIndex > enemyMovements.Count - 1)
        {
            EnemyMovementIndex = enemyMovements.Count;
        }


    }

    void Update()
    {

    }

    public void OnPointerDown()
    {
        rewinding = true;



    }

    public void OnPointerUp()
    {
        rewinding = false;
        player.SetActive(true);
        playerTransformObj.SetActive(false);

        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(true);
            enemyTransformObj.SetActive(false);
        }

    }


    void RewindTime()
    {

        PlayerMovementIndex--;
        //whilst list is not empty...
        if (PlayerMovementIndex > 0)
        {
            //go back through the list, and set the player pos to each position
            player.transform.position = Movements[PlayerMovementIndex];
            
            playerTransformObj.transform.position = Movements[PlayerMovementIndex] + new Vector3(0,1,0);
            player.SetActive(false);
            playerTransformObj.SetActive(true);
            //remove the positions that have been used
            Movements.RemoveAt(PlayerMovementIndex);
        }
        else
        {
            //reset the index - stops IndexOutOfBound exceptions
            PlayerMovementIndex = 0;
        }

        EnemyMovementIndex--;
        //whilst list is not empty...
        if (EnemyMovementIndex > 0)
        {
            //go back through the list, and set the player pos to each position
            foreach(GameObject enemy in enemies)
            {
                enemy.transform.position = enemyMovements[EnemyMovementIndex];

                enemyTransformObj.transform.position = enemyMovements[EnemyMovementIndex] + new Vector3(0, 1, 0);
                enemy.SetActive(false);
                enemyTransformObj.SetActive(true);
                //remove the positions that have been used
                enemyMovements.RemoveAt(EnemyMovementIndex);
            }
            
        }
        else
        {
            //reset the index - stops IndexOutOfBound exceptions
            EnemyMovementIndex = 0;
        }

    }


    void HasActorMoved()
    {
        int i = 0;
		//if the player's current pos is not the previous pos
        if(playerLastPosition != player.transform.position)
        {
			//set the new pos
            hasPlayerMoved = true;
            playerLastPosition = player.transform.position;
        }
        else
        {
            hasPlayerMoved = false;
        }


        foreach(GameObject enemy in enemies)
        {
            if (enemylastPosition[i] != enemy.transform.position)
            {
                //set the new pos
                hasEnemyMoved = true;
                enemylastPosition[i] = enemy.transform.position;
                i++;
            }
            else
            {
                hasEnemyMoved = false;
                i++;
            }

        }
        
    }
}