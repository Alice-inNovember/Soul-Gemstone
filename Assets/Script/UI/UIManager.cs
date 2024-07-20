using System;
using System.Linq;
using Script.Gem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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
		public const float AnimationTime = 0.5f;
		public EuiState uiState = EuiState.BookShelf;
		public DiaryManager diaryManager;
		public BookManager bookManager;
		[SerializeField] private Button startEditButton;
		[SerializeField] private Button taskInputFinishButton;
		[SerializeField] private Button toBookShelfButton;
		[SerializeField] private Button saveButton;
		[SerializeField] private Button gemInspectorButton;
		[SerializeField] private Button gemInspectorExitButton;
		[SerializeField] private GameObject diaryPage;
		[SerializeField] private GameObject gemInspector;
		[SerializeField] private TMP_InputField taskAInput;
		[SerializeField] private TMP_InputField taskBInput;

		private void Start()
		{
			gemInspectorButton.onClick.AddListener(InspectGem);
			gemInspectorExitButton.onClick.AddListener(InspectOut);
			startEditButton.onClick.AddListener(StartEdit);
			taskInputFinishButton.onClick.AddListener(FinishTaskInputButton);
			toBookShelfButton.onClick.AddListener(() => SetUIState(EuiState.BookShelf));
			saveButton.onClick.AddListener(SaveButton);

			bookManager = GetComponent<BookManager>();
			diaryManager = GetComponent<DiaryManager>();
			SetUIState(EuiState.BookShelf);
		}

		private static bool IsToday(string dateTime)
		{
			return DateTime.Parse(dateTime).ToString("yy-MM-dd") == DateTime.Today.ToString("yy-MM-dd");
		}

		private void StartEdit()
		{
			foreach (var book in bookManager.bookList)
			{
				if (!book.Data.DiaryDataList.Any(diaryData => IsToday(diaryData.Date)))
					continue;
				SetUIState(EuiState.Diary);
				diaryManager.SetDiaryInfo(book.Data, book.Data.DiaryDataList);
				diaryManager.SetDiaryFocus((int)DateTime.Today.DayOfWeek + 1);
				return;
			}
			SetUIState(EuiState.TaskInput);
		}

		private void FinishTaskInputButton()
		{
			SetUIState(EuiState.Diary);
			var bookData = bookManager.CreateBook();
			bookData.TaskA = taskAInput.text;
			bookData.TaskB = taskBInput.text;
			diaryManager.SetDiaryInfo(bookData, bookData.DiaryDataList);
			diaryManager.SetDiaryFocus((int)DateTime.Today.DayOfWeek + 1);
		}

		private void InspectGem()
		{
			FindObjectOfType<InputManager>().enabled = false;
			FindObjectOfType<UniqueGem>().killRotate();
			diaryPage.SetActive(false);
			gemInspectorExitButton.gameObject.SetActive(true);
			gemInspector.SetActive(true);
		}

		private void InspectOut()
		{
			FindObjectOfType<InputManager>().enabled = true;
			FindObjectOfType<UniqueGem>().DoRotate();
			diaryPage.SetActive(true);
			gemInspectorExitButton.gameObject.SetActive(false);
			gemInspector.SetActive(false);
		}

		private void SaveButton()
		{
			Debug.Log("Save Button Clicked");
			EventManager.Instance.PostNotification(EEventType.DataSave, this);
		}

		public void SetUIState(EuiState state)
		{
			uiState = state;
			EventManager.Instance.PostNotification(EEventType.UIStateChange, this, state);
		}
	}
}