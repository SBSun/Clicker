using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class BackEndDataSave : MonoBehaviour
{
    public GachaSystem gachaSystem;

    //뽑기 시스템 데이터 가져오기
    public void GetGachaSystemData()
    {
        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents( "GachaSystem" );

        if (BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();

            if (data != null)
            {
                if (data.Keys.Contains( "rows" ))
                {
                    JsonData rowsData = data["rows"][0];

                    if (rowsData.Keys.Contains( "bTierDenominator" ))
                    {
                        Debug.Log( "데이터 가져오기 성공" );
                        
                        gachaSystem.bTierDenominator = int.Parse(rowsData["bTierDenominator"][0].ToString());
                        gachaSystem.bTierNumerator = int.Parse(rowsData["bTierNumerator"][0].ToString());
                        gachaSystem.sTierDenominator = int.Parse(rowsData["sTierDenominator"][0].ToString());
                        gachaSystem.sTierNumerator = int.Parse( rowsData["sTierNumerator"][0].ToString() );

                        gachaSystem.bTierCount = int.Parse( rowsData["bTierCount"][0].ToString() );
                        gachaSystem.sTierCount = int.Parse( rowsData["sTierCount"][0].ToString() );
                    }
                    else
                    {
                        Debug.Log( "데이터 가져오기 실패" );
                    }
                }
                else
                {
                    Debug.Log( "저장된 데이터가 없습니다." );
                    InsertGachaSystemData();
                }
            }
            else
                Debug.Log( "잉" );
        }
        else
        {
            switch(BRO.GetStatusCode())
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

    //처음 접속하는 유저의 테이블에 뽑기 시스템 초기 Data 값 저장
    public void InsertGachaSystemData()
    {
        Param param = new Param();

        param.Add( "bTierDenominator", gachaSystem.bTierDenominator );
        param.Add( "bTierNumerator", gachaSystem.bTierNumerator );
        param.Add( "sTierDenominator", gachaSystem.sTierDenominator );
        param.Add( "sTierNumerator", gachaSystem.sTierNumerator );

        param.Add( "bTierCount", gachaSystem.bTierCount );
        param.Add( "sTierCount", gachaSystem.sTierCount );

        BackendReturnObject BRO = Backend.GameInfo.Insert( "GachaSystem", param );

        if(BRO.IsSuccess())
        {
            Debug.Log( "Data Insert 성공" );
        }
        else
        {
            switch(BRO.GetStatusCode())
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
    }

    //뽑기 시스템 데이터 수정
    public void UpdateGachaSystemData()
    {
        Param param = new Param();
        
        param.Add( "bTierDenominator", gachaSystem.bTierDenominator );
        param.Add( "bTierNumerator", gachaSystem.bTierNumerator );
        param.Add( "sTierDenominator", gachaSystem.sTierDenominator );
        param.Add( "sTierNumerator", gachaSystem.sTierNumerator );

        param.Add( "bTierCount", gachaSystem.bTierCount );
        param.Add( "sTierCount", gachaSystem.sTierCount );

        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents( "GachaSystem" );

        if(BRO.IsSuccess())
        {
            JsonData data = BRO.GetReturnValuetoJSON();

            if(data != null)
            {
                string inDate = data["rows"][0]["inDate"]["S"].ToString();

                BackendReturnObject BRO_Update = Backend.GameInfo.Update( "GachaSystem", inDate, param );

                if(BRO_Update.IsSuccess())
                {
                    Debug.Log( "데이터 수정 완료" );
                }
                else
                {
                    switch(BRO_Update.GetStatusCode())
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
