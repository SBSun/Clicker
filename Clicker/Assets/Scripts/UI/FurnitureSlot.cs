using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureSlot : MonoBehaviour
{
    public Image furniture_Image;

    public FurnitureItem furnitureItem;
    public Text haveNumber_Text;        //아이템을 몇 개 가지고 있는지

    public void OnClickFurnitureSlot()
    {

    }

    //Sprite, 텍스트 등 해당 아이템에 맞게 변경
    public void SetFurnitureSlot()
    {
        furniture_Image.sprite = furnitureItem.itemSprite;
        furniture_Image.GetComponent<RectTransform>().sizeDelta = furnitureItem.itemImageSize;

        if (furnitureItem.currentHaveNumber == 0)
            haveNumber_Text.text = "미보유";
        else
            haveNumber_Text.text = furnitureItem.currentHaveNumber + " / " + furnitureItem.currentHaveNumber;
    }
}
