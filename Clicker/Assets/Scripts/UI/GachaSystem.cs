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

        CatSlotFind( RandomSelectCat( 2 ) );
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

        CatSlotFind( RandomSelectCat( 0 ) );
    }

    //S티어 뽑기 실패
    public void STierFailure()
    {
        if (sTierCount < sTierCountMax)
        {
            sTierCount++;
            sTierDenominator -= 8;
        }

        CatSlotFind( RandomSelectCat( 1 ) );
    }

    //인수로 들어온 클래스 등급의 고양이를 랜덤 뽑기
    public Cat RandomSelectCat(int catClass)
    {
        //해당 등급의 고양이 종류 
        int randNum = Random.Range( 0, UIManager.instance.catInventory.classCatSlots[catClass].catSlotList.Count);

        Debug.Log( UIManager.instance.catInventory.classCatSlots[catClass].catSlotList[randNum].cat.catName );
        return UIManager.instance.catInventory.classCatSlots[catClass].catSlotList[randNum].cat;
    }

    //뽑은 고양이의 정보를 가지고 있는 고양이 슬롯 찾기
    public void CatSlotFind(Cat cat)
    {
        List<CatSlot> catSlotList = UIManager.instance.catInventory.classCatSlots[(int)cat.catClass].catSlotList.ToList();

        //인수로 받은 고양이의 등급 슬롯들에서 찾는다.
        for (int i = 0; i < catSlotList.Count; i++)
        {
            if(cat.catName == catSlotList[i].cat.catName)
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
