using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class GachaSystem : MonoBehaviour
{
    public int bTierDenominator; //분모
    public int bTierNumerator;   //분자
    public int sTierDenominator;
    public int sTierNumerator;

    //각 티어에서 몇번 실패하였는지
    public int bTierCount;
    public int sTierCount;

    //최대 10번까지 누적
    public int bTierCountMax;
    public int sTierCountMax;

    //B티어 뽑기
    public void BTierGacha()
    {
        int randomNum = Mathf.RoundToInt( (bTierDenominator + bTierNumerator) * Random.Range( 0.0f, 1.0f ) );

        if (randomNum > bTierDenominator - bTierNumerator)
        {
            BTierSuccess();
            Debug.Log( "BTier 뽑기 성공" );
            STierGacha();
        }
        else
        {
            BTierFailure();
            Debug.Log( "BTier 뽑기 실패" );
        }

        BackEndManager.instance.backEndDataSave.UpdateGachaSystemData();
    }

    //B티어 뽑기 성공
    public void BTierSuccess()
    {
        bTierCount = 0;
        bTierDenominator = 112;
    }

    //B티어 뽑기 실패
    public void BTierFailure()
    {
        if (bTierCount < bTierCountMax)
        {
            bTierCount++;
            bTierDenominator -= 8;
        }
    }

    //S티어 뽑기
    public void STierGacha()
    {
        int randomNum = Mathf.RoundToInt( (sTierDenominator + sTierNumerator) * Random.Range( 0.0f, 1.0f ) );

        if (randomNum > sTierDenominator - sTierNumerator)
        {
            STierSuccess();
            Debug.Log( "STier 뽑기 성공" );
        }
        else
        {
            STierFailure();
            Debug.Log( "STier 뽑기 실패" );
        }

        BackEndManager.instance.backEndDataSave.UpdateGachaSystemData();
    }

    //S티어 뽑기 성공
    public void STierSuccess()
    {
        sTierCount = 0;
        sTierDenominator = 86;
    }

    //S티어 뽑기 실패
    public void STierFailure()
    {
        if (sTierCount < sTierCountMax)
        {
            sTierCount++;
            sTierDenominator -= 8;
        }
    }
}
