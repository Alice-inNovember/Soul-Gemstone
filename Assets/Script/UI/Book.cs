using System;
using Script.SQLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class Book : MonoBehaviour
    {
        [SerializeField] private TMP_Text datText;
        public BookData Data
        {
            get => _data;
            set
            {
                _data = value;
                UpdateUI();
            }
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            UIManager.Instance.SetUIState(EuiState.Diary);
            UIManager.Instance.diaryManager.SetDiaryInfo(_data, _data.DiaryDataList);
            UIManager.Instance.diaryManager.SetDiaryFocus(0);
        }

        private void UpdateUI()
        {
            var startingDay = DateTime.Parse(_data.StartingDay);
            datText.text = startingDay.ToString("yyyy") + "\n" + startingDay.ToString("MMMM DD") + " ~ " + startingDay.ToString("MMMM DD");
            UIManager.Instance.diaryManager.SetDiaryInfo(_data, _data.DiaryDataList);
        }

        private BookData _data;
    }
}

