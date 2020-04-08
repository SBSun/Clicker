using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleCatInformationUI : MonoBehaviour
{
    public GameObject       go_SimpleCatInformationUI;
    public RectTransform    rt_SimpleCatInformationUI;

    public Text name_Text;          //이름
    public Text job_Text;           //직업
    public Text level_Text;         //레벨
    public Text consumeGold_Text;   //초당 소비 골드
    public Text maxKeepGold_Text;   //최대 소유 가능 골드

    public FloorInformation currentFloorInformation;

    public CameraZoomMove cameraZoomMove;

    Vector3 catScreenPos = Vector3.zero;

    public void UIActivation()
    {
        go_SimpleCatInformationUI.SetActive( true );
    }

    public void SetInformation(FloorInformation floorInformation)
    {
        currentFloorInformation = floorInformation;

        if(currentFloorInformation.catConsume.catSlot != null)
        {
            Cat cat = currentFloorInformation.catConsume.catSlot.cat;

            name_Text.text = cat.catInformation.catName;
            job_Text.text = cat.catInformation.job;
            level_Text.text = cat.level.ToString();
        }
        else
        {
            name_Text.text = "???";
            job_Text.text = "???";
            level_Text.text = "???";
        }
              
        UIActivation();

        StartCoroutine( UpdateUIPosition() );
    }

    public IEnumerator UpdateUIPosition()
    {
        bool isFollow = true;

        float ratio = 0f;

        while (isFollow)
        {
            //고양이의 포지션은 항상 같지만 카메라가 줌인, 줌아웃 됨으로써 스크린 좌표가 바뀜
            catScreenPos = Camera.main.WorldToScreenPoint( currentFloorInformation.catConsume.transform.position );

            ratio = Mathf.Lerp( 1, 0.6f, cameraZoomMove.mainCamera.orthographicSize / cameraZoomMove.zoomMin);

            go_SimpleCatInformationUI.transform.position = catScreenPos;
            rt_SimpleCatInformationUI.anchoredPosition = new Vector2( rt_SimpleCatInformationUI.anchoredPosition.x, rt_SimpleCatInformationUI.anchoredPosition.y + (rt_SimpleCatInformationUI.sizeDelta.y * ratio) );

            Vector3 viewFloorInformationPos = Camera.main.WorldToViewportPoint( currentFloorInformation.transform.position );

            Debug.Log( viewFloorInformationPos.y );
            if(viewFloorInformationPos.y < 0 || viewFloorInformationPos.y > 1)
            {
                isFollow = false;
                go_SimpleCatInformationUI.SetActive( false );
                break;
            }

            yield return null;
        }
    }
}
