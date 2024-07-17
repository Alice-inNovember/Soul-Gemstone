using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class Book : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.Instance.CreateDiary();
            });
        }
    
        public int bookID;

        [SerializeField]
        private TMP_Text dateText;
    
        public void Bookinit(int id, string date)
        {
            bookID = id;
            dateText.text = date;
        }

        public string GetDate()
        {
            return dateText.text;
        }
    }
}

