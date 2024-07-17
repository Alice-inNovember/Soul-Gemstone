using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Util.EventSystem;
using Util.SingletonSystem;
using EventType = Util.EventSystem.EventType;

public class DiaryManager : MonoBehaviourSingleton<BookManager>, IEventListener
{
    public GameObject[] diaryArray;
    public int currentDiaryIndex = 0;

    void Start()
    {
        EventManager.Instance.AddListener(EventType.ScreenInterection, this);
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

    void SetDiaryFocus(int targetIndex)
    {
        if (targetIndex < 0 || targetIndex >= diaryArray.Length)
            return;
        for (int i = 0; i < diaryArray.Length; i++)
        {
            var DiaryTransform = diaryArray[i].GetComponent<RectTransform>();
            DiaryTransform.DOPause();
            if (targetIndex <= i)
            {
                DiaryTransform.DOAnchorPos(Vector3.zero, 0.5f);
            }
            else
            {
                DiaryTransform.DOAnchorPos(new Vector3(1200, 0, 0), 0.5f);
            }
        }

        currentDiaryIndex = targetIndex;
    }
}