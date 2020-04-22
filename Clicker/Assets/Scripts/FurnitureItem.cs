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
    [ContextMenuItem("SetItemSprite","SetItemSprite")]
    public Sprite itemSprite;        //아이템의 Sprite
    [ContextMenuItem( "SetItemIconSprite", "SetItemIconSprite" )]
    public Sprite itemIconSprite;    //아이템의 아이콘 Sprite
    [ContextMenuItem("SetItemSpriteSize", "SetItemSpriteSize")]
    public Vector2 itemSpriteSize;    //아이템의 이미지 사이즈
    public int maxHaveNumber;        //아이템의 최대 소유 개수
    public List<int> itemPriceGoldList = new List<int>();

    public void SetItemSprite()
    {
        Sprite sprite = Resources.Load<Sprite>( "Sprite/Furniture/" + furnitureType.ToString() + "/" + itemName );

        if(sprite != null)
        {
            itemSprite = sprite;
        }
    }

    public void SetItemIconSprite()
    {
         Sprite sprite = Resources.Load<Sprite>( "Sprite/Furniture/" + furnitureType.ToString() + "/" + itemName + " 아이콘" );

         if(sprite != null)
        {
            itemIconSprite = sprite;
        }
    }

    public void SetItemSpriteSize()
    {
        if(itemSprite != null)
        {
            if (itemIconSprite != null)
                itemSpriteSize = itemIconSprite.rect.size;

            else
                itemSpriteSize = itemSprite.rect.size;
        }
    }
}
