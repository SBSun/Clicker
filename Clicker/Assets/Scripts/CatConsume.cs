using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatConsume : MonoBehaviour
{
    public CatSlot catSlot;

    public Coroutine coroutine;

    void Awake()
    {
        //coroutine = StartCoroutine( CatConsumeCoroutine() );
    } 

    public IEnumerator CatConsumeCoroutine()
    {
        float time = 0f;

        while (true)
        {
            while (time < catSlot.cat.consumeCycleTime)
            {
                time += Time.deltaTime;

                yield return null;
            }

            time = 0f;

            //고양이가 가지고 있는 골드가 초당 소비 골드보다 많으면 소비한다.
            if(GoodsController.instance.SubGoldCheck( catSlot.cat.currentKeepGoldList, catSlot.cat.consumeGoldList ))
            {
                //소비
                GoodsController.instance.SubGold( catSlot.cat.currentKeepGoldList, catSlot.cat.consumeGoldList );
                //전체 골드에 추가해준다.
                GoodsController.instance.AddGold( GoodsController.instance.goldList, catSlot.cat.consumeGoldList );
            }
        }
    }
}
