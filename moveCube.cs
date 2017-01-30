using UnityEngine;
using System.Collections;

public class moveCube : MonoBehaviour
{

    public ArrayList Movements = new ArrayList(); //list of player vector coords every frame
    public int MovementIndex = 0;  //index of current player position
    public bool rewinding;  //bool to check if rewinding time
    public float speed = 2f;  //player speeed

    void Update()
    {
        //character controls
        if (Input.GetKey("w"))
        {
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


        if (!rewinding)
        {
            //if not rewinding, add players position to list
            Movements.Add(transform.position);
            MovementIndex++;
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
    }
}