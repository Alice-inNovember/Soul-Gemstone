using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;
using Util.EventSystem;
using Util.SingletonSystem;
using EventType = Util.EventSystem.EventType;

public class BookManager : MonoBehaviour, IEventListener
{
    [FormerlySerializedAs("books")] [Header("Book Sprites")]
    public Sprite[] bookSprites;
    [FormerlySerializedAs("bookIndex")] public int bookTotalCnt = 0;
    [Header("Book Creation")]
    public Canvas bookCanvas;
    public GameObject bookPrefab;
    
    [Header("Book Movement")]
    public List<GameObject> bookList = new List<GameObject>();
    [SerializeField] private int currentBookIndex = 0;
    [SerializeField] private Vector3 sideScale = new Vector3(0.8f, 0.8f, 1f);
    [SerializeField] private Vector3 centerScale = Vector3.one;
    [SerializeField] private float _moveDistance = 660f;
    
    void Start()
    {
        EventManager.Instance.AddListener(EventType.ScreenInterection, this);
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
            .DOAnchorPos(new Vector3(_moveDistance * (bookTotalCnt - currentBookIndex), 0, 0), 0)
            .OnComplete(() => SetBookFocus(bookTotalCnt - 1));
        bookTotalCnt++;
    }
    
    public void OnBookInteraction(InputManager.InteractionType type)
    {
        if (GameManager.Instance.currentStatus != GameManager.PlayerStatus.Book)
            return;
        if (GameManager.Instance.currentStatus == GameManager.PlayerStatus.Book)
        {
            if (type == InputManager.InteractionType.LeftSwipe)
            {
                SetBookFocus(currentBookIndex + 1);
            }
            else if (type == InputManager.InteractionType.RightSwipe)
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
            bookTransform.DOAnchorPosX(_moveDistance * -(targetIndex - i), 0.5f).SetEase(Ease.InOutQuad);
        }

        foreach (var book in bookList)
        {
            book.transform.DOScale(sideScale, 0.5f).SetEase(Ease.InOutQuad);
        }
        bookList[targetIndex].transform.DOScale(centerScale, 0.5f).SetEase(Ease.InOutQuad);
        currentBookIndex = targetIndex;
    }

    public void OnEvent(EventType eventType, Component sender, object param = null)
    {
        if (eventType == EventType.ScreenInterection)
        {
            InputManager.InteractionType type = (InputManager.InteractionType)param;
            OnBookInteraction(type);
        }
    }
}
