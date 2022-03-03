using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遊戲單位管理器
/// </summary>

public class PlaceableManager : MonoBehaviour
{
    public static PlaceableManager Instance;
    public List<PlaceableView> Mine = new List<PlaceableView>();
    public List<PlaceableView> Opponent = new List<PlaceableView>();
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
