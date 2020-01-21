using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassCatSlot
{
    public List<CatSlot> catSlotList;
}

public class CatInventory : MonoBehaviour
{
    //등급 별 슬롯들의 부모 객체
    public GameObject[] go_Contents;
    //발견된 고양이들만 보여주는 함수를 실행할 때 발견 못한 슬롯들을 go_DisableSlots의 자식으로 옮겨 비활성화 시켜준다.
    public GameObject[] go_DisableContents;

    public RectTransform[] rt_Classes;
    private Rect[] rect_Classes = new Rect[4];

    public ClassCatSlot[] classCatSlots = new ClassCatSlot[4];

    public Image[] divisionLine_Images;

    //true면 발견한 고양이만 보여줌
    public bool isShowDiscovery = false;

    void Awake()
    {
        for (int i = 0; i < go_Contents.Length; i++)
        {
            CatSlot[] catSlots = go_Contents[i].GetComponentsInChildren<CatSlot>();

            classCatSlots[i] = new ClassCatSlot
            {
                catSlotList = new List<CatSlot>()
            };

            for (int j = 0; j < catSlots.Length; j++)
            {
                classCatSlots[i].catSlotList.Add( catSlots[j] );
            }
        }

        for (int i = 0; i < rt_Classes.Length; i++)
        {
            rect_Classes[i] = rt_Classes[i].rect;
        }

        for (int i = 0; i < classCatSlots.Length; i++)
        {
            CatSlotSort( classCatSlots[i].catSlotList );
        }

        ContentTransformSort();
    }

    //슬롯 정렬
    public void CatSlotSort(List<CatSlot> catSlotList)
    {
        List<CatSlot> openCatSlotList = new List<CatSlot>();
        List<CatSlot> rockCatSlotList = new List<CatSlot>();

        for (int i = 0; i < catSlotList.Count; i++)
        {
            if (catSlotList[i].slotStatus == CatSlot.SlotStatus.Open)
                openCatSlotList.Add( catSlotList[i] );
            else
                rockCatSlotList.Add( catSlotList[i] );
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
        int share = 0;
        int remainder = 0;

        for (int i = 0; i < classCatSlots.Length; i++)
        {
            share = classCatSlots[i].catSlotList.Count / 3;
            remainder = classCatSlots[i].catSlotList.Count % 3;


            //50은 위에 Star_Image와 DivisionLine_Image를 위한 여유 공간
            if (remainder != 0)
                rt_Classes[i].sizeDelta = new Vector2( rect_Classes[i].width ,(share + 1) * 500 + 50);
            else
                rt_Classes[i].sizeDelta = new Vector2( rect_Classes[i].width, share * 500 + 50 );
        }

        rt_Classes[0].anchoredPosition = new Vector2( 0, -200f );
        rt_Classes[1].anchoredPosition = new Vector2( 0, rt_Classes[0].anchoredPosition.y - rt_Classes[0].sizeDelta.y );
        rt_Classes[2].anchoredPosition = new Vector2( 0, rt_Classes[1].anchoredPosition.y - rt_Classes[1].sizeDelta.y );
        rt_Classes[3].anchoredPosition = new Vector2( 0, rt_Classes[2].anchoredPosition.y - rt_Classes[2].sizeDelta.y );
    }

    //발견된 고양이들만 보여주는 함수
    public void OnClickShowDiscovery()
    {
        List<CatSlot> rockCatSlotList = new List<CatSlot>();

        isShowDiscovery = !isShowDiscovery;

        //발견된 고양이들만 보이게
        if(isShowDiscovery)
        {
            for (int i = 0; i < classCatSlots.Length; i++)
            {
                for (int j = 0; j < classCatSlots[i].catSlotList.Count; j++)
                {
                    //발견 못 한 고양이 슬롯을 rockCatSlotList에 추가
                    if (classCatSlots[i].catSlotList[j].slotStatus == CatSlot.SlotStatus.Rock)
                    {
                        rockCatSlotList.Add( classCatSlots[i].catSlotList[j] );
                    }
                }

                //해당 등급의 고양이를 한 마리라도 발견 못했으면
                if (rockCatSlotList.Count != 0)
                {
                    // go_DisableContents의 자식으로 옮긴 뒤 비활성화 시켜준다.
                    for (int k = 0; k < rockCatSlotList.Count; k++)
                    {
                        rockCatSlotList[k].gameObject.SetActive( false );
                        rockCatSlotList[k].transform.SetParent( go_DisableContents[i].transform );
                    }

                    //해당 등급의 고양이들이 1마리도 발견이 안되었다면 아예 rt_Classes를 비활성화 시켜준다.
                    if (rockCatSlotList.Count == classCatSlots[i].catSlotList.Count)
                    {
                        rt_Classes[i].gameObject.SetActive( false );
                    }

                    //리스트 초기화
                    rockCatSlotList.Clear();          
                }
            }

            //별 5개 등급의 고양이를 한 마리도 발견 못했다면 제일 위에 있는 구분선 비활성화
            if(!rt_Classes[0].gameObject.activeSelf)
            {
                for (int i = 1; i < rt_Classes.Length; i++)
                {
                    if(rt_Classes[i].gameObject.activeSelf)
                    {
                        divisionLine_Images[i - 1].enabled = false;
                        break;
                    }
                }
            }
        }
        //모든 고양이 슬롯 보이게
        else
        {
            for (int i = 0; i < rt_Classes.Length; i++)
            {
                //비활성화 되어있다면
                if(!rt_Classes[i].gameObject.activeSelf)
                {
                    rt_Classes[i].gameObject.SetActive( true );
                }
                else
                {
                    CatSlot[] disableSlots;

                    disableSlots = go_DisableContents[i].GetComponentsInChildren<CatSlot>();

                    for (int j = 0; j < disableSlots.Length; j++)
                    {
                        disableSlots[i].transform.SetParent( go_Contents[i].transform );
                        disableSlots[i].gameObject.SetActive( true );
                    }
                }
            }
        }    
    }
}
