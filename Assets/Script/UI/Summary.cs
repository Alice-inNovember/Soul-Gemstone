using TMPro;
using UnityEngine;

namespace Script.UI
{
    public class Summary : MonoBehaviour
    {
        [SerializeField] private TMP_Text dateText;
        [SerializeField] private TMP_Text summaryWeekText;
    
        public void SetSummary(string date, string summaryWeek)
        {
            dateText.text = date;
            summaryWeekText.text = summaryWeek;
        }
    }
}
