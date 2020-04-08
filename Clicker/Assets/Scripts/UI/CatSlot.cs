using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatSlot : MonoBehaviour
{
    public enum SlotStatus
    {
        Rock,
        Open
    }

    public SlotStatus slotStatus;

    public Image slot_Image;
    public Image slotInside_Image;
    public Image cat_Image;

    public Sprite openSlot_Sprite;
    public Sprite openSlotInside_Sprite;

    public Text catLevel_Text;
    public Text catConsumeGold_Text;

    public Cat cat;
    public CatWork catWork;

    public void SlotOpen()
    {
        slotStatus = SlotStatus.Open;

        slot_Image.sprite = openSlot_Sprite;
        slotInside_Image.sprite = openSlotInside_Sprite;
        cat_Image.sprite = cat.catInformation.catSprite;    

        catLevel_Text.enabled = true;
        catConsumeGold_Text.enabled = true;

        //catWork.StartCatWork();
    }

    public void OnClickCatSlot( Cat cat )
    {
        if(slotStatus == SlotStatus.Open)
        {
            UIManager.instance.catInventoryUI.beforeScrollPosY = UIManager.instance.catInventoryUI.rt_Scroll.anchoredPosition.y;
            UIManager.instance.catInventoryUI.CatInformationPopUp( this );
            UIManager.instance.PopUpActivation( UIManager.instance.catInventoryUI.go_CatInformationUI );
            UIManager.instance.catInventoryUI.name_Text.text = cat.catInformation.catName;
            UIManager.instance.catInventoryUI.job_Text.text = cat.catInformation.job;
            UIManager.instance.catInventoryUI.introduction_Text.text = cat.catInformation.introduction;
            UIManager.instance.catInventoryUI.level_Text.text = cat.level.ToString();
        }
    }
}    

