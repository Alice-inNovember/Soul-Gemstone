using System;
using Script.SQLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
	public class Book : MonoBehaviour
	{
		[SerializeField] private TMP_Text dateText;

		public BookData Data;

		private void Start()
		{
			GetComponent<Button>().onClick.AddListener(OnButtonClick);
			UpdateUI();
		}

		private void OnButtonClick()
		{
			UIManager.Instance.SetUIState(EuiState.Diary);
			UIManager.Instance.diaryManager.SetDiaryInfo(Data, Data.DiaryDataList);
			UIManager.Instance.diaryManager.SetDiaryFocus(0);
		}

		public void UpdateUI()
		{
			var startingDay = DateTime.Parse(Data.StartingDay);
			dateText.text = startingDay.ToString("yyyy MMMM dd") + "\n" + startingDay.AddDays(6).ToString("yyyy MMMM dd");
			UIManager.Instance.diaryManager.SetDiaryInfo(Data, Data.DiaryDataList);
		}
	}
}