using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AIState
{
    Idle,
    Seek,
    Attack,
    Dead
}
public class AIBase : MonoBehaviour
{
    public AIBase target = null;
    public AIState aiState = AIState.Idle;
    public virtual void OnIdle() { }
    public virtual void OnSeek() { }
    public virtual void OnAttack() { }    
    public virtual void OnDead() { }  
}
