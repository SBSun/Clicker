using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedFurniture : MonoBehaviour
{
    //배치된 가구
    public FurnitureItem furnitureItem;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ArrangeFurniture(FurnitureItem _furnitureItem)
    {
        furnitureItem = _furnitureItem;

        spriteRenderer.sprite = furnitureItem.itemSprite;
    }
}
