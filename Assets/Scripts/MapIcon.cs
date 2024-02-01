/*
    �j�a�ϹϼФW
    ����p�a�Ϫ��ϼ�
    �ϼЩ�������
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CosmicTrash;

public class MapIcon : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed; //����t��
    public TrashType type;  //���A(BonusA�BBonusB�BMeteorite)
    public int maxSize; //�ͦ����̤j�ؤo

    public float lineRow;   //�ͦ��b�ĴX��    
    public int thisSize;    //������ͦ����j�p

    public bool isBig;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.name== "Cabin")//�p�G�ϼиI���N����
        {
            if (this.isBig == false)
            {
                Destroy(this.gameObject);
            }            
        }
        else
        {
            ScreenBoundary.Instance.SpawnCosmicTrash(this);//�p�G�ϼиI��e����ɴN�b�u��@�ɹ�����m�ͦ�
        }
        
    }

    void Start()
    {
        rb=GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        rb.velocity = new Vector2(-1* speed, 0f);

        //���}�p�a�Ͻd��R���ϼ�
        if (GetComponent<RectTransform>().anchoredPosition.x < -1120f)
        {
            Destroy(gameObject);
        }
    }
}
