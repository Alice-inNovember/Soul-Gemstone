using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Util.EventSystem;
using Util.SingletonSystem;
using EventType = Util.EventSystem.EventType;

public class DiaryManager : MonoBehaviour, IEventListener
{
    public GameObject[] diaryArray;
    public int currentDiaryIndex = 0;
    private string checkText1, checkText2;
    
    void Start()
    {
        EventManager.Instance.AddListener(EventType.ScreenInterection, this);
        SetDiaryFocus(0);
    }

    public void OnDiaryInteraction(InputManager.InteractionType type)
    {
        if (GameManager.Instance.currentStatus != GameManager.PlayerStatus.Diary)
            return;
        if (type == InputManager.InteractionType.LeftSwipe)
        {
            SetDiaryFocus(currentDiaryIndex + 1);
        }
        else if (type == InputManager.InteractionType.RightSwipe)
        {
            SetDiaryFocus(currentDiaryIndex - 1);
        }
    }

    public void OnEvent(EventType eventType, Component sender, object param = null)
    {
        if (eventType == EventType.ScreenInterection)
        {
            OnDiaryInteraction((InputManager.InteractionType)param);
        }
    }

    public void SetDiaryFocus(int targetIndex)
    {
        if (targetIndex < 0 || targetIndex >= diaryArray.Length)
            return;
        for (int i = 0; i < diaryArray.Length; i++)
        {
            var DiaryTransform = diaryArray[i].GetComponent<RectTransform>();
            DiaryTransform.DOPause();
            DiaryTransform.DOAnchorPosX(1200 * -(targetIndex - i), 0.5f);
        }

        currentDiaryIndex = targetIndex;
    }

    public void SetCheck1(TMP_InputField check)
    {
        checkText1 = check.text;
    }
    
    public void SetCheck2(TMP_InputField check)
    {
        checkText2 = check.text;
        UIManager.Instance.MovePreparation(false);
    }
    
    
    public void CreateDiary()
    {
        string dateTime = DateTime.Now.ToString("yy-MM-dd");
        for (int i = 1; i < diaryArray.Length; i++)
        {
            diaryArray[i].GetComponent<Diary>().SetDiary(dateTime, "", checkText1, checkText2, false, false, 1, 1);
            dateTime = DateTime.Now.AddDays(i).ToString("yy-MM-dd");
        }
    }
}