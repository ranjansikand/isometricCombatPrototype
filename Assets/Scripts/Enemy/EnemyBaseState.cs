
public abstract class EnemyBaseState
{
    private EnemyBase _ctx;

    protected EnemyBase Ctx { get { return _ctx; } set { _ctx = value; }}

    public EnemyBaseState(EnemyBase currentContext) { _ctx = currentContext; }

    public abstract void EnterState();

    public abstract void UpdateState();
    
    public abstract void ExitState();
}
