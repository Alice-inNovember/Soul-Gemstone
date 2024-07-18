using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using Util.SingletonSystem;

namespace Script.SQLite
{
    public class DataManager : MonoBehaviourSingleton<DataManager>
    {
        [SerializeField] private bool doReset;
        private string _dbPath;

        private void Start()
        {
            _dbPath = Application.persistentDataPath + "/DiaryDB.sqlite";
            if (doReset)
                ResetDB();
            if (!System.IO.File.Exists(_dbPath))
                CreateDB();
            
            //AddBook(new BookData());
        }

        private void CreateDB()
        {
            SqliteConnection.CreateFile(_dbPath);
            CreateBookTable();
            CreateDiaryTable();
            Debug.Log("Database created at " + _dbPath);
        }
        
        private void CreateBookTable()
        {
            using var connection = new SqliteConnection("URI=file:" + _dbPath);

            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Books (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            BookGUID TEXT NOT NULL UNIQUE,
                            TaskA TEXT,
                            TaskB TEXT,
                            GemSeed INTEGER,
                            IsCompleted BOOLEAN,
                            StartingDay TEXT,
                            WeeklySummary TEXT
                           )";
            command.ExecuteNonQuery();
            connection.Close();
            Debug.Log("Table created or already exists.");
        }

        private void CreateDiaryTable()
        {
            using var connection = new SqliteConnection("URI=file:" + _dbPath);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
        CREATE TABLE IF NOT EXISTS Diaries (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        BookGUID TEXT NOT NULL,
        IsTaskADone BOOLEAN,
        IsTaskBDone BOOLEAN,
        Context TEXT,
        RateA INTEGER,
        RateB INTEGER,
        Weather INTEGER,
        Date TEXT,
        UNIQUE(BookGUID, Date)
    )";
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void ResetDB()
        {
            System.IO.File.Delete(_dbPath);
            CreateDB();
        }
        
        public void AddBook(BookData bookData)
        {
            using var connection = new SqliteConnection("URI=file:" + _dbPath);

            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO Books (BookGUID, TaskA, TaskB, GemSeed, IsCompleted, StartingDay, WeeklySummary)
            VALUES (@BookGUID, @TaskA, @TaskB, @GemSeed, @IsCompleted, @StartingDay, @WeeklySummary)";
            command.Parameters.Add(new SqliteParameter("@BookGUID", DbType.String) { Value = bookData.BookGUID });
            command.Parameters.Add(new SqliteParameter("@TaskA", DbType.String) { Value = bookData.TaskA });
            command.Parameters.Add(new SqliteParameter("@TaskB", DbType.String) { Value = bookData.TaskB });
            command.Parameters.Add(new SqliteParameter("@GemSeed", DbType.Int32) { Value = bookData.GemSeed });
            command.Parameters.Add(new SqliteParameter("@IsCompleted", DbType.Boolean) { Value = bookData.IsCompleted });
            command.Parameters.Add(new SqliteParameter("@StartingDay", DbType.String) { Value = bookData.StartingDay });
            command.Parameters.Add(new SqliteParameter("@WeeklySummary", DbType.String) { Value = bookData.WeeklySummary });
            command.ExecuteNonQuery();
            connection.Close();

            foreach (var diaryData in bookData.DiaryDataList)
            {
                AddDiary(diaryData);
            }
            
            Debug.Log("Book data inserted.");
        }
        
        public string[] GetBookGuidList()
        {
            List<string> bookGuids = new List<string>();

            using var connection = new SqliteConnection("URI=file:" + _dbPath);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT BookGUID 
            FROM Books";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var bookGuid = reader.GetString(0);
                bookGuids.Add(bookGuid);
            }

            connection.Close();
            return bookGuids.ToArray();
        }

        /*public BookData GetBook(string bookGuid)
        {
            BookData bookData = null;

            
            Debug.Log("GET BOOK 실행중");
            using var connection = new SqliteConnection("URI=file:" + _dbPath);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
            SELECT BookGUID, TaskA, TaskB, GemSeed, IsCompleted, StartingDay, WeeklySummary 
            FROM Books 
            WHERE BookGUID = @BookGUID";
            command.Parameters.Add(new SqliteParameter("@BookGUID", DbType.String) { Value = bookGuid });

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var guid = reader.GetString(0);
                var taskA = reader.IsDBNull(1) ? null : reader.GetString(1);
                var taskB = reader.IsDBNull(2) ? null : reader.GetString(2);
                var gemSeed = reader.GetInt32(3);
                var isCompleted = reader.GetBoolean(4);
                var startingDay = reader.GetString(5);
                var weeklySummary = reader.IsDBNull(6) ? null : reader.GetString(6);
                bookData = new BookData(guid, startingDay, isCompleted, gemSeed, GetDiaryDataList(guid), taskA, taskB, weeklySummary);
            }

            connection.Close();
            return bookData;
        }*/
        
        public BookData GetBook(string bookGuid)
        {
            BookData bookData = null;
            
                Debug.Log("Creating connection to database.");
                using var connection = new SqliteConnection("URI=file:" + _dbPath);
        
                Debug.Log("Opening connection to database.");
                connection.Open();

                Debug.Log("Creating SQL command.");
                using var command = connection.CreateCommand();
                command.CommandText = @"
        SELECT BookGUID, TaskA, TaskB, GemSeed, IsCompleted, StartingDay, WeeklySummary 
        FROM Books 
        WHERE BookGUID = @BookGUID";
                command.Parameters.Add(new SqliteParameter("@BookGUID", DbType.String) { Value = bookGuid });

                Debug.Log("Executing SQL command.");
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    bookData = new BookData();
                    Debug.Log("Reading data from database.");
                    bookData.BookGUID = reader.GetString(0);
                    bookData.TaskA = reader.IsDBNull(1) ? null : reader.GetString(1);
                    bookData.TaskB = reader.IsDBNull(2) ? null : reader.GetString(2);
                    bookData.GemSeed = reader.GetInt32(3);
                    bookData.IsCompleted = reader.GetBoolean(4);
                    bookData.StartingDay = reader.GetString(5);
                    bookData.WeeklySummary = reader.IsDBNull(6) ? null : reader.GetString(6);
                    bookData.DiaryDataList = GetDiaryDataList(bookData);

                    // GetDiaryDataList 호출
                    Debug.Log("Calling GetDiaryDataList.");
                    Debug.Log("GetDiaryDataList called successfully.");

                }

                Debug.Log("Closing connection to database.");
                connection.Close();
            

            return bookData;
        }



        public void SetBook(BookData bookData)
        {
            using var connection = new SqliteConnection("URI=file:" + _dbPath);
            connection.Open();
    
            using var command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO Books (BookGUID, TaskA, TaskB, GemSeed, IsCompleted, StartingDay, WeeklySummary)
            VALUES (@BookGUID, @TaskA, @TaskB, @GemSeed, @IsCompleted, @StartingDay, @WeeklySummary)
            ON CONFLICT(BookGUID) DO UPDATE SET
            TaskA = excluded.TaskA,
            TaskB = excluded.TaskB,
            GemSeed = excluded.GemSeed,
            IsCompleted = excluded.IsCompleted,
            StartingDay = excluded.StartingDay,
            WeeklySummary = excluded.WeeklySummary";
    
            command.Parameters.Add(new SqliteParameter("@BookGUID", DbType.String) { Value = bookData.BookGUID });
            command.Parameters.Add(new SqliteParameter("@TaskA", DbType.String) { Value = bookData.TaskA });
            command.Parameters.Add(new SqliteParameter("@TaskB", DbType.String) { Value = bookData.TaskB });
            command.Parameters.Add(new SqliteParameter("@GemSeed", DbType.Int32) { Value = bookData.GemSeed });
            command.Parameters.Add(new SqliteParameter("@IsCompleted", DbType.Boolean) { Value = bookData.IsCompleted });
            command.Parameters.Add(new SqliteParameter("@StartingDay", DbType.String) { Value = bookData.StartingDay });
            command.Parameters.Add(new SqliteParameter("@WeeklySummary", DbType.String) { Value = bookData.WeeklySummary });

            command.ExecuteNonQuery();
            connection.Close();

            foreach (var diaryData in bookData.DiaryDataList)
            {
                SetDiary(diaryData);
            }
            
            Debug.Log("Book data inserted or updated.");
        }

        public void DeleteBook(string bookGuid)
        {
            using var connection = new SqliteConnection("URI=file:" + _dbPath);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
            DELETE FROM Books 
            WHERE BookGUID = @BookGUID";
            command.Parameters.Add(new SqliteParameter("@BookGUID", DbType.String) { Value = bookGuid });
            command.ExecuteNonQuery();
            connection.Close();
            Debug.Log("Book data deleted.");
        }

        public void AddDiary(DiaryData diaryData)
        {
            using var connection = new SqliteConnection("URI=file:" + _dbPath);

            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO Diaries (BookGUID, IsTaskADone, IsTaskBDone, Context, RateA, RateB, Weather, Date)
            VALUES (@BookGUID, @IsTaskADone, @IsTaskBDone, @Context, @RateA, @RateB, @Weather, @Date)";
            command.Parameters.Add(new SqliteParameter("@BookGUID", DbType.String)
                { Value = diaryData.TargetBook.BookGUID });
            command.Parameters.Add(
                new SqliteParameter("@IsTaskADone", DbType.Boolean) { Value = diaryData.IsTaskADone });
            command.Parameters.Add(
                new SqliteParameter("@IsTaskBDone", DbType.Boolean) { Value = diaryData.IsTaskBDone });
            command.Parameters.Add(new SqliteParameter("@Context", DbType.String) { Value = diaryData.Context });
            command.Parameters.Add(new SqliteParameter("@RateA", DbType.Int32) { Value = diaryData.RateA });
            command.Parameters.Add(new SqliteParameter("@RateB", DbType.Int32) { Value = diaryData.RateB });
            command.Parameters.Add(new SqliteParameter("@Weather", DbType.Int32) { Value = (int)diaryData.Weather });
            command.Parameters.Add(new SqliteParameter("@Date", DbType.String) { Value = diaryData.Date });
            command.ExecuteNonQuery();
            connection.Close();
            Debug.Log("Diary data inserted.");
        }

         public List<DiaryData> GetDiaryDataList(BookData bookData)
         {
             List<DiaryData> diaryDataList = new List<DiaryData>();

             using var connection = new SqliteConnection("URI=file:" + _dbPath);
             connection.Open();
             using var command = connection.CreateCommand();
             command.CommandText = @"
             SELECT BookGUID, IsTaskADone, IsTaskBDone, Context, RateA, RateB, Weather, Date
             FROM Diaries
             WHERE BookGUID = @BookGUID";
             command.Parameters.Add(new SqliteParameter("@BookGUID", DbType.String) { Value = bookData.BookGUID });

             using var reader = command.ExecuteReader();
             while (reader.Read())
             {
                 var targetBook = bookData;
                 var isTaskADone = reader.GetBoolean(1);
                 var isTaskBDone = reader.GetBoolean(2);
                 var context = reader.IsDBNull(3) ? null : reader.GetString(3);
                 var rateA = reader.GetInt32(4);
                 var rateB = reader.GetInt32(5);
                 var weather = (EWeather)reader.GetInt32(6);
                 var date = reader.GetString(7);
                 diaryDataList.Add(new DiaryData(targetBook, isTaskADone, isTaskBDone, context, rateA, rateB, weather, date));
             }
             connection.Close();
             return diaryDataList;
         }
        
//         public List<DiaryData> GetDiaryDataList(string bookGuid, string taskA, string taskB, int gemSeed, bool isCompleted, string startingDay, string weeklySummary)
// {
//     List<DiaryData> diaryDataList = new List<DiaryData>();
//     
//         Debug.Log("Creating connection to database (GetDiaryDataList).");
//         using var connection = new SqliteConnection("URI=file:" + _dbPath);
//         
//         Debug.Log("Opening connection to database (GetDiaryDataList).");
//         connection.Open();
//
//         Debug.Log("Creating SQL command (GetDiaryDataList).");
//         using var command = connection.CreateCommand();
//         command.CommandText = @"
//         SELECT IsTaskADone, IsTaskBDone, Context, RateA, RateB, Weather, Date
//         FROM Diaries
//         WHERE BookGUID = @BookGUID";
//         command.Parameters.Add(new SqliteParameter("@BookGUID", DbType.String) { Value = bookGuid });
//
//         Debug.Log("Executing SQL command (GetDiaryDataList).");
//         using var reader = command.ExecuteReader();
//         while (reader.Read())
//         {
//             Debug.Log("Reading data from database (GetDiaryDataList).");
//             var isTaskADone = reader.GetBoolean(0);
//             var isTaskBDone = reader.GetBoolean(1);
//             var context = reader.IsDBNull(2) ? null : reader.GetString(2);
//             var rateA = reader.GetInt32(3);
//             var rateB = reader.GetInt32(4);
//             var weather = (EWeather)reader.GetInt32(5);
//             var date = reader.GetString(6);
//
//             // BookData 객체 생성
//             var bookData = new BookData(bookGuid, startingDay, isCompleted, gemSeed, null, taskA, taskB, weeklySummary);
//             var diaryData = new DiaryData(bookData, isTaskADone, isTaskBDone, context, rateA, rateB, weather, date);
//             diaryDataList.Add(diaryData);
//         }
//
//         Debug.Log("Closing connection to database (GetDiaryDataList).");
//         connection.Close();
//     
//
//     return diaryDataList;
// }


        public void SetDiary(DiaryData diaryData)
        {
            using var connection = new SqliteConnection("URI=file:" + _dbPath);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
        INSERT INTO Diaries (BookGUID, IsTaskADone, IsTaskBDone, Context, RateA, RateB, Weather, Date)
    VALUES (@BookGUID, @IsTaskADone, @IsTaskBDone, @Context, @RateA, @RateB, @Weather, @Date)
    ON CONFLICT(BookGUID, Date) DO UPDATE SET
        IsTaskADone = excluded.IsTaskADone,
        IsTaskBDone = excluded.IsTaskBDone,
        Context = excluded.Context,
        RateA = excluded.RateA,
        RateB = excluded.RateB,
        Weather = excluded.Weather";

            command.Parameters.Add(new SqliteParameter("@BookGUID", DbType.String) { Value = diaryData.TargetBook.BookGUID });
            Debug.Log(diaryData.TargetBook.BookGUID);
            command.Parameters.Add(new SqliteParameter("@IsTaskADone", DbType.Boolean) { Value = diaryData.IsTaskADone });
            Debug.Log(diaryData.IsTaskADone);
            command.Parameters.Add(new SqliteParameter("@IsTaskBDone", DbType.Boolean) { Value = diaryData.IsTaskBDone });
            Debug.Log(diaryData.IsTaskBDone);
            command.Parameters.Add(new SqliteParameter("@Context", DbType.String) { Value = diaryData.Context });
            Debug.Log(diaryData.Context);
            command.Parameters.Add(new SqliteParameter("@RateA", DbType.Int32) { Value = diaryData.RateA });
            Debug.Log(diaryData.RateA);
            command.Parameters.Add(new SqliteParameter("@RateB", DbType.Int32) { Value = diaryData.RateB });
            Debug.Log(diaryData.RateB);
            command.Parameters.Add(new SqliteParameter("@Weather", DbType.Int32) { Value = (int)diaryData.Weather });
            Debug.Log((int)diaryData.Weather);
            command.Parameters.Add(new SqliteParameter("@Date", DbType.String) { Value = diaryData.Date });
            Debug.Log(diaryData.Date);

            command.ExecuteNonQuery();
            connection.Close();
            Debug.Log("Diary data inserted or updated.");
        }

        public void DeleteDiary(string bookGuid)
        {
            using var connection = new SqliteConnection("URI=file:" + _dbPath);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
            DELETE FROM Diaries 
            WHERE BookGUID = @BookGUID";
            command.Parameters.Add(new SqliteParameter("@BookGUID", DbType.String) { Value = bookGuid });
            command.ExecuteNonQuery();
            connection.Close();
            Debug.Log("Diary data deleted.");
        }
    }
}
