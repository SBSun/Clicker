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

    public void AfterBuyFurniturePopUp()
    {
        explain_Text.text = "가구를 구매 후 다시 시도해 주세요.";
        UIManager.instance.PopUpActivation( go_PopUp );
    }

    public void OnClickOffButton()
    {
        UIManager.instance.PopUpDeactivate();
    }
}
