using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUI : MonoBehaviour
{
    public GameObject go_BottomUI;
    public RectTransform[] rt_BottomBtns;

    void Start()
    {
        //가로 비율이 기준 비율보다 클 경우
        if (UIManager.instance.scale > 1)
        {

            for (int i = 0; i < rt_BottomBtns.Length; i++)
            {
                UIManager.instance.SetUIRT( rt_BottomBtns[i] );
            }
        }      
    }

    public void BottomUIActivation()
    {
        go_BottomUI.SetActive( true );
    }

    public void BottomUIDeactivate()
    {
        go_BottomUI.SetActive( false );
    }
}
