using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    public GameObject bookPrefab;
    public Sprite[] books;
    public List<GameObject> bookList = new List<GameObject>();
    public int bookIndex = 0;
    public int currentBookIndex = 0;
    public Canvas bookCanvas;
    
    private static BookManager _instance;
    
    public static BookManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BookManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<BookManager>();
                }
                //DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void CreateBook()
    {
        GameObject book = Instantiate(bookPrefab, bookCanvas.transform);
        bookList.Add(book);
        for (int i = 0; i < bookList.Count; i++)
        {
            book.GetComponent<Image>().sprite = books[bookIndex];
            bookIndex++;
            if (bookIndex > 5)
                bookIndex = 0;
        }
    }
}
