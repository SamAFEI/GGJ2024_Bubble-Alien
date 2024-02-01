using Assets.Script.Player;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStateDie : PlayerState
    {
        public PlayerStateDie(PlayerController _player, PlayerFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }
        public override void OnEnter()
        {
            base.OnEnter();
            stateTime = 3f;
            Physics2D.IgnoreLayerCollision(7, 8, true);
            Player.RB.velocity = Vector3.zero;
            AudioManager.Instance.PlayDie();
        }

        public override void OnExit()
        {
            base.OnExit();
            Physics2D.IgnoreLayerCollision(7, 8, false);
        }

        public override void OnUpdate()
        {
            //base.OnUpdate();
            stateTime -= Time.deltaTime;
            if (stateTime < 0)
            {
                FSM.ChangeState(Player.ResetState);
                return;
            }
        }
    }
}
