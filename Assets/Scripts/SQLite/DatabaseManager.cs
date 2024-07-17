using UnityEngine;
using System.Collections.Generic;
using System;
using Mono.Data.Sqlite;
using Unity.VisualScripting.Dependencies.Sqlite;

public class DatabaseManager : MonoBehaviour
{
    private string dbPath;
    private List<BookData> bookDataList = new List<BookData>();

    void Start()
    {
        dbPath = Application.dataPath + "/Plugins/DiaryDatabase.sqlite";
        CreateDatabase(); // 데이터베이스 파일을 생성하거나 연결
        CreateBookTable(); // BookData 테이블 생성
        CreateDiaryTable(); // DiaryData 테이블 생성
        ClearDatabase(); // 기존 데이터를 삭제
        InsertSampleData(); // 샘플 데이터 삽입
        LoadBookData(); // BookData 및 DiaryData 로드
    }

    void CreateDatabase()
    {
        if (!System.IO.File.Exists(dbPath))
        {
            SqliteConnection.CreateFile(dbPath);
            Debug.Log("Database created at " + dbPath);
        }
    }

    void CreateBookTable()
    {
        using (var connection = new SqliteConnection("Data Source=" + dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS BookData (
                                        BookID INTEGER PRIMARY KEY,
                                        Period TEXT,
                                        IsCompleted INTEGER,
                                        Seed INTEGER,
                                        CheckListComp TEXT,
                                        CheckListComp2 TEXT,
                                        WeekSum TEXT)";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    void CreateDiaryTable()
    {
        using (var connection = new SqliteConnection("Data Source=" + dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS DiaryData (
                                        DiaryID INTEGER PRIMARY KEY AUTOINCREMENT,
                                        BookID INTEGER,
                                        CheckListLog TEXT,
                                        CheckListChecked INTEGER,
                                        CheckListLog2 TEXT,
                                        CheckListChecked2 INTEGER,
                                        Log TEXT,
                                        Rate INTEGER,
                                        Rate2 INTEGER,
                                        Weather TEXT,
                                        Day TEXT,
                                        Date TEXT,
                                        FOREIGN KEY(BookID) REFERENCES BookData(BookID))";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    void ClearDatabase()
    {
        using (var connection = new SqliteConnection("Data Source=" + dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM BookData";
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM DiaryData";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    void InsertSampleData()
    {
        BookData book1 = new BookData("Week 1", true, 42, "Completed all tasks", "Do Great","Great week overall", 1);
        book1._diaryDatas.Add(new DiaryData(1, "Task1", true, "Task2", false, "Had a good day", 5, 4, EWeather.Sunny, EDays.Monday, DateTime.Now));
        book1._diaryDatas.Add(new DiaryData(1, "Task3", false, "Task4", true, "It was raining", 3, 2, EWeather.Rainy, EDays.Tuesday, DateTime.Now));
        InsertBookData(book1);

        BookData book2 = new BookData("Week 2", false, 24, "Some tasks left", "Do Somthing","Average week", 2);
        book2._diaryDatas.Add(new DiaryData(2, "Task1", true, "Task2", true, "Worked hard", 4, 3, EWeather.Cloudy, EDays.Wednesday, DateTime.Now));
        book2._diaryDatas.Add(new DiaryData(2, "Task3", true, "Task4", false, "Relaxed", 5, 4, EWeather.Sunny, EDays.Thursday, DateTime.Now));
        InsertBookData(book2);

        // 추가 데이터 삽입...
    }

    public void InsertBookData(BookData bookData)
    {
        using (var connection = new SqliteConnection("Data Source=" + dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO BookData (Period, IsCompleted, Seed, CheckListComp, CheckListComp2, WeekSum, BookID) 
                                        VALUES (@Period, @IsCompleted, @Seed, @CheckListComp, @CheckListComp2, @WeekSum, @BookID)";
                command.Parameters.AddWithValue("@Period", bookData._period);
                command.Parameters.AddWithValue("@IsCompleted", bookData._isCompleted ? 1 : 0);
                command.Parameters.AddWithValue("@Seed", bookData._seed);
                command.Parameters.AddWithValue("@CheckListComp", bookData._checkListComp);
                command.Parameters.AddWithValue("@CheckListComp2", bookData._checkListComp2);
                command.Parameters.AddWithValue("@WeekSum", bookData._weekSum);
                command.Parameters.AddWithValue("@BookID", bookData._bookID);
                command.ExecuteNonQuery();
            }

            foreach (var diaryData in bookData._diaryDatas)
            {
                InsertDiaryData(diaryData, connection);
            }

            connection.Close();
        }
    }
    public void InsertDiaryData(DiaryData diaryData, SqliteConnection connection = null)
    {
        bool shouldCloseConnection = false;
        if (connection == null)
        {
            connection = new SqliteConnection("Data Source=" + dbPath);
            connection.Open();
            shouldCloseConnection = true;
        }

        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"INSERT INTO DiaryData (BookID, CheckListLog, CheckListChecked, CheckListLog2, CheckListChecked2, Log, Rate, Rate2, Weather, Day, Date) 
                                    VALUES (@BookID, @CheckListLog, @CheckListChecked, @CheckListLog2, @CheckListChecked2, @Log, @Rate, @Rate2, @Weather, @Day, @Date)";
            command.Parameters.AddWithValue("@BookID", diaryData.BookID);
            command.Parameters.AddWithValue("@CheckListLog", diaryData.CheckListLog);
            command.Parameters.AddWithValue("@CheckListChecked", diaryData.CheckListChecked ? 1 : 0);
            command.Parameters.AddWithValue("@CheckListLog2", diaryData.CheckListLog2);
            command.Parameters.AddWithValue("@CheckListChecked2", diaryData.CheckListChecked2 ? 1 : 0);
            command.Parameters.AddWithValue("@Log", diaryData.Log);
            command.Parameters.AddWithValue("@Rate", diaryData.Rate);
            command.Parameters.AddWithValue("@Rate2", diaryData.Rate2);
            command.Parameters.AddWithValue("@Weather", diaryData.Weather.ToString());
            command.Parameters.AddWithValue("@Day", diaryData.Day.ToString());
            command.Parameters.AddWithValue("@Date", diaryData.Date);
            command.ExecuteNonQuery();
        }

        if (shouldCloseConnection)
        {
            connection.Close();
        }
    }

    public void LoadBookData()
    {
        bookDataList.Clear();
        using (var connection = new SqliteConnection("Data Source=" + dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM BookData";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int bookID = Convert.ToInt32(reader["BookID"]);
                        BookData bookData = new BookData(
                            reader["Period"].ToString(),
                            reader["IsCompleted"].ToString() == "1",
                            Convert.ToInt32(reader["Seed"]),
                            reader["CheckListComp"].ToString(),
                            reader["CheckListComp2"].ToString(),
                            reader["WeekSum"].ToString(),
                            bookID);
                        
                        LoadDiaryDataForBook(bookData);
                        bookDataList.Add(bookData);
                    }
                }
            }
            connection.Close();
        }
    }

    void LoadDiaryDataForBook(BookData bookData)
    {
        using (var connection = new SqliteConnection("Data Source=" + dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM DiaryData WHERE BookID = @BookID";
                command.Parameters.AddWithValue("@BookID", bookData._bookID);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DiaryData diaryData = new DiaryData(
                            bookData._bookID,
                            reader["CheckListLog"].ToString(),
                            reader["CheckListChecked"].ToString() == "1",
                            reader["CheckListLog2"].ToString(),
                            reader["CheckListChecked2"].ToString() == "1",
                            reader["Log"].ToString(),
                            Convert.ToInt32(reader["Rate"]),
                            Convert.ToInt32(reader["Rate2"]),
                            (EWeather)Enum.Parse(typeof(EWeather), reader["Weather"].ToString()),
                            (EDays)Enum.Parse(typeof(EDays), reader["Day"].ToString()),
                            DateTime.Parse(reader["Date"].ToString()));

                        bookData._diaryDatas.Add(diaryData);
                    }
                }
            }
            connection.Close();
        }
    }
}
