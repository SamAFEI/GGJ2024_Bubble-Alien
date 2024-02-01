
using Assets.Script.Player;

namespace Assets.Scripts.Player
{
    public class PlayerStateJump : PlayerState
    {
        public PlayerStateJump(PlayerController _player, PlayerFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Player.RB.velocity.y < 0)
            {
                FSM.ChangeState(Player.FallState);
                return;
            }
        }
    }
}
