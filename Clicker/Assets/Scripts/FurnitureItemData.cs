using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureItemData : MonoBehaviour
{
    public FurnitureItem furnitureItem;

    public FurnitureSlot furnitureSlot;

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

    public void DisposeAdd()
    {
        if (currentHaveNumber - currentDisposeNumber > 0)
        {
            currentDisposeNumber++;
            furnitureSlot.SetHaveNumberText();
        }
    }

    public void DisposeSub()
    {
        if(currentDisposeNumber > 0)
        {
            currentDisposeNumber--;
            furnitureSlot.SetHaveNumberText();
        }
    }

    public void HaveAdd()
    {
        if(furnitureItem.maxHaveNumber > currentHaveNumber)
        {
            currentHaveNumber++;
            furnitureSlot.SetHaveNumberText();
        }
    }

    public void FurnitureBuy()
    {
        if (currentHaveNumber < furnitureItem.maxHaveNumber)
        {
            HaveAdd();

            //해당 가구를 처음 구매하는 거라면 데이터 삽입
            if (currentHaveNumber == 1)
            {
                BackEndManager.instance.backEndDataSave.InsertFurnitureItemData( this );
                return;
            }

            //이미 구매했던 가구라면 수량만 수정해준다.
            BackEndManager.instance.backEndDataSave.UpdateFurnitureItemData( this );
        }
    }

    public void UpdateFurnitureItemData()
    {
        BackEndManager.instance.backEndDataSave.UpdateFurnitureItemData( this );
    }

    public void SetFurnitureSlot(FurnitureSlot _furnitureSlot)
    {
        furnitureSlot = _furnitureSlot;
    }
}
