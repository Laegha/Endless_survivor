using UnityEngine;

public class PlayerStateFactory
{
    PlayerStateMachine context;

    public PlayerStateFactory(PlayerStateMachine newContext)
    {
        context = newContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(context, this);
    }

    public PlayerBaseState Run()
    {
        return new PlayerRunState(context, this);
    }

}
