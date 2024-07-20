using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Script.Gem;
using Script.SQLite;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util.EventSystem;

namespace Script.UI
{
	public class Diary : MonoBehaviour
	{
		[Header("Diary Information")] [SerializeField]
		public EDays days;

		[SerializeField] private TMP_Text dateText;
		[SerializeField] private TMP_InputField diaryText;
		[SerializeField] private EWeather weather;
		[SerializeField] private TMP_Text weatherText;
		[SerializeField] private Button sunnyButton;
		[SerializeField] private Button cloudyButton;
		[SerializeField] private Button rainyButton;
		[SerializeField] private TMP_Text checkListText1;
		[SerializeField] private TMP_Text checkListText2;
		[SerializeField] private Toggle isCheck1;
		[SerializeField] private Toggle isCheck2;
		[SerializeField] private List<Button> evaluationA;
		[SerializeField] private List<Button> evaluationB;

		private DiaryData _diaryData;
		private DateTime _thisDateTime;

		private void Start()
		{
			_thisDateTime = DateTime.Today;
			if (dateText.text == _thisDateTime.ToString("yyyy MMMM dd"))
			{
				sunnyButton.onClick.AddListener(() =>
                			{
                				_diaryData.Weather = EWeather.Sunny;
                				weatherText.text = "맑음";
                			});
                			cloudyButton.onClick.AddListener(() =>
                			{
                				_diaryData.Weather = EWeather.Cloudy;
                				weatherText.text = "흐림";
                			});
                			rainyButton.onClick.AddListener(() =>
                			{
                				_diaryData.Weather = EWeather.Rainy;
                				weatherText.text = "비";
                			});
                			diaryText.onEndEdit.AddListener(text =>
                			{
                				if (_diaryData == null)
                					return;
                				_diaryData.Context = text;
                			});
                			isCheck1.onValueChanged.AddListener(value =>
                			{
                				if (_diaryData == null)
                					return;
                				_diaryData.IsTaskADone = value;
                			});
                			isCheck2.onValueChanged.AddListener(value =>
                			{
                				if (_diaryData == null)
                					return;
                				_diaryData.IsTaskBDone = value;
                			});
			}
			else
			{
				sunnyButton.interactable = false;
				cloudyButton.interactable = false;
				rainyButton.interactable = false;
				diaryText.interactable = false;
				isCheck1.interactable = false;
				isCheck2.interactable = false;
				foreach (var button in evaluationA)
				{
					button.interactable = false;
				}
				foreach (var button in evaluationB)
				{
					button.interactable = false;
				}
			}
		}

		public void SetEvalA(int score)
		{
			_diaryData.RateA = score;
			foreach (var rateA in evaluationA)
				rateA.GetComponent<Image>().color = Color.white;
			evaluationA[_diaryData.RateA].GetComponent<Image>().color = Color.cyan;
		}
		
		public void SetEvalB(int score)
		{
			_diaryData.RateB = score;
			foreach (var rateB in evaluationB)
				rateB.GetComponent<Image>().color = Color.white;
			evaluationB[_diaryData.RateB].GetComponent<Image>().color = Color.cyan;
		}
		
		public void SetDiary(DiaryData diaryData)
		{
			_diaryData = diaryData;
			dateText.text = diaryData.Date;
			diaryText.text = diaryData.Context;
			
			switch (diaryData.Weather)
			{
				case EWeather.Sunny:
					weatherText.text = "맑음";

					break;
				case EWeather.Rainy:
					weatherText.text = "비";
					break;
				case EWeather.Cloudy:
					weatherText.text = "흐림";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			checkListText1.text = diaryData.TargetBook.TaskA;
			checkListText2.text = diaryData.TargetBook.TaskB;
			isCheck1.isOn = diaryData.IsTaskADone;
			isCheck2.isOn = diaryData.IsTaskBDone;
			foreach (var rateA in evaluationA)
				rateA.GetComponent<Image>().color = Color.white;
			evaluationA[diaryData.RateA].GetComponent<Image>().color = Color.cyan;
			foreach (var rateB in evaluationB)
				rateB.GetComponent<Image>().color = Color.white;
			evaluationB[diaryData.RateB].GetComponent<Image>().color = Color.cyan;
		}
	}
}