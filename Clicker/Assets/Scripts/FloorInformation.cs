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
    public PlacedFurniture[] placedFurnitures;

    public List<string> saveFloorInfo = new List<string>();

    public bool isInvisible = false; //뷰포트 안에 존재하는지 체크, 존재하면 false 존재하지 않으면 true

    public void OnPointerClick( PointerEventData eventData )
    {
        if(!Camera.main.GetComponent<CameraZoomMove>().isZooming && UIManager.instance.currentViewUI == UIManager.ViewUI.Main)
        {
            UIManager.instance.simpleCatInformationUI.SetInformation( this );     
        }
    }

    //Save 버튼을 누르면 호출된다
    public void SaveFloorInformation()
    {
        saveFloorInfo.Clear();

        for (int i = 0; i < placedFurnitures.Length; i++)
        {
            if(placedFurnitures[i].disposeFurniture != null)
            {
                saveFloorInfo.Add( placedFurnitures[i].disposeFurniture.furnitureItem.itemName );
                placedFurnitures[i].disposeFurniture.UpdateFurnitureItemData();              
            }
            else
            {
                saveFloorInfo.Add( "null" );
            }

            placedFurnitures[i].BeforePlacedFurniture();
        }
    }

    //층 정보 가져옴
    public void LoadFloorInformation()
    {
        //배치할 수 있는 가구 수만큼 반복
        for (int i = 0; i < placedFurnitures.Length; i++)
        {
            //저장된 배치된 가구가 없으면 넘어감
            if (saveFloorInfo[i] != null)
            {
                //0,1,2 - 바닥장식 3,4,5 - 벽장식 6,7,8 상단장식 9 - 바닥 10 - 벽지
                switch (i)
                {
                    case 0:
                    case 1:
                    case 2:
                        for (int j = 0; j < UIManager.instance.furnitureDisposeUI.itemDataDic[0].Count; j++)
                        {
                            if(saveFloorInfo[i] == UIManager.instance.furnitureDisposeUI.itemDataDic[0][j].furnitureItem.itemName)
                            {
                                placedFurnitures[i].LoadPlacedFurniture( UIManager.instance.furnitureDisposeUI.itemDataDic[0][j] );
                                break;
                            }
                        }
                        break;
                    case 3:
                    case 4:
                    case 5:
                        for (int j = 0; j < UIManager.instance.furnitureDisposeUI.itemDataDic[1].Count; j++)
                        {
                            if (saveFloorInfo[i] == UIManager.instance.furnitureDisposeUI.itemDataDic[1][j].furnitureItem.itemName)
                            {
                                placedFurnitures[i].LoadPlacedFurniture( UIManager.instance.furnitureDisposeUI.itemDataDic[1][j] );
                                break;
                            }
                        }
                        break;
                    case 6:
                    case 7:
                    case 8:
                        for (int j = 0; j < UIManager.instance.furnitureDisposeUI.itemDataDic[2].Count; j++)
                        {
                            if (saveFloorInfo[i] == UIManager.instance.furnitureDisposeUI.itemDataDic[2][j].furnitureItem.itemName)
                            {
                                placedFurnitures[i].LoadPlacedFurniture( UIManager.instance.furnitureDisposeUI.itemDataDic[2][j] );
                                break;
                            }
                        }
                        break;
                    case 9:
                        for (int j = 0; j < UIManager.instance.furnitureDisposeUI.itemDataDic[3].Count; j++)
                        {
                            if (saveFloorInfo[i] == UIManager.instance.furnitureDisposeUI.itemDataDic[3][j].furnitureItem.itemName)
                            {
                                placedFurnitures[i].LoadPlacedFurniture( UIManager.instance.furnitureDisposeUI.itemDataDic[3][j] );
                                break;
                            }
                        }
                        break;
                    case 10:
                        for (int j = 0; j < UIManager.instance.furnitureDisposeUI.itemDataDic[4].Count; j++)
                        {
                            if (saveFloorInfo[i] == UIManager.instance.furnitureDisposeUI.itemDataDic[4][j].furnitureItem.itemName)
                            {
                                placedFurnitures[i].LoadPlacedFurniture( UIManager.instance.furnitureDisposeUI.itemDataDic[4][j] );
                                break;
                            }
                        }
                        break;
                }
            }
        }
    }
}
