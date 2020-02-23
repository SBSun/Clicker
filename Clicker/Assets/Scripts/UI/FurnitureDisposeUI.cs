using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureItemData
{
    public string itemName;
    public int furnitureType;
    public int currentHaveNumber;

    public FurnitureItemData( string _itemName, int _furnitureType, int _currentHaveNumber )
    {
        itemName = _itemName;
        furnitureType = _furnitureType;
        currentHaveNumber = _currentHaveNumber;
    }
}

public class FurnitureDisposeUI : MonoBehaviour
{
    public RectTransform[] rt_Buttons;
    public Sprite[] activationButtons_Sprite;
    public Sprite[] deactivationButtons_Sprite;

    public GameObject go_Content;
    public GameObject go_FurnitureSlotStorage;

    public List<FurnitureSlot> furnitureSlotList = new List<FurnitureSlot>();

    float activationButtonYHeight = 100;
    float deactivationButtonYHeight = 70;

    int activationNumber = 0;

    //모든 가구 아이템 리스트
    public List<FurnitureItem> allFurnitureItemList = new List<FurnitureItem>();
    //보유 하고 있는 가구 아이템 리스트
    public List<FurnitureItemData> myFurnitureItemDataList = new List<FurnitureItemData>();

    //가구 타입 별로 분류한 딕셔너리
    public Dictionary<int, List<FurnitureItem>> furnitureListDic = new Dictionary<int, List<FurnitureItem>>()
    {
        {0, new List<FurnitureItem>()},
        {1, new List<FurnitureItem>()},
        {2, new List<FurnitureItem>()},
        {3, new List<FurnitureItem>()},
        {4, new List<FurnitureItem>()}
    };

    void Awake()
    {
        //모든 아이템을 각 가구 타입에 맞는 리스트에 추가한다.
        for (int i = 0; i < allFurnitureItemList.Count; i++)
        {
            furnitureListDic[(int)allFurnitureItemList[i].furnitureType].Add( allFurnitureItemList[i] );
        }

        for (int i = 0; i < furnitureListDic.Count; i++)
        {
            furnitureListDic[i].Sort( delegate ( FurnitureItem item1, FurnitureItem item2 )
             {
                 return item1.itemName.CompareTo( item2.itemName );
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

    void Start()
    {
        StoragePullFurnitureSlot();
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
        for (int i = 0; i < myFurnitureItemDataList.Count; i++)
        {
            for (int j = 0; j < furnitureListDic[myFurnitureItemDataList[i].furnitureType].Count; j++)
            {
                if(furnitureListDic[myFurnitureItemDataList[i].furnitureType][j].itemName == myFurnitureItemDataList[i].itemName)
                {
                    furnitureListDic[myFurnitureItemDataList[i].furnitureType][j].currentHaveNumber = myFurnitureItemDataList[i].currentHaveNumber;
                }
            }
        }
    }

    //go_FurnitureSlotStorage의 자식으로 있는 FurnitureSlot들을 해당 타입의 아이템 개수 만큼 꺼내온다.
    public void StoragePullFurnitureSlot()
    {
        for (int i = 0; i < furnitureListDic[activationNumber].Count; i++)
        {
            furnitureSlotList[i].transform.SetParent( go_Content.transform );
            furnitureSlotList[i].furnitureItem = furnitureListDic[activationNumber][i];
            furnitureSlotList[i].SetFurnitureSlot();
        }
    }

    public void StoragePutFurnitureSlot()
    {
        for (int i = 0; i < furnitureListDic[activationNumber].Count; i++)
        {
            furnitureSlotList[i].transform.SetParent( go_FurnitureSlotStorage.transform );
        }
    }
}
