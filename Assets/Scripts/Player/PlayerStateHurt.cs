using Assets.Script.Player;

namespace Assets.Scripts.Player
{
    public class PlayerStateHurt : PlayerState
    {
        public PlayerStateHurt(PlayerController _player, PlayerFSM _FSM, string _animName) : base(_player, _FSM, _animName)
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
            if (!Player.IsHurting)
            {
                base.OnUpdate();
                //if (Player.IsOnGround)
                {
                    FSM.ChangeState(Player.IdleState);
                    return;
                }
            }
        }
    }
}
