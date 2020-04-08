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
}
