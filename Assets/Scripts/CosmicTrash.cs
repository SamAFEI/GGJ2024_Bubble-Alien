/*
    �j�b�ӪũU��(BonusA�BBonusB�BMeteorite)�W

    ����ӪũU��
    �w�q�U�����A(BonusA�BBonusB�BMeteorite)
    ������
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

        //�쭸����ɲM��
        if (transform.position.x < -40f)
        {
            Destroy(gameObject);
        }
    }
}
