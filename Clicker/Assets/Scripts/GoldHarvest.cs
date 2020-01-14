using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoldHarvest : MonoBehaviour, IPointerClickHandler
{
    public Cat cat;

    //골드를 수확 했는지 안했는지
    public bool isGoldHarvest = false;

    public void OnPointerClick( PointerEventData eventData )
    {
        if (!isGoldHarvest)
        {
            GoodsController.instance.AddGold( GoodsController.instance.goldList ,cat.makeGoldList );
            isGoldHarvest = true;
        }
    }
}
