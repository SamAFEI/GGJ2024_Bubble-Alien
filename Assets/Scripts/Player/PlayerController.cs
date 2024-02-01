using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerData Data;

        #region COMPONENTS
        public Rigidbody2D RB { get; private set; }
        public PlayerFSM FSM { get; private set; }
        public Animator animator { get; private set; }
        public SpriteRenderer Spr { get; private set; }
        public GameObject HPCanvas { get; private set; }
        public GameObject StateCanvas { get; private set; }
        public GameObject StrongImage { get; private set; }
        public GameObject SpeedImage { get; private set; }
        public GameObject SuperImage { get; private set; }
        public GameObject PowerImage { get; private set; }
        public Image HPBar { get; private set; }
        public GameObject FXPoint { get; private set; }
        public Collider2D Collider;

        public GameObject AttackPoint;

        [Header("FX")]
        [SerializeField] private Material flashFXMAT;
        [SerializeField] private Material stunFXMAT;
        private Material originalMAT;
        #endregion

        //Timers (also all fields, could be private and a method returning a bool could be used)
        public float LastOnGroundTime { get; private set; }
        public float PowerUPTime { get; private set; }
        public float StrongTime { get; private set; }
        public float SpeedUPTime { get; private set; }
        public float SuperTime { get; private set; }

        //Jump
        private bool _isJumpCut;
        private bool _isJumpFalling;

        #region FSM STATE 
        public PlayerStateHurt HurtState { get; private set; }
        public PlayerStateIdle IdleState { get; private set; }
        public PlayerStateRun RunState { get; private set; }
        public PlayerStateAttack AttackState { get; private set; }
        public PlayerStateJump JumpState { get; private set; }
        public PlayerStateFall FallState { get; private set; }
        public PlayerStateLand LandState { get; private set; }
        public PlayerStateDie DieState { get; private set; }
        public PlayerStateReset ResetState { get; private set; }
        #endregion

        #region PLAYER STATE
        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public int AttackDamage { get; private set; }
        public bool IsPowerUP { get { return PowerUPTime > 0; } }
        public bool IsStrong { get { return StrongTime > 0; } }
        public bool IsSpeedUP { get { return SpeedUPTime > 0; } }
        public bool IsSuper { get { return SuperTime > 0; } }
        #endregion

        #region CONTROL PARAMETERS
        //Variables control the various actions the player can perform at any time.
        //These are fields which can are public allowing for other sctipts to read them
        //but can only be privately written to.
        public bool IsFacingRight { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsHurting { get; private set; }
        public bool IsAttacking { get; private set; }
        public bool IsOnGround { get { return LastOnGroundTime > 0; } }
        public bool IsDie { get { return CurrentHP <= 0; } }
        #endregion

        #region INPUT PARAMETERS
        private Vector2 _moveInput;
        public float InputX { get { return _moveInput.x; } }
        public float InputY { get { return _moveInput.y; } }
        public float LastPressedJumpTime { get; private set; }
        public float LastPressedAttackTime { get; private set; }
        public bool IsPressedAttack { get { return LastPressedAttackTime > 0; } }
        #endregion

        #region CHECK PARAMETERS
        //Set all of these up in the inspector
        [Header("Checks")]
        [SerializeField] private Transform _groundCheckPoint;
        //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
        [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
        #endregion

        #region LAYERS & TAGS
        [Header("Layers & Tags")]
        [SerializeField] private LayerMask _groundLayer;
        #endregion

        private void Awake()
        {
            RB = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            Spr = GetComponent<SpriteRenderer>();
            Collider = GetComponent<Collider2D>();
            HPCanvas = transform.Find("HPCanvas").GameObject();
            HPBar = HPCanvas.transform.Find("HPBar").GameObject().GetComponent<Image>();
            FXPoint = transform.Find("FXPoint").GameObject();
            StateCanvas = transform.Find("StateCanvas").GameObject();
            StrongImage = StateCanvas.transform.Find("Strong").GameObject();
            SpeedImage = StateCanvas.transform.Find("Speed").GameObject();
            SuperImage = StateCanvas.transform.Find("Super").GameObject();
            PowerImage = StateCanvas.transform.Find("Power").GameObject();
            FSM = new PlayerFSM();

            HurtState = new PlayerStateHurt(this, FSM, "Hurt");
            IdleState = new PlayerStateIdle(this, FSM, "Idle");
            RunState = new PlayerStateRun(this, FSM, "Run");
            AttackState = new PlayerStateAttack(this, FSM, "Attack");
            JumpState = new PlayerStateJump(this, FSM, "Jump");
            FallState = new PlayerStateFall(this, FSM, "Fall");
            LandState = new PlayerStateLand(this, FSM, "Land");
            DieState = new PlayerStateDie(this, FSM, "Die");
            ResetState = new PlayerStateReset(this, FSM, "Reset");

            FSM.InitState(IdleState);
            SetAttackEffect(false);
            originalMAT = Spr.material;
            flashFXMAT = Resources.Load<Material>("Material/FlashFXMAT");
            MaxHP = 3;
            CurrentHP = MaxHP;
            AttackDamage = 1;
            HPCanvas.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            SetGravityScale(Data.gravityScale);
            IsFacingRight = true;
        }

        // Update is called once per frame
        void Update()
        {
            #region TIMERS
            LastOnGroundTime -= Time.deltaTime;
            LastPressedAttackTime -= Time.deltaTime;
            PowerUPTime -= Time.deltaTime;
            StrongTime -= Time.deltaTime;
            SpeedUPTime -= Time.deltaTime;
            SuperTime -= Time.deltaTime;
            #endregion

            FSM.CurrentState.OnUpdate();
            SuperImage.SetActive(IsSuper);
            SpeedImage.SetActive(IsSpeedUP);
            StrongImage.SetActive(IsStrong);
            PowerImage.SetActive(IsPowerUP);
            if (IsDie) return;

            #region INPUT HANDLER
            if (this.tag == "Player1")
            {
                _moveInput.x = Input.GetAxisRaw("Horizontal");
                _moveInput.y = Input.GetAxisRaw("Vertical");
                if (Input.GetKeyDown(KeyCode.W))
                {
                    OnJumpInput();
                }
                if (Input.GetKeyUp(KeyCode.W))
                {
                    OnJumpUpInput();
                }
                if (Input.GetKeyDown(KeyCode.G))
                {
                    OnAttackInput();
                }
            }
            else if (this.tag == "Player2")
            {
                _moveInput.x = Input.GetAxisRaw("Horizontal_2");
                _moveInput.y = Input.GetAxisRaw("Vertical_2");
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    OnJumpInput();
                }
                if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    OnJumpUpInput();
                }
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    OnAttackInput();
                }
            }
            if (!IsAttacking && _moveInput.x != 0)
                CheckDirectionToFace(_moveInput.x > 0);

            #endregion

            #region COLLISION CHECKS
            if (!IsJumping)
            {
                //Ground Check
                if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
                {
                    LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
                }
            }
            #endregion

            #region JUMP CHECKS
            if (IsJumping && RB.velocity.y < 0)
            {
                IsJumping = false;

                _isJumpFalling = true;
            }

            if (LastOnGroundTime > 0 && !IsJumping)
            {
                _isJumpCut = false;

                _isJumpFalling = false;
            }

            //Jump
            if (CanJump() && LastPressedJumpTime > 0)
            {
                IsJumping = true;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();
            }
            #endregion

            #region ATTACK CHECKS
            if (CanAttack() && IsPressedAttack)
            {
                SetAttacking(true);
            }
            #endregion

            #region GRAVITY
            /*
            //Higher gravity if we've released the jump input or are falling
            if (RB.velocity.y < 0 && _moveInput.y < 0)
            {
                //Much higher gravity if holding down
                SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
                //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
            }
            else if (_isJumpCut)
            {
                //Higher gravity if jump button released
                SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
            {
                SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
            }
            else if (RB.velocity.y < 0)
            {
                //Higher gravity if falling
                SetGravityScale(Data.gravityScale * Data.fallGravityMult);
                //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else
            {
                //Default gravity if standing on a platform or moving upwards
                SetGravityScale(Data.gravityScale);
            }*/
            #endregion
        }
        private void FixedUpdate()
        {
            FSM.CurrentState.OnFixedUpdate();
            //Handle Run
            //if (!IsAttacking && _moveInput.y != 0)
            {
                Run(1);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Item")
            {
                AudioManager.Instance.PlayGetItem();
                ItemData _item = collision.GetComponent<ItemData>();
                if (_item.itemType == ItemEnum.Super)
                {
                    SuperTime = _item.DurationTime;
                    //EffectManager.Instance.DoSuperFX(FXPoint, _item.DurationTime);
                }
                if (_item.itemType == ItemEnum.SpeedUP)
                {
                    SpeedUPTime = _item.DurationTime;
                    //EffectManager.Instance.DoSpeedFX(FXPoint, _item.DurationTime);
                }
                if (_item.itemType == ItemEnum.Strong)
                {
                    StrongTime = _item.DurationTime;
                    //EffectManager.Instance.DoStrongFX(FXPoint, _item.DurationTime);
                }
                if (_item.itemType == ItemEnum.PowerUP)
                {
                    PowerUPTime = _item.DurationTime;
                    //EffectManager.Instance.DoPowerFX(FXPoint, _item.DurationTime);
                }
                Destroy(collision.gameObject);
            }
            if (collision.tag == "Button")
            {
                ButtonController _button = collision.GetComponent<ButtonController>();
            }
        }

        #region INPUT CALLBACKS
        //Methods which whandle input detected in Update()
        public void OnJumpInput()
        {
            LastPressedJumpTime = Data.jumpInputBufferTime;
        }
        public void OnJumpUpInput()
        {
            if (CanJumpCut())
                _isJumpCut = true;
        }
        private void OnAttackInput()
        {
            LastPressedAttackTime = Data.attackInputBufferTime;
        }
        #endregion

        #region CHECK METHODS
        public void CheckDirectionToFace(bool isMovingRight)
        {
            if (isMovingRight != IsFacingRight)
                Turn();
        }

        private bool CanJump()
        {
            return LastOnGroundTime > 0 && !IsJumping && !IsAttacking && !_isJumpFalling;
        }
        private bool CanJumpCut()
        {
            return IsJumping && RB.velocity.y > 0;
        }

        private bool CanAttack()
        {
            return !IsAttacking;
        }
        #endregion

        #region GENERAL METHODS
        public void SetGravityScale(float scale)
        {
            RB.gravityScale = scale;
        }

        public void AnimationTrigger() => FSM.CurrentState.AnimationFinishTrigger();
        #endregion

        #region ATTACK METHODS
        public void SetAttacking(bool _isAttacking)
        {
            IsAttacking = _isAttacking;
        }
        public void SetAttackEffect(bool value)
        {
            //AttackPoint.SetActive(value);
        }
        public void AttackHits()
        {
            Collider2D HitCollider = AttackPoint.GetComponent<Collider2D>();
            List<Collider2D> collidersToDamage = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;
            Physics2D.OverlapCollider(HitCollider, filter, collidersToDamage);
            foreach (var collider in collidersToDamage)
            {
                PlayerController enemy = collider.GetComponent<PlayerController>();
                if (enemy != null && this.tag != enemy.tag)
                {
                    if (!enemy.IsHurting)
                    {
                        Vector2 vector = new Vector2(enemy.transform.position.x, AttackPoint.transform.position.y);
                    }
                    int damage = AttackDamage;
                    if (IsPowerUP) damage *= 3;
                    enemy.StartCoroutine(enemy.Hurt(damage, this.transform));
                }
            }
        }
        #endregion

        #region HURT METHODS
        public virtual IEnumerator Hurt(int Damage, Transform source)
        {
            if (IsHurting || IsDie || IsSuper) { yield break; }
            AudioManager.Instance.PlayHurt();
            SetHPBar(Damage);
            IsHurting = true;
            //被攻擊 會卡狀態 要取消跳躍
            IsJumping = false;
            if (!IsStrong)
            {
                Repel(source);
            }
            StartCoroutine(HurtFlashFX());
            HPCanvas.SetActive(true);
            yield return new WaitForSeconds(Data.HurtResetTime);
            IsHurting = false;
            HPCanvas.SetActive(false);
            if (IsDie)
            {
                FSM.ChangeState(DieState);
                AudioManager.Instance.PlayDie();
                PowerUPTime = 0f;
                StrongTime = 0f;
                SpeedUPTime = 0f;
                SuperTime = 0f;
            }
        }

        public void Repel(Transform source)
        {
            Vector2 vector = Vector2.right * 50;
            if (transform.position.x < source.position.x)
            {
                vector.x *= -1;
            }
            RB.velocity = Vector2.zero;
            RB.AddForce(vector, ForceMode2D.Impulse);
        }
        public void SetHPBar(int damage)
        {
            CurrentHP = (int)Mathf.Clamp(CurrentHP - damage, 0, MaxHP);
            float hp = (float)CurrentHP / (float)MaxHP;
            HPBar.fillAmount = hp;
        }
        #endregion

        //MOVEMENT METHODS
        #region RUN METHODS
        private void Run(float lerpAmount)
        {
            float speedUP = 1;
            if (IsSpeedUP)
            {
                speedUP = 2;
            }

            //Calculate the direction we want to move in and our desired velocity
            float targetSpeed = _moveInput.x * Data.runMaxSpeed * speedUP;
            //We can reduce are control using Lerp() this smooths changes to are direction and speed
            targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

            #region Calculate AccelRate
            float accelRate;

            //Gets an acceleration value based on if we are accelerating (includes turning) 
            //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
            if (LastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
            #endregion

            #region Add Bonus Jump Apex Acceleration
            //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
            if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
            {
                accelRate *= Data.jumpHangAccelerationMult;
                targetSpeed *= Data.jumpHangMaxSpeedMult;
            }
            #endregion

            #region Conserve Momentum
            //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
            if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
            {
                //Prevent any deceleration from happening, or in other words conserve are current momentum
                //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
                accelRate = 0;
            }
            #endregion

            //Calculate difference between current velocity and desired velocity
            float speedDif = targetSpeed - RB.velocity.x;
            //Calculate force along x-axis to apply to thr player

            float movement = speedDif * accelRate;

            //Convert this to a vector and apply to rigidbody
            RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

            /*
             * For those interested here is what AddForce() will do
             * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
             * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
            */
        }

        private void Turn()
        {
            //stores scale and flips the player along the x axis, 
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            IsFacingRight = !IsFacingRight;
        }
        #endregion

        #region JUMP METHODS
        private void Jump()
        {
            //Ensures we can't call Jump multiple times from one press
            LastPressedJumpTime = 0;
            LastOnGroundTime = 0;

            #region Perform Jump
            //We increase the force applied if we are falling
            //This means we'll always feel like we jump the same amount 
            //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
            float force = Data.jumpForce;
            if (RB.velocity.y < 0)
                force -= RB.velocity.y;

            RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            #endregion
        }
        #endregion

        #region FX METHODS
        public IEnumerator HurtFlashFX()
        {
            Spr.material = flashFXMAT;
            yield return new WaitForSeconds(.2f);
            Spr.material = originalMAT;
        }
        #endregion

        #region EDITOR METHODS
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
            Gizmos.color = Color.blue;
        }
        #endregion
    }
}
