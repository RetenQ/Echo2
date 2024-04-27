using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShortSave
{
    public float theScore_sum;
    public List<GameObject> rogueItems_canChose = new List<GameObject>();
    public List<GameObject> rogueItems_chosen = new List<GameObject>();
    public int stage = 1; //����һ��stage��
    public int scene_index;
    public string mapseed;// ��ͼ����

}