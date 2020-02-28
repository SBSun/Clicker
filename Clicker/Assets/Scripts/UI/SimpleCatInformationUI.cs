using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleCatInformationUI : MonoBehaviour
{
    public GameObject       go_SimpleCatInformationUI;
    public RectTransform    rect_SimpleCatInformationUI;

    public Text name_Text;          //이름
    public Text job_Text;           //직업
    public Text level_Text;         //레벨
    public Text consumeGold_Text;   //초당 소비 골드
    public Text maxKeepGold_Text;   //최대 소유 가능 골드

    public void UIActivation()
    {
        go_SimpleCatInformationUI.SetActive( true );
    }

    public void SetInformation(CatConsume catConsume)
    {
        Cat cat = catConsume.catSlot.cat;

        name_Text.text = cat.name;
        job_Text.text = cat.job;
        level_Text.text = cat.level.ToString();

        Vector2 catPos = Camera.main.WorldToScreenPoint( catConsume.transform.position );
        go_SimpleCatInformationUI.transform.position = catPos;

        rect_SimpleCatInformationUI.anchoredPosition = new Vector2( rect_SimpleCatInformationUI.anchoredPosition.x, rect_SimpleCatInformationUI.anchoredPosition.y + rect_SimpleCatInformationUI.sizeDelta.y );
        UIActivation();
    }
}
