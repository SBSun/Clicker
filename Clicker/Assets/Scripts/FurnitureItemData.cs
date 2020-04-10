using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureItemData : MonoBehaviour
{
    public FurnitureItem furnitureItem;

    //현재 소유하고 있는 개수
    public int currentHaveNumber = 0;
    //현재 배치되어 있는 가구의 개수
    public int currentDisposeNumber = 0;

    public FurnitureItemData( FurnitureItem _furnitureItem, int _currentHaveNumber = 0, int _currentDisposeNumber = 0)
    {
        furnitureItem = _furnitureItem;
        currentHaveNumber = _currentHaveNumber;
        currentDisposeNumber = _currentDisposeNumber;
    }
}
