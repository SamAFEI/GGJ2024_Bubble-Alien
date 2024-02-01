/*
    綁小地圖上
    控制小地圖的圖標生成
    隨機種類
    隨機大小
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CosmicTrash;

public class MiniMap : MonoBehaviour
{
    public GameObject[] mapIcons;  //要生成的地圖圖標
    public float spaceInterval;   //生成的空間間隔
    public float timeInterval; //生成的時間間隔
    public GameObject bigMeteorite; //巨大隕石

    private void Start()
    {
        InvokeRepeating("SpawnMapIcon", 0f, timeInterval);

        Invoke("SpawnBigMeteorite",116f);
        
    }

    public void SpawnMapIcon()
    {
        int randInt = Random.Range(0, 4);   //隨機種類
        GameObject newMapIcon = Instantiate(mapIcons[randInt], Vector3.zero, Quaternion.identity, transform);   //生成
        RectTransform rectTransform = newMapIcon.GetComponent<RectTransform>();

        //隨機大小
        int maxSize = newMapIcon.GetComponent<MapIcon>().maxSize;
        int randSize = Random.Range(1, maxSize+1);   
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * randSize, rectTransform.sizeDelta.y * randSize);  //放大
        newMapIcon.GetComponent<MapIcon>().thisSize=randSize;


        //隨機一行
        L:
        int lineRow = Random.Range(-2+(randSize-1), 3);
        if(randInt==0 || randInt == 1)  //如果是獎勵球
        {
            if(randSize==1) //大小是1的時候
            {
                if (lineRow == 0)   //是中間行的時候
                {
                    goto L; //重骰
                }
            }
            else if(randSize == 2)
            {
                if (lineRow == 0 || lineRow == 1)   //是中間行的時候
                {
                    //Debug.Log(randSize + "//" + lineRow);
                    goto L; //重骰
                }
            }
        }else if (randInt == 2 || randInt == 3) //如果是隕石
        {
            if (randSize == 1) //大小是1的時候
            {
                lineRow = 0;
            }
        }
        
        rectTransform.anchoredPosition = new Vector2(1080f, spaceInterval* ((float)lineRow -((float)randSize-1)*0.5f));    //擺到對應的行上
        newMapIcon.GetComponent<MapIcon>().lineRow = ((float)lineRow - ((float)randSize - 1) * 0.5f);
    }

    public void SpawnBigMeteorite()
    {
        CancelInvoke("SpawnMapIcon");
        GameObject newMapIcon = Instantiate(mapIcons[2], Vector3.zero, Quaternion.identity, transform);   //生成
        RectTransform rectTransform = newMapIcon.GetComponent<RectTransform>();
        int randSize = 7;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * randSize, rectTransform.sizeDelta.y * randSize);  //放大
        newMapIcon.GetComponent<MapIcon>().thisSize = randSize;
        rectTransform.anchoredPosition = new Vector2(1080f, spaceInterval * 0);    //擺到對應的行上
        newMapIcon.GetComponent<MapIcon>().lineRow =0;
        newMapIcon.GetComponent<MapIcon>().isBig = true;
    }

}
