using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUI : MonoBehaviour
{
    public RectTransform[] rt_BottomBtns;

    void Start()
    {
        //가로 비율이 기준 비율보다 클 경우
        if (UIManager.instance.scale > 1)
        {

            for (int i = 0; i < rt_BottomBtns.Length; i++)
            {
                rt_BottomBtns[i].sizeDelta = rt_BottomBtns[i].sizeDelta * UIManager.instance.multiple;

                if (i != 0)
                    rt_BottomBtns[i].anchoredPosition = new Vector2( rt_BottomBtns[i - 1].anchoredPosition.x + rt_BottomBtns[i - 1].sizeDelta.x, rt_BottomBtns[i].anchoredPosition.y );
            }
        }      
    }
}
