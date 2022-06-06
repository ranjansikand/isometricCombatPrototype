public class PlayerStateFactory
{
    PlayerController _context;
    PlayerWalkState _walk;
    PlayerDodgeState _dodge;
    PlayerAttackState _attack;
    PlayerIdleState _idle;
    PlayerDeathState _death;

    public PlayerStateFactory(PlayerController currentContext)
    {
        _context = currentContext;
    }

    public void GenerateStates()
    {
        _walk = new PlayerWalkState(_context);
        _dodge = new PlayerDodgeState(_context);
        _attack = new PlayerAttackState(_context);
        _idle = new PlayerIdleState(_context);
        _death = new PlayerDeathState(_context);
    }

    public PlayerBaseState GetState(int index) {
        switch (index) {
            case 0: return _walk;
            case 1: return _dodge;
            case 2: return _attack;
            case 3: return _idle;
            case 4: return _death;
            default: return _idle;
        }
    }

    public void SwitchState(PlayerBaseState newState) {
        _context.CurrentState?.ExitState();
        _context.CurrentState = newState;
        _context.CurrentState.EnterState();
    }
}
