using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Script.Player
{
    public abstract class PlayerState
    {
        protected float stateTime { get; set; }
        public PlayerFSM FSM;
        public PlayerController Player;
        private string animName;
        protected bool isAnimFinish;

        public PlayerState(PlayerController _player, PlayerFSM _FSM, string _animName)
        {
            Player = _player;
            FSM = _FSM;
            animName = _animName;
        }

        public virtual void OnEnter()
        {
            isAnimFinish = false;
            AnimatorPlay();
        }

        public virtual void OnUpdate()
        {
            /*if (Player.IsHurting)
            {
                FSM.ChangeState(Player.HurtState);
                return;
            }*/
            if (Player.IsAttacking)
            {
                FSM.ChangeState(Player.AttackState);
                return;
            }
            if (Player.IsJumping)
            {
                FSM.ChangeState(Player.JumpState);
                return;
            }
            else if (!Player.IsOnGround)
            {
                FSM.ChangeState(Player.FallState);
                return;
            }
        }

        public virtual void OnFixedUpdate()
        {
            
        }
        public virtual void OnLateUpdate()
        {
        }

        public virtual void OnExit()
        {

        }
        public virtual void AnimatorPlay()
        {
            if (animName != "")
            {
                Player.animator.Play(animName);
            }
        }

        public virtual void AnimationFinishTrigger()
        {
            isAnimFinish = true;
        }
    }
}
