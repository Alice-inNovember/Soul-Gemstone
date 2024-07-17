using UnityEngine;
using Util.EventSystem;

namespace Script
{
    public class InputManager : MonoBehaviour
    {
        public enum EInteractionType
        {
            LeftSwipe,
            RightSwipe,
            LeftTap,
            RightTap
        }

        private static InputManager _instance;

        public float swipeDistance;
        public bool isSwiping;
        public EEventType eventType;
        private Vector3 endPos;
        private Vector3 startPos;

        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InputManager>();
                    if (_instance == null)
                    {
                        var singletonObject = new GameObject();
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
                _instance = this;
            //DontDestroyOnLoad(gameObject);
            else if (_instance != this) Destroy(gameObject);
        }

        private void Update()
        {
            MouseInput();
        }

        private void TouchInput()
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began) startPos = Input.GetTouch(0).position;

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    endPos = Input.GetTouch(0).position;
                    swipeDistance = (endPos - startPos).magnitude;
                    if (swipeDistance > 100)
                        if (Mathf.Abs(endPos.x - startPos.x) > Mathf.Abs(endPos.y - startPos.y))
                        {
                            if (endPos.x > startPos.x)
                            {
                                EventManager.Instance.PostNotification(EEventType.ScreenInterection, this,
                                    EInteractionType.RightSwipe);
                                Debug.Log("Right Swipe");
                            }
                            else
                            {
                                EventManager.Instance.PostNotification(EEventType.ScreenInterection, this,
                                    EInteractionType.LeftSwipe);
                                Debug.Log("Left Swipe");
                            }
                        }
                }
            }
        }

        private void MouseInput()
        {
            if (Input.GetMouseButtonDown(0)) startPos = Input.mousePosition;
            if (Input.GetMouseButtonUp(0))
            {
                endPos = Input.mousePosition;
                swipeDistance = (endPos - startPos).magnitude;
                if (swipeDistance > 100)
                    if (Mathf.Abs(endPos.x - startPos.x) > Mathf.Abs(endPos.y - startPos.y))
                    {
                        if (endPos.x > startPos.x)
                            EventManager.Instance.PostNotification(EEventType.ScreenInterection, this,
                                EInteractionType.RightSwipe);
                        //Debug.Log("Right Swipe");
                        else
                            EventManager.Instance.PostNotification(EEventType.ScreenInterection, this,
                                EInteractionType.LeftSwipe);
                        //Debug.Log("Left Swipe");
                    }
            }
        }
    }
}