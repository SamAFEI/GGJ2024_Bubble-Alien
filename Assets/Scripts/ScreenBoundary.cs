/*
    �j�a����ɤW
    ����a�����
    �ͦ��ӪũU��(BonusA�BBonusB�BMeteorite)
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CosmicTrash;

public class ScreenBoundary : MonoBehaviour
{
    public GameObject[] cosmicTrashs;  //�n�ͦ����ӪũU��
    public float spatialInterval;   //�ͦ����Ŷ����j
    public static ScreenBoundary Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnCosmicTrash(MapIcon mapIcon)
    {
        GameObject newTrash = Instantiate(cosmicTrashs[(int)mapIcon.type], Vector3.zero, Quaternion.identity);

        // �]�w�s���󪺤j�p
        float sizeMultiplier = Mathf.Pow(mapIcon.thisSize, 2)*cosmicTrashs[(int)mapIcon.type].transform.localScale.x;
        newTrash.transform.localScale = new Vector3(sizeMultiplier, sizeMultiplier, sizeMultiplier);

        // �]�w�s���󪺦�m
        newTrash.transform.position = new Vector3(
            this.transform.position.x + newTrash.GetComponent<SpriteRenderer>().bounds.size.x / 2,
            this.transform.position.y + (spatialInterval * mapIcon.lineRow),
            this.transform.position.z
        );

        if (mapIcon.isBig) { newTrash.gameObject.tag = "BigMeteorite"; }
    }
}
