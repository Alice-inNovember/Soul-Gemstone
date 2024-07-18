using System;
using System.Globalization;
using DG.Tweening;
using Script.SQLite;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Util.EventSystem;
using Util.SingletonSystem;

namespace Script.UI
{
    public enum EuiState
    {
        BookShelf,
        Diary,
        TaskInput
    }
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        public EuiState uiState = EuiState.BookShelf;
        public const float AnimationTime = 0.5f;
        public DiaryManager diaryManager;
        public BookManager bookManager;
        [SerializeField] private Button startEditButton;
        [SerializeField] private Button taskInputFinishButton;
        [SerializeField] private Button toBookShelfButton;
        [SerializeField] private Button saveButton;
        
        void Start()
        {
            bookManager = GetComponent<BookManager>();
            diaryManager = GetComponent<DiaryManager>();
            SetUIState(EuiState.BookShelf);
            startEditButton.onClick.AddListener(StartEdit);
            taskInputFinishButton.onClick.AddListener(FinishTaskInput);
            toBookShelfButton.onClick.AddListener(() => SetUIState(EuiState.BookShelf));
            saveButton.onClick.AddListener(Save);
            bookManager.Load();
        }
        
        private void StartEdit()
        {
            foreach (var book in bookManager.bookList)
            {
                foreach (var diaryData in book.Data.DiaryDataList)
                {
                    if (DateTime.Parse(diaryData.Date).ToString("yyyy MMMM dd") == DateTime.Today.ToString("yyyy MMMM dd"))
                    {
                        SetUIState(EuiState.Diary);
                        diaryManager.SetDiaryInfo(book.Data, book.Data.DiaryDataList);
                        diaryManager.SetDiaryFocus((int)DateTime.Today.DayOfWeek + 1);
                        return;
                    }
                }
            }
            SetUIState(EuiState.TaskInput);
        }
        
        private void FinishTaskInput()
        {
            SetUIState(EuiState.Diary);
            bookManager.CreateBook();
        }

        private void Save()
        {
            Debug.Log("Save Button Clicked");
            bookManager.Save();
        }
        
        
        public void SetUIState(EuiState state)
        {
            uiState = state;
            EventManager.Instance.PostNotification(EEventType.UIStateChange, this, state);
        }
    }
}
