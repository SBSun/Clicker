using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloorInformation : MonoBehaviour, IPointerClickHandler
{
    //층에 배치되어 있는 고양이
    public CatConsume catConsume;

    //층에 배치되어 있는 가구들
    public FurnitureButton[] furnitureButtons;

    public bool isInvisible = false; //뷰포트 안에 존재하는지 체크, 존재하면 false 존재하지 않으면 true

    public void OnPointerClick( PointerEventData eventData )
    {
        if(catConsume != null && !Camera.main.GetComponent<CameraZoomMove>().isZooming)
        {
            UIManager.instance.simpleCatInformationUI.SetInformation( this );        
        }
    }
}
