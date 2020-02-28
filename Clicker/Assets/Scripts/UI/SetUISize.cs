using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUISize : MonoBehaviour
{
    private RectTransform rt_BackgroundUI;

    [Header("가로가 가득 차게")]
    public bool isSetFullWidth = false;
    [Header("세로가 가득 차게")]
    public bool isSetFullHeight = false;

    void Start()
    {
        rt_BackgroundUI = GetComponent<RectTransform>();

        if(UIManager.instance.scale != 0)
        {
            if (UIManager.instance.scale < 1 && isSetFullHeight)
            {
                rt_BackgroundUI.sizeDelta = new Vector2( rt_BackgroundUI.sizeDelta.x, UIManager.instance.heightMaxUI );
            }
            else if (UIManager.instance.scale > 1 && isSetFullWidth)
            {
                rt_BackgroundUI.sizeDelta = new Vector2( UIManager.instance.widthMaxUI , rt_BackgroundUI.sizeDelta.y );
            } 
        }
    }
}
 