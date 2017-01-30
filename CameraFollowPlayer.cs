using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour
{
    //using a public variable here but would be better to instantiate the player then store a reference instead
    public Transform player;  //player location
    
    private Vector3 offset;  //distance from player to camera

	// Use this for initialization
	void Start ()
    {
        offset = transform.position - player.position;  //get distance from player
        transform.position = player.position;  //set camera to same pos as player
           
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        //move camera position with the player whilst keeping distance
        transform.position = player.position + offset;
    }
}
