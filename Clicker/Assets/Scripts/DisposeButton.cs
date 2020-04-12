using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//가구 배치 모드가 되면 층마다 나오는 버튼 오브젝트
public class DisposeButton : MonoBehaviour, IPointerClickHandler
{
    public PlacedFurniture placedFurniture;

    //배치된 가구가 있다면 교체, 없으면 배치 이미지로 변경
    [SerializeField]
    private SpriteRenderer buttonSprite;

    public void OnPointerClick( PointerEventData eventData )
    {
        //선택된 슬롯이 있다면
        if (UIManager.instance.furnitureDisposeUI.selectFurnitureSlot != null)
        {
            if (UIManager.instance.furnitureDisposeUI.selectFurnitureSlot == placedFurniture.disposeSlot)
            {
                Debug.Log( "배치된 가구를 뺀다" );
                placedFurniture.SubFurniture();
                return;
            }

            placedFurniture.ArrangeFurniture( UIManager.instance.furnitureDisposeUI.selectFurnitureSlot );
        }
    }

    public void ChangeDisposeButton()
    {
        buttonSprite.sprite = UIManager.instance.furnitureDisposeUI.disposeButton_Sprite;
    }

    public void ChangeReplaceButton()
    {
        buttonSprite.sprite = UIManager.instance.furnitureDisposeUI.replaceButton_Sprite;
    }
}
