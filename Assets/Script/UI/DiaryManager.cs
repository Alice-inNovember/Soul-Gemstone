using System;
using System.Collections.Generic;
using DG.Tweening;
using Script.SQLite;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util.EventSystem;

namespace Script.UI
{
    public class DiaryManager : MonoBehaviour, IEventListener
    {
        [SerializeField] private Diary[] diaryList;
        public GameObject[] diaryArray;
        [SerializeField] private int currentDiaryIndex = 0;
        [SerializeField] private string checkText1;
        [SerializeField] private string checkText2;

        private void Start()
        {
            EventManager.Instance.AddListener(EEventType.ScreenInterection, this);
        }

        private void OnDiaryInteraction(InputManager.EInteractionType type)
        {
            if (UIManager.Instance.uiState != EuiState.Diary)
                return;
            if (type == InputManager.EInteractionType.LeftSwipe)
            {
                SetDiaryFocus(currentDiaryIndex + 1);
            }
            else if (type == InputManager.EInteractionType.RightSwipe)
            {
                SetDiaryFocus(currentDiaryIndex - 1);
            }
        }

        public void OnEvent(EEventType eventType, Component sender, object param = null)
        {
            if (eventType == EEventType.ScreenInterection)
            {
                OnDiaryInteraction((InputManager.EInteractionType)param);
            }
        }

        public void SetDiaryFocus(int targetIndex)
        {
            if (targetIndex < 0 || targetIndex >= diaryArray.Length)
                return;
            for (int i = 0; i < diaryArray.Length; i++)
            {
                var diaryTransform = diaryArray[i].GetComponent<RectTransform>();
                diaryTransform.DOPause();
                diaryTransform.DOAnchorPosX(1200 * -(targetIndex - i), 0.5f);
            }
            currentDiaryIndex = targetIndex;
        }

        public void SetDiaryInfo(BookData bookData, List<DiaryData> diaryDatas)
        {
            var startingDay = DateTime.Parse(bookData.StartingDay);
            var fullDateTime = startingDay.ToString("yyyy") + " " + startingDay.ToString("MMMM dd") + " ~ " + startingDay.AddDays(6).ToString("MMMM dd");
            diaryArray[0].GetComponent<DiaryInfo>().SetInfo(fullDateTime, "Test Summary");
            foreach (var diary in diaryList)
            {
                diary.SetDiary(diaryDatas.Find(data => data.DayType == diary.days));
            }
        }
    }
}