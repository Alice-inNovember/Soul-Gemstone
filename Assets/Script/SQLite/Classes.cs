using System;
using System.Collections.Generic;
using System.Linq;

namespace Script.SQLite
{
    public class BookData
    {
        public string Period { get; private set; } //일기 기간
        public bool IsCompleted { get; private set; } //작성 완료 여부
        public int Seed { get; private set; } //보석 시드
        public List<DiaryData> DiaryDatas { get; private set; } //BookData에 해당하는 DiaryDate 리스트
        public string CheckListComp { get; private set; } // 목표 달성
        public string CheckListComp2 { get; private set; } // 목표 달성 2
        public string WeekSum { get; private set; } // 1주일 요약
        public string BookID { get; private set; } // BookID

        public BookData(string mPeriod, bool mIsCompleted, int mSeed, string mCheckListComp,string mCheckListComp2, string mWeekSum, string mBookID)
        {
            DiaryDatas = new List<DiaryData>();
            Period = mPeriod;
            IsCompleted = mIsCompleted;
            Seed = mSeed;
            CheckListComp = mCheckListComp;
            CheckListComp2 = mCheckListComp2;
            WeekSum = mWeekSum;
            BookID = mBookID;
        }
        
        public DiaryData GetDiaryData(EDays days)
        {
            return DiaryDatas.FirstOrDefault(diaryData => diaryData.Day == days);
        }
    }
    public class DiaryData
    {
        public string BookID { get; private set; } // BookData 공유
        public string CheckListLog { get; private set; } // 체크리스트 내용
        public bool CheckListChecked { get; private set; } // 체크리스트 수행내용
        public string CheckListLog2 { get; private set; } // 체크리스트 내용2
        public bool CheckListChecked2 { get; private set; } // 체크리스트 수행내용2
        public string Log { get; private set; } // 일기 내용 
        public int Rate { get; private set; } // 하루 평점
        public int Rate2 { get; private set; } // 하루 평점 2
        public EWeather Weather { get; private set; } // 날씨
        public EDays Day { get; private set; } // 요일
        public string Date { get; private set; } // 날짜
        public DiaryData(string mBookID, string mCheckListLog, bool mCheckListChecked,string mCheckListLog2, bool mCheckListChecked2, string mLog, int mRate,int mRate2, EWeather mEWeather, EDays mEDays, DateTime mDate)
        {
            BookID = mBookID;
            CheckListLog = mCheckListLog;
            CheckListChecked = mCheckListChecked;
            CheckListLog2 = mCheckListLog2;
            CheckListChecked2 = mCheckListChecked2;
            Log = mLog;
            Rate = mRate;
            Rate2 = mRate2;
            Weather = mEWeather;
            Day = mEDays;
            Date = mDate.ToString("yyyy MMMM dd"); // ISO 8601 형식으로 변환하여 저장
        }
    }
}