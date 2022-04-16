using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 兵種畫面表現
/// </summary>
public class PlaceableView : MonoBehaviour
{
    public MyPlaceable placeableData;

    public float dieDuration = 10f;
    public float dieProgress = 0f;

    private void OnDestroy()
    {
        PlaceableManager.Instance.Mine.Remove(this); 
        PlaceableManager.Instance.Opponent.Remove(this);
    }
}
