//状态机接口
public interface IState
{
    void OnEnter(); //进入时   
    void OnUpdate(); //执行时  
    void OnExit(); //退出时    
}
