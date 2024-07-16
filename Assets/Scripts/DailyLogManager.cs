using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class DailyLogData
{
    public string checkListLog;
    public bool checkListChecked;
    public string log;
    public int rate;
    [FormerlySerializedAs("weather")] public EWeather eWeather;
    [FormerlySerializedAs("weeks")] [FormerlySerializedAs("days")] public EDays eDays;
    public string date; // DateTime을 문자열로 저장

    public DailyLogData(string mCheckListLog, bool mCheckListChecked,string mLog, int mRate, EWeather mEWeather, EDays mEDays, DateTime mDate)
    {
        checkListLog = mCheckListLog;
        checkListChecked = mCheckListChecked;
        log = mLog;
        rate = mRate;
        eWeather = mEWeather;
        eDays = mEDays;
        date = mDate.ToString("o"); // ISO 8601 형식으로 변환하여 저장
    }
}

[System.Serializable]
public class DailyLogDataList
{
    public List<DailyLogData> items;

    public DailyLogDataList(List<DailyLogData> items)
    {
        this.items = items;
    }
}

public class DailyLogManager : MonoBehaviour
{
    private string _filePath;
    private List<DailyLogData> _logDataList;

    private void Awake()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "dailylogs.json");
        _logDataList = new List<DailyLogData>();

        LoadLogs();
    }

    public void AddLog(DailyLogData logData)
    {
        _logDataList.Add(logData);
        SaveLogs();
    }

    public List<DailyLogData> GetLogsByDate(DateTime date)
    {
        List<DailyLogData> result = _logDataList.FindAll(log => DateTime.Parse(log.date).Date == date.Date);
        return result;
    }

    public List<DailyLogData> GetLogsByWeather(EWeather eWeather)
    {
        List<DailyLogData> result = _logDataList.FindAll(log => log.eWeather == eWeather);
        return result;
    }

    private void SaveLogs()
    {
        DailyLogDataList dataList = new DailyLogDataList(_logDataList);
        string json = JsonUtility.ToJson(dataList);
        WriteToFile(_filePath, json);
    }

    private void LoadLogs()
    {
        string json = ReadFromFile(_filePath);
        if (json != null)
        {
            Debug.Log(_filePath);
            DailyLogDataList dataList = JsonUtility.FromJson<DailyLogDataList>(json);
            _logDataList = dataList.items ?? new List<DailyLogData>();
        }
    }

    private void WriteToFile(string path, string json)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }
        else
        {
            Debug.LogWarning("File not found: " + path);
            return null;
        }
    }
}
