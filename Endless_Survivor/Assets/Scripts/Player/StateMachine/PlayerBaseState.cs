using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    protected bool isRootState = false;
    protected PlayerStateMachine context;
    protected PlayerStateFactory factory;

    protected PlayerBaseState currSuperState;
    protected PlayerBaseState currSubState;
    
    public PlayerBaseState CurrSubState { get { return currSubState; } }
    public PlayerBaseState(PlayerStateMachine newContext, PlayerStateFactory newFactory)
    {
        context = newContext;
        factory = newFactory;
    }
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void OnCollisionEnter(Collision collision);

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    public void UpdateStates() 
    {
        UpdateState();
        if (currSubState != null)
            currSubState.UpdateStates();
    }

    public void ExitStates()
    {
        ExitState();
        if (currSubState != null)
            currSubState.ExitState();
    }

    protected void SwitchState(PlayerBaseState newState) 
    {
        ExitStates();

        if (isRootState)
        {
            newState.EnterState();
            context.CurrState = newState;
        }

        else if (currSuperState != null)
            currSuperState.SetSubState(newState);

    }

    protected void SetSuperState(PlayerBaseState newSuperState) 
    {
        currSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState) 
    {
        currSubState = newSubState;
        currSubState.SetSuperState(this);
        currSubState.EnterState();
    }
}
