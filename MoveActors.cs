using UnityEngine;
using System.Collections;

public class MoveActors : MonoBehaviour
{

    public ArrayList Movements = new ArrayList(); //list of player vector coords every frame
    public int MovementIndex = 0;  //index of current player position
    public bool rewinding;  //bool to check if rewinding time
    public bool isListEmpty;
    public bool hasMoved;
    public float speed = 2f;  //player speeed

    public Vector3 lastPosition;

    private Rigidbody charRigidbody;

    void Start()
    {
        lastPosition = transform.position;
        hasMoved = false;
        isListEmpty = false;
        charRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HasActorMoved();

        if(hasMoved == true)
        {
            //hasMoved = false;
            if (!rewinding)
            {
                //if not rewinding, add players position to list
                Movements.Add(transform.position);
                MovementIndex++;
            }
        }


        if (MovementIndex > Movements.Count - 1)
        {
            MovementIndex = Movements.Count;
        }

        if (Input.GetKey("e"))
        {
            //rewind
            rewinding = true;
            RewindTime();
        }
        else
        {
            rewinding = false;
        }
    }

    void Update()
    {
          //character controls
        if (Input.GetKey("w"))
        {
           /* if (!rewinding)
            {
                //if not rewinding, add players position to list
                Movements.Add(transform.position);
                MovementIndex++;
            }*/
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey("a"))
        {

            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if (Input.GetKey("s"))
        {

            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        if (Input.GetKey("d"))
        {

            transform.Translate(Vector3.right * Time.deltaTime * speed);
        } 
    }


    void RewindTime()
    {
        MovementIndex--;
        //whilst list is not empty...
        if (MovementIndex > 0)
        {
            //go back through the list, and set the player pos to each position
            transform.position = (Vector3)Movements[MovementIndex];
            //remove the positions that have been used
            Movements.RemoveAt(MovementIndex);
        }
        else
        {
            //reset the index - stops IndexOutOfBound exceptions
            MovementIndex = 0;
        }
        
    }


    void HasActorMoved()
    {
        if(lastPosition != transform.position)
        {
            hasMoved = true;
            lastPosition = transform.position;
        }
        else
        {
            hasMoved = false;
        }
    }
}