using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviour
{
    public GameObject go_PopUp;

    public RectTransform rt_Top;
    public RectTransform rt_Middle;
    public RectTransform rt_Bottom;

    public Text explain_Text;

    public void GoldLackPopUp()
    {
        explain_Text.text = "골드가 부족합니다.";
        UIManager.instance.PopUpActivation( go_PopUp );
    }

    public void OnClickOffButton()
    {
        go_PopUp.SetActive( false );
    }
}
