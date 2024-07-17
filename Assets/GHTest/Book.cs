using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Util.EventSystem;
using EventType = Util.EventSystem.EventType;

public class Book : MonoBehaviour, IEventListener
{
    public int bookIndex;
    
    private float moveDistance = 660f;
    private Vector3 sideScale = new Vector3(0.7f, 0.7f, 1f);
    private Vector3 centerScale = Vector3.one;
    
    void Start()
    {
        //EventManager.Instance.AddListener(EventType.BookInterection, this);
    }

    public void OnEvent(EventType eventType, Component sender, object param = null)
    {
        
    }
    
    
}

