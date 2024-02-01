using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject OnButton;
    public GameObject OffButton;
    public bool IsUP;
    public GameObject AnotherButton;

    public float LastOnTime { get; private set; }

    public bool IsOn { get { return LastOnTime > 0; } }
    public bool LastState { get; private set; }

    #region CHECK PARAMETERS
    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform _onCheckPoint;
    [SerializeField] private Vector2 _onCheckSize = new Vector2(0.49f, 0.03f);
    #endregion
    #region LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _playerLayer;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SwitchButton(IsOn);
    }

    // Update is called once per frame
    void Update()
    {
        #region TIMERS
        LastOnTime -= Time.deltaTime;
        #endregion

        //CheckOnOff(IsOn);
        if (IsUP)
        {
            Cabin.instance.up_1 = false;
            Cabin.instance.up_2 = false;
        }
        else
        {
            Cabin.instance.down_1 = false;
            Cabin.instance.down_2 = false;
        }
        #region COLLISION CHECKS
        if (Physics2D.OverlapBox(_onCheckPoint.position, _onCheckSize, 0, _playerLayer))
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(_onCheckPoint.position, _onCheckSize, 0, _playerLayer);
            LastOnTime = 0.2f;

            // 1°¦
            if (colliders.Length > 0)
            {
                if (IsUP) Cabin.instance.up_1 = true;
                else Cabin.instance.down_1 = true;
            }
            // 2°¦
            if (colliders.Length > 1)
            {
                if (IsUP) Cabin.instance.up_2 = true;
                else Cabin.instance.down_2 = true;
            }
        }
        
        SwitchButton(IsOn);
        
        #endregion
    }

    public void SwitchButton(bool value)
    {
        OnButton.SetActive(value);
        OffButton.SetActive(!value);
        if (LastState != value)
        {
            LastState = value;
            if (value)
            {
                AudioManager.Instance.PlayOnButton();
            }
        }
    }

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_onCheckPoint.position, _onCheckSize);
        Gizmos.color = Color.blue;
    }
    #endregion
}
