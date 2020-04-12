using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureSaveData
{
    public string itemName;
    public int currentHaveNumber;
    public int currentDisposeNumber;

    public FurnitureSaveData( string _itemName,int _currentHaveNumber = 0, int _currentDisposeNumber = 0)
    {
        itemName = _itemName;
        currentHaveNumber = _currentHaveNumber;
        currentDisposeNumber = _currentDisposeNumber;
    }
}

public class FurnitureDisposeUI : MonoBehaviour
{
    public RectTransform[] rt_Buttons;

    [Space(5), Header("Sprite 변수")]
    //가구 종류별 버튼들의 활성화 Sprite
    public Sprite[] activationButtons_Sprite;
    //가구 종류별 버튼들의 비활성화 Sprite
    public Sprite[] deactivationButtons_Sprite;
    //가구 배치 버튼 Sprite
    public Sprite disposeButton_Sprite;
    //가구 교체 버튼 Sprite
    public Sprite replaceButton_Sprite;
    //기본 가구 슬롯 Sprite;
    public Sprite defaultFurnitureSlot_Sprite;
    //선택된 가구 슬롯 Sprite
    public Sprite selectFurnitureSlot_Sprite;
    //구매한 가구 보여주는 버튼 비활성화
    public Sprite toggleDeactivation_Sprite;
    //활성화
    public Sprite toggleActivate_Sprite;

    //버튼 이미지
    public Image toggle_Image;

    [Space( 5 ), Header( "가구 구매 팝업 관련 변수" )]
    public Text furniturePrice_Text;
    public Image gold_Image;

    [Space( 5 ), Header( "GameObject 변수" )]
    //가구 배치 UI 오브젝트
    public GameObject go_FurnitureDisposeUI;
    //보여줘야 하는 슬롯들의 부모 오브젝트
    public GameObject go_Content;
    //슬롯들을 보관하는 창고 오브젝트
    public GameObject go_FurnitureSlotStorage;
    //가구 구매 팝업 오브젝트
    public GameObject go_FurnitureBuyPopUp;

    public List<FurnitureSlot> furnitureSlotList = new List<FurnitureSlot>();

    //선택된 가구 슬롯
    [HideInInspector]
    public FurnitureSlot selectFurnitureSlot;
    //구매하려는 가구 슬롯
    [HideInInspector]
    public FurnitureSlot buyFurnitureSlot;

    private float activationButtonYHeight = 100;
    private float deactivationButtonYHeight = 70;

    private int activationNumber = -1;
    [HideInInspector]
    public Vector2 maxSlotImageSize = new Vector2( 230, 170 );

    [Space( 5 ), Header( "가구 아이템 리스트" )]
    //모든 가구 아이템 리스트
    public List<FurnitureItem> allFurnitureItem = new List<FurnitureItem>();

    [SerializeField]
    //보유 하고 있는 가구 아이템 정보 리스트
    public Dictionary<int, List<FurnitureItemData>> myFurnitureItemDic = new Dictionary<int, List<FurnitureItemData>>()
    {
        {0, new List<FurnitureItemData>()},
        {1, new List<FurnitureItemData>()},
        {2, new List<FurnitureItemData>()},
        {3, new List<FurnitureItemData>()},
        {4, new List<FurnitureItemData>()}
    };

    [SerializeField]
    //가구 타입 별로 분류한 딕셔너리
    public Dictionary<int, List<FurnitureItemData>> itemDataDic = new Dictionary<int, List<FurnitureItemData>>()
    {
        {0, new List<FurnitureItemData>()},
        {1, new List<FurnitureItemData>()},
        {2, new List<FurnitureItemData>()},
        {3, new List<FurnitureItemData>()},
        {4, new List<FurnitureItemData>()}
    };

    //구매한 가구만 보여주는 상태인지
    private bool isShowBuy = false;

    void Awake()
    {
        //모든 아이템을 각 가구 타입에 맞는 리스트에 추가한다.
        for (int i = 0; i < allFurnitureItem.Count; i++)
        {
            itemDataDic[(int)allFurnitureItem[i].furnitureType].Add( new FurnitureItemData(allFurnitureItem[i]));
        }

        for (int i = 0; i < itemDataDic.Count; i++)
        {
            FurnitureItemListSort( itemDataDic[i] );
        }
    }

    //가구 배치 버튼을 누르면 처리되어야 하는 것들
    public void SetFurnitureDisposeUI()
    {
        go_FurnitureDisposeUI.SetActive( true );

        isShowBuy = false;

        activationNumber = 0;
        toggle_Image.sprite = defaultFurnitureSlot_Sprite;

        //가구 배치 버튼을 누르면 기본으로 첫 번째 타입의 가구를 보여주게 설정
        OnClickSelectType( 0 );

        //BottomUI를 비활성화
        UIManager.instance.bottomUI.BottomUIDeactivate();
        
        UIManager.instance.topUI.rt_RankingButton.gameObject.SetActive( false );
        UIManager.instance.topUI.rt_AchievementButton.gameObject.SetActive( false );
        UIManager.instance.topUI.rt_SettingButton.gameObject.SetActive( false );
        UIManager.instance.topUI.rt_QuestButton.gameObject.SetActive( false );

        UIManager.instance.topUI.rt_SaveButton.gameObject.SetActive( true );

        Camera.main.GetComponent<CameraZoomMove>().SetFurnitureDispose();
    }

    public void ResetFurnitureDisposeUI()
    {
        go_FurnitureDisposeUI.SetActive( false );

        CatHouseManager.instance.FurnitureDisposeModeOff( activationNumber );

        //BottomUI를 활성화
        UIManager.instance.bottomUI.BottomUIActivation();

        UIManager.instance.topUI.rt_RankingButton.gameObject.SetActive( true );
        UIManager.instance.topUI.rt_AchievementButton.gameObject.SetActive( true );
        UIManager.instance.topUI.rt_SettingButton.gameObject.SetActive( true );
        UIManager.instance.topUI.rt_QuestButton.gameObject.SetActive( true );

        UIManager.instance.topUI.rt_SaveButton.gameObject.SetActive( false );

        Camera.main.GetComponent<CameraZoomMove>().ResetFurnitureDispose();
    }

    //어떤 종류의 가구를 보여줄지
    public void OnClickSelectType(int _typeNumber)
    {
        if(activationNumber != _typeNumber || (_typeNumber == 0 && activationNumber == 0))
        {
            if (selectFurnitureSlot != null)
            {
                selectFurnitureSlot.DefaultSlotImage();
                selectFurnitureSlot = null;
            }

            rt_Buttons[activationNumber].GetComponent<Image>().sprite = deactivationButtons_Sprite[activationNumber];
            rt_Buttons[activationNumber].sizeDelta = new Vector2( rt_Buttons[activationNumber].sizeDelta.x, 70 );

            rt_Buttons[_typeNumber].GetComponent<Image>().sprite = activationButtons_Sprite[_typeNumber];
            rt_Buttons[_typeNumber].sizeDelta = new Vector2( rt_Buttons[_typeNumber].sizeDelta.x, 100 );

            CatHouseManager.instance.FurnitureDisposeModeOn(_typeNumber, activationNumber);

            StoragePutFurnitureSlot();

            activationNumber = _typeNumber;

            StoragePullFurnitureSlot();
        }
    }

    public void SelectSlotNull()
    {
        selectFurnitureSlot.DefaultSlotImage();
        selectFurnitureSlot = null;
    }

    //DB에서 Load해온 아이템들을 가지고 있는 myFurnitureItemList에서 아이템들의 각 타입에 맞는 furnitureListDic 딕셔너리에 할당
    public void LoadFurnitureItem()
    {
        //가구 종류만큼 반복
        for (int i = 0; i < myFurnitureItemDic.Count; i++)
        {
            //가구 종류마다 자신이 소유하고 있는 가구 개수 만큼 반복
            for (int j = 0; j < myFurnitureItemDic[i].Count; j++)
            {
                //해당 종류의 가구들과 비교하여 같으면 저장되었던 데이터 할당
                for (int k = 0; k < itemDataDic[i].Count; k++)
                {
                    if (itemDataDic[i][k].furnitureItem.itemName == myFurnitureItemDic[i][j].furnitureItem.itemName)
                    {
                        itemDataDic[i][k].currentHaveNumber = myFurnitureItemDic[i][j].currentHaveNumber;
                        itemDataDic[i][k].currentDisposeNumber = myFurnitureItemDic[i][j].currentDisposeNumber;
                        break;
                    }
                }
            }
        }
    }

    //go_FurnitureSlotStorage의 자식으로 있는 FurnitureSlot들을 해당 타입의 아이템 개수 만큼 꺼내온다.
    public void StoragePullFurnitureSlot()
    {
        if(isShowBuy)
        {
            int index = 0;
            for (int i = 0; i < itemDataDic[activationNumber].Count; i++)
            {
                if (itemDataDic[activationNumber][i].currentHaveNumber > 0)
                {
                    furnitureSlotList[index].furnitureItemData = itemDataDic[activationNumber][i];
                    furnitureSlotList[index].transform.SetParent( go_Content.transform );
                    furnitureSlotList[index].SetFurnitureSlot();

                    index++;
                }
            }
        }
        else
        {
            for (int i = 0; i < itemDataDic[activationNumber].Count; i++)
            {
                Debug.Log( "하이" );
                furnitureSlotList[i].furnitureItemData = itemDataDic[activationNumber][i];
                furnitureSlotList[i].transform.SetParent( go_Content.transform );
                furnitureSlotList[i].SetFurnitureSlot();
            }
        }
    }

    //다시 Storage로 옮김
    public void StoragePutFurnitureSlot()
    {
        for (int i = 0; i < itemDataDic[activationNumber].Count; i++)
        {
            furnitureSlotList[i].transform.SetParent( go_FurnitureSlotStorage.transform );
        }
    }

    //가구 아이템 리스트의 요소들을 이름 순서대로 정렬
    public void FurnitureItemListSort(List<FurnitureItemData> _furnitureDataList)
    {
        _furnitureDataList.Sort( delegate ( FurnitureItemData itemData1, FurnitureItemData itemData2 )
        {
            return itemData1.furnitureItem.itemName.CompareTo( itemData2.furnitureItem.itemName );
        } );
    }

    //가구 구매 팝업
    public void FurnitureBuyPopUp( FurnitureSlot _buyFurnitureSlot )
    {
        buyFurnitureSlot = _buyFurnitureSlot;

        //구매 가능하면
        if (GoodsController.instance.SubGoldCheck( GoodsController.instance.goldList, buyFurnitureSlot.furnitureItemData.furnitureItem.itemPriceGoldList ))
        {
            UIManager.instance.PopUpActivation( go_FurnitureBuyPopUp );

            furniturePrice_Text.text = UIManager.instance.GoldChangeString( buyFurnitureSlot.furnitureItemData.furnitureItem.itemPriceGoldList ) + "가 소모 됩니다.";
            furniturePrice_Text.rectTransform.sizeDelta = new Vector2( furniturePrice_Text.preferredWidth, furniturePrice_Text.preferredHeight );

            gold_Image.rectTransform.anchoredPosition = new Vector2( -furniturePrice_Text.rectTransform.sizeDelta.x / 2 - 20f, gold_Image.rectTransform.anchoredPosition.y );
        }
        //구매 불가능
        else
        {
            UIManager.instance.popUpUI.GoldLackPopUp();
        }
    }

    //누르면 buyFurnitureSlot의 가구를 구매함
    public void OnClickFurnitureBuy()
    {
        //가구 가격만큼 골드를 뺌
        GoodsController.instance.SubGold( GoodsController.instance.goldList, buyFurnitureSlot.furnitureItemData.furnitureItem.itemPriceGoldList );

        buyFurnitureSlot.FurnitureBuyUpdate();

        //팝업 비활성화
        UIManager.instance.PopUpDeactivate();

        Debug.Log( buyFurnitureSlot.furnitureItemData.furnitureItem.itemName + " 구매" );
    }

    //가구 구매 취소
    public void OnClickFurnitureBuyCancel()
    {
        //팝업 비활성화
        UIManager.instance.PopUpDeactivate();
    }

    //구매한 가구들만 보여줌
    public void OnClickShowBuy()
    {
        isShowBuy = !isShowBuy;

        if(isShowBuy)
        {
            toggle_Image.sprite = toggleActivate_Sprite;

            //일단 슬롯들을 창고에 넣어준다.
            StoragePutFurnitureSlot();

            StoragePullFurnitureSlot();         
        }
        else
        {
            toggle_Image.sprite = toggleDeactivation_Sprite;

            StoragePutFurnitureSlot();
            StoragePullFurnitureSlot();
        }
    }
}
