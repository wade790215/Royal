using UnityEngine;

public class UnitAI : AIBase
{
    public GameObject projectile;
    public Transform firePos;
    private MyPlaceable targetPlaceable;
    public void OnDealDamage()
    {
        if (target == null) return;
        targetPlaceable = target.GetComponent<PlaceableView>().placeableData;
        targetPlaceable.hitPoints -= this.GetComponent<PlaceableView>().placeableData.damagePerAttack;
        if (targetPlaceable.hitPoints < 0)
        { 
            targetPlaceable.hitPoints = 0;
            target = null;
        }
    }

    public void OnFireProjectile()
    {
        //放在手位置(世界座標)、但是不以firePos為父節點
        var GO = Instantiate(projectile, firePos.position,Quaternion.identity,ProjectileMgr.Instance.transform);
        GO.GetComponent<MyProjectile>().caster = this;
        GO.GetComponent<MyProjectile>().target = target;
        ProjectileMgr.Instance.MineProjectile.Add(GO.GetComponent<MyProjectile>());
    }
}

