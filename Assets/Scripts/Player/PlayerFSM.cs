using Assets.Script.Player;
public class PlayerFSM
{
    public PlayerState CurrentState { get; private set; }

    public void InitState(PlayerState _startState)
    {
        CurrentState = _startState;
        CurrentState.OnEnter();
    }

    public void ChangeState(PlayerState _newState)
    {
        CurrentState.OnExit();
        CurrentState = _newState;
        CurrentState.OnEnter();
    }
}