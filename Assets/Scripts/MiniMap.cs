/*
    �j�p�a�ϤW
    ����p�a�Ϫ��ϼХͦ�
    �H������
    �H���j�p
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CosmicTrash;

public class MiniMap : MonoBehaviour
{
    public GameObject[] mapIcons;  //�n�ͦ����a�Ϲϼ�
    public float spaceInterval;   //�ͦ����Ŷ����j
    public float timeInterval; //�ͦ����ɶ����j
    public GameObject bigMeteorite; //���j�k��

    private void Start()
    {
        InvokeRepeating("SpawnMapIcon", 0f, timeInterval);

        Invoke("SpawnBigMeteorite",116f);
        
    }

    public void SpawnMapIcon()
    {
        int randInt = Random.Range(0, 4);   //�H������
        GameObject newMapIcon = Instantiate(mapIcons[randInt], Vector3.zero, Quaternion.identity, transform);   //�ͦ�
        RectTransform rectTransform = newMapIcon.GetComponent<RectTransform>();

        //�H���j�p
        int maxSize = newMapIcon.GetComponent<MapIcon>().maxSize;
        int randSize = Random.Range(1, maxSize+1);   
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * randSize, rectTransform.sizeDelta.y * randSize);  //��j
        newMapIcon.GetComponent<MapIcon>().thisSize=randSize;


        //�H���@��
        L:
        int lineRow = Random.Range(-2+(randSize-1), 3);
        if(randInt==0 || randInt == 1)  //�p�G�O���y�y
        {
            if(randSize==1) //�j�p�O1���ɭ�
            {
                if (lineRow == 0)   //�O�����檺�ɭ�
                {
                    goto L; //����
                }
            }
            else if(randSize == 2)
            {
                if (lineRow == 0 || lineRow == 1)   //�O�����檺�ɭ�
                {
                    //Debug.Log(randSize + "//" + lineRow);
                    goto L; //����
                }
            }
        }else if (randInt == 2 || randInt == 3) //�p�G�O�k��
        {
            if (randSize == 1) //�j�p�O1���ɭ�
            {
                lineRow = 0;
            }
        }
        
        rectTransform.anchoredPosition = new Vector2(1080f, spaceInterval* ((float)lineRow -((float)randSize-1)*0.5f));    //�\���������W
        newMapIcon.GetComponent<MapIcon>().lineRow = ((float)lineRow - ((float)randSize - 1) * 0.5f);
    }

    public void SpawnBigMeteorite()
    {
        CancelInvoke("SpawnMapIcon");
        GameObject newMapIcon = Instantiate(mapIcons[2], Vector3.zero, Quaternion.identity, transform);   //�ͦ�
        RectTransform rectTransform = newMapIcon.GetComponent<RectTransform>();
        int randSize = 7;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * randSize, rectTransform.sizeDelta.y * randSize);  //��j
        newMapIcon.GetComponent<MapIcon>().thisSize = randSize;
        rectTransform.anchoredPosition = new Vector2(1080f, spaceInterval * 0);    //�\���������W
        newMapIcon.GetComponent<MapIcon>().lineRow =0;
        newMapIcon.GetComponent<MapIcon>().isBig = true;
    }

}
