using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Virtaul_Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image BGImage;
    private Image joystickImage;
    private Vector3 inputVector;

    private void Start()
    {
        BGImage = GetComponent<Image>();
        joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(BGImage.rectTransform,
            ped.position,
            ped.pressEventCamera,
            out pos))
        {
            pos.x = (pos.x / BGImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / BGImage.rectTransform.sizeDelta.y);

            Debug.Log(pos);


            inputVector = new Vector3(pos.x*2 + 1,0, pos.y*2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            //Move joystick image
            joystickImage.rectTransform.anchoredPosition =
                new Vector3(inputVector.x * (BGImage.rectTransform.sizeDelta.x / 3),
                inputVector.z * (BGImage.rectTransform.sizeDelta.y / 3));

            //Debugging purposes
            //Debug.Log();
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }


    ///Used to get Horizontal and Virtical direction for the player to move
    //public float Horizontral()
    //{
    //    if (inputVector.x != 0)
    //        return inputVector.x;
    //    else
    //        return inputVector.GeAxis("Horizontal");
    //}

    //public float Vertical()
    //{
    //    if (inputVector.z != 0)
    //        return inputVector.z;
    //    else
    //        return inputVector.GeAxis("Vertical");
    //}
}
