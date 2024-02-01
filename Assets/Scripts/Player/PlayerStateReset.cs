
using Assets.Script.Player;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStateReset : PlayerState
    {
        public PlayerStateReset(PlayerController _player, PlayerFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            //base.OnUpdate();
            if (isAnimFinish)
            {
                Player.SetHPBar(-3);
                FSM.ChangeState(Player.IdleState);
                return;
            }
        }
    }
}
