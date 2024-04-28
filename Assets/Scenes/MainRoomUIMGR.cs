using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MainRoomUIMGR : MonoBehaviour
{

    public TextMeshProUGUI PlayNum;


    private void Start()
    {
        PlayNum.text = GameManager.GetInstance().gameTime.ToString();
    }
}
