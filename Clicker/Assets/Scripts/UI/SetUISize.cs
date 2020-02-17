using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUISize : MonoBehaviour
{
    private RectTransform rt_BackgroundUI;

    public bool isSetWidth = false;
    public bool isSetHeight = false;

    void Start()
    {
        rt_BackgroundUI = GetComponent<RectTransform>();

        if(UIManager.instance.scale != 0)
        {
            if (UIManager.instance.scale < 1 && isSetHeight)
            {
                rt_BackgroundUI.sizeDelta = new Vector2( rt_BackgroundUI.sizeDelta.x, UIManager.instance.heightMaxUI );
            }
            else if (UIManager.instance.scale > 1 && isSetWidth)
            {
                rt_BackgroundUI.sizeDelta = new Vector2( UIManager.instance.widthMaxUI , rt_BackgroundUI.sizeDelta.y );
            }
        }
    }
}
