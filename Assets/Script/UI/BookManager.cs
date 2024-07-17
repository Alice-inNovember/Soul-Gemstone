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
        [Header("Book Data")] [SerializeField] private List<Book> books;
        
        [Header("Book Sprites")]
        [SerializeField] private Sprite[] bookSprites;
        [SerializeField] private int bookTotalCnt = 0;
        [Header("Book Creation")]
        [SerializeField] private Canvas bookCanvas;
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
            //var bookIDList = DatabaseManager.Instance.GetBookIDList();
            //foreach (var bookID in bookIDList)
            //{
            //    CreateBook(DatabaseManager.Instance.GetBookData(bookID));
            //}
        }

        public void OnPreparation()
        {
            UIManager.Instance.MovePreparation(true);
        }
    
        public void CreateBook()
        {
            var book = Instantiate(bookPrefab, bookCanvas.transform);
            book.GetComponent<Image>().sprite = bookSprites[bookTotalCnt % bookSprites.Length];
            bookList.Add(book);
            book.GetComponent<RectTransform>()
                .DOAnchorPos(new Vector3(moveDistance * (bookTotalCnt - currentBookIndex), 0, 0), 0)
                .OnComplete(() => SetBookFocus(bookTotalCnt - 1));
            //book.GetComponent<Book>().Bookinit();
            //DatabaseManager.Instance.InsertBookData(Boo);
            bookTotalCnt++;
        }
        
        private void CreateBook(BookData bookData)
        {
            var book = Instantiate(bookPrefab, bookCanvas.transform);
            book.GetComponent<Image>().sprite = bookSprites[bookTotalCnt % bookSprites.Length];
            bookList.Add(book);
            
            book.GetComponent<RectTransform>()
                .DOAnchorPos(new Vector3(moveDistance * (bookTotalCnt - currentBookIndex), 0, 0), 0)
                .OnComplete(() => SetBookFocus(bookTotalCnt - 1));
            bookTotalCnt++;
        }
    
        public void OnBookInteraction(InputManager.EInteractionType type)
        {
            if (GameManager.Instance.currentStatus != EPlayerStatus.Book)
                return;
            if (GameManager.Instance.currentStatus == EPlayerStatus.Book)
            {
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
        }
    
        private void HandleLeftSwipe()
        {
            //if (bookList.Count <= 1 || currentBookIndex >= bookList.Count - 1)
            //    return;
            //int targetIndex = currentBookIndex + 1;
            //for (int i = 0; i < bookList.Count; i++)
            //{
            //    var bookTransform = bookList[i].GetComponent<RectTransform>();
            //    bookTransform.DOPause();
            //    bookTransform.DOAnchorPosX(_moveDistance * -(targetIndex - i), 0.5f).SetEase(Ease.InOutQuad);
            //}
            //bookList[currentBookIndex].transform.DOScale(sideScale, 0.5f).SetEase(Ease.InOutQuad);
            //bookList[currentBookIndex + 1].transform.DOScale(centerScale, 0.5f).SetEase(Ease.InOutQuad);
            //currentBookIndex++;
        }

        private void HandleRightSwipe()
        {
            //if (bookList.Count <= 1 || currentBookIndex <= 0)
            //    return;
            //int targetIndex = currentBookIndex - 1;
            //for (int i = 0; i < bookList.Count; i++)
            //{
            //    var bookTransform = bookList[i].GetComponent<RectTransform>();
            //    bookTransform.DOPause();
            //    bookTransform.DOAnchorPosX(_moveDistance * -(targetIndex - i), 0.5f).SetEase(Ease.InOutQuad);
            //}
            //bookList[currentBookIndex].transform.DOScale(sideScale, 0.5f).SetEase(Ease.InOutQuad);
            //bookList[currentBookIndex - 1].transform.DOScale(centerScale, 0.5f).SetEase(Ease.InOutQuad);
            //currentBookIndex--;
            SetBookFocus(currentBookIndex - 1);
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
