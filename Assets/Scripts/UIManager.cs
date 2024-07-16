using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text TODOCheckListText;
    public TMP_Text DayLogText;
    public TMP_Text DayRateText;
    public TMP_Text DateText;
    public TMP_Dropdown WeatherDropdown;
    public TMP_Dropdown WeekDropDown;
    public Toggle TODOChecked;
    public Button DoneButton;
    
    private DailyLogManager logManager;

    private void Start()
    {
        DoneButton.onClick.AddListener(DoneOnClicked);
        // 씬 전체에서 DailyLogManager 찾기
        logManager = FindObjectOfType<DailyLogManager>();

        if (logManager == null)
        {
            Debug.LogError("DailyLogManager를 찾을 수 없습니다.");
            return;
        }

        
        DailyLogData test2 = new DailyLogData("Check List Test 2",false,"Sample log 2", 3, EWeather.rain, EDays.tue, DateTime.Now.AddDays(-1));

        
        logManager.AddLog(test2);
    }

    private void DoneOnClicked()
    {
        Debug.Log(DateText.text);
        DateTime date;
        DateTime.TryParse(DateText.text, out date);
        DailyLogData test1 = new DailyLogData(TODOCheckListText.text,TODOChecked.isOn,DayLogText.text, int.Parse(DayRateText.text), (EWeather)WeatherDropdown.value, (EDays)WeekDropDown.value, date);
        logManager.AddLog(test1);
        
        List<DailyLogData> logsByDate = logManager.GetLogsByDate(DateTime.Now);
        Debug.Log("Logs by Date:");
        foreach (var log in logsByDate)
        {
            Debug.Log($"Check List Log: {log.checkListLog}, Checked: {log.checkListChecked}, Log: {log.log}, Rate: {log.rate}, Days: {log.eDays}, Weather: {log.eWeather}, Date: {log.date}");
        }
    }
}
