using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedFurniture : MonoBehaviour
{
    //배치된 가구 슬롯
    public FurnitureItemData disposeFurniture;
    public FurnitureItemData beforeDisposeFurniture;

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

    public void ArrangeFurniture( FurnitureSlot _furnitureSlot )
    {
        //배치 된 가구가 있다면 교체
        if(disposeFurniture != null)
        {
            //배치되어 있는 가구와 인수로 들어온 가구가 같다면
            if(disposeFurniture.furnitureSlot == _furnitureSlot)
            {
                Debug.Log( "가구를 뺀다" );
                //배치되어 있는 가구를 빼준다.
                SubFurniture();
                return;
            }

            //소유하고 있는 가구 개수 - 배치한 가구 개수 > 0
            if (_furnitureSlot.furnitureItemData.currentHaveNumber - _furnitureSlot.furnitureItemData.currentDisposeNumber > 0)
            {
                SubFurniture();

                //현재 가구를 인수로 들어온 가구로 변경
                AddFurniture( _furnitureSlot.furnitureItemData );
            }
            else
                return;

        }
        //없다면 배치
        else
        {
            if(_furnitureSlot.furnitureItemData.currentHaveNumber - _furnitureSlot.furnitureItemData.currentDisposeNumber > 0)
            {
                AddFurniture( _furnitureSlot.furnitureItemData );
            }
            else
                return;
        }

        SetFurnitureSprite();
    }

    public void AddFurniture(FurnitureItemData _furnitureItemData)
    {
        disposeFurniture = _furnitureItemData;
        disposeFurniture.DisposeAdd();
        disposeButton.ChangeReplaceButton();
        SetFurnitureSprite();
    }

    public void SubFurniture()
    {
        disposeFurniture.DisposeSub();
        disposeFurniture = null;
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
        switch(disposeFurniture.furnitureItem.furnitureType)
        {
            case FurnitureItem.FurnitureType.FloorDecoration:
                disposePosY = CatHouseManager.instance.furnitureDisposePosY[(int)disposeFurniture.furnitureItem.furnitureType] + localSpriteSize.y / 2;
                break;
            case FurnitureItem.FurnitureType.WallDecoration:
                disposePosY = CatHouseManager.instance.furnitureDisposePosY[(int)disposeFurniture.furnitureItem.furnitureType];
                break;
            case FurnitureItem.FurnitureType.TopDecoration:
                disposePosY = CatHouseManager.instance.furnitureDisposePosY[(int)disposeFurniture.furnitureItem.furnitureType] - localSpriteSize.y / 2;
                break;
            case FurnitureItem.FurnitureType.Floor:
                disposePosY = CatHouseManager.instance.furnitureDisposePosY[(int)disposeFurniture.furnitureItem.furnitureType] + localSpriteSize.y / 2;
                break;
            case FurnitureItem.FurnitureType.Wallpaper:
                disposePosY = CatHouseManager.instance.furnitureDisposePosY[(int)disposeFurniture.furnitureItem.furnitureType];
                break;
        }

        transform.localPosition = new Vector2( transform.localPosition.x, disposePosY );
    }
    
    //가구 이미지를 설정
    public void SetFurnitureSprite()
    {
        spriteRenderer.sprite = disposeFurniture.furnitureItem.itemSprite;

        GetSpriteSize();

        CalculateFurniturePosY();
    }

    public void LoadPlacedFurniture(FurnitureItemData _furnitureItemData )
    {
        disposeFurniture = _furnitureItemData;
        disposeButton.ChangeReplaceButton();
        SetFurnitureSprite();
    }

    public void BeforePlacedFurniture()
    {
        if(disposeFurniture != null)
        {
            beforeDisposeFurniture = disposeFurniture;
        }
        else
        {
            beforeDisposeFurniture = null;
        }
    }

    public void GoBackBefore()
    {
        if(beforeDisposeFurniture != null)
        {
            //같으면 변경할 필요가 없다.
            if(beforeDisposeFurniture == disposeFurniture)
            {
                return;
            }

            SubFurniture();

            AddFurniture( beforeDisposeFurniture );

            beforeDisposeFurniture = null;
        }
        else
        {
            if(disposeFurniture != null)
            {
                SubFurniture();
            }
        }
    }
}
