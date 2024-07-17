using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public enum EWeather
{
    //맑음, 흐림, 비, 눈, 안개, 황사
    clear,
    cloud,
    rain,
    snow,
    fog,
    dust

}

public enum EDays
{
    //요일
    sun,
    mon,
    tue,
    wen,
    thr,
    fri,
    sat
}
public class DailyLog : MonoBehaviour
{
    private string _filePath;
    private DailyLogData _data;

    private void Awake()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "dailylog.json");
        Debug.Log(_filePath);
    }

    private void Start()
    {
        
    }

    public void SetDailyLog(string mGroup, string mCheckListLog, bool mChecked, string mLog, int mRate, EWeather mEWeather, EDays mEDays, int mYear, int mMonth, int mDate)
    {
        _data = new DailyLogData(mGroup, mCheckListLog, mChecked, mLog, mRate, mEWeather, mEDays,
            new DateTime(mYear, mMonth, mDate));
    }

    public void GetDailyLog(DailyLogData deserializedLog)
    {
        _data = deserializedLog;
    }

    private void Serialization(DailyLogData mLogData)
    {
        WriteToFile(_filePath, JsonUtility.ToJson(mLogData));
    }

    private DailyLogData Deserialization()
    {
        string json = ReadFromFile(_filePath);
        if (json != null)
        {
            // JSON을 DailyLog 객체로 역직렬화
            DailyLogData deserializedLog = JsonUtility.FromJson<DailyLogData>(ReadFromFile(_filePath));
            return deserializedLog;
        }

        return null;
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
