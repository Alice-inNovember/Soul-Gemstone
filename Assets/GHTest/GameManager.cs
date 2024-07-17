using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventType = Util.EventSystem.EventType;

public class GameManager : MonoBehaviour
{
    public enum PlayerStatus
    {
        Book,
        Diary
    }

    public PlayerStatus currentStatus;
    
    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameManager>();
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

    void Start()
    {
        currentStatus = PlayerStatus.Book;
        InputManager.Instance.eventType = EventType.ScreenInterection;
    }
}
