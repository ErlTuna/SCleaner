public interface IState
{
    void StateUpdate();
    void StateFixedUpdate();
    void OnEnter();
    void OnExit();
}
