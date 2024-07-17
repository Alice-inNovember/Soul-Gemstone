using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Util.EventSystem;
using EventType = Util.EventSystem.EventType;

public class Diary : MonoBehaviour, IEventListener
{
    public int diaryIndex;
    void Start()
    {
        EventManager.Instance.AddListener(EventType.DirayInterection, this);
    }
    
    public void OnEvent(EventType eventType, Component sender, object param = null)
    {
        if (eventType == EventType.DirayInterection && InputManager.Instance.isSwiping == false)
        {
            InputManager.InteractionType type = (InputManager.InteractionType)param;
            if (GameManager.Instance.currentStatus == GameManager.PlayerStatus.Diary
                && diaryIndex == DiaryManager.Instance.currentDiaryIndex)
            {
                InputManager.Instance.isSwiping = true;
                if (type == InputManager.InteractionType.LeftSwipe && DiaryManager.Instance.currentDiaryIndex <
                    DiaryManager.Instance.diaryArray.Length - 1)
                {
                    DiaryManager.Instance.currentDiaryIndex++;
                    GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBounce).onComplete
                        += () => InputManager.Instance.isSwiping = false;
                }
                else if (type == InputManager.InteractionType.RightSwipe && DiaryManager.Instance.currentDiaryIndex > 0)
                {
                    DiaryManager.Instance.currentDiaryIndex--;
                    GetComponent<RectTransform>().DOAnchorPos(new Vector3(1200, 0, 0), 0.5f).SetEase(Ease.OutBounce).onComplete
                        += () => InputManager.Instance.isSwiping = false;
                }

                //InputManager.Instance.isSwiping = false;
            }
        }
    }
}