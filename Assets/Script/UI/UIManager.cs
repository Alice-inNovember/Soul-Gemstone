using System;
using System.Globalization;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Util.SingletonSystem;

namespace Script.UI
{
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        public DiaryManager diaryManager;
        public BookManager bookManager;
        public GameObject preparation;
        public GameObject createDiaryBtn;
        public GameObject backBtn;
        public GameObject saveBtn;
    
        private Vector3 openPos = new Vector3(0, 230f, 0);
        private Vector3 closePos = new Vector3(0, -1600f, 0);

        void Start()
        {
            for (int i = 0; i < bookManager.bookList.Count; i++)
            {
                if (bookManager.bookList[i].GetComponent<Book>().GetDate() == GetLastSunday().AddDays(i).ToString("yy-MM-dd"))
                {
                    createDiaryBtn.GetComponent<Button>().interactable = false;
                }
            }
           
        }
        
        public void MovePreparation(bool isOpen)
        {
            if (isOpen)
            {
                preparation.SetActive(true);
                preparation.GetComponent<RectTransform>().DOAnchorPos(openPos, 1f);
            }
            else
            {
                preparation.GetComponent<RectTransform>().DOAnchorPos(closePos, 1f).onComplete += () =>
                {
                    preparation.SetActive(false);
                    bookManager.CreateBook();
                };
            }
        }
        
        public DateTime GetLastSunday()
        {
            DateTime today = DateTime.Today;
            int daysSinceSunday = (int)today.DayOfWeek % 7;
            DateTime lastSunday = today.AddDays(-daysSinceSunday);
            return lastSunday;
        }

        public void CreateDiary()
        {
            diaryManager.CreateDiary();
            createDiaryBtn.SetActive(false);
            foreach (var book in bookManager.bookList)
            {
                book.SetActive(false);
            }
            foreach (var diary in diaryManager.diaryArray)
            {
                diary.SetActive(true);
            }
            diaryManager.diaryArray[0].GetComponent<RectTransform>().DOAnchorPos(new Vector3(0f,0f,0f), 1f);
            backBtn.SetActive(true);
            saveBtn.SetActive(true);
            GameManager.Instance.currentStatus = EPlayerStatus.Diary;
        }
        
        public void BackToBook()
        {
            foreach (var book in bookManager.bookList)
            {
                book.SetActive(true);
            }
            foreach (var diary in diaryManager.diaryArray)
            {
                diary.SetActive(false);
            }
            diaryManager.SetDiaryFocus(0);
            diaryManager.diaryArray[0].GetComponent<RectTransform>().DOAnchorPos(new Vector3(0f,-1800f,0f), 0f);
            createDiaryBtn.SetActive(true);
            backBtn.SetActive(false);
            saveBtn.SetActive(false);
            GameManager.Instance.currentStatus = EPlayerStatus.Book;
        }
    }
}
