using System;
using Script.SQLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class Diary : MonoBehaviour
    {
        [Header("Diary Information")]
        [SerializeField] public EDays days;
        [SerializeField] private TMP_Text dateText;
        [SerializeField] private TMP_InputField diaryText;
        [SerializeField] private EWeather weather;
        [SerializeField] private TMP_Text checkListText1;
        [SerializeField] private TMP_Text checkListText2;
        [SerializeField] private Toggle isCheck1;
        [SerializeField] private Toggle isCheck2;
        [SerializeField] private Slider evaluation1;
        [SerializeField] private Slider evaluation2;

        private DiaryData _diaryData;

        private void Start()
        {
            diaryText.onEndEdit.AddListener(text => {
                if (_diaryData == null)
                    return;
                _diaryData.Context = text;
            });
            isCheck1.onValueChanged.AddListener(value => {
                if (_diaryData == null)
                    return;
                _diaryData.IsTaskADone = value;
            });
            isCheck2.onValueChanged.AddListener(value => {
                if (_diaryData == null)
                    return;
                _diaryData.IsTaskBDone = value;
            });
            evaluation1.onValueChanged.AddListener(value => {
                if (_diaryData == null)
                    return;
                _diaryData.RateA = (int)value;
            });
            evaluation2.onValueChanged.AddListener(value => {
                if (_diaryData == null)
                    return;
                _diaryData.RateB = (int)value;
            });
        }

        public void SetDiary(DiaryData diaryData)
        {
            _diaryData = diaryData;
            dateText.text = diaryData.Date;
            diaryText.text = diaryData.Context;
            checkListText1.text = diaryData.TargetBook.TaskA;
            checkListText2.text = diaryData.TargetBook.TaskB;
            isCheck1.isOn = diaryData.IsTaskADone;
            isCheck2.isOn =  diaryData.IsTaskBDone;
            evaluation1.value = diaryData.RateA;
            evaluation2.value =  diaryData.RateA;
        }
    }
}