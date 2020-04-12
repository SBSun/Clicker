using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedFurniture : MonoBehaviour
{
    //배치된 가구 슬롯
    public FurnitureSlot disposeSlot;
    //전에 배치된 가구 슬롯
    public FurnitureSlot beforeDisposeSlot;

    //배치, 교체 버튼
    public DisposeButton disposeButton;

    private SpriteRenderer spriteRenderer;

    //가구를 배치할 위치를 계산할 때 쓰임
    private Vector2 localSpriteSize;

    private float disposePosY;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();      
    }

    public void ArrangeFurniture(FurnitureSlot _furnitureSlot)
    {
        //소유하고 있는 가구 개수 - 배치한 가구 개수 > 0, 배치한 가구 개수 > 0
        if (_furnitureSlot.furnitureItemData.currentHaveNumber - _furnitureSlot.furnitureItemData.currentDisposeNumber  == 0)
            return;

        //배치 된 가구가 있다면 교체
        if(disposeSlot != null)
        {
            //배치되어 있는 가구와 인수로 들어온 가구가 같다면
            if(disposeSlot == _furnitureSlot)
            {
                //배치되어 있는 가구를 빼준다.
                SubFurniture();
                return;
            }

            //전에 배치했던 가구를 현재 가구로 변경
            beforeDisposeSlot = disposeSlot;
            //현재 가구를 인수로 들어온 가구로 변경
            disposeSlot = _furnitureSlot;

            beforeDisposeSlot.FurnitureDisposeSub();
            disposeSlot.FurnitureDisposeAdd();
        }
        //없다면 배치
        else
        {
            //교체 이미지로 변경
            disposeButton.ChangeReplaceButton();

            beforeDisposeSlot = null;
            disposeSlot = _furnitureSlot;

            disposeSlot.FurnitureDisposeAdd();
        }
            
        spriteRenderer.sprite = _furnitureSlot.furnitureItemData.furnitureItem.itemSprite;

        GetSpriteSize();

        CalculateFurniturePosY();
    }

    public void SubFurniture()
    {
        disposeSlot.FurnitureDisposeSub();
        disposeSlot = null;
        spriteRenderer.sprite = null;
        disposeButton.ChangeDisposeButton();
    }

    public void DisposeButtonActivate()
    {
        disposeButton.gameObject.SetActive( true );
    }

    public void DisposeButtonDeactivate()
    {
        disposeButton.gameObject.SetActive( false );
    }

    public void GetSpriteSize()
    {
       localSpriteSize = (spriteRenderer.sprite.rect.size / spriteRenderer.sprite.pixelsPerUnit) * transform.localScale.x;
    }

    //배치할 가구 이미지 Y 좌표 계산
    public void CalculateFurniturePosY()
    {
        switch(disposeSlot.furnitureItemData.furnitureItem.furnitureType)
        {
            case FurnitureItem.FurnitureType.FloorDecoration:
                disposePosY = CatHouseManager.instance.furnitureDisposePosY[(int)disposeSlot.furnitureItemData.furnitureItem.furnitureType] + localSpriteSize.y / 2;
                break;
            case FurnitureItem.FurnitureType.WallDecoration:
                disposePosY = CatHouseManager.instance.furnitureDisposePosY[(int)disposeSlot.furnitureItemData.furnitureItem.furnitureType] + localSpriteSize.y / 2;
                break;
            case FurnitureItem.FurnitureType.TopDecoration:
                disposePosY = CatHouseManager.instance.furnitureDisposePosY[(int)disposeSlot.furnitureItemData.furnitureItem.furnitureType] - localSpriteSize.y / 2;
                break;
            case FurnitureItem.FurnitureType.Floor:
                disposePosY = CatHouseManager.instance.furnitureDisposePosY[(int)disposeSlot.furnitureItemData.furnitureItem.furnitureType] + localSpriteSize.y / 2;
                break;
            case FurnitureItem.FurnitureType.Wallpaper:
                disposePosY = CatHouseManager.instance.furnitureDisposePosY[(int)disposeSlot.furnitureItemData.furnitureItem.furnitureType];
                break;
        }

        transform.localPosition = new Vector2( transform.localPosition.x, disposePosY );
    }
}
