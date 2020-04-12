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
                    for (int j = 0; j < floorInformationList[i].floorDecorations.Length; j++)
                    {
                        floorInformationList[i].floorDecorations[j].DisposeButtonActivate();
                    }
                    break;
                case 1:
                    for (int j = 0; j < floorInformationList[i].wallDecorations.Length; j++)
                    {
                        floorInformationList[i].wallDecorations[j].DisposeButtonActivate();
                    }
                    break;
                case 2:
                    for (int j = 0; j < floorInformationList[i].topDecorations.Length; j++)
                    {
                        floorInformationList[i].topDecorations[j].DisposeButtonActivate();
                    }
                    break;
            }

            if (_typeNumber != _activationNumber)
            {
                switch (_activationNumber)
                {
                    case 0:
                        for (int j = 0; j < floorInformationList[i].floorDecorations.Length; j++)
                        {
                            floorInformationList[i].floorDecorations[j].DisposeButtonDeactivate();
                        }
                        break;
                    case 1:
                        for (int j = 0; j < floorInformationList[i].wallDecorations.Length; j++)
                        {
                            floorInformationList[i].wallDecorations[j].DisposeButtonDeactivate();
                        }
                        break;
                    case 2:
                        for (int j = 0; j < floorInformationList[i].topDecorations.Length; j++)
                        {
                            floorInformationList[i].topDecorations[j].DisposeButtonDeactivate();
                        }
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
                    for (int j = 0; j < floorInformationList[i].floorDecorations.Length; j++)
                    {
                        floorInformationList[i].floorDecorations[j].DisposeButtonDeactivate();
                    }
                    break;
                case 1:
                    for (int j = 0; j < floorInformationList[i].wallDecorations.Length; j++)
                    {
                        floorInformationList[i].wallDecorations[j].DisposeButtonDeactivate();
                    }
                    break;
                case 2:
                    for (int j = 0; j < floorInformationList[i].topDecorations.Length; j++)
                    {
                        floorInformationList[i].topDecorations[j].DisposeButtonDeactivate();
                    }
                    break;
            }
        }
    }
}
