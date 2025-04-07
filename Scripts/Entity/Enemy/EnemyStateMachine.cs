using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine 
{
    public EnemyState currentEnemyState{get; private set;}

    public void Initialize(EnemyState enemyState_start)
    {   
        enemyState_start.Enter();
        currentEnemyState=enemyState_start;
    }

    public void ChangeState(EnemyState targetEnemyState) 
    {
        currentEnemyState.Exit();
        targetEnemyState.Enter();
        currentEnemyState=targetEnemyState;
    }

}
