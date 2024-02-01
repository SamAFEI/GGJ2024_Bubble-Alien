using Assets.Script.Player;

namespace Assets.Scripts.Player
{
    public class PlayerStateLand : PlayerState
    {
        public PlayerStateLand(PlayerController _player, PlayerFSM _FSM, string _animName) : base(_player, _FSM, _animName)
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
            base.OnUpdate();
            if (Player.InputX != 0)
            {
                FSM.ChangeState(Player.RunState);
                return;
            }
            if (isAnimFinish)
            {
                FSM.ChangeState(Player.IdleState);
                return;
            }
        }
    }
}
