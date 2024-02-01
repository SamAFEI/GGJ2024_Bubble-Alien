/*
    綁地圖邊界上
    控制地圖邊界
    生成太空垃圾(BonusA、BonusB、Meteorite)
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CosmicTrash;

public class ScreenBoundary : MonoBehaviour
{
    public GameObject[] cosmicTrashs;  //要生成的太空垃圾
    public float spatialInterval;   //生成的空間間隔
    public static ScreenBoundary Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnCosmicTrash(MapIcon mapIcon)
    {
        GameObject newTrash = Instantiate(cosmicTrashs[(int)mapIcon.type], Vector3.zero, Quaternion.identity);

        // 設定新物件的大小
        float sizeMultiplier = Mathf.Pow(mapIcon.thisSize, 2)*cosmicTrashs[(int)mapIcon.type].transform.localScale.x;
        newTrash.transform.localScale = new Vector3(sizeMultiplier, sizeMultiplier, sizeMultiplier);

        // 設定新物件的位置
        newTrash.transform.position = new Vector3(
            this.transform.position.x + newTrash.GetComponent<SpriteRenderer>().bounds.size.x / 2,
            this.transform.position.y + (spatialInterval * mapIcon.lineRow),
            this.transform.position.z
        );

        if (mapIcon.isBig) { newTrash.gameObject.tag = "BigMeteorite"; }
    }
}
