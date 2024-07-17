using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
            SetDiaryFocus(0);
        }

        private void OnDiaryInteraction(InputManager.EInteractionType type)
        {
            if (GameManager.Instance.currentStatus != EPlayerStatus.Diary)
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
            var dateTime = DateTime.Now.ToString("yy-MM-dd");
            for (var i = 1; i < diaryArray.Length; i++)
            {
                diaryArray[i].GetComponent<Diary>().SetDiary(dateTime, "", checkText1, checkText2, false, false, 1, 1);
                dateTime = DateTime.Now.AddDays(i).ToString("yy-MM-dd");
            }
        }
    }
}