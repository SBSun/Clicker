using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System.Linq;

public class GachaSystem : MonoBehaviour
{
    public List<Cat> fiveCatList = new List<Cat>();
    public List<Cat> fourCatList = new List<Cat>();
    public List<Cat> threeCatList = new List<Cat>();
    public List<Cat> twoCatList = new List<Cat>();

    public int bTierDenominator; //분모
    public int bTierNumerator;   //분자
    public int sTierDenominator;
    public int sTierNumerator;

    //각 티어에서 몇번 실패하였는지
    public int normalBTierCount;
    public int normalSTierCount;
    public int highBTierCount;
    public int highSTierCount;

    //최대 10번까지 누적
    public int bTierCountMax;
    public int sTierCountMax;


    //일반 뽑기 2 ~ 4성, 인수로 1 또는 10번 뽑기
    public void NormalGacha(int gachaCount)
    {
        for (int i = 0; i < gachaCount; i++)
        {
            NormalBTierGacha();
        }   
    }

    //고급 뽑기 3 ~ 5성
    public void HighGacha(int gachaCount)
    {
        for (int i = 0; i < gachaCount; i++)
        {
            HighBTierGacha();
        }
    }

    //성공하면 NormalSTierGacha 실패하면 2성
    public void NormalBTierGacha()
    {
        int randomNum = 0;

        //일반 뽑기면 normalBTierCount로 분모 계산
        randomNum = Mathf.RoundToInt((bTierDenominator - 8 * normalBTierCount + bTierNumerator) * Random.Range( 0.0f, 1.0f ) );

        if (randomNum > bTierDenominator - 8 * normalBTierCount - bTierNumerator)
        {
            normalBTierCount = 0;
            Debug.Log( "NormalBTier 뽑기 성공" );
            NormalSTierGacha();
        }
        else
        {
            if (normalBTierCount < bTierCountMax)
            {
                normalBTierCount++;
            }

            CatSlotFind( RandomSelectCat( 3 ) );
            Debug.Log( "NormalBTier 뽑기 실패" );

            BackEndManager.instance.backEndDataSave.UpdateGachaSystemData();
        }
    }

    //3 ~ 4성 뽑기
    public void NormalSTierGacha( )
    {
        int randomNum = Mathf.RoundToInt( (sTierDenominator - 8 * normalSTierCount + sTierNumerator) * Random.Range( 0.0f, 1.0f ) );

        if (randomNum > sTierDenominator - 8 * normalSTierCount - sTierNumerator)
        {
            normalSTierCount = 0;

            CatSlotFind( RandomSelectCat(1) );
            Debug.Log( "NormalSTier 뽑기 성공" );
        }
        else
        {
            if (normalSTierCount < sTierCountMax)
            {
                normalSTierCount++;
            }

            CatSlotFind( RandomSelectCat(2) );
            Debug.Log( "NormalSTier 뽑기 실패" );
        }

        BackEndManager.instance.backEndDataSave.UpdateGachaSystemData();
    }

    //성공하면 HighSTierGacha 실패하면 3성
    public void HighBTierGacha()
    {
        int randomNum = 0;

        //일반 뽑기면 normalBTierCount로 분모 계산
        randomNum = Mathf.RoundToInt( (bTierDenominator - 8 * highBTierCount + bTierNumerator) * Random.Range( 0.0f, 1.0f ) );

        if (randomNum > bTierDenominator - 8 * highBTierCount - bTierNumerator)
        {
            highBTierCount = 0;
            Debug.Log( "HighBTier 뽑기 성공" );
            HighSTierGacha();
        }
        else
        {
            if (highBTierCount < bTierCountMax)
            {
                highBTierCount++;
            }

            CatSlotFind( RandomSelectCat( 2 ) );
            Debug.Log( "HighBTier 뽑기 실패" );

            BackEndManager.instance.backEndDataSave.UpdateGachaSystemData();
        }
    }

    //4 ~ 5성 뽑기
    public void HighSTierGacha()
    {
        int randomNum = Mathf.RoundToInt( (sTierDenominator - 8 * highSTierCount + sTierNumerator) * Random.Range( 0.0f, 1.0f ) );

        if (randomNum > sTierDenominator - 8 * highSTierCount - sTierNumerator)
        {
            highSTierCount = 0;

            CatSlotFind( RandomSelectCat( 0 ) );
            Debug.Log( "HighSTier 뽑기 성공" );
        }
        else
        {
            if (highSTierCount < sTierCountMax)
            {
                highSTierCount++;
            }

            CatSlotFind( RandomSelectCat( 1 ) );
            Debug.Log( "HighSTier 뽑기 실패" );
        }

        BackEndManager.instance.backEndDataSave.UpdateGachaSystemData();
    }

    //인수로 들어온 클래스 등급의 고양이를 랜덤 뽑기
    public Cat RandomSelectCat(int catClass)
    {
        //해당 등급의 고양이 종류 
        int randNum = Random.Range( 0, UIManager.instance.catInventoryUI.classCatSlots[catClass].catSlotList.Count);

        Debug.Log( UIManager.instance.catInventoryUI.classCatSlots[catClass].catSlotList[randNum].cat.name );
        return UIManager.instance.catInventoryUI.classCatSlots[catClass].catSlotList[randNum].cat;
    }

    //뽑은 고양이의 정보를 가지고 있는 고양이 슬롯 찾기
    public void CatSlotFind(Cat cat)
    {
        List<CatSlot> catSlotList = UIManager.instance.catInventoryUI.classCatSlots[(int)cat.catClass].catSlotList.ToList();

        //인수로 받은 고양이의 등급 슬롯들에서 찾는다.
        for (int i = 0; i < catSlotList.Count; i++)
        {
            if(cat.name == catSlotList[i].cat.name)
            {
                if(catSlotList[i].slotStatus == CatSlot.SlotStatus.Rock)
                {
                    catSlotList[i].SlotOpen();
                }
                else
                {
                    //아직 기획이 안나옴
                }
            }
        }
    }
}
