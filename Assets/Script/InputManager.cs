using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.EventSystem;
using EventType = Util.EventSystem.EventType;

public class InputManager : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    
    public float swipeDistance;
    public bool isSwiping = false;

    public enum InteractionType
    { 
        LeftSwipe,
        RightSwipe,
        LeftTap,
        RightTap
    }

    private static InputManager _instance;
    
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<InputManager>();
                }
                //DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void TouchInput()
    {
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
                            EventManager.Instance.PostNotification(EventType.DirayInterection, this, InteractionType.RightSwipe);
                            Debug.Log("Right Swipe");
                        }
                        else
                        {
                            EventManager.Instance.PostNotification(EventType.DirayInterection, this, InteractionType.LeftSwipe);
                            Debug.Log("Left Swipe");
                        }
                    }
                }
            }
        }
    }

    void MouseInput()
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
                        EventManager.Instance.PostNotification(EventType.DirayInterection, this, InteractionType.RightSwipe);
                        //Debug.Log("Right Swipe");
                    }
                    else
                    {
                        EventManager.Instance.PostNotification(EventType.DirayInterection, this, InteractionType.LeftSwipe);
                        //Debug.Log("Left Swipe");
                    }
                }
            }
        }
    }
    
    void Update()
    {
        MouseInput();
        
    }
}
