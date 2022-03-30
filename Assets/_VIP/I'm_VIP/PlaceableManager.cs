using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityRoyale.Placeable;

public partial class MyPlaceable
{
    public Faction faction = Faction.None;
    public MyPlaceable Clone()
    {
        return this.MemberwiseClone() as MyPlaceable;
    }
}

/// <summary>
/// 遊戲單位管理器
/// </summary>

public class PlaceableManager : MonoBehaviour
{
    public static PlaceableManager Instance;
    public List<PlaceableView> Mine = new List<PlaceableView>();
    public List<PlaceableView> Opponent = new List<PlaceableView>();

    public AIBase opponentTower, mineTower;

    void Awake()
    {
        Instance = this;
        Opponent.Add(opponentTower.GetComponent<PlaceableView>());
        Mine.Add(mineTower.GetComponent<PlaceableView>());
    }

    void Update()
    {
        UpdatePlaceable(Mine);
        UpdatePlaceable(Opponent);
    }

    private void UpdatePlaceable(List<PlaceableView> placeableViewsList)
    {
        for (int i = 0; i < placeableViewsList.Count; i++)
        {
            PlaceableView PV = placeableViewsList[i];
            MyPlaceable PData = PV.placeableData;
            AIBase AIB = PV.GetComponent<AIBase>();
            NavMeshAgent NMA = AIB.GetComponent<NavMeshAgent>();
            Animator Ani = AIB.GetComponent<Animator>();


            switch (AIB.aiState)
            {
                case AIState.Idle:
                    if (AIB is BuildingAI)
                    {
                        //TODO 如果塔要攻擊則設定為攻擊狀態
                        break;
                    }
                    //場景內最近的敵人
                    AIB.target = FindClosestEnemy(PData.faction, AIB.transform.position);

                    if (AIB.target != null)
                    {
                        Debug.Log($"找到最近的敵人{AIB.target.gameObject}");
                        AIB.aiState = AIState.Seek;
                        NMA.enabled = true;
                        Ani.SetBool("IsMoving", true);
                    }
                    else
                    {
                        Ani.SetBool("IsMoving", false);
                    }

                    //判斷是否有敵人在範圍內
                    //有則切換Seek狀態
                    break;
                case AIState.Seek:
                    //往敵人方向前進
                    if (AIB.target == null)
                    {
                        AIB.aiState = AIState.Idle;
                        break;
                    }
                    NMA.SetDestination(AIB.target.transform.position);

                    //判定是否進入攻擊範圍，若是則進入Attack狀態                    
                    if (IsInAttackRange(PV.transform.position, AIB.target.transform.position, PData.attackRange))
                    {
                        NMA.enabled = false;
                        AIB.aiState = AIState.Attack;
                    }
                    break;
                case AIState.Attack:

                    if (AIB.target == null)
                    {
                        AIB.aiState = AIState.Idle;
                        break;
                    }
                    if (IsInAttackRange(PV.transform.position, AIB.target.transform.position, PData.attackRange) == false)
                    {
                        AIB.aiState = AIState.Idle;
                        break;
                    }
                    //在攻擊間隔時間內不連續攻擊
                    if (Time.time < AIB.lastBlowTime + PData.attackRatio)
                    {
                        break;
                    }
                    AIB.transform.LookAt(AIB.target.transform.position);
                    Ani.SetBool("IsMoving", false);
                    Ani.SetTrigger("Attack");
                    //AIB.target.GetComponent<PlaceableView>().placeableData.hitPoints -= PData.damagePerAttack;
                    if (AIB.target.GetComponent<PlaceableView>().placeableData.hitPoints <= 0)
                    {
                        OnEnterDie(AIB.target);
                        AIB.aiState = AIState.Idle;
                    }
                    AIB.lastBlowTime = Time.time;
                    break;
                case AIState.Dead:
                    {
                        if (AIB is BuildingAI)
                            break;

                        var Ren = AIB.GetComponentsInChildren<Renderer>();
                        PV.dieProgress += (1 / PV.dieDuration) * Time.deltaTime;

                         foreach (var item in Ren)
                        {
                            item.material.SetFloat("_DissolveFatcor", PV.dieProgress);
                        }

                    }
                    break;
            }
        }
    }

    public void OnEnterDie(AIBase target)
    {
        if (target.aiState == AIState.Dead) return;
        if (target is BuildingAI) return;
        target.GetComponent<NavMeshAgent>().enabled = false;
        target.GetComponent<AIBase>().aiState = AIState.Dead;
        target.GetComponent<PlaceableView>().placeableData.hitPoints = 0;
        target.GetComponent<Animator>()?.SetTrigger("IsDead");
        var Ren = target.GetComponentsInChildren<Renderer>();
        var view = target.GetComponent<PlaceableView>();
        var color = view.placeableData.faction == Faction.Player ? Color.red : Color.blue;
        view.dieProgress = 0f;
        foreach (var item in Ren)
        {
            item.material.SetColor("_EdgeColor", color *8);
            item.material.SetFloat("_EdgeWidth", 0.1f);
            item.material.SetFloat("_DissolveFatcor", view.dieProgress);
        }
        //Destroy(target.gameObject, view.dieDuration);
    }

    private bool IsInAttackRange(Vector3 myPos, Vector3 targetPos, float attackRange)
    {
        return Vector3.Distance(myPos, targetPos) < attackRange;
    }

    private AIBase FindClosestEnemy(Faction faction, Vector3 myPos)
    {
        var units = faction == Faction.Player ? Opponent : Mine;
        float min = float.MaxValue;
        AIBase closest = null;
        foreach (var unit in units)
        {
            var dis = Vector3.Distance(unit.transform.position, myPos);
            //如果小兵距離比較則紀錄跟下一個做比對
            if (dis < min && unit.GetComponent<AIBase>().aiState != AIState.Dead)
            {
                min = dis;
                closest = unit.GetComponent<AIBase>();
            }
        }
        return closest;
    }
}
