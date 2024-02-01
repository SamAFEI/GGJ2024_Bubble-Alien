
using Assets.Script.Player;

namespace Assets.Scripts.Player
{
    public class PlayerStateRun : PlayerState
    {
        public PlayerStateRun(PlayerController _player, PlayerFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Player.InputX == 0)
            {
                FSM.ChangeState(Player.IdleState);
                return;
            }
        }
    }
}
