using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public GameObject go_ShopUI;

    public RectTransform rt_Scroll;

    public void SetShopUI()
    {
        rt_Scroll.anchoredPosition = Vector2.zero;
    }
}
