using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util.EventSystem;
using EventType = Util.EventSystem.EventType;

public class InputManager : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    public float swipeDistance;

    public enum SwipeType
    {
        LeftSwipe,
        RightSwipe
    }

    
    
    
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            swipeDistance = (endPos - startPos).magnitude;
            if (swipeDistance > 100)
            {
                if (Mathf.Abs(endPos.x - startPos.x) > Mathf.Abs(endPos.y - startPos.y))
                {
                    if (endPos.x > startPos.x)
                    {
                        EventManager.Instance.PostNotification(EventType.ScreenInterection, this, SwipeType.RightSwipe);
                        //Debug.Log("Right Swipe");
                    }
                    else
                    {
                        EventManager.Instance.PostNotification(EventType.ScreenInterection, this, SwipeType.LeftSwipe);
                        //Debug.Log("Left Swipe");
                    }
                }
            }
        }
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startPos = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endPos = Input.GetTouch(0).position;
                swipeDistance = (endPos - startPos).magnitude;
                if (swipeDistance > 100)
                {
                    if (Mathf.Abs(endPos.x - startPos.x) > Mathf.Abs(endPos.y - startPos.y))
                    {
                        if (endPos.x > startPos.x)
                        {
                            EventManager.Instance.PostNotification(EventType.ScreenInterection, this, SwipeType.RightSwipe);
                            Debug.Log("Right Swipe");
                        }
                        else
                        {
                            EventManager.Instance.PostNotification(EventType.ScreenInterection, this, SwipeType.LeftSwipe);
                            Debug.Log("Left Swipe");
                        }
                    }
                }
            }
        }
    }
}
