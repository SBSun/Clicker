using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FurnitureItem", menuName = "Create FurnitureItem" )]
public class FurnitureItem : ScriptableObject
{
    public enum FurnitureType
    {
        FloorDecoration, //바닥 장식
        WallDecoration,  //벽 장식
        TopDecoration,   //상단 장식
        Floor,           //바닥
        Wallpaper        //벽지
    }
    public FurnitureType furnitureType;

    public string itemName;
    public Sprite itemSprite;        //아이템의 Sprite
    public Vector2 itemImageSize;    //아이템의 이미지 사이즈
    public int currentHaveNumber;    //현재 소유하고 있는 개수
    public int currentDisPoseNumber; //현재 배치되어 있는 가구의 개수
    public int maxHaveNumber;        //아이템의 최대 소유 개수
    public List<int> itemPriceGoldList = new List<int>();
}
