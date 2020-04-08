using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloorInformation : MonoBehaviour, IPointerClickHandler
{
    public GameObject go_BlankCat;
    //층에 배치되어 있는 고양이
    public CatConsume catConsume;

    //층에 배치되어 있는 가구들
    public PlacedFurniture[] floorDecorations;
    public PlacedFurniture[] wallDecorations;
    public PlacedFurniture[] topDecorations;
    public PlacedFurniture floor;
    public PlacedFurniture wallPaper;   

    public bool isInvisible = false; //뷰포트 안에 존재하는지 체크, 존재하면 false 존재하지 않으면 true

    public void OnPointerClick( PointerEventData eventData )
    {
        if(!Camera.main.GetComponent<CameraZoomMove>().isZooming)
        {
            UIManager.instance.simpleCatInformationUI.SetInformation( this );     
        }
    }
}
