using System;
using System.Collections.Generic;
using DG.Tweening;
using Script.SQLite;
using UnityEngine;
using UnityEngine.UI;
using Util.EventSystem;

namespace Script.UI
{
	public class BookManager : MonoBehaviour, IEventListener
	{
		[Header("Book Data")] [SerializeField] public List<Book> bookList;

		[Header("Book Sprites")] [SerializeField]
		private Sprite[] bookSprites;

		[SerializeField] private int bookTotalCnt;

		[Header("Book Creation")] [SerializeField]
		private GameObject booksParent;

		[SerializeField] private GameObject bookPrefab;

		[Header("Book Movement")] public List<GameObject> bookObjectList = new();

		[SerializeField] private int currentBookIndex;
		[SerializeField] private Vector3 sideScale = new(0.8f, 0.8f, 1f);
		[SerializeField] private Vector3 centerScale = Vector3.one;
		[SerializeField] private float moveDistance = 660f;

		private void Start()
		{
			EventManager.Instance.AddListener(EEventType.DataLoad, this);
			EventManager.Instance.AddListener(EEventType.DataSave, this);
			EventManager.Instance.AddListener(EEventType.ScreenInteraction, this);
		}

		public void OnEvent(EEventType eventType, Component sender, object param = null)
		{
			switch (eventType)
			{
				case EEventType.None:
					break;
				case EEventType.ScreenInteraction:
					if (param != null)
						OnBookInteraction((EInteractionType)param);
					break;
				case EEventType.UIStateChange:
					break;
				case EEventType.DataSave:
					SaveBooks();
					break;
				case EEventType.DataLoad:
					LoadBooks();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}
		}

		private void SaveBooks()
		{
			foreach (var book in bookList)
			{
				DataManager.Instance.SetBook(book.Data);
			}
		}

		private void LoadBooks()
		{
			var bookIDList = DataManager.Instance.GetBookList();

			foreach (var bookID in bookIDList)
			{
				var bookData = DataManager.Instance.GetBook(bookID);
				CreateBook(bookData);
			}
		}

		public BookData CreateBook()
		{
			var book = Instantiate(bookPrefab, booksParent.transform);
			var bookData = new BookData();

			book.GetComponent<Image>().sprite = bookSprites[bookTotalCnt % bookSprites.Length];
			book.GetComponent<Book>().Data = bookData;

			SetBookFocus(bookTotalCnt - 1);

			bookObjectList.Add(book);
			bookList.Add(book.GetComponent<Book>());

			bookTotalCnt++;
			return bookData;
		}

		public void CreateBook(BookData bookData)
		{
			var book = Instantiate(bookPrefab, booksParent.transform);

			book.GetComponent<Image>().sprite = bookSprites[bookTotalCnt % bookSprites.Length];
			book.GetComponent<Book>().Data = bookData;

			bookObjectList.Add(book);
			bookList.Add(book.GetComponent<Book>());
			bookTotalCnt++;

			SetBookFocus(0);
		}

		public void OnBookInteraction(EInteractionType type)
		{
			if (UIManager.Instance.uiState != EuiState.BookShelf)
				return;
			switch (type)
			{
				case EInteractionType.LeftSwipe:
					SetBookFocus(currentBookIndex + 1);
					break;
				case EInteractionType.RightSwipe:
					SetBookFocus(currentBookIndex - 1);
					break;
			}
		}

		public void SetBookFocus(int targetIndex)
		{
			if (targetIndex < 0 || targetIndex >= bookObjectList.Count)
				return;
			for (var i = 0; i < bookObjectList.Count; i++)
			{
				var bookTransform = bookObjectList[i].GetComponent<RectTransform>();
				bookTransform.DOPause();
				bookTransform.DOAnchorPosX(moveDistance * -(targetIndex - i), 0.5f).SetEase(Ease.InOutQuad);
			}

			foreach (var book in bookObjectList) book.transform.DOScale(sideScale, 0.5f).SetEase(Ease.InOutQuad);
			bookObjectList[targetIndex].transform.DOScale(centerScale, 0.5f).SetEase(Ease.InOutQuad);
			currentBookIndex = targetIndex;
		}
	}
}