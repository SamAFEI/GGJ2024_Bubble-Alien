/*
    綁地圖圖標上
    控制小地圖的圖標
    圖標往左移動
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CosmicTrash;

public class MapIcon : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed; //飛行速度
    public TrashType type;  //型態(BonusA、BonusB、Meteorite)
    public int maxSize; //生成的最大尺寸

    public float lineRow;   //生成在第幾行    
    public int thisSize;    //此物件生成的大小

    public bool isBig;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.name== "Cabin")//如果圖標碰到船就消失
        {
            if (this.isBig == false)
            {
                Destroy(this.gameObject);
            }            
        }
        else
        {
            ScreenBoundary.Instance.SpawnCosmicTrash(this);//如果圖標碰到畫面邊界就在真實世界對應位置生成
        }
        
    }

    void Start()
    {
        rb=GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        rb.velocity = new Vector2(-1* speed, 0f);

        //離開小地圖範圍刪除圖標
        if (GetComponent<RectTransform>().anchoredPosition.x < -1120f)
        {
            Destroy(gameObject);
        }
    }
}
