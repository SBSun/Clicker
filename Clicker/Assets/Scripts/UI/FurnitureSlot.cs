using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureSlot : MonoBehaviour
{
    private Image slot_Image;
    public Image furniture_Image;
    public Image gold_Image;

    public Text haveNumber_Text;        //아이템을 몇 개 가지고 있는지
    public Text furniturePrice_Text;    //가구 아이템 가격

    public FurnitureItemData furnitureItemData;

    private void Awake()
    {
        slot_Image = GetComponent<Image>();
    }

    public void OnClickFurnitureSlot()
    {
        //전에 선택된 가구 슬롯이 있다면
        if(UIManager.instance.furnitureDisposeUI.selectFurnitureSlot != null)
        {
            if(UIManager.instance.furnitureDisposeUI.selectFurnitureSlot == this)
            {
                UIManager.instance.furnitureDisposeUI.selectFurnitureSlot = null;
                slot_Image.sprite = UIManager.instance.furnitureDisposeUI.defaultFurnitureSlot_Sprite;
                return;
            }

            //그 가구 슬롯의 Sprite를 기본 슬롯 Sprite로 변경
            UIManager.instance.furnitureDisposeUI.selectFurnitureSlot.slot_Image.sprite = UIManager.instance.furnitureDisposeUI.defaultFurnitureSlot_Sprite;
        }

        //슬롯 Sprite를 선택된 슬롯 Sprite로 변경
        slot_Image.sprite = UIManager.instance.furnitureDisposeUI.selectFurnitureSlot_Sprite;
        //해당 슬롯을 선택함
        UIManager.instance.furnitureDisposeUI.selectFurnitureSlot = this;
    }

    //Sprite, 텍스트 등 해당 아이템에 맞게 변경
    public void SetFurnitureSlot()
    {
        if (furnitureItemData.currentHaveNumber == 0)
            haveNumber_Text.text = "미보유";
        else
            haveNumber_Text.text = furnitureItemData.currentHaveNumber + " / " + furnitureItemData.currentHaveNumber;

        SetFurnitureImageSize();

        SetPriceText();
    }

    public void SetFurnitureImageSize()
    {
        if(furnitureItemData.furnitureItem.itemImageSize.x > UIManager.instance.furnitureDisposeUI.maxSlotImageSize.x ||
            furnitureItemData.furnitureItem.itemImageSize.y > UIManager.instance.furnitureDisposeUI.maxSlotImageSize.y)
        {
            //최대 공약수를 구함
            float gcd = UIManager.instance.GetGCD( furnitureItemData.furnitureItem.itemImageSize.x, furnitureItemData.furnitureItem.itemImageSize.y );

            //가로, 세로 비율 구함
            float widthRatio = furnitureItemData.furnitureItem.itemImageSize.x / gcd;
            float heightRatio = furnitureItemData.furnitureItem.itemImageSize.y / gcd;

            Vector2 imageSize = Vector2.zero;

            if (widthRatio > heightRatio)
            {
                imageSize.x = UIManager.instance.furnitureDisposeUI.maxSlotImageSize.x;
                imageSize.y = imageSize.x / widthRatio * heightRatio;
            }
            else if (widthRatio < heightRatio)
            {
                imageSize.y = UIManager.instance.furnitureDisposeUI.maxSlotImageSize.y;
                imageSize.x = imageSize.y / heightRatio * widthRatio;
            }

            furniture_Image.GetComponent<RectTransform>().sizeDelta = imageSize;
        }
        else
        {
            furniture_Image.GetComponent<RectTransform>().sizeDelta = furnitureItemData.furnitureItem.itemImageSize;
        }

        if(furnitureItemData.furnitureItem.itemIconSprite != null)
        {
            furniture_Image.sprite = furnitureItemData.furnitureItem.itemIconSprite;
        }
        else
        {
            furniture_Image.sprite = furnitureItemData.furnitureItem.itemSprite;
        }
    }

    //아이템 가격 텍스트 설정
    public void SetPriceText()
    {
        furniturePrice_Text.text = UIManager.instance.GoldChangeString( furnitureItemData.furnitureItem.itemPriceGoldList );
        furniturePrice_Text.rectTransform.sizeDelta = new Vector2( furniturePrice_Text.preferredWidth, furniturePrice_Text.preferredHeight );
        gold_Image.rectTransform.anchoredPosition = new Vector2( -furniturePrice_Text.rectTransform.sizeDelta.x / 2 - 10f,
            -2.7f );
    }

    //가구 구매 팝업
    public void FurnitureBuyPopUp()
    {
        //구매 가능하면
        if(GoodsController.instance.SubGoldCheck(GoodsController.instance.goldList, furnitureItemData.furnitureItem.itemPriceGoldList))
        {
            UIManager.instance.PopUpActivation( UIManager.instance.furnitureDisposeUI.go_FurnitureBuyPopUp );

            UIManager.instance.furnitureDisposeUI.furniturePrice_Text.text = UIManager.instance.GoldChangeString( furnitureItemData.furnitureItem.itemPriceGoldList ) + "가 소모 됩니다.";
            UIManager.instance.furnitureDisposeUI.furniturePrice_Text.rectTransform.sizeDelta = new Vector2( UIManager.instance.furnitureDisposeUI.furniturePrice_Text.preferredWidth, UIManager.instance.furnitureDisposeUI.furniturePrice_Text.preferredHeight );

            UIManager.instance.furnitureDisposeUI.gold_Image.rectTransform.anchoredPosition = new Vector2( -UIManager.instance.furnitureDisposeUI.furniturePrice_Text.rectTransform.sizeDelta.x / 2 - 20f , UIManager.instance.furnitureDisposeUI.gold_Image.rectTransform.anchoredPosition.y);
        }
        else
        {
            UIManager.instance.popUpUI.GoldLackPopUp();
        }
    }
}
