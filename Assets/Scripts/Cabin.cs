/*
    綁飛船身上
    控制飛船
    控制五段上下
    和小地圖上的飛船顯示
    
    A的分數
    B的分數
    船的血量

    碰到物件的行為
    BonusA加A的分數
    BonusB加B的分數
    Meteorite扣船的血量

*/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cabin : MonoBehaviour
{
    public bool up_1, up_2, down_1, down_2;    //測試用，到時候砍
    public static Cabin instance;
    public RectTransform miniCabin;    //小地圖上面的飛船圖案
    public float mapScale;  //小地圖比例尺單位
    public float moveInterval;  //移動間隔


    public GameObject cosmic;   //宇宙背景
    public float speed; //移動速度

    public ItemSpaw[] spawPoints {  get; private set; }
    public float spawItemInterval = 10f;
    public float lastSpawItemTime { get; private set; }

    public GameManager gameManager;

    

    public Image redGlow;
    public float flickerSpeed;
    public float flickerDuration;

    private IEnumerator Flash()
    {
        float elapsedTime = 0f;

        while (elapsedTime < flickerDuration)
        {
            redGlow.enabled = !redGlow.enabled; // 切換圖像的顯示狀態
            yield return new WaitForSeconds(flickerSpeed);

            elapsedTime += flickerSpeed;
        }

        redGlow.enabled = false;
    }


    private void Update()
    {
        /*
        //測試用，到時候砍↓
        if(Input.GetKey(KeyCode.W)) //角色1往上
        {
            up_1=true;
        }
        else{
            up_1=false;
        }

        if (Input.GetKey(KeyCode.UpArrow))  //角色2往上
        {
            up_2=true;
        }
        else{
            up_2=false; 
        }

        if (Input.GetKey(KeyCode.S)) //角色1往下
        {
            down_1=true;
        }
        else
        {
            down_1 = false;
        }

        if (Input.GetKey(KeyCode.DownArrow)) //角色2往下
        {
            down_2=true;
        }
        else
        {
            down_2 = false;
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag== "BigMeteorite")
        {
            SceneManager.LoadScene("GameEnding");
        }
        if (collision.gameObject.GetComponent<CosmicTrash>()==null) { return; }
        switch (collision.gameObject.GetComponent<CosmicTrash>().type)  //判斷目前船撞到哪種物體
        {
            case CosmicTrash.TrashType.BonusA:
                GameManager.pointsA++;
                this.GetComponent<AudioSource>().clip = collision.GetComponent<AudioSource>().clip;
                this.GetComponent<AudioSource>().Play();
                break;

            case CosmicTrash.TrashType.BonusB:
                GameManager.pointsB++;
                this.GetComponent<AudioSource>().clip = collision.GetComponent<AudioSource>().clip;
                this.GetComponent<AudioSource>().Play();
                break;

            case CosmicTrash.TrashType.Meteorite:
                GameManager.hp--;
                this.GetComponent<AudioSource>().clip = collision.GetComponent<AudioSource>().clip;
                this.GetComponent<AudioSource>().Play();
                StartCoroutine(Flash());
                if (GameManager.hp ==0)
                {
                    SceneManager.LoadScene("GameOver");
                }
                break;
        }
        gameManager.ShowPoints();
        Destroy(collision.gameObject);

    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.pointsA = 0;
        GameManager.pointsB = 0;
        GameManager.hp = 3;
        gameManager.ShowPoints();

        spawPoints = GetComponentsInChildren<ItemSpaw>();

        
    }

    private void LateUpdate()
    {
        Move();
        cosmic.transform.Translate(Vector3.left * speed * Time.deltaTime);
        lastSpawItemTime -= Time.deltaTime;
        if (lastSpawItemTime < 0)
        {
            lastSpawItemTime = spawItemInterval;
            spawPoints[Random.Range(0, spawPoints.Length)].SpawItem();
        }
    }

    public void Move()
    {
        float verticalPosition=0f;
        if (up_1) { verticalPosition += moveInterval;}
        if (up_2) {  verticalPosition += moveInterval;}
        if (down_1) {  verticalPosition -= moveInterval;}
        if (down_2) {  verticalPosition -= moveInterval;}

        this.transform.position = new Vector2(this.transform.position.x, verticalPosition);
               
        miniCabin.anchoredPosition = new Vector2(128, verticalPosition * mapScale);
    }
    public void Move(bool _up_1, bool _up_2, bool _down_1, bool _down_2)
    {
        up_1 = _up_1;
        up_2 = _up_2;
        down_1 = _down_1;
        down_2 = _down_2;
        Move();
    }
}
