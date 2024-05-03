using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelUIMgr : MonoBehaviour
{
    [Header("����UI")]
    public TextMeshProUGUI levelScore; 

    [Header("ѡ��")]
    public GameObject RogueItem; // �����

    public Button RIButton1;
    public Button RIButton2;
    public Button RIButton3;

    public TextMeshProUGUI RIText1;
    public TextMeshProUGUI RIText2;
    public TextMeshProUGUI RIText3;

    public GameObject Item1;
    public GameObject Item2;
    public GameObject Item3;

    [Header("��Ƶ")]
    public float fadeInTime = 2.0f; // ����ʱ��
    public AudioSource audioSource;
    private float startVolume;



    private void Start()
    {
        UpdatetheRoughItemUI();

        StartCoroutine(FadeIn());

        setTheScoreUI();

        setItem();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.U)) setItem(); 

    }

    public void setTheScoreUI()
    {
        GameManager.GetInstance().UpdatePlayerScore();
        float tmpScore = GameManager.GetInstance().theScore;
        levelScore.text = tmpScore.ToString();
    }

    public void UpdatetheRoughItemUI()
    {
        RogueItem = GameObject.Find("TheRogueItem");
        RIButton1 = RogueItem.transform.Find("backGround").Find("Right").Find("Item1").GetComponent<Button>();
        RIButton2 = RogueItem.transform.Find("backGround").Find("Right").Find("Item2").GetComponent<Button>();
        RIButton3 = RogueItem.transform.Find("backGround").Find("Right").Find("Item3").GetComponent<Button>();

        RIText1 = RIButton1.gameObject.transform.Find("ItemDescribe").gameObject.GetComponent<TextMeshProUGUI>();
        RIText2 = RIButton2.gameObject.transform.Find("ItemDescribe").gameObject.GetComponent<TextMeshProUGUI>();
        RIText3 = RIButton3.gameObject.transform.Find("ItemDescribe").gameObject.GetComponent<TextMeshProUGUI>();

        levelScore = RogueItem.transform.Find("backGround").Find("Right").Find("YourScore").GetComponent<TextMeshProUGUI>();
    }

    public void RIButton1Fun()
    {
        GameManager.GetInstance().addItem(Item1);
        // GameManager .GetInstance().LoadNextScene();
        GameManager .GetInstance().LoadNextScene("Boss1");
    }

    public void RIButton2Fun()
    {
        GameManager.GetInstance().addItem(Item2);
        // GameManager.GetInstance().LoadNextScene();
        GameManager.GetInstance().LoadNextScene("Boss1");


    }

    public void RIButton3Fun()
    {
        GameManager.GetInstance().addItem(Item3);
        // GameManager.GetInstance().LoadNextScene();
        GameManager.GetInstance().LoadNextScene("Boss1");

    }

    public void setItem()
    {
        List<GameObject> rogueItems = GameManager.GetInstance().rogueItems_canChose; // �õ�MGR��ģ���û���õ�


        GameManager.Shuffle<GameObject>(rogueItems); // ϴ��

        Item1 = rogueItems[0];
        Item2 = rogueItems[1];
        Item3 = rogueItems[2];

        // ����
        addItemToUI(Item1, RIButton1, RIText1);
        addItemToUI(Item2, RIButton2, RIText2);
        addItemToUI(Item3, RIButton3, RIText3);

    }

    public void addItemToUI(GameObject itemObj, Button button, TextMeshProUGUI tmp)
    {
        button.image.sprite = itemObj.GetComponent<RogueItem>().itemImg;
        tmp.text = itemObj.GetComponent<RogueItem>().describe;

    }
    IEnumerator FadeIn()
    {
        startVolume = audioSource.volume;
        audioSource.volume = 0f; // ��ʼ����Ϊ0

        float currentTime = 0f;

        while (currentTime < fadeInTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, startVolume, currentTime / fadeInTime);
            yield return null;
        }

        audioSource.volume = startVolume; // ȷ������׼ȷ�ﵽĿ��ֵ
    }

}
