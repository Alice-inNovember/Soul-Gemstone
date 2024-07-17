using System;
using System.Collections.Generic;
using DG.Tweening;
using Script.SQLite;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util.EventSystem;

namespace Script.UI
{
    public class BookManager : MonoBehaviour, IEventListener
    {
        [FormerlySerializedAs("books")]
        [Header("Book Data")]
        [SerializeField] public List<Book> bookDataList;
        
        [Header("Book Sprites")]
        [SerializeField] private Sprite[] bookSprites;
        [SerializeField] private int bookTotalCnt = 0;
        [Header("Book Creation")]
        [SerializeField] private GameObject booksParent;
        [SerializeField] private GameObject bookPrefab;
    
        [Header("Book Movement")]
        public List<GameObject> bookList = new List<GameObject>();
        [SerializeField] private int currentBookIndex = 0;
        [SerializeField] private Vector3 sideScale = new Vector3(0.8f, 0.8f, 1f);
        [SerializeField] private Vector3 centerScale = Vector3.one;
        [SerializeField] private float moveDistance = 660f;
        
        void Start()
        {
            EventManager.Instance.AddListener(EEventType.ScreenInterection, this);
        }

        public void CreateBook()
        {
            var book = Instantiate(bookPrefab, booksParent.transform);
            var bookData = new BookData();
            
            book.GetComponent<Image>().sprite = bookSprites[bookTotalCnt % bookSprites.Length];
            book.GetComponent<Book>().Data = bookData;
            
            SetBookFocus(bookTotalCnt - 1);
            
            bookList.Add(book);
            bookDataList.Add(book.GetComponent<Book>());
            
            bookTotalCnt++;
            UIManager.Instance.diaryManager.SetDiaryInfo(bookData, bookData.DiaryDataList);
            UIManager.Instance.diaryManager.SetDiaryFocus((int)DateTime.Today.DayOfWeek + 1);
        }
        
        private void CreateBook(BookData bookData)
        {
            var book = Instantiate(bookPrefab, booksParent.transform);
            book.GetComponent<Image>().sprite = bookSprites[bookTotalCnt % bookSprites.Length];
            bookList.Add(book);
            SetBookFocus(0);
            bookTotalCnt++;
        }
    
        public void OnBookInteraction(InputManager.EInteractionType type)
        {
            if (UIManager.Instance.uiState != EuiState.BookShelf)
                return;
            if (type == InputManager.EInteractionType.LeftSwipe)
            {
                SetBookFocus(currentBookIndex + 1);
            }
            else if (type == InputManager.EInteractionType.RightSwipe)
            {
                SetBookFocus(currentBookIndex - 1);
            }
            InputManager.Instance.isSwiping = false;
            
        }
    
        public void SetBookFocus(int targetIndex)
        {
            if (targetIndex < 0 || targetIndex >= bookList.Count)
                return;
            for (int i = 0; i < bookList.Count; i++)
            {
                var bookTransform = bookList[i].GetComponent<RectTransform>();
                bookTransform.DOPause();
                bookTransform.DOAnchorPosX(moveDistance * -(targetIndex - i), 0.5f).SetEase(Ease.InOutQuad);
            }

            foreach (var book in bookList)
            {
                book.transform.DOScale(sideScale, 0.5f).SetEase(Ease.InOutQuad);
            }
            bookList[targetIndex].transform.DOScale(centerScale, 0.5f).SetEase(Ease.InOutQuad);
            currentBookIndex = targetIndex;
        }

        public void OnEvent(EEventType eventType, Component sender, object param = null)
        {
            if (eventType == EEventType.ScreenInterection)
            {
                InputManager.EInteractionType type = (InputManager.EInteractionType)param;
                OnBookInteraction(type);
            }
        }
    }
}
