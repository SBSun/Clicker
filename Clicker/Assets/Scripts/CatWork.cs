using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWork : MonoBehaviour
{
    public CatSlot catSlot;

    public Coroutine coroutine;

    public void StartCatWork()
    {
        coroutine = StartCoroutine( CatWorkCoroutine() );
    }

    public IEnumerator CatWorkCoroutine()
    {
        float time = 0f;

        while (true)
        {
            while(time < catSlot.cat.workCycleTime)
            {
                time += Time.deltaTime;

                yield return null;
            }

            time = 0f;

            GoodsController.instance.AddGold( catSlot.cat.currentKeepGoldList, catSlot.cat.makeGoldList );

            //현재 가지고 있는 골드가 최대 보유할 수 있는 골드보다 많으면
            if(!GoodsController.instance.SubGoldCheck( catSlot.cat.maxKeepGoldList, catSlot.cat.currentKeepGoldList ))
            {
                //현재 골드를 최대 보유 가능 골드로 초기화
                for (int i = 0; i < catSlot.cat.maxKeepGoldList.Count; i++)
                {
                    catSlot.cat.currentKeepGoldList[i] = catSlot.cat.maxKeepGoldList[i];
                }
     
                //코루틴 종료
                yield break;
            }

            yield return null;
        }
    }
}