using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UnitAI : AIBase
{
    //public GameObject projectile;
    public AssetReference AssetReference;
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

    public async void OnFireProjectile()
    {
        //放在手位置(世界座標)、但是不以firePos為父節點
        var GO = await Addressables.InstantiateAsync(AssetReference, firePos.position, Quaternion.identity, ProjectileMgr.Instance.transform).Task;
        GO.GetComponent<MyProjectile>().caster = this;
        GO.GetComponent<MyProjectile>().target = target;
        ProjectileMgr.Instance.MineProjectile.Add(GO.GetComponent<MyProjectile>());
    }
}

