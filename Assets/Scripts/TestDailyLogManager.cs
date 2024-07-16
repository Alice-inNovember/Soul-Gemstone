using System;
using System.Collections.Generic;
using UnityEngine;

public class TestDailyLogManager : MonoBehaviour
{
    private DailyLogManager logManager;

    private void Start()
    {
        // 씬 전체에서 DailyLogManager 찾기
        logManager = FindObjectOfType<DailyLogManager>();

        if (logManager == null)
        {
            Debug.LogError("DailyLogManager를 찾을 수 없습니다.");
            return;
        }

        DailyLogData test1 = new DailyLogData("Check List Test 1",true,"Sample log 1", 5, EWeather.clear, EDays.mon, DateTime.Now);
        DailyLogData test2 = new DailyLogData("Check List Test 2",false,"Sample log 2", 3, EWeather.rain, EDays.tue, DateTime.Now.AddDays(-1));

        logManager.AddLog(test1);
        logManager.AddLog(test2);

        // 날짜로 로그 검색
        List<DailyLogData> logsByDate = logManager.GetLogsByDate(DateTime.Now);
        Debug.Log("Logs by Date:");
        foreach (var log in logsByDate)
        {
            Debug.Log($"Check List Log: {log.checkListLog}, Checked: {log.checkListChecked}, Log: {log.log}, Rate: {log.rate}, Days: {log.eDays}, Weather: {log.eWeather}, Date: {log.date}");
        }

        List<DailyLogData> logsByDate1 = logManager.GetLogsByDate(DateTime.Now.AddDays(-1));
        foreach (var VARIABLE in logsByDate1)
        {
            Debug.Log($"Check List Log: {VARIABLE.checkListLog}, Checked: {VARIABLE.checkListChecked}, Log: {VARIABLE.log}, Rate: {VARIABLE.rate}, Days: {VARIABLE.eDays}, Weather: {VARIABLE.eWeather}, Date: {VARIABLE.date}");
        }
    }
}