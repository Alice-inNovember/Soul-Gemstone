using TMPro;
using UnityEngine;

namespace Script.UI
{
    public class DiaryInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text dateText;
        [SerializeField] private TMP_Text summaryWeekText;
    
        public void SetInfo(string datePeriod, string summaryWeek)
        {
            dateText.text = datePeriod;
            summaryWeekText.text = summaryWeek;
        }
    }
}
