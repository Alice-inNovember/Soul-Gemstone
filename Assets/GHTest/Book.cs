using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Util.EventSystem;
using EventType = Util.EventSystem.EventType;

public class Book : MonoBehaviour, IEventListener
{
    public int bookIndex;

    private Vector3 rightPosition = new Vector3(660, 0, 0);
    private Vector3 leftPosition = new Vector3(-660, 0, 0);
    private Vector3 centerPosition = new Vector3(0, 0, 0);
    
    private Vector3 sideScale = new Vector3(0.7f, 0.7f, 1f);
    private Vector3 centerScale = new Vector3(1f, 1f, 1f);
    
    void Start()
    {
        EventManager.Instance.AddListener(EventType.BookInterection, this);
    }

    public void OnEvent(EventType eventType, Component sender, object param = null)
    {
        if (eventType == EventType.BookInterection && InputManager.Instance.isSwiping == false)
        {
            InputManager.InteractionType type = (InputManager.InteractionType)param;
            if (GameManager.Instance.currentStatus == GameManager.PlayerStatus.Book
                && bookIndex == BookManager.Instance.currentBookIndex)
            {
                InputManager.Instance.isSwiping = true;
                if (type == InputManager.InteractionType.LeftSwipe)
                {
                    if (BookManager.Instance.currentBookIndex == 0)
                    {
                        transform.DOMove(leftPosition, 0.5f).SetEase(Ease.InOutQuad);
                        transform.DOScale(sideScale, 0.5f).SetEase(Ease.InOutQuad);
                        if (BookManager.Instance.bookList.Count > 1)
                        {
                            //foreach()
                        }
                    }
                }
            }
        }
    }
}
