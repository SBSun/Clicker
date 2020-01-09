using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Cat : MonoBehaviour, IPointerClickHandler
{
    public enum CatClass
    {
        Two,
        Three,
        Four,
        Five
    }

    public CatClass catClass;

    public enum CatStatus
    {
        Work,
        Consume
    }

    public CatStatus catStatus;

    public GoldHarvest goldHarvest;

    //몇 초마다 골드를 버는지
    public float workCycleTime;
    //몇 초마다 골드를 소비하는지
    public float consumeCycleTime;

    //자동으로
    public bool isAutoHarvest = false;

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

    //고양이 이미지
    public Sprite catSprite;
    //고양이의 이름
    public string catName;
    //고양이의 레벨
    public int catLevel;

    void Start()
    {
        StartCoroutine( WorkConsumeCoroutine() );
    }

    //고양이가 레벨업 했을 때 변경되어야 할 것들
    public void CatLevelUp()
    {
        catLevel++;

        if (catLevel == 3)
            isAutoHarvest = true;
    }

    public IEnumerator WorkConsumeCoroutine()
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

                if (isAutoHarvest)
                {
                    GoodsController.instance.AddGold( makeGoldList );
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
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        if(catStatus == CatStatus.Consume)
        {
            UIManager.instance.catInformation.SetCatInformation( this );
        }
    }
}
