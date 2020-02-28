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

    public void OnPointerClick( PointerEventData eventData )
    {
        if(catConsume != null && !Camera.main.GetComponent<CameraZoomMove>().isZooming)
        {
            Camera.main.GetComponent<CameraZoomMove>().SetCameraZoomMax(transform);           
        }
    }
}
