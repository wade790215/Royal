using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyCardView : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
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
            // 讓小兵跟著鼠標走
            previewHolder.transform.position = raycastHit.point;
            // 如果小兵還沒拖到場上
            if (isDragging == false)
            {
                //隱藏卡牌
                GetComponent<CanvasGroup>().alpha = 0;
                //從卡牌數據中取出資料
                for (int i = 0; i < data.placeablesIndices.Length; i++)
                {
                    //尋找小兵Data
                    var unitID  = data.placeablesIndices[i];
                    MyPlaceable MP = null;
                    for (int j = 0; j < MyPlaceableModel.instance.list.Count; j++)
                    {
                        if(MyPlaceableModel.instance.list[j].id == unitID)
                        {
                            MP = MyPlaceableModel.instance.list[j];
                            break;
                        }
                    }
                    //生成卡牌對應的小兵，並將其設置為預覽用卡牌
                    Vector3 offset = data.relativeOffsets[i];
                    GameObject unitPrefabs = Resources.Load<GameObject>(MP.associatedPrefab);
                    var unit = Instantiate(unitPrefabs, previewHolder,false);                    
                    unit.transform.localPosition = offset;
                    unit.GetComponent<MyPlaceableView>().placeableData = MP;
                }
                isDragging = true;
            }
            else
            {

            }
        }
        //鼠標沒有放在地面上
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


    public void OnPointerUp(PointerEventData eventData)
    {
        var ray = mainCamera.ScreenPointToRay(eventData.position);
        bool hitGround = Physics.Raycast(ray, float.PositiveInfinity, 1 << LayerMask.NameToLayer("PlayingField"));
        if (hitGround)
        {
            OnCardUsed();
        }
        else
        {
            transform.DOMove(CardsManager.Instance.cardsTrans[playAreaIndex].position, 0.2f);
        }
    }

    private void OnCardUsed()
    {
        
    }
}
