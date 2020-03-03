using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleCatInformationUI : MonoBehaviour
{
    public GameObject       go_SimpleCatInformationUI;
    public RectTransform    rect_SimpleCatInformationUI;

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

        Cat cat = currentFloorInformation.catConsume.catSlot.cat;

        name_Text.text = cat.name;
        job_Text.text = cat.job;
        level_Text.text = cat.level.ToString();
       
        UIActivation();

        StartCoroutine( UpdateUIPosition() );
    }

    public IEnumerator UpdateUIPosition()
    {
        bool isFollow = true;

        Vector3 beforeCatScreenPos = Camera.main.WorldToScreenPoint( currentFloorInformation.catConsume.transform.position );

        //고양이의 포지션은 항상 같지만 카메라가 줌인, 줌아웃 됨으로써 스크린 좌표가 바뀜
        catScreenPos = Camera.main.WorldToScreenPoint( currentFloorInformation.catConsume.transform.position );

        float spacePosY = 150f;   

        go_SimpleCatInformationUI.transform.position = new Vector3( catScreenPos.x, catScreenPos.y + spacePosY, go_SimpleCatInformationUI.transform.position.z );

        while (isFollow)
        {
            catScreenPos = Camera.main.WorldToScreenPoint( currentFloorInformation.catConsume.transform.position );

            go_SimpleCatInformationUI.transform.position = new Vector3( catScreenPos.x, go_SimpleCatInformationUI.transform.position.y - (beforeCatScreenPos.y - catScreenPos.y), go_SimpleCatInformationUI.transform.position.z );

            beforeCatScreenPos = catScreenPos;

            Debug.Log( Camera.main.WorldToScreenPoint( currentFloorInformation.catConsume.transform.position ) + ", " + go_SimpleCatInformationUI.transform.position );

            Vector3 viewFloorInformationPos = Camera.main.WorldToViewportPoint( currentFloorInformation.transform.position );

            if((viewFloorInformationPos.x < 0 || viewFloorInformationPos.x > 1) && (viewFloorInformationPos.y < 0 || viewFloorInformationPos.y > 1))
            {
                isFollow = false;
                Debug.Log( "화면에서 벗어남" );
            }

            yield return null;
        }
    }
}
