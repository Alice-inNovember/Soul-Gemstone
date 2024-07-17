using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util.EventSystem;
using EventType = Util.EventSystem.EventType;

public class Diary : MonoBehaviour
{
    [Header("Diary Infomation")]
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_InputField diaryText;
    //[SerializeField] private EWeather weather;
    [SerializeField] private TMP_Text checkListText1;
    [SerializeField] private TMP_Text checkListText2;
    [SerializeField] private bool isCheck1;
    [SerializeField] private bool isCheck2;
    [SerializeField] private Slider evaluation1;
    [SerializeField] private Slider evaluation2;
    
    

    public void SetDiary(string date, string diary, string checkList1, string checkList2, bool check1, bool check2, int eval1, int eval2)
    {
        dateText.text = date;
        diaryText.text = diary;
        checkListText1.text = checkList1;
        checkListText2.text = checkList2;
        isCheck1 = check1;
        isCheck2 = check2;
        evaluation1.value = eval1;
        evaluation2.value = eval2;
    }
}