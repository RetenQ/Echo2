using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    [Header("过关UI")]
    public GameObject RoughItem;
    [SerializeField] private bool isOpenRI =false;
    public Button RIButton1;
    public Button RIButton2;
    public Button RIButton3;

    public TextMeshProUGUI RIText1;
    public TextMeshProUGUI RIText2;
    public TextMeshProUGUI RIText3;

    [Header("道具存储")]
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

    // 实现洗牌算法
    // Fisher-Yates洗牌算法
    static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void setItem()
    {
        // 将Item插入槽，这里默认选择前3个
        Shuffle<GameObject>(roughItems); // 洗牌
        // 加入
        addItemToUI(roughItems[0], RIButton1 , RIText1);
        addItemToUI(roughItems[1], RIButton2 , RIText2);
        addItemToUI(roughItems[2], RIButton3 , RIText3);

    }

    public void addItemToUI(GameObject itemObj , Button button , TextMeshProUGUI tmp)
    {
        button.image.sprite = itemObj.GetComponent<RoughItem>().itemImg;
        tmp.text = itemObj.GetComponent<RoughItem>().describe;

    }


}
