using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//가구 배치 모드가 되면 층마다 나오는 버튼 오브젝트
public class FurnitureButton : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick( PointerEventData eventData )
    {
        if(Input.touchCount == 1)
        {

        }
    }
}
