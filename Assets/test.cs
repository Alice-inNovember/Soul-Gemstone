using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Util.EventSystem;
using EventType = Util.EventSystem.EventType;

public class test : MonoBehaviour, IEventListener
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EventType.ScreenInterection, this);
    }
    
    public void OnEvent(EventType eventType, Component sender, object param = null)
    {
        if (eventType == EventType.ScreenInterection && (InputManager.SwipeType)param == InputManager.SwipeType.LeftSwipe)
            GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBounce);
    }
}
