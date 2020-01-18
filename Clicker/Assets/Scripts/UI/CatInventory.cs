using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatInventory : MonoBehaviour
{
    public GameObject[] go_Contents;
    public RectTransform[] rt_Classes;
    private Rect[] rect_Classes = new Rect[2];

    public CatSlot[] two_CatSlots;
    public CatSlot[] three_CatSlots;
    public CatSlot[] four_CatSlots;
    public CatSlot[] five_CatSlots;

    void Awake()
    {
        two_CatSlots = go_Contents[0].GetComponentsInChildren<CatSlot>();

        for (int i = 0; i < rt_Classes.Length; i++)
        {
            rect_Classes[i] = rt_Classes[i].rect;
        }

        CatSlotSort(two_CatSlots);

        ContentTransformSort();
    }

    //슬롯 정렬
    public void CatSlotSort(CatSlot[] catSlots)
    {
        List<CatSlot> openCatSlotList = new List<CatSlot>();
        List<CatSlot> rockCatSlotList = new List<CatSlot>();

        for (int i = 0; i < catSlots.Length; i++)
        {
            if (catSlots[i].slotStatus == CatSlot.SlotStatus.Open)
                openCatSlotList.Add( catSlots[i] );
            else
                rockCatSlotList.Add( catSlots[i] );
        }

        //catName 기준으로 오름차순 정렬
        openCatSlotList.Sort( delegate ( CatSlot catSlot1, CatSlot catSlot2 )
         {
             return catSlot1.cat.catName.CompareTo( catSlot2.cat.catName );
         } );

        //catLevel 기준으로 내림차순 정렬
        openCatSlotList.Sort( delegate ( CatSlot catSlot1, CatSlot catSlot2 )
        {
            if (catSlot1.cat.catLevel > catSlot2.cat.catLevel)
                return -1;
            else if (catSlot1.cat.catLevel < catSlot2.cat.catLevel)
                return 1;

            return 0;
        } );


        for (int i = 0; i < openCatSlotList.Count; i++)
        {
            openCatSlotList[i].transform.SetSiblingIndex( i );
        }
    }

    public void ContentTransformSort()
    {
        Debug.Log( rect_Classes[1].yMin + ", " + rect_Classes[1].yMax ); 
        rt_Classes[1].anchoredPosition = new Vector2( 0, rt_Classes[0].anchoredPosition.y - (rect_Classes[0].height / 2) - (rect_Classes[1].height / 2));
    }

    //발견된 고양이들만 보여주는 함수
    public void OnClickShowDiscovery()
    {

    }
}
