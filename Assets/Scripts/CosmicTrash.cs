/*
    綁在太空垃圾(BonusA、BonusB、Meteorite)上

    控制太空垃圾
    定義垃圾型態(BonusA、BonusB、Meteorite)
    往左飛
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicTrash : MonoBehaviour
{
    public enum TrashType
    {
        BonusA,
        BonusB,
        Meteorite
    }
    public TrashType type;
    private Rigidbody2D rb;
    public float speed;


    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(-1* speed, 0);

        //到飛船後方時清除
        if (transform.position.x < -40f)
        {
            Destroy(gameObject);
        }
    }
}
