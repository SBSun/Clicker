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

    #region 뽑기시스템 데이터 읽기, 삽입, 수정
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
                if (data.Keys.Contains( "rows" ) && data["rows"].Count > 0)
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
                else
                {
                    InsertGachaSystemData();
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

    #region 가구 아이템 데이터 읽기, 삽입, 수정

    public void GetFurnitureItemData()
    {
        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents( "FurnitureItem" );

        if (BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();

            if (data != null)
            {
                if (data.Keys.Contains( "rows" ) && data["rows"].Count > 0)
                {
                    JsonData rowsData = data["rows"];

                    //저장된 가구 개수 만큼 반복
                    for (int i = 0; i < rowsData.Count; i++)
                    {
                        for (int j = 0; j < furnitureDisposeUI.allFurnitureItem.Count; j++)
                        {
                            FurnitureItem furnitureItem = furnitureDisposeUI.allFurnitureItem[j];

                            if (rowsData[i].Keys.Contains( furnitureItem.itemName ))
                            {
                                furnitureDisposeUI.myFurnitureItemDic[(int)furnitureItem.furnitureType].Add(
                                    new FurnitureItemData( furnitureItem, int.Parse( rowsData[i][furnitureItem.itemName][0][0][0].ToString() ), int.Parse( rowsData[i][furnitureItem.itemName][0][1][0].ToString() ) ) );

                                break;
                            }
                        }
                    }
                    
                    furnitureDisposeUI.LoadFurnitureItem();
                }
                else
                {
                    Debug.Log( "저장된 FurnitureItem 데이터가 없습니다." );
                }
            }
        }
        else
        {
            GetDataError( BRO.GetStatusCode() );
        }
    }

    //처음 가구 아이템을 구매했을 때 실행
    public void InsertFurnitureItemData( FurnitureItemData _furnitureItemData )
    {
        Param param = new Param();

        string[] furnitureData = { _furnitureItemData.currentHaveNumber.ToString(),
                                _furnitureItemData.currentDisposeNumber.ToString()};

        param.Add( _furnitureItemData.furnitureItem.itemName, furnitureData );

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
    public void UpdateFurnitureItemData( FurnitureItemData _furnitureItemData )
    {
        Param param = new Param();

        string[] furnitureData = { _furnitureItemData.currentHaveNumber.ToString(),
                                _furnitureItemData.currentDisposeNumber.ToString()};

        param.Add( _furnitureItemData.furnitureItem.itemName, furnitureData );

        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents( "FurnitureItem" );

        if (BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();

            if (data != null)
            {
                JsonData rowsData = data["rows"];

                string inDate = "";

                for (int i = 0; i < rowsData.Count; i++)
                {
                    if(rowsData[i].Keys.Contains( _furnitureItemData.furnitureItem.itemName))
                    {
                        inDate = data["rows"][i]["inDate"]["S"].ToString();
                        break;
                    }
                }

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

    public void GetFloorInformation()
    {
        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents( "FloorInformation" );

        if (BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();

            if (data != null)
            {
                if (data.Keys.Contains( "rows" ) && data["rows"].Count > 0)
                {
                    JsonData rowsData = data["rows"];

                    //저장된 가구 개수 만큼 반복
                    for (int i = 0; i < rowsData.Count; i++)
                    {
                        CatHouseManager.instance.floorInformationList[i].saveFloorInfo.Clear();
                        for (int j = 0; j < rowsData.Count; j++)
                        {
                            if (rowsData[i].Keys.Contains( "F" + i.ToString() ))
                            {
                                JsonData floorInfo = rowsData[i]["F" + i.ToString()][0];

                                for (int k = 0; k < floorInfo.Count; k++)
                                {
                                    CatHouseManager.instance.floorInformationList[i].saveFloorInfo.Add( floorInfo[k][0].ToString() );
                                }

                                break;
                            }
                        }

                        CatHouseManager.instance.floorInformationList[i].LoadFloorInformation();
                    }
                }
                else
                {
                    Debug.Log( "저장된 FloorInformation 데이터가 없습니다." );
                }
            }
        }
        else
        {
            GetDataError( BRO.GetStatusCode() );
        }
    }

    //처음 가구 아이템을 구매했을 때 실행
    public void InsertFloorInformation( FloorInformation _floorInformation)
    {
        Param param = new Param();

        string[] floorInformationData = _floorInformation.saveFloorInfo.ToArray();

        param.Add( "F" + _floorInformation.transform.GetSiblingIndex(), floorInformationData );

        BackendReturnObject BRO = Backend.GameInfo.Insert( "FloorInformation", param );

        if (BRO.IsSuccess())
        {
            Debug.Log( "FloorInformation Data Insert 성공" );
        }
        else
        {
            InsertDataError( BRO.GetStatusCode() );
        }
    }

    //Save 버튼을 누르면 실행
    public void UpdateFloorInformation( FloorInformation _floorInformation )
    {
        Param param = new Param();

        string[] floorInformationData = _floorInformation.saveFloorInfo.ToArray();

        param.Add( "F" + _floorInformation.transform.GetSiblingIndex().ToString(), floorInformationData );

        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents( "FloorInformation" );

        if (BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();

            if (data != null)
            {
                JsonData rowsData = data["rows"];

                string inDate = "";

                for (int i = 0; i < rowsData.Count; i++)
                {
                    if (rowsData[i].Keys.Contains( "F" + _floorInformation.transform.GetSiblingIndex().ToString() ))
                    {
                        inDate = rowsData[i]["inDate"]["S"].ToString();
                        Debug.Log( "업데이트" );
                        break;
                    }
                }

                BackendReturnObject BRO_Update = Backend.GameInfo.Update( "FloorInformation", inDate, param );

                if (BRO_Update.IsSuccess())
                {
                    Debug.Log( "FloorInformation 데이터 수정 완료" );
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
}

