using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureSaveData
{
    public string itemName;
    public int furnitureType;
    public int currentHaveNumber;

    public FurnitureSaveData( string _itemName, int _furnitureType, int _currentHaveNumber = 0)
    {
        itemName = _itemName;
        furnitureType = _furnitureType;
        currentHaveNumber = _currentHaveNumber;
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

    private float activationButtonYHeight = 100;
    private float deactivationButtonYHeight = 70;

    private int activationNumber = 0;
    [HideInInspector]
    public Vector2 maxSlotImageSize = new Vector2( 230, 170 );

    [Space( 5 ), Header( "가구 아이템 리스트" )]
    //모든 가구 아이템 리스트
    public List<FurnitureItem> allFurnitureItem = new List<FurnitureItem>();
    //보유 하고 있는 가구 아이템 정보 리스트
    public List<FurnitureItemData> myFurnitureItemData = new List<FurnitureItemData>();

    //가구 타입 별로 분류한 딕셔너리
    public Dictionary<int, List<FurnitureItemData>> itemDataDic = new Dictionary<int, List<FurnitureItemData>>()
    {
        {0, new List<FurnitureItemData>()},
        {1, new List<FurnitureItemData>()},
        {2, new List<FurnitureItemData>()},
        {3, new List<FurnitureItemData>()},
        {4, new List<FurnitureItemData>()}
    };

    void Awake()
    {
        //모든 아이템을 각 가구 타입에 맞는 리스트에 추가한다.
        for (int i = 0; i < allFurnitureItem.Count; i++)
        {
            itemDataDic[(int)allFurnitureItem[i].furnitureType].Add( new FurnitureItemData(allFurnitureItem[i]));
        }

        for (int i = 0; i < itemDataDic.Count; i++)
        {
            itemDataDic[i].Sort( delegate ( FurnitureItemData itemData1, FurnitureItemData itemData2 )
             {
                 return itemData1.furnitureItem.itemName.CompareTo( itemData2.furnitureItem.itemName );
             } );
        }

        /* //내가 소유하고 있는 가구 아이템의 데이터를 불러온 후 각 가구 타입에 대입
        for (int i = 0; i < myFurnitureItemList.Count; i++)
        {
            for (int j = 0; j < furnitureListDic[(int)myFurnitureItemList[i].furnitureType].Count; j++)
            {
                if (myFurnitureItemList[i].itemName == furnitureListDic[(int)myFurnitureItemList[i].furnitureType][j].itemName)
                {
                    furnitureListDic[(int)myFurnitureItemList[i].furnitureType][j].currentHaveNumber = myFurnitureItemList[i].currentHaveNumber;
                }
            }
        }*/
    }

    //가구 배치 버튼을 누르면 처리되어야 하는 것들
    public void SetFurnitureDisposeUI()
    {
        go_FurnitureDisposeUI.SetActive( true );

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

        StoragePullFurnitureSlot();
    }

    public void ResetFurnitureDisposeUI()
    {
        go_FurnitureDisposeUI.SetActive( false );
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
    public void OnClickSelectType(int typeNumber)
    {
        if(activationNumber != typeNumber)
        {
            rt_Buttons[activationNumber].GetComponent<Image>().sprite = deactivationButtons_Sprite[activationNumber];
            rt_Buttons[activationNumber].sizeDelta = new Vector2( rt_Buttons[activationNumber].sizeDelta.x, 70 );

            rt_Buttons[typeNumber].GetComponent<Image>().sprite = activationButtons_Sprite[typeNumber];
            rt_Buttons[typeNumber].sizeDelta = new Vector2( rt_Buttons[typeNumber].sizeDelta.x, 100 );

            StoragePutFurnitureSlot();

            activationNumber = typeNumber;

            StoragePullFurnitureSlot();
        }
    }

    
    //DB에서 Load해온 아이템들을 가지고 있는 myFurnitureItemList에서 아이템들의 각 타입에 맞는 furnitureListDic 딕셔너리에 할당
    public void LoadFurnitureItem()
    {
        for (int i = 0; i < myFurnitureItemData.Count; i++)
        {
            for (int j = 0; j < itemDataDic[(int)myFurnitureItemData[i].furnitureItem.furnitureType].Count; j++)
            {
                if(itemDataDic[(int)myFurnitureItemData[i].furnitureItem.furnitureType][j].furnitureItem.itemName == myFurnitureItemData[i].furnitureItem.itemName)
                {
                    itemDataDic[(int)myFurnitureItemData[i].furnitureItem.furnitureType][j].currentHaveNumber = myFurnitureItemData[i].currentHaveNumber;
                }
            }
        }
    }

    //go_FurnitureSlotStorage의 자식으로 있는 FurnitureSlot들을 해당 타입의 아이템 개수 만큼 꺼내온다.
    public void StoragePullFurnitureSlot()
    {
        for (int i = 0; i < itemDataDic[activationNumber].Count; i++)
        {
            furnitureSlotList[i].furnitureItemData = itemDataDic[activationNumber][i];
            furnitureSlotList[i].transform.SetParent( go_Content.transform );
            furnitureSlotList[i].SetFurnitureSlot();
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
}
