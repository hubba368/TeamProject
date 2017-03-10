using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MoveActors : MonoBehaviour
{

	public List<Vector3> Movements = new List<Vector3>();                                        //list of player vector coords every frame
    public Dictionary<int, List<Vector3>> enemyMovements = new Dictionary<int, List<Vector3>>(); //dictionary of enemy movements every frame

    public int PlayerMovementIndex = 0;     //index of current player position
    public int EnemyMovementIndex = 0;      //index of current enemy position
    public bool rewinding;  		        //bool to check if rewinding time
    public bool hasPlayerMoved;             //bool to check if the player is moving
    public bool hasEnemyMoved;              //bool to check if an enemy is moving


    public GameObject player;               //player gameobject
    public GameObject[] enemies;            //enemy gameobjects
    public GameObject playerTransformObj;   //player magic sphere
    public GameObject enemyTransform;
    public List<GameObject> enemyTransformObjs = new List<GameObject>();

    public Vector3 playerLastPosition;                              //the last position of the player
    public List<Vector3> enemyLastPosition = new List<Vector3>();   //the last position of the enemy(ies)

    private Rigidbody charRigidbody;        //player rigid body
    public int enemyInitialIndex = 0;

    void Start()
    {
        //initialise player
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransformObj.transform.position = player.transform.position;
        playerTransformObj.SetActive(false);

        playerLastPosition = player.transform.position;
        hasPlayerMoved = false;
        hasEnemyMoved = false;
        charRigidbody = player.GetComponent<Rigidbody>();

        playerTransformObj = Instantiate(playerTransformObj);  //player magic sphere

        //initialise enemy list
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //initialise each enemy's last position
        for(int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            enemyLastPosition.Add(enemy.transform.position);
        }
        for(int j = 0; j < enemies.Length; j++)
        {
            GameObject enemySphereObj = enemyTransform;
            Instantiate(enemySphereObj);
            enemyTransformObjs.Add(enemySphereObj);
            enemySphereObj.SetActive(false);
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
                //if only 1 enemy...
                if(enemies.Length == 1)
                {
                    GameObject enemy = enemies[0];

                    if (!enemyMovements.ContainsKey(0))
                    {                      
                        //add a new item to dictionary
                        enemyMovements.Add(0, new List<Vector3>());
                    }
                    else
                    {
                        //append the dictionary at first key due to only being 1 enemy
                        EnemyMovementIndex++;
                        enemyMovements[0].Add(enemy.transform.position);
                    }
                    
                }
                else
                {
                    //if not rewinding, add all enemy positions to list
                    foreach (GameObject enemy in enemies)
                    {

                        if (!enemyMovements.ContainsKey(0))
                        {
                            enemyMovements.Add(0, new List<Vector3>());
                        }
                        else
                        {
                            EnemyMovementIndex++;
                            enemyMovements[0].Add(enemy.transform.position);
                        }

                    }
                }
                

            }
        }



        if (rewinding)
        {
            if (enemies.Length > 0)
            {
                //rewind if there are enemies 
                for (int i = 0; i < enemies.Length; i++)
                {
                    PlayerRewindTime();
                    EnemyRewindTime();
                }
            }
            else
            {
                //rewind player
                PlayerRewindTime();
            }
            
            
        }

        if (PlayerMovementIndex > Movements.Count - 1)
        {
            PlayerMovementIndex = Movements.Count;
        }
   /*     if (EnemyMovementIndex > enemyMovements.Count - 1)
        {
            EnemyMovementIndex = enemyMovements.Count;
        }*/
    }


    public void OnPointerDown()
    {
        rewinding = true;
        //turn off player magic sphere
        player.SetActive(false);
        playerTransformObj.SetActive(true);

        //for each enemy, turn off enemy magic sphere(s)
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            enemy.SetActive(false);
        }

        ActivateEnemySpheres();

    }

    public void OnPointerUp()
    {
        rewinding = false;
        //turn on player magic sphere
        player.SetActive(true);
        playerTransformObj.SetActive(false);

        //for each enemy, turn on enemy magic sphere(s)
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            enemy.SetActive(true);
        }

        DeactivateEnemySpheres();

    }

    void ActivateEnemySpheres()
    {
        foreach(GameObject obj in enemyTransformObjs)
        {
            obj.SetActive(true);

        }
    }

    void DeactivateEnemySpheres()
    {
        for (int j = 0; j < enemyTransformObjs.Count; j++)
        {
            GameObject obj = enemyTransformObjs[j];
            obj.SetActive(false);
        }
    }

    void PlayerRewindTime()
    {
        PlayerMovementIndex--;
        //whilst list is not empty...
        if (PlayerMovementIndex > 0)
        {
            //go back through the list, and set the player pos to each position
            player.transform.position = Movements[PlayerMovementIndex];

            playerTransformObj.transform.position = Movements[PlayerMovementIndex] + new Vector3(0, 1, 0);
            //remove the positions that have been used
            Movements.RemoveAt(PlayerMovementIndex);
        }
        else
        {
            //reset the index - stops IndexOutOfBound exceptions
            PlayerMovementIndex = 0;
        }
    }

    
    void EnemyRewindTime()
    {
        EnemyMovementIndex--;
        //whilst list is not empty...
        if (EnemyMovementIndex > 0)
        {
            //if there is only 1 enemy...
            if (enemies.Length == 1)
            {
                GameObject enemy = enemies[0]; //get the enemy from enemy list
                List<Vector3> enemyList = new List<Vector3>();

                if (enemyMovements.TryGetValue(0, out enemyList))
                {
                    //get the enemy's list of vector3s from dictionary
                    enemy.transform.position = enemyList[EnemyMovementIndex];
                }

                GameObject obj = enemyTransformObjs[0];
                obj.transform.position = enemy.transform.position + new Vector3(0, 1, 0);

                //remove the positions that have been used
                enemyList.RemoveAt(EnemyMovementIndex);
            }
            else
            {
                //if there is more than 1 enemy...
                for (int j = 0; j < enemies.Length; j++)
                {
                    //go back through the list, and set each enemy pos to previous pos
                    foreach (GameObject enemy in enemies)
                    {
                        List<Vector3> enemyList = new List<Vector3>();
                        EnemyMovementIndex = enemyList.Count;  //set the index to the respective length of each enemy vector3 list - stops ArgumentException

                        if (enemyMovements.TryGetValue(j, out enemyList))
                        {
                            //get the enemy vector3 list
                            enemy.transform.position = enemyList[EnemyMovementIndex];
                        }

                        GameObject obj = enemyTransformObjs[j];
                        obj.transform.position = enemy.transform.position + new Vector3(0, 1, 0);

                        //remove the positions that have been used
                        enemyList.RemoveAt(EnemyMovementIndex);
                        Debug.Log(enemyList);
                    }
                }
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

        int i = 0;
        //for each enemy in scene...
        foreach (GameObject enemy in enemies)
        {
            //if current pos is not the previous pos
            if (enemyLastPosition[i] != enemy.transform.position)
            {
                //set the new pos and go to next enemy
                hasEnemyMoved = true;
                enemyLastPosition[i] = enemy.transform.position;
                i++;
            }
            else
            {
                //go to next enemy
                hasEnemyMoved = false;
                i++;
            }

        }
        
    }
}