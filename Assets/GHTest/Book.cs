using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util.EventSystem;
using EventType = Util.EventSystem.EventType;

public class Book : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            UIManager.Instance.CreateDiary();
        });
    }
    
    public int bookid;

    [SerializeField]
    private TMP_Text dateText;
    
    public void Bookinit(int id, string date)
    {
        bookid = id;
        dateText.text = date;
    }
}

