
using Assets.Script.Player;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStateIdle : PlayerState
    {
        public PlayerStateIdle(PlayerController _player, PlayerFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }
        public override void OnEnter()
        {
            base.OnEnter();
            //Player.RB.velocity = Vector2.zero;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Player.InputX != 0)
            {
                FSM.ChangeState(Player.RunState);
                return;
            }
        }
    }
}
