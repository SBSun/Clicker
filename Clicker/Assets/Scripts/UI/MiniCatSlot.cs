using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniCatSlot : MonoBehaviour
{
    public Image inside_Image;
    public Image cat_Image;
    public Image star_Image;

    public Cat cat;

    //설정
    public void SetMiniCatSlot(Cat _cat)
    {
        cat = _cat;

        if (cat.catInformation.catClass == CatInformation.CatClass.Five)
            inside_Image.sprite = UIManager.instance.recruitmentUI.slotInside5_Sprite;
        else
            inside_Image.sprite = UIManager.instance.recruitmentUI.slotInside_Sprite;

        cat_Image.sprite = _cat.catInformation.catSprite;

        star_Image.sprite = UIManager.instance.star_Sprites[(int)cat.catInformation.catClass];
        star_Image.rectTransform.sizeDelta = new Vector2( 30 * (5 - (int)cat.catInformation.catClass), star_Image.rectTransform.sizeDelta.y );
    }

    //미니 슬롯을 클릭했을 때 실행
    public void OnClickMiniCatSlot()
    {
        UIManager.instance.recruitmentUI.SetPickCatInfo( cat );
    }
}
