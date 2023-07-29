using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState<StateType> where StateType : Enum
{
    public abstract StateType GameStateType
    {
        get;
    }

    public abstract StateType GetNextState();

    public virtual bool CanExitState()
    {
        return !GetNextState().Equals(GameStateType);
    }
    

    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnFixedUpdate()
    {

    }
}
