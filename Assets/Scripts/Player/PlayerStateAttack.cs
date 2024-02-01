using Assets.Script.Player;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStateAttack : PlayerState
    {
        public PlayerStateAttack(PlayerController _player, PlayerFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }
        public override void OnEnter()
        {
            base.OnEnter();
            Player.SetAttacking(true);
            Player.SetAttackEffect(true);
            if (Player.InputX != 0)
            { Player.CheckDirectionToFace(Player.InputX > 0); }
            //if (Player.IsOnGround)
            {
                //Player.RB.velocity = Vector2.zero;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Player.SetAttacking(false);
            Player.SetAttackEffect(false);
        }

        public override void OnUpdate()
        {
            Player.AttackHits();
            if (isAnimFinish)
            {
                //if (Player.IsOnGround)
                {
                    FSM.ChangeState(Player.IdleState);
                    return;
                }
            }
            base.OnUpdate();
        }
    }
}
