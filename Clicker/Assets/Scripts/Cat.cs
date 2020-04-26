using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public enum CatStatus
    {
        Work,
        Consume
    }
    public CatStatus catStatus;

    public CatInformation catInformation;

    public GoldHarvest goldHarvest;

    //몇 초마다 골드를 버는지
    public float workCycleTime;
    //몇 초마다 골드를 소비하는지
    public float consumeCycleTime;

    //자동으로
    public bool isAutoConsume = false;

    #region GoldList 변수
    //얼마를 버는지
    public List<int> makeGoldList;
    //초당 소비하는 골드
    public List<int> consumeGoldList;
    //레벨업에 필요한 골드
    public List<int> levelUpNeedGoldList;
    //가지고 있을 수 있는 최대 골드 양
    public List<int> maxKeepGoldList;
    //현재 가지고 있는 골드 양
    public List<int> currentKeepGoldList;
    #endregion
   
    //고양이의 레벨
    public int level = 1;
    public int count = 0;

    //고양이가 레벨업 했을 때 변경되어야 할 것들
    public void CatLevelUp()
    {
        level++;

        if (level == 3)
            isAutoConsume = true;
    }

    public void CatCountAdd()
    {
        count++;
    }

    /*public IEnumerator WorkConsumeCoroutine()
    {
        float time = 0f;

        while(true)
        {
            if(catStatus == CatStatus.Work)
            {
                //인벤토리에 있을 경우 돈을 번다.
                
                while(time < workCycleTime)
                {
                    time += Time.deltaTime;

                    yield return null;
                }


            }
            else
            {
                //집에 있을 경우 돈을 소비한다.
                while (time < 3f)
                {
                    time += Time.deltaTime;

                    yield return null;
                }

                if (isAutoConsume)
                {
                    GoodsController.instance.AddGold( GoodsController.instance.goldList , makeGoldList );
                }
                else
                {
                    goldHarvest.gameObject.SetActive( true );

                    while (!goldHarvest.isGoldHarvest)
                    {
                        yield return null;
                    }

                    goldHarvest.isGoldHarvest = false;

                    goldHarvest.gameObject.SetActive( false );
                }

                time = 0;
            }
            

            yield return null;
        }
    }*/
}
