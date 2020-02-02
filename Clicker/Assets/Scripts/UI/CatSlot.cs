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

        catLevel_Text.enabled = true;
        catConsumeGold_Text.enabled = true;

        catWork.StartCatWork();
    }

    public void OnClickCatSlot( Cat cat )
    {
        UIManager.instance.catInventory.go_CatInformationUI.SetActive( true );
        UIManager.instance.catInventory.catName_Text.text = "이름 : " + cat.catName;
        UIManager.instance.catInventory.catJob_Text.text = "직업 : " + cat.catJob;
        UIManager.instance.catInventory.catIntroduction_Text.text = "소개 : " + cat.catIntroduction;
        UIManager.instance.catInventory.catLevel_Text.text = "LV : " + cat.catLevel;
    }
}
