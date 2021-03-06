﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//등급 별 슬롯들
public class ClassCatSlot
{
    public List<CatSlot> catSlotList;
}

public class CatInventoryUI : MonoBehaviour
{
    public GameObject go_CatInventoryUI;

    [Header( "등급별 컴포넌트" )]
    //등급 별 슬롯들의 부모 객체
    public GameObject[] go_Contents;
    //등급 별 GridLayoutGroup
    public GridLayoutGroup[] gridLayout_Contents;

    //각 등급 객체의 공간
    public RectTransform[] rt_Classes;
    private Rect[] rect_Classes = new Rect[4];

    //각 등급의 슬롯들
    public ClassCatSlot[] classCatSlots = new ClassCatSlot[4];

    [Space( 3 )]
    [Header("Scoll관련 변수")]
    //스크롤 범위로 설정되는 공간
    public GameObject go_Scroll;
    public RectTransform rt_Scroll;
    private Rect rect_Scroll;
    public float beforeScrollPosY;   //고양이 슬롯을 누를 때의 스크롤 Y좌표
    public float beforeScrollHeight; //고양이 슬롯을 누를 때의 스크롤 Height

    [Space( 3 )]
    [Header("등급별 별, 구분선 이미지")]
    public RectTransform[] rt_divisionLineImages;
    public RectTransform[] rt_StartImages;

    [Space( 3 )]
    [Header("발견한 고양이만 보여주는 버튼")]
    public RectTransform rt_ShowDiscoveryButton;
    public Image showDiscovery_Image;
    public Sprite showDiscoveryActivation_Sprite; //활성화
    public Sprite showDiscoveryDisabled_Sprite; //비활성화

    [Space(3)]
    //고양이 슬롯을 눌렀을 때 나오는 고양이 정보 UI 구성 Text
    [Header( "Slot 터치 시 나오는 고양이 정보 UI" )]
    public GameObject go_CatInformationUI;
    public RectTransform rt_CatInformationImage;
    public RectTransform rt_ArrowImage;
    public Text name_Text;           //고양이 이름
    public Text job_Text;            //고양이 직업
    public Text introduction_Text;   //고양이 소개
    public Text level_Text;          //고양이 레벨
    public Text makeGold_Text;       //고양이 초당 급여 골드
    public Text consumeGold_Text;    //고양이 초당 소비 골드
    public Text maxKeepGold_Text;    //고양이 최대소지 가능 골드

    //true면 발견한 고양이만 보여줌
    public bool isShowDiscovery = false;

    void Awake()
    {
        rect_Scroll = rt_Scroll.rect;

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

        ContentSort();
        ScrollHeightChange();
    }
   
    void Start()
    {
        //가로 비율이 기준 비율보다 클 경우
        if (UIManager.instance.scale > 1)
        {
            SetScrollContent();
        }
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
             return catSlot1.cat.catInformation.catName.CompareTo( catSlot2.cat.catInformation.catName );
         } );

        //catLevel 기준으로 내림차순 정렬
        openCatSlotList.Sort( delegate ( CatSlot catSlot1, CatSlot catSlot2 )
        {
            if (catSlot1.cat.level > catSlot2.cat.level)
                return -1;
            else if (catSlot1.cat.level < catSlot2.cat.level)
                return 1;

            return 0;
        } );


        for (int i = 0; i < openCatSlotList.Count; i++)
        {
            openCatSlotList[i].transform.SetSiblingIndex( i );
        }
    }

    //등급별 포지션 정렬
    public void ContentSort()
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

    //발견한 등급들을 정렬
    public void ShowDiscoveryContentSort(List<int> rockCatSlotCountList )
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
            share = (classCatSlots[i].catSlotList.Count - rockCatSlotCountList[i]) / 3;     //몫
            remainder = (classCatSlots[i].catSlotList.Count - rockCatSlotCountList[i]) % 3; //나머지

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
            //버튼 이미지 변경
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
                        rt_divisionLineImages[i - 1].gameObject.SetActive(false);
                        break;
                    }
                }
            }

            ShowDiscoveryContentSort( rockCatSlotCountList );

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

            for (int i = 0; i < rt_divisionLineImages.Length; i++)
            {
                //비활성화 된 구분선이 있을 수 있으니 활성화
                rt_divisionLineImages[i].gameObject.SetActive( true );
            }

            ContentSort();
        }

        for (int i = 0; i < classCatSlots.Length; i++)
        {
            CatSlotSort( classCatSlots[i].catSlotList );
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

            rt_Scroll.sizeDelta = new Vector2( rt_Scroll.sizeDelta.x, height + 200);
        }
        else
        {
            for (int i = 0; i < rt_Classes.Length; i++)
            {
                if(rt_Classes[i].gameObject.activeSelf)
                     height += rt_Classes[i].sizeDelta.y;
            }

            if(height + 200 >= Screen.height)
                rt_Scroll.sizeDelta = new Vector2( rt_Scroll.sizeDelta.x, height + 200);
            else
                rt_Scroll.sizeDelta = new Vector2( rt_Scroll.sizeDelta.x, Screen.height );
        }

        beforeScrollHeight = rt_Scroll.sizeDelta.y;
        Debug.Log( "beforeScrollHeight" + beforeScrollHeight );
    }

    public void CatInformationPopUp(CatSlot _catSlot)
    { 
        //선택한 슬롯이 상단에 보이게 되는 Y좌표
        float yPos = 0f;

        //고양이의 등급 만큼 반복해서 더한다.
        for (int i = 0; i < (int)_catSlot.cat.catInformation.catClass; i++)
        {
            if (!rt_Classes[i].gameObject.activeSelf)
            {
                continue;
            }

            if (i < (int)_catSlot.cat.catInformation.catClass)
            {
                yPos += rt_Classes[i].sizeDelta.y;
            }
        }

        int share = 0;
        int remainder = 0;

        share = _catSlot.transform.GetSiblingIndex() / 3;
        remainder = _catSlot.transform.GetSiblingIndex() % 3;

        //화살표 이미지 x좌표 계산
        float arrowImagePosX = 0f;

        //나머지가 있으면
        if (remainder != 0)
        {
            yPos += share * 465;

            arrowImagePosX = (gridLayout_Contents[0].cellSize.x * (remainder - 1) + gridLayout_Contents[0].cellSize.x / 2) + gridLayout_Contents[0].padding.left + (gridLayout_Contents[0].spacing.x * (remainder - 1))
            - rt_ArrowImage.sizeDelta.x / 2 - 2f;
        }
        //나머지가 없으면
        else
        {
            if(share != 0)
                yPos += (share - 1) * 465;

            arrowImagePosX = (gridLayout_Contents[0].cellSize.x * 2 + gridLayout_Contents[0].cellSize.x / 2) + gridLayout_Contents[0].padding.left + (gridLayout_Contents[0].spacing.x * 2)
            - rt_ArrowImage.sizeDelta.x / 2 - 2f;
        }

        //화살표 이미지 위치 조정
        rt_ArrowImage.anchoredPosition = new Vector2( arrowImagePosX + 10f, rt_ArrowImage.anchoredPosition.y );

        //윗 공백 추가
        yPos = yPos + 220f;

        //yPos가 스크롤 범위를 초과하면
        if (yPos > rt_Scroll.sizeDelta.y - UIManager.instance.canvasScaler.referenceResolution.y)
        {
            float addHeight = yPos - (rt_Scroll.sizeDelta.y - UIManager.instance.canvasScaler.referenceResolution.y);

            beforeScrollHeight = rt_Scroll.sizeDelta.y;
            Debug.Log( "beforeScrollHeight" + beforeScrollHeight );

            rt_Scroll.sizeDelta = new Vector2( rt_Scroll.sizeDelta.x, rt_Scroll.sizeDelta.y + addHeight );       
        }

        rt_Scroll.anchoredPosition = new Vector2( 0, yPos);
    }

    //게임이 시작할 때 인벤토리에서 변경되어야 할 UI의 위치나 사이즈
    public void SetScrollContent()
    {
        //화면 크기에 따라 GridLayoutGroup의 Spacing, Padding 조절
        //현재 화면의 width - 왼쪽 공백 - 오른쪽 공백 - CellSize * (Constraint - 1)
        float space = (UIManager.instance.widthMaxUI - gridLayout_Contents[0].cellSize.x * gridLayout_Contents[0].constraintCount) / 4;

        for (int i = 0; i < rt_Classes.Length; i++)
        {
            rt_Classes[i].sizeDelta = new Vector2( UIManager.instance.widthMaxUI, rt_Classes[i].sizeDelta.y );
            gridLayout_Contents[i].spacing = new Vector2( space, gridLayout_Contents[i].spacing.y );
            gridLayout_Contents[i].padding.left = Mathf.RoundToInt(space);
            gridLayout_Contents[i].padding.right = Mathf.RoundToInt( space );

            //별 이미지의 위치도 변경
            rt_StartImages[i].anchoredPosition = new Vector2( gridLayout_Contents[i].padding.left, rt_StartImages[i].anchoredPosition.y );
        }

        rt_Scroll.sizeDelta = new Vector2( UIManager.instance.widthMaxUI, rt_Scroll.sizeDelta.y );

        //버튼 위치 변경
        rt_ShowDiscoveryButton.anchoredPosition = new Vector2( -gridLayout_Contents[0].padding.right, rt_ShowDiscoveryButton.anchoredPosition.y );
    }

    //인벤토리 버튼을 눌렀을 때 초기화 해야 하는 것들
    public void SetCatInventoryUI()
    {
        //스크롤을 원위치로
        rt_Scroll.anchoredPosition = Vector2.zero;
        
        //발견한 고양이만 보이게 설정되어 있다면 모든 고양이가 보이게
        if(isShowDiscovery)
        {
            OnClickShowDiscovery();
          
            //버튼 Sprte를 비활성화 Sprite로 변경
            showDiscovery_Image.sprite = showDiscoveryDisabled_Sprite;
        }
    }
}

