using System;
using System.Collections.Generic;
using DG.Tweening;
using Script.SQLite;
using UnityEngine;
using Util.EventSystem;

namespace Script.UI
{
	public class DiaryManager : MonoBehaviour, IEventListener
	{
		[SerializeField] private Diary[] diaryList;
		public GameObject[] diaryArray;
		[SerializeField] private int currentDiaryIndex;

		private void Start()
		{
			EventManager.Instance.AddListener(EEventType.ScreenInteraction, this);
		}

		public void OnEvent(EEventType eventType, Component sender, object param = null)
		{
			if (eventType != EEventType.ScreenInteraction)
				return;
			if (param != null)
				OnDiaryInteraction((EInteractionType)param);
		}

		private void OnDiaryInteraction(EInteractionType type)
		{
			if (UIManager.Instance.uiState != EuiState.Diary)
				return;
			switch (type)
			{
				case EInteractionType.LeftSwipe:
					SetDiaryFocus(currentDiaryIndex + 1);
					break;
				case EInteractionType.RightSwipe:
					SetDiaryFocus(currentDiaryIndex - 1);
					break;
			}
		}

		public void SetDiaryFocus(int targetIndex)
		{
			if (targetIndex < 0 || targetIndex >= diaryArray.Length)
				return;
			for (var i = 0; i < diaryArray.Length; i++)
			{
				var diaryTransform = diaryArray[i].GetComponent<RectTransform>();
				diaryTransform.DOPause();
				diaryTransform.DOAnchorPosX(1200 * -(targetIndex - i), 0.5f);
			}

			currentDiaryIndex = targetIndex;
		}

		public void SetDiaryInfo(BookData bookData, List<DiaryData> diaryDataList)
		{
			var startingDay = DateTime.Parse(bookData.StartingDay);
			var fullDateTime = startingDay.ToString("yyyy") + " " + startingDay.ToString("MMMM dd") + " ~ " +
			                   startingDay.AddDays(6).ToString("MMMM dd");
			diaryArray[0].GetComponent<DiaryInfo>().SetInfo(fullDateTime, "Default Weekly Summary");
			foreach (var diary in diaryList)
				diary.SetDiary(diaryDataList.Find(data => data.DayType == diary.days));
		}
	}
}