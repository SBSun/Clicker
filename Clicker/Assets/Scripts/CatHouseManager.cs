using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatHouseManager : MonoBehaviour
{
    private static CatHouseManager m_instance;

    public static CatHouseManager instance
    {
        get
        {
            if (m_instance != null)
                return m_instance;

            m_instance = FindObjectOfType<CatHouseManager>();

            if (m_instance == null)
                m_instance = new GameObject( name: "CatHouseManager" ).AddComponent<CatHouseManager>();

            return m_instance;
        }
    }

    //CatHoust의 각 층 정보 리스트
    public List<FloorInformation> floorInformationList = new List<FloorInformation>();

    public float[] furnitureDisposePosY;

    //가구 배치 모드로 변경
    public void FurnitureDisposeModeOn(int _typeNumber, int _activationNumber)
    {
        for (int i = 0; i < floorInformationList.Count; i++)
        {
            switch(_typeNumber)
            {
                case 0:
                    for (int j = 0; j < 3; j++)
                    {
                        floorInformationList[i].placedFurnitures[j].DisposeButtonActivate();
                    }
                    break;
                case 1:
                    for (int j = 3; j < 6; j++)
                    {
                        floorInformationList[i].placedFurnitures[j].DisposeButtonActivate();
                    }
                    break;
                case 2:
                    for (int j = 6; j < 9; j++)
                    {
                        floorInformationList[i].placedFurnitures[j].DisposeButtonActivate();
                    }
                    break;
                case 3:
                    floorInformationList[i].placedFurnitures[9].DisposeButtonActivate();
                    break;
                case 4:
                    floorInformationList[i].placedFurnitures[10].DisposeButtonActivate();
                    break;
            }

            if (_typeNumber != _activationNumber)
            {
                switch (_activationNumber)
                {
                    case 0:
                        for (int j = 0; j < 3; j++)
                        {
                            floorInformationList[i].placedFurnitures[j].DisposeButtonDeactivate();
                        }
                        break;
                    case 1:
                        for (int j = 3; j < 6; j++)
                        {
                            floorInformationList[i].placedFurnitures[j].DisposeButtonDeactivate();
                        }
                        break;
                    case 2:
                        for (int j = 6; j < 9; j++)
                        {
                            floorInformationList[i].placedFurnitures[j].DisposeButtonDeactivate();
                        }
                        break;
                    case 3:
                        floorInformationList[i].placedFurnitures[9].DisposeButtonDeactivate();
                        break;
                    case 4:
                        floorInformationList[i].placedFurnitures[10].DisposeButtonDeactivate();
                        break;
                }
            }
        }
    }

    //가구 배치 모드 해제, 현재 타입을 인수로 받음
    public void FurnitureDisposeModeOff(int _activationNumber)
    {
        for (int i = 0; i < floorInformationList.Count; i++)
        {
            switch (_activationNumber)
            {
                case 0:
                    for (int j = 0; j < 3; j++)
                    {
                        floorInformationList[i].placedFurnitures[j].DisposeButtonDeactivate();
                    }
                    break;
                case 1:
                    for (int j = 3; j < 6; j++)
                    {
                        floorInformationList[i].placedFurnitures[j].DisposeButtonDeactivate();
                    }
                    break;
                case 2:
                    for (int j = 6; j < 9; j++)
                    {
                        floorInformationList[i].placedFurnitures[j].DisposeButtonDeactivate();
                    }
                    break;
                case 3:
                    floorInformationList[i].placedFurnitures[9].DisposeButtonDeactivate();
                    break;
                case 4:
                    floorInformationList[i].placedFurnitures[10].DisposeButtonDeactivate();
                    break;
            }
        }

        NotSaveCatHouse();
    }

    public void SaveCatHouse()
    {
        for (int i = 0; i < floorInformationList.Count; i++)
        {
            floorInformationList[i].SaveFloorInformation();
            BackEndManager.instance.backEndDataSave.UpdateFloorInformation( floorInformationList[i] );
        }
    }

    //Save 버튼을 누르지 않고 배치모드 나왔을 때 호출, 전의 집 상태로 되돌린다.
    public void NotSaveCatHouse()
    {
        for (int i = 0; i < floorInformationList.Count; i++)
        {
            for (int j = 0; j < floorInformationList[i].placedFurnitures.Length; j++)
            {
                floorInformationList[i].placedFurnitures[j].GoBackBefore();
            }
        }
    }

    //배치모드 들어오기 전의 집 상태 저장
    public void SaveBeforeCatHouse()
    {
        for (int i = 0; i < floorInformationList.Count; i++)
        {
            for (int j = 0; j < floorInformationList[i].placedFurnitures.Length; j++)
            {
                floorInformationList[i].placedFurnitures[j].BeforePlacedFurniture();
            }
        }
    }
}
