using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//가구 배치 모드가 되면 층마다 나오는 버튼 오브젝트
public class FurnitureButton : MonoBehaviour, IPointerClickHandler
{

    private PlacedFurniture placedFurniture;
    //배치된 가구가 있다면 교체, 없으면 배치 이미지로 변경
    private SpriteRenderer buttonSprite;

    private void Awake()
    {
       placedFurniture = transform.parent.GetComponent<PlacedFurniture>();
        buttonSprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        if(Input.touchCount == 1)
        {
            //배치되어 있는 가구가 없다면
            if(placedFurniture.furnitureItem == null)
            {
                //가구를 배치 했으니 교체 스프라이트로 변경
                buttonSprite.sprite = UIManager.instance.furnitureDisposeUI.replaceButton_Sprite;
            }

            placedFurniture.ArrangeFurniture( UIManager.instance.furnitureDisposeUI.selectFurnitureSlot.furnitureItemData.furnitureItem );
        }
    }
}
