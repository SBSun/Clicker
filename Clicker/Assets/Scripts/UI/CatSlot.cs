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

    public void SlotOpen()
    {
        slotStatus = SlotStatus.Open;

        slot_Image.sprite = openSlot_Sprite;
        slotInside_Image.sprite = openSlotInside_Sprite;
    }
}
