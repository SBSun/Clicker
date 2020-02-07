using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//등급 별 슬롯들
public class ClassCatSlot
{
    public List<CatSlot> catSlotList;
}

public class CatInventory : MonoBehaviour
{
    //등급 별 슬롯들의 부모 객체
    public GameObject[] go_Contents;

    //각 등급 객체의 공간
    public RectTransform[] rt_Classes;
    private Rect[] rect_Classes = new Rect[4];

    //스크롤 범위로 설정되는 공간
    public GameObject go_ContentsGroup;
    public RectTransform rt_ContentsGroup;
    private Rect rect_ContentsGroup;

    //각 등급의 슬롯들
    public ClassCatSlot[] classCatSlots = new ClassCatSlot[4];

    public Image[] divisionLine_Images;

    //버튼
    public Image showDiscovery_Image;
    public Sprite showDiscoveryActivation_Sprite; //활성화
    public Sprite showDiscoveryDisabled_Sprite; //비활성화

    //고양이 슬롯을 눌렀을 때 나오는 고양이 정보 UI 구성 Text
    [Header( "Slot 터치 시 나오는 고양이 정보 UI" )]
    public GameObject go_CatInformationUI;
    public Text catName_Text;           //고양이 이름
    public Text catJob_Text;            //고양이 직업
    public Text catIntroduction_Text;   //고양이 소개
    public Text catLevel_Text;          //고양이 레벨
    public Text catMakeGold_Text;       //고양이 초당 급여 골드
    public Text catConsumeGold_Text;    //고양이 초당 소비 골드
    public Text catMaxKeepGold_Text;    //고양이 최대소지 가능 골드

    //true면 발견한 고양이만 보여줌
    public bool isShowDiscovery = false;

    void Awake()
    {
        rect_ContentsGroup = rt_ContentsGroup.rect;

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

            rect_Classes[i] = rt_Classes[i].rect;

            CatSlotSort( classCatSlots[i].catSlotList );
        }

        ContentTransformSort();

        Debug.Log(go_ContentsGroup.transform.position + ", " + classCatSlots[2].catSlotList[4].transform.position );
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

    public void ShowDiscoveryContentTransformSort(List<int> rockCatSlotCountList )
    {
        int share = 0;
        int remainder = 0;

        //발견한 고양이가 1마리 이상인 등급
        List<int> showDiscoveryContentList = new List<int>();   

        for (int i = 0; i < classCatSlots.Length; i++)
        {
            if (!rt_Classes[i].gameObject.activeSelf)
                continue;

            showDiscoveryContentList.Add( i );
        }

        for (int i = 0; i < showDiscoveryContentList.Count; i++)
        {
            share = (classCatSlots[i].catSlotList.Count - rockCatSlotCountList[i]) / 3;
            remainder = (classCatSlots[i].catSlotList.Count - rockCatSlotCountList[i]) % 3;

            //50은 위에 Star_Image와 DivisionLine_Image를 위한 여유 공간
            if (remainder != 0)
            {
                rt_Classes[showDiscoveryContentList[i]].sizeDelta = new Vector2( rect_Classes[showDiscoveryContentList[i]].width, (share + 1) * 500 + 50 );
            }             
            else
            {
                rt_Classes[showDiscoveryContentList[i]].sizeDelta = new Vector2( rect_Classes[showDiscoveryContentList[i]].width, share * 500 + 50 );
            }
                

            if (i == 0)
                rt_Classes[showDiscoveryContentList[0]].anchoredPosition = new Vector2( 0, -200f );
            else
                rt_Classes[showDiscoveryContentList[i]].anchoredPosition = new Vector2( 0, rt_Classes[showDiscoveryContentList[i - 1]].anchoredPosition.y - rt_Classes[showDiscoveryContentList[i - 1]].sizeDelta.y );
        }       

        showDiscoveryContentList.Clear();
    }

    //발견된 고양이들만 보여주는 함수
    public void OnClickShowDiscovery()
    {
        List<CatSlot> rockCatSlotList = new List<CatSlot>();
        List<int> rockCatSlotCountList = new List<int>();

        isShowDiscovery = !isShowDiscovery;

        //발견된 고양이들만 보이게
        if(isShowDiscovery)
        {
            showDiscovery_Image.sprite = showDiscoveryActivation_Sprite;

            for (int i = 0; i < classCatSlots.Length; i++)
            {
                int rockCatSlotCount = 0;

                for (int j = 0; j < classCatSlots[i].catSlotList.Count; j++)
                {                  
                    //발견 못 한 고양이 슬롯을 rockCatSlotList에 추가
                    if (classCatSlots[i].catSlotList[j].slotStatus == CatSlot.SlotStatus.Rock)
                    {
                        rockCatSlotList.Add( classCatSlots[i].catSlotList[j] );
                        rockCatSlotCount++;
                    }
                }

                //발견한 고양이가 1마리 이상일 경우에만 추가
                if(rockCatSlotCount < classCatSlots[i].catSlotList.Count)
                    rockCatSlotCountList.Add( rockCatSlotCount );

                //해당 등급의 고양이를 한 마리라도 발견 못했으면
                if (rockCatSlotList.Count != 0)
                {
                    for (int k = 0; k < rockCatSlotList.Count; k++)
                    {
                        if (rockCatSlotList.Count == classCatSlots[i].catSlotList.Count)
                        {
                            rt_Classes[i].gameObject.SetActive( false );
                            break;
                        }

                        rockCatSlotList[k].gameObject.SetActive( false );
                    }

                    //해당 등급의 고양이들이 1마리도 발견이 안되었다면 아예 rt_Classes를 비활성화 시켜준다.                
                }

                rockCatSlotList.Clear();
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

            ShowDiscoveryContentTransformSort( rockCatSlotCountList );

            //리스트 초기화
            rockCatSlotCountList.Clear();
        }
        //모든 고양이 슬롯 보이게
        else
        {
            showDiscovery_Image.sprite = showDiscoveryDisabled_Sprite;

            for (int i = 0; i < classCatSlots.Length; i++)
            {
                //비활성화 되어있다면
                if(!rt_Classes[i].gameObject.activeSelf)
                {
                    rt_Classes[i].gameObject.SetActive( true );
                }
                else
                {
                    for (int j = 0; j < classCatSlots[i].catSlotList.Count; j++)
                    {
                        classCatSlots[i].catSlotList[j].gameObject.SetActive( true );
                    }
                }
            }

            for (int i = 0; i < divisionLine_Images.Length; i++)
            {
                //비활성화 된 구분선이 있을 수 있으니 활성화
                divisionLine_Images[i].enabled = true;
            }

            ContentTransformSort();
        }

        ScrollHeightChange();
    }

    //스크롤의 범위를 변경
    public void ScrollHeightChange()
    {
        float height = 0;

        if (!isShowDiscovery)
        {
            for (int i = 0; i < rt_Classes.Length; i++)
            {
                height += rt_Classes[i].sizeDelta.y;
            }

            rt_ContentsGroup.sizeDelta = new Vector2( rt_ContentsGroup.sizeDelta.x, height + 200);
        }
        else
        {
            for (int i = 0; i < rt_Classes.Length; i++)
            {
                if(rt_Classes[i].gameObject.activeSelf)
                     height += rt_Classes[i].sizeDelta.y;
            }

            rt_ContentsGroup.sizeDelta = new Vector2( rt_ContentsGroup.sizeDelta.x, height + 200);
        }
    }
}
