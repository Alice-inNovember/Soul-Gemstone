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
        [SerializeField] private bool isCheck1;
        [SerializeField] private bool isCheck2;
        [SerializeField] private Slider evaluation1;
        [SerializeField] private Slider evaluation2;

        public void SetDiary(DiaryData diaryData)
        {
            dateText.text = diaryData.Date;
            diaryText.text = diaryData.Context;
            checkListText1.text = diaryData.TargetBook.TaskA;
            checkListText2.text = diaryData.TargetBook.TaskB;
            isCheck1 = diaryData.IsTaskADone;
            isCheck2 =  diaryData.IsTaskBDone;
            evaluation1.value = diaryData.RateA;
            evaluation2.value =  diaryData.RateA;
        }
    }
}