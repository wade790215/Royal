using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityRoyale;
using static UnityRoyale.Placeable;

public class CardView : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public MyCard data;
    public int playAreaIndex;
    private bool isDragging;
    internal  Transform previewHolder;
    private Camera mainCamera;
    

    private void Start()
    {
        mainCamera = Camera.main; //Camer.main避免放在會一直更新的方法內，因為會一直去Get屬性                                
    }

    public void OnPointerDown(PointerEventData eventData)
    {        
        transform.SetAsLastSibling(); //將選中物件層級移到最後面
    }

    public void OnDrag(PointerEventData eventData)
    {
        //將UI座標轉換為世界座標
        RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent as RectTransform, eventData.position, null, out Vector3 worldPos);
        transform.position = worldPos;
        var ray = mainCamera.ScreenPointToRay(eventData.position);
        bool hitGround = Physics.Raycast(ray, out RaycastHit raycastHit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("PlayingField"));
        // 鼠標有放在地面上
        if (hitGround)
        {
            // previewHolder跟著鼠標走
            previewHolder.transform.position = raycastHit.point;
            // 如果小兵還沒拖到場上
            if (isDragging == false)
            {
                //隱藏卡牌
                GetComponent<CanvasGroup>().alpha = 0;
                CreatePlaceable(data, raycastHit.point,previewHolder.transform,Faction.Player);
                isDragging = true;
            }
            else
            {

            }
        }
        //鼠標沒有放在地面上(放回出牌區域)
        else
        {
            // 卡牌已經在場景中顯示
            if (isDragging)
            {
                //1.標示為未顯示 2.顯示卡牌 3.銷毀預覽小兵
                isDragging = false;
                GetComponent<CanvasGroup>().alpha = 1;
                foreach (Transform unit in previewHolder)
                {
                    Destroy(unit.gameObject);
                }
            }
        }
    }

    public static List<PlaceableView> CreatePlaceable(MyCard cardData,Vector3 pos,Transform parent,Faction faction)
    {
        if (cardData == null)       
            throw new NullReferenceException("cardData");
        if (cardData.placeablesIndices.Length == 0)
            throw new ArgumentException("檢查Excel的placeablesIndices是否有設定兵種編號");

        List<PlaceableView> placeables = new List<PlaceableView>();
        //從卡牌數據中取出資料
        for (int i = 0; i < cardData.placeablesIndices.Length; i++)
        {
            //尋找小兵Data
            var unitID = cardData.placeablesIndices[i];
            MyPlaceable MP = null;
            for (int j = 0; j < PlaceableModel.instance.list.Count; j++)
            {
                if (PlaceableModel.instance.list[j].id == unitID)
                {
                    MP = PlaceableModel.instance.list[j].Clone();
                    break;
                }
            }
            //生成卡牌對應的小兵，並將其設置為預覽用卡牌
            Vector3 offset = cardData.relativeOffsets[i];
            GameObject unitPrefabs = Resources.Load<GameObject>(MP.associatedPrefab);
            //var unit = Instantiate(unitPrefabs, previewHolder, false);
            //unit.transform.localPosition = offset;
            //parent.position = pos;
            var unit = Instantiate(unitPrefabs, parent, false);
            unit.transform.localPosition = offset;
            unit.transform.position = pos + offset;
            MP.faction = faction;
            var PV = unit.GetComponent<PlaceableView>();
            PV.placeableData = MP;
            placeables.Add(PV);            
        }      
        return placeables;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var ray = mainCamera.ScreenPointToRay(eventData.position);
        bool hitGround = Physics.Raycast(ray, float.PositiveInfinity, 1 << LayerMask.NameToLayer("PlayingField"));
        if (hitGround)
        {
            OnCardUsed();
            Destroy(this.gameObject);
            CardsManager.Instance.StartCoroutine(CardsManager.Instance.PreveiwAreaToPlayingArea(playAreaIndex,0.5f));
            CardsManager.Instance.StartCoroutine(CardsManager.Instance.CreateOneCardToPreveiwArea(1f));
        }
        else
        {
            //卡牌放回出牌區
            transform.DOMove(CardsManager.Instance.cardsTrans[playAreaIndex].position, 0.2f);
        }
    }

    private void OnCardUsed()
    {
        //放到PlaceableView下
        for (int i = previewHolder.childCount-1; i >= 0; i--) //因為子節點數量會變更，所以從後面取出來
        {
            //SetParent第二個參數worldPositionStays用途 -> 物件是否維持在世界座標
            Transform unit = previewHolder.GetChild(i);
            unit.SetParent(PlaceableManager.Instance.transform, true);
            PlaceableManager.Instance.Mine.Add(unit.GetComponent<PlaceableView>());
        }    
    }
}
