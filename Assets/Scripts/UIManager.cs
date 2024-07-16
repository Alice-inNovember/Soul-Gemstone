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
    public Slider DayRate;
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
        Debug.Log(TODOCheckListText.text + "\n");
        Debug.Log(TODOChecked.isOn + "\n");
        Debug.Log(DayLogText.text + "\n");
        Debug.Log((EWeather)WeatherDropdown.value + "\n");
        Debug.Log((EDays)WeekDropDown.value + "\n");
        Debug.Log(DateTime.Now + "\n");
        
        DailyLogData test1 = new DailyLogData(TODOCheckListText.text,TODOChecked.isOn,DayLogText.text, (int)DayRate.value, (EWeather)WeatherDropdown.value, (EDays)WeekDropDown.value, DateTime.Now);
        
        
        
        logManager.AddLog(test1);
        
        List<DailyLogData> logsByDate = logManager.GetLogsByDate(DateTime.Now);
        Debug.Log("Logs by Date:");
        foreach (var log in logsByDate)
        {
            Debug.Log($"Check List Log: {log.checkListLog}, Checked: {log.checkListChecked}, Log: {log.log}, Rate: {log.rate}, Days: {log.eDays}, Weather: {log.eWeather}, Date: {log.date}");
        }
    }
}
