using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using Util.SingletonSystem;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    public DiaryManager diaryManager;
    public BookManager bookManager;
    public GameObject preparation;
    public GameObject createDiaryBtn;
    
    private Vector3 openPos = new Vector3(0, 230f, 0);
    private Vector3 closePos = new Vector3(0, -1600f, 0);
    
    public void MovePreparation(bool isOpen)
    {
        if (isOpen)
        {
            preparation.SetActive(true);
            preparation.GetComponent<RectTransform>().DOAnchorPos(openPos, 1f);
        }
        else
        {
            preparation.GetComponent<RectTransform>().DOAnchorPos(closePos, 1f).onComplete += () =>
            {
                preparation.SetActive(false);
                bookManager.CreateBook();
            };
        }
    }

    public void CreateDiary()
    {
        diaryManager.CreateDiary();
        createDiaryBtn.SetActive(false);
        foreach (var book in bookManager.bookList)
        {
            book.SetActive(false);
        }
        foreach (var diary in diaryManager.diaryArray)
        {
            diary.SetActive(true);
        }

        GameManager.Instance.currentStatus = GameManager.PlayerStatus.Diary;
    }
}
