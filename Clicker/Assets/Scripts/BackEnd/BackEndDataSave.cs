using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class BackEndDataSave : MonoBehaviour
{
    public GachaSystem gachaSystem;
    public FurnitureDisposeUI furnitureDisposeUI;

    #region 에러코드 출력
    public void GetDataError( string statusCode )
    {
        switch (statusCode)
        {
            case "400":
                Debug.Log( "private table 아닌 tableName을 입력하였습니다." );
                break;

            case "412":
                Debug.Log( "비활성화 된 tableName입니다." );
                break;
        }
    }

    public void InsertDataError( string statusCode )
    {
        switch (statusCode)
        {
            case "404":
                Debug.Log( "존재하지 않는 tableName입니다." );
                break;

            case "412":
                Debug.Log( "비활성화 된 tableName입니다." );
                break;

            case "413":
                Debug.Log( "하나의 row( column들의 집합 )이 400KB를 넘었습니다." );
                break;
        }
    }

    public void UpdateDataError( string statusCode )
    {
        switch (statusCode)
        {
            case "403":
                Debug.Log( "퍼블릭테이블의 타인정보를 수정하고자 했습니다." );
                break;

            case "404":
                Debug.Log( "존재하지 않는 tableName입니다." );
                break;

            case "405":
                Debug.Log( "param에 partition, gamer_id, inDate, updatedAt 네가지 필드가 있습니다." );
                break;

            case "412":
                Debug.Log( "비활성화 된 tableName입니다." );
                break;

            case "413":
                Debug.Log( "하나의 row( column들의 집합 )이 400KB를 넘었습니다." );
                break;
        }
    }
    #endregion

    #region 뽑기시스템 데이터 저장, 삽입, 수정
    //뽑기 시스템 데이터 가져오기
    public void GetGachaSystemData()
    {
        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents( "GachaSystem" );

        if (BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();
            Debug.Log( data.ToJson() );
            if (data != null)
            {
                if (data.Keys.Contains( "rows" ))
                {
                    JsonData rowsData = data["rows"][0];

                    if (rowsData.Keys.Contains( "normalBTierCount" ))
                    {
                        Debug.Log( "GachaSystem 데이터 가져오기 성공" );

                        gachaSystem.normalBTierCount = int.Parse( rowsData["normalBTierCount"][0].ToString() );
                        gachaSystem.normalSTierCount = int.Parse( rowsData["normalSTierCount"][0].ToString() );
                        gachaSystem.highBTierCount = int.Parse( rowsData["highBTierCount"][0].ToString() );
                        gachaSystem.highSTierCount = int.Parse( rowsData["highSTierCount"][0].ToString() );
                    }
                    else
                    {
                        Debug.Log( "GachaSystem 데이터 가져오기 실패" );
                    }
                }
            }
        }
        else
        {
            GetDataError( BRO.GetStatusCode() );
        }
    }

    //처음 접속하는 유저의 테이블에 뽑기 시스템 초기 Data 값 저장
    public void InsertGachaSystemData()
    {
        Param param = new Param();

        param.Add( "normalBTierCount", gachaSystem.normalBTierCount );
        param.Add( "normalSTierCount", gachaSystem.normalSTierCount );
        param.Add( "highBTierCount", gachaSystem.highBTierCount );
        param.Add( "highSTierCount", gachaSystem.highSTierCount );

        BackendReturnObject BRO = Backend.GameInfo.Insert( "GachaSystem", param );

        if (BRO.IsSuccess())
        {
            Debug.Log( "GachaSystem Data Insert 성공" );
        }
        else
        {
            InsertDataError( BRO.GetStatusCode() );
        }
    }

    //뽑기 시스템 데이터 수정
    public void UpdateGachaSystemData()
    {
        Param param = new Param();

        param.Add( "normalBTierCount", gachaSystem.normalBTierCount );
        param.Add( "normalSTierCount", gachaSystem.normalSTierCount );
        param.Add( "highBTierCount", gachaSystem.highBTierCount );
        param.Add( "highSTierCount", gachaSystem.highSTierCount );

        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents( "GachaSystem" );

        if (BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();

            if (data != null)
            {
                string inDate = data["rows"][0]["inDate"]["S"].ToString();

                BackendReturnObject BRO_Update = Backend.GameInfo.Update( "GachaSystem", inDate, param );

                if (BRO_Update.IsSuccess())
                {
                    Debug.Log( "GachaSystem 데이터 수정 완료" );
                }
                else
                {
                    UpdateDataError( BRO_Update.GetStatusCode() );
                }
            }
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "400":
                    Debug.Log( "private table 아닌 tableName을 입력하였습니다." );
                    break;

                case "412":
                    Debug.Log( "비활성화 된 tableName입니다." );
                    break;
            }
        }
    }
    #endregion

    #region 가구 아이템 데이터 저장, 삽입, 수정

    public void GetFurnitureItemData()
    {
        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents( "FurnitureItem" );

        if (BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();
            Debug.Log( data.ToJson() );
            if (data != null)
            {
                if (data.Keys.Contains( "rows" ))
                {
                    JsonData rowsData = data["rows"][0];

                    if (rowsData.Keys.Contains( "furnitureItem" ))
                    {
                        Debug.Log( "FurnitureItem 데이터 가져오기 성공" );

                        JsonData itemData = rowsData["furnitureItem"][0];

                        
                        for (int i = 0; i < itemData.Count; i++)
                        {
                            for (int j = 0; j < furnitureDisposeUI.allFurnitureItem.Count; j++)
                            {
                                if (furnitureDisposeUI.allFurnitureItem[i].itemName == itemData[i][0][1][0].ToString())
                                {
                                    furnitureDisposeUI.myFurnitureItemData.Add( new FurnitureItemData( furnitureDisposeUI.allFurnitureItem[i], int.Parse( itemData[i][0][2][0].ToString() ) ) );
                                }
                            }                
                        }

                        furnitureDisposeUI.myFurnitureItemData.Sort( delegate ( FurnitureItemData itemData1, FurnitureItemData itemData2 )
                         {
                             if (itemData1.furnitureItem.furnitureType < itemData2.furnitureItem.furnitureType)
                                 return -1;
                             else if (itemData1.furnitureItem.furnitureType > itemData2.furnitureItem.furnitureType)
                                 return 1;

                             return 0;
                         } );
                    }
                    else
                    {
                        Debug.Log( "FurnitureItem 데이터 가져오기 실패" );
                    }
                }
                else
                {
                    Debug.Log( "저장된 FurnitureItem 데이터가 없습니다." );
                    //InsertFurnitureItemData();
                }
            }
        }
        else
        {
            GetDataError( BRO.GetStatusCode() );
        }
    }

    
    //처음 가구 아이템을 구매했을 때 실행
    public void InsertFurnitureItemData(FurnitureItemData _furnitureItemData)
    {
        Param param = new Param();

        Dictionary<string, FurnitureSaveData> furnitureInsertData = new Dictionary<string, FurnitureSaveData>();

        if (UIManager.instance.furnitureDisposeUI.myFurnitureItemData.Count == 1)
        {
            furnitureInsertData.Add( _furnitureItemData.furnitureItem.itemName,
                new FurnitureSaveData( _furnitureItemData.furnitureItem.itemName, (int)_furnitureItemData.furnitureItem.furnitureType, _furnitureItemData.currentHaveNumber) );
        }
        else
            return;

        param.Add( "furnitureItem", furnitureInsertData );

        BackendReturnObject BRO = Backend.GameInfo.Insert( "FurnitureItem", param );

        if (BRO.IsSuccess())
        {
            Debug.Log( "FurnitureItem Data Insert 성공" );
        }
        else
        {
            InsertDataError( BRO.GetStatusCode() );
        }
    }

    //가구 아이템을 구매할 때 마다 실행
    public void UpdateFurnitureItemData()
    {
        Param param = new Param();

        Dictionary<string, FurnitureSaveData> furnitureItemDataUpdate = new Dictionary<string, FurnitureSaveData>();

        for (int i = 0; i < furnitureDisposeUI.myFurnitureItemData.Count; i++)
        {
            furnitureItemDataUpdate.Add( furnitureDisposeUI.myFurnitureItemData[i].furnitureItem.itemName,
                new FurnitureSaveData( furnitureDisposeUI.myFurnitureItemData[i].furnitureItem.itemName, (int)furnitureDisposeUI.myFurnitureItemData[i].furnitureItem.furnitureType
                , furnitureDisposeUI.myFurnitureItemData[i].currentHaveNumber) );
        }

        param.Add( "furnitureItem", furnitureItemDataUpdate );

        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents( "FurnitureItem" );

        if (BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();

            if (data != null)
            {
                string inDate = data["rows"][0]["inDate"]["S"].ToString();

                BackendReturnObject BRO_Update = Backend.GameInfo.Update( "FurnitureItem", inDate, param );

                if (BRO_Update.IsSuccess())
                {
                    Debug.Log( "FurnitureItem 데이터 수정 완료" );
                }
                else
                {
                    UpdateDataError( BRO_Update.GetStatusCode() );
                }
            }
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "400":
                    Debug.Log( "private table 아닌 tableName을 입력하였습니다." );
                    break;

                case "412":
                    Debug.Log( "비활성화 된 tableName입니다." );
                    break;
            }
        }
    }
    #endregion
}

