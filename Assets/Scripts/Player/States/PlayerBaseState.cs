// Base for action states

public abstract class PlayerBaseState
{
    private PlayerController _ctx;

    protected PlayerController Ctx { get { return _ctx; }}

    public PlayerBaseState(PlayerController currentContext) {
        _ctx = currentContext;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

    public void SwitchState(EnemyBaseState newState) {
        ExitState();
        newState.EnterState();
    }
}