using UnityEngine;
using System.Collections;

public class teleCube : MonoBehaviour
{
    public GameObject target;   //teleport target which is an empty gameobject (or any object that you choose)
    public GameObject[] shapes;  //list of shapes to be changed
    public Vector3 prevPos;    //previous location of objects (would probs change this to dictionary if multiple objects


    // Use this for initialization
    void Start ()
    {
        prevPos = transform.position; //set the previous position as initial position

        shapes = GameObject.FindGameObjectsWithTag("scenery1"); //find all objects with scenery1 tag
        target = GameObject.Find("TeleTarget");   //find the teleport target gameobject
        //Gameobject.Find should not be used as it is resource intensive, you would probably have to instantiate an object in this script and store a reference
        //using it here for simplicity
    }

	
    void MoveThings()
    {
        //very basic, move the shape to the teleport target gameobject
        transform.position = target.transform.position;

        foreach (GameObject shape in shapes)
        {
            //deactivate all the scenery1 objects
            shape.SetActive(false);
        }
    }

    void ResetThings()
    {
        foreach(GameObject shape in shapes)
        {
            //reactivate all the scenery1 objects
            shape.SetActive(true);
        }
        //move the shape back
        transform.position = prevPos;
    }

	// Update is called once per frame
	void Update ()
    {
        //player input
        if (Input.GetKey("o"))
        {
            MoveThings();
        }

        if (Input.GetKey("l"))
        {
            ResetThings();
        }
    }
}
