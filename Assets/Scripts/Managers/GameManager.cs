using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    [Header("¹ý¹ØUI")]
    public GameObject RoughItem;
    [SerializeField] private bool isOpenRI =false;
    public Button RIButton1;
    public Button RIButton2;
    public Button RIButton3;
    List<GameObject> roughItems = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)){
            if (!isOpenRI)
            {
                OpenRoughItemUI();
            }
            else
            {
                CloseRoughItemUI();
            }
        }
    }

    public void OpenRoughItemUI()
    {
        isOpenRI = true; 
        Time.timeScale = 0.0f;
        RoughItem.SetActive(true);
    }

    public void CloseRoughItemUI()
    {
        isOpenRI = false;
        Time.timeScale = 1.0f;
        RoughItem.SetActive(false);
    }

    public void Set3RoughtItem()
    {
        
    }

    public void RIButton1Fun()
    {
        Debug.Log("1");
    }

    public void RIButton2Fun()
    {
        Debug.Log("2");

    }

    public void RIButton3Fun()
    {
        Debug.Log("3");

    }
}
