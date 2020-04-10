using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureSlot : MonoBehaviour
{
    private Image slot_Image;
    public Image furniture_Image;
    public Image gold_Image;

    //소유량 - 배치된 가구 수 / 소유량
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
        SetFurnitureImageSize();

        SetPriceText();
        SetHaveNumberText();
    }

    //가구 이미지 사이즈를 조정한다.
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

    //아이템 가격 텍스트 값, 위치 설정
    public void SetPriceText()
    {
        //가구 가격 표시
        furniturePrice_Text.text = UIManager.instance.GoldChangeString( furnitureItemData.furnitureItem.itemPriceGoldList );
        //텍스트 길이에 따라 골드 이미지 위치 설정
        furniturePrice_Text.rectTransform.sizeDelta = new Vector2( furniturePrice_Text.preferredWidth, furniturePrice_Text.preferredHeight );
        gold_Image.rectTransform.anchoredPosition = new Vector2( -furniturePrice_Text.rectTransform.sizeDelta.x / 2 - 10f,
            -2.7f );
    }

    //아이템 소유량과 배치 되고 남은 수
    public void SetHaveNumberText()
    {
        if (furnitureItemData.currentHaveNumber == 0)
            haveNumber_Text.text = "미보유";
        else
            haveNumber_Text.text = furnitureItemData.currentHaveNumber - furnitureItemData.currentDisposeNumber + " / " + furnitureItemData.currentHaveNumber;
    }

    //가구를 구매했을 때 실행
    public void FurnitureBuyUpdate()
    {
        //가구 소유 개수가 최대 소유 개수보다 적으면 추가
        if(furnitureItemData.currentHaveNumber < furnitureItemData.furnitureItem.maxHaveNumber)
        {
            furnitureItemData.currentHaveNumber++;

            FurnitureSaveData furnitureSaveData = new FurnitureSaveData( furnitureItemData.furnitureItem.itemName, furnitureItemData.currentHaveNumber, furnitureItemData.currentDisposeNumber );

            //해당 가구를 처음 구매하는 거라면 데이터 삽입
            if(furnitureItemData.currentHaveNumber == 1)
            {
                BackEndManager.instance.backEndDataSave.InsertFurnitureItemData( furnitureSaveData );
                SetHaveNumberText();
                return;
            }

            //이미 구매했던 가구라면 수량만 수정해준다.
            BackEndManager.instance.backEndDataSave.UpdateFurnitureItemData( furnitureSaveData );
            SetHaveNumberText();
        }
    }

    //가구 구매 버튼을 눌렀을 때 실행
    public void OnClickFurnitureBuy()
    {
        //가구 구매 팝업 활성화, 인수값으로 자기 자신을 넘겨준다.
        UIManager.instance.furnitureDisposeUI.FurnitureBuyPopUp( this );
    }
}
