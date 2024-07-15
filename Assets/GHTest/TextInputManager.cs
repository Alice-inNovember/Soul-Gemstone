using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextInputManager : MonoBehaviour
{
    public TextManager textManager; 
    public TMP_InputField inputField; // 텍스트 입력 필드
    public Button updateButton; // 텍스트 업데이트 버튼
    public int currentPageIndex; // 현재 페이지 인덱스
    void Start()
    {
        updateButton.onClick.AddListener(OnUpdateButtonClick);
    }

    void OnUpdateButtonClick()
    {
        string newText = inputField.text;
        textManager.UpdatePageText(currentPageIndex, newText);
    }
}
