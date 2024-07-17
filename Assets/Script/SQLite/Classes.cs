using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;

namespace Script.SQLite
{
	public class BookData
	{
		public string BookGUID;
		public string TaskA;
		public string TaskB;
		public List<DiaryData> DiaryDataList;
		public int GemSeed;
		public bool IsCompleted;
		public string StartingDay;
		public string WeeklySummary;

		public BookData(string bookGuid, string startingDay, bool isCompleted, int gemSeed,
			List<DiaryData> diaryDataList, string checkListA, string checkListB, string weeklySummary)
		{
			BookGUID = bookGuid;
			StartingDay = startingDay;
			IsCompleted = isCompleted;
			GemSeed = gemSeed;
			DiaryDataList = diaryDataList;
			TaskA = checkListA;
			TaskB = checkListB;
			WeeklySummary = weeklySummary;
		}

		public BookData()
		{
			BookGUID = Guid.NewGuid().ToString();
			StartingDay = Utility.GetLastSunday().ToString("yyyy MMMM dd");
			IsCompleted = false;
			GemSeed = -1;
			DiaryDataList = new List<DiaryData>();
			for (int i = 0; i < 7; i++)
			{
				DiaryDataList.Add(new DiaryData(this, (EDays)i));
			}
			TaskA = "";
			TaskB = "";
			WeeklySummary = "";
		}

		public DiaryData GetDiaryData(EDays days)
		{
			return DiaryDataList.FirstOrDefault(diaryData => diaryData.DayType == days);
		}
	}

	public class DiaryData
	{
		public BookData TargetBook;
		public bool IsTaskADone;
		public bool IsTaskBDone;
		public string Context; //내용
		public int RateA;  // 하루 평점A
		public int RateB;// 하루 평점B
		public EWeather Weather; // 날씨
		public EDays DayType;
		public string Date; // 날짜
		public DiaryData(BookData targetBook, bool isTaskADone, bool isTaskBDone, string context, int rateA, int rateB, EWeather weather, string date)
		{
			TargetBook = targetBook;
			IsTaskADone = isTaskADone;
			IsTaskBDone = isTaskBDone;
			Context = context;
			RateA = rateA;
			RateB = rateB;
			Weather = weather;
			DayType = (EDays)DateTime.Parse(date).DayOfWeek;
			Date = date;
		}
		public DiaryData(BookData targetBook, EDays dayType)
		{
			TargetBook = targetBook;
			IsTaskADone = false;
			IsTaskBDone = false;
			Context = "";
			RateA = 0;
			RateB = 0;
			Weather = EWeather.Sunny;
			DayType = dayType;
			Date = DateTime.Parse(targetBook.StartingDay).AddDays((int)dayType).ToString("yyyy MMMM dd");
		}

		private DiaryData()
		{
			 
		}
	}

	public static class Utility
	{
		public static DateTime GetLastSunday()
		{
			var today = DateTime.Today;
			var daysSinceSunday = (int)today.DayOfWeek % 7;
			var lastSunday = today.AddDays(-daysSinceSunday);
			return lastSunday;
		}
	}
}