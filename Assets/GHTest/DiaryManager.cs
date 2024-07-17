using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    public GameObject[] diaryArray;
    public int currentDiaryIndex;
    
    private static DiaryManager _instance;
    
    public static DiaryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DiaryManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<DiaryManager>();
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
        currentDiaryIndex = 0;
        for (int i = 0; i < diaryArray.Length; i++)
        {
            diaryArray[i].GetComponent<Diary>().diaryIndex = i;
        }
    }
}
