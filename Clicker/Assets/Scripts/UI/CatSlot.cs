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

    public Sprite openSlot_Sprite;
    public Sprite openSlotInside_Sprite;

    public Text catLevel_Text;
    public Text catConsumeGold_Text;

    public Cat cat;
}
