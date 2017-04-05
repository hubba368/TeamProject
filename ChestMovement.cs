﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ChestMovement : MonoBehaviour
{
    public GameObject chest;
    public Text scoreDisplay;
    public float speed = 1f;
	public static Text score;
	public Text timee;

    private string currentScene;

    //public int touch = 0;

	//GameObject gObj = null   
	void Rotate()
	{
		float rotationAngle = 0f;
		while (rotationAngle < 60f)
		{
			chest.transform.Rotate(speed, 0, 0);
			rotationAngle++;
		}

		Invoke("ShowScore", 0);
	}

	void ShowScore()
	{
		score = Timer.timerText;
		Debug.Log (score);
		//score = timee / 100 * 0.5;
		scoreDisplay.text = "Your Score is: ";
		scoreDisplay.GetComponent<Text>().enabled = true;      //Enable the text so it is not visible anymore
		print(scoreDisplay);
	}

	void InitialiseVars()
	{
		chest = GameObject.FindGameObjectWithTag("Chest");
		scoreDisplay = GetComponent<Text>();
	}
    //Plane objPlane;
    //Vector3 mo;

    // Use this for initialization
    void Start () {
        InitialiseVars();
    }



    // Update is called once per frame
    void Update()
    {
		timee = Timer.timerText;
		Debug.Log (timee);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                BoxCollider bC = hit.collider as BoxCollider;
                if (bC != null)
                {
					if(hit.transform.tag == "Chest")
					{
						Invoke("Rotate", 0);
					}
					else {
						//Debugging purpose
						//Debug.Log ("This isn't a Player");                
					}
                }
            }
        }

        //touch = Input.touchCount;
        //if (Input.touchCount > 0)
        //{
        //    if (Input.GetTouch(0).phase == TouchPhase.Began)
        //    {
        //        Ray mouseRay = GenerateMouseRay(Input.GetTouch(0).position);
        //        RaycastHit hit;

        //        if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
        //        {
        //            gObj = hit.transform.gameObject;
        //            objPlane = new Plane(Camera.main.transform.forward*=1, gObj.transform.position);

        //            //calculate touch offset
        //            Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //            float rayDistance;
        //            objPlane.Raycast(mRay, out rayDistance);
        //            mo = gObj.transform.position = mRay.GetPoint(rayDistance);
        //        }
        //    }
        //    else if ()
        //}
    }

    //Ray GenerateMouseRay(Vector3 touchPos)
    //{
    //    Vector3 mousePosFar = new Vector3(touchPos.x,
    //        touchPos.y,
    //        Camera.main.farClipPlane);
    //    Vector3 mousePosNear = new Vector3(touchPos.x,
    //        touchPos.y,
    //        Camera.main.nearClipPlane);

    //    Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
    //    Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

    //    Ray nr = new Ray(mousePosN, mousePosF - mousePosN);
    //    return nr;
    //}
}