using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatLevelUpButton : MonoBehaviour
{
    private Button catLevelUp_Button;
    public Text LevelUpNeedGold_Text;

    public Cat cat;

    private void Awake()
    {
        catLevelUp_Button = GetComponent<Button>();

        GoodsController.instance.SubGoldCheck( cat.levelUpNeedGoldList );

        if (GoodsController.instance.isBuyPossible)
        {
            catLevelUp_Button.interactable = true;
        }
        else
            catLevelUp_Button.interactable = false;

        UIManager.instance.UpdateGoldText( cat.levelUpNeedGoldList, LevelUpNeedGold_Text );
    }

    private void Update()
    {
        GoodsController.instance.SubGoldCheck( cat.levelUpNeedGoldList );

        if (GoodsController.instance.isBuyPossible)
        {
            catLevelUp_Button.interactable = true;
        }
        else
            catLevelUp_Button.interactable = false;

        UIManager.instance.UpdateGoldText( cat.levelUpNeedGoldList, LevelUpNeedGold_Text );
    }

    public void CatLevelUp_Button()
    {
        GoodsController.instance.SubGold( cat.levelUpNeedGoldList );

        UIManager.instance.UpdateGoldText( cat.levelUpNeedGoldList, LevelUpNeedGold_Text );

        cat.CatLevelUp();
        UIManager.instance.catInformation.SetCatLevelText( cat );
    }
}
