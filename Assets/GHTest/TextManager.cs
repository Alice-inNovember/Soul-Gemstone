using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI[] pageTexts;

    public void UpdatePageText(int pageIndex, string newText)
    {
        if (pageIndex >= 0 && pageIndex < pageTexts.Length)
        {
            pageTexts[pageIndex].text = newText;
        }
    }
}
