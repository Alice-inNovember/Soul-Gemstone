using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Mono.Data.Sqlite;
using Script.Gem;
using UnityEngine;
using Util.EventSystem;
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
			Debug.Log(_dbPath);
			if (doReset)
				ResetDB();
			else if (!File.Exists(_dbPath))
				CreateDB();
			EventManager.Instance.PostNotification(EEventType.DataLoad, this);
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
			using var connection = new SqliteConnection("Data Source=" + _dbPath);

			connection.Open();
			using var command = connection.CreateCommand();
			command.CommandText = @"CREATE TABLE IF NOT EXISTS Books (
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
			using var connection = new SqliteConnection("Data Source=" + _dbPath);
			connection.Open();

			using var command = connection.CreateCommand();
			command.CommandText = @"CREATE TABLE IF NOT EXISTS Diaries (
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
			File.Delete(_dbPath);
			CreateDB();
		}

		// public void AddBook(BookData bookData)
		// {
		// 	using var connection = new SqliteConnection("Data Source=" + _dbPath);
  //
		// 	connection.Open();
		// 	using var command = connection.CreateCommand();
		// 	command.CommandText = @"
  //           INSERT INTO Books (BookGUID, TaskA, TaskB, GemSeed, IsCompleted, StartingDay, WeeklySummary)
  //           VALUES (@BookGUID, @TaskA, @TaskB, @GemSeed, @IsCompleted, @StartingDay, @WeeklySummary)";
		// 	command.Parameters.Add(new SqliteParameter("@BookGUID", DbType.String) { Value = bookData.BookGUID });
		// 	command.Parameters.Add(new SqliteParameter("@TaskA", DbType.String) { Value = bookData.TaskA });
		// 	command.Parameters.Add(new SqliteParameter("@TaskB", DbType.String) { Value = bookData.TaskB });
		// 	command.Parameters.Add(new SqliteParameter("@GemSeed", DbType.Int32) { Value = bookData.GemSeed });
		// 	command.Parameters.Add(new SqliteParameter("@IsCompleted", DbType.Boolean)
		// 		{ Value = bookData.IsCompleted });
		// 	command.Parameters.Add(new SqliteParameter("@StartingDay", DbType.String) { Value = bookData.StartingDay });
		// 	command.Parameters.Add(new SqliteParameter("@WeeklySummary", DbType.String)
		// 		{ Value = bookData.WeeklySummary });
		// 	command.ExecuteNonQuery();
		// 	connection.Close();
  //
		// 	foreach (var diaryData in bookData.DiaryDataList) AddDiary(diaryData);
  //
		// 	Debug.Log("Book data inserted.");
		// }

		public string[] GetBookList()
		{
			var bookGuids = new List<string>();

			using var connection = new SqliteConnection("Data Source=" + _dbPath);
			connection.Open();
			using var command = connection.CreateCommand();
			command.CommandText = @"SELECT BookGUID FROM Books";

			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				var bookGuid = reader.GetString(0);
				bookGuids.Add(bookGuid);
			}

			connection.Close();
			return bookGuids.ToArray();
		}

		public BookData GetBook(string bookGuid)
		{
			BookData bookData = null;

			Debug.Log("Creating connection to database.");
			using var connection = new SqliteConnection("Data Source=" + _dbPath);


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
			using var connection = new SqliteConnection("Data Source=" + _dbPath);
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
			command.Parameters.Add(new SqliteParameter("@IsCompleted", DbType.Boolean)
				{ Value = bookData.IsCompleted });
			command.Parameters.Add(new SqliteParameter("@StartingDay", DbType.String) { Value = bookData.StartingDay });
			command.Parameters.Add(new SqliteParameter("@WeeklySummary", DbType.String)
				{ Value = bookData.WeeklySummary });

			command.ExecuteNonQuery();
			connection.Close();

			foreach (var diaryData in bookData.DiaryDataList) SetDiary(diaryData);

			Debug.Log("Book data inserted or updated.");
		}

		public void DeleteBook(string bookGuid)
		{
			using var connection = new SqliteConnection("Data Source=" + _dbPath);
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

		private void AddDiary(DiaryData diaryData)
		{
			using var connection = new SqliteConnection("Data Source=" + _dbPath);

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

		private List<DiaryData> GetDiaryDataList(BookData bookData)
		{
			var diaryDataList = new List<DiaryData>();

			using var connection = new SqliteConnection("Data Source=" + _dbPath);
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
				diaryDataList.Add(new DiaryData(targetBook, isTaskADone, isTaskBDone, context, rateA, rateB, weather,
					date));
			}

			connection.Close();
			return diaryDataList;
		}

		private void SetDiary(DiaryData diaryData)
		{
			using var connection = new SqliteConnection("Data Source=" + _dbPath);
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
			using var connection = new SqliteConnection("Data Source=" + _dbPath);
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

		public void IsCompletedCheck(BookData bookData)
		{
			var isWritten = 0;
			if (!bookData.IsCompleted)
			{
				foreach (var diaryData in bookData.DiaryDataList)
				{
					if (diaryData.Context != "")
					{
						isWritten += 1;
					}
				}

				if (isWritten == 7 && !bookData.IsCompleted)
				{
					bookData.IsCompleted = true;
					UpdateGemSeed(bookData);
				}
			}
		}

		public void UpdateGemSeed(BookData bookData)
		{
			if (bookData.IsCompleted)
			{
				bookData.GemSeed = GenerateGemSeed(bookData);
				using var connection = new SqliteConnection("Data Source=" + _dbPath);
				connection.Open();

				string query = @"UPDATE Books SET GemSeed = @GemSeed WHERE BookGUID = @BookGUID";

				using (var command = new SqliteCommand(query, connection))
				{
					command.Parameters.AddWithValue("GemSeed", bookData.GemSeed);
					command.Parameters.AddWithValue("@BookGUID", bookData.BookGUID);
					command.ExecuteNonQuery();
					connection.Close();
				}
			}
		}

		public int GenerateGemSeed(BookData bookData)
		{
			string combinedString = "";
			// 모든 요소를 문자열로 결합
			foreach (var diaryData in bookData.DiaryDataList)
			{
				combinedString += diaryData.Context;
				combinedString += diaryData.Date;
				combinedString += diaryData.DayType;
				combinedString += diaryData.IsTaskADone;
				combinedString += diaryData.IsTaskBDone;
				combinedString += diaryData.RateA;
				combinedString += diaryData.RateB;
				combinedString += diaryData.TargetBook;
				combinedString += diaryData.Weather;
			}
			// MD5 해시 생성
			using (MD5 md5Hasher = MD5.Create())
			{
				var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(combinedString));
				return BitConverter.ToInt32(hashed, 0);
			}
		}
	}
}