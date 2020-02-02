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

        if (GoodsController.instance.SubGoldCheck( GoodsController.instance.goldList, cat.levelUpNeedGoldList ))
        {
            catLevelUp_Button.interactable = true;
        }
        else
            catLevelUp_Button.interactable = false;

        UIManager.instance.UpdateGoldText( cat.levelUpNeedGoldList, LevelUpNeedGold_Text );
    }

    private void Update()
    {
        if (GoodsController.instance.SubGoldCheck( GoodsController.instance.goldList, cat.levelUpNeedGoldList ))
        {
            catLevelUp_Button.interactable = true;
        }
        else
            catLevelUp_Button.interactable = false;

        UIManager.instance.UpdateGoldText( cat.levelUpNeedGoldList, LevelUpNeedGold_Text );
    }

    public void CatLevelUp_Button()
    {
        GoodsController.instance.SubGold( GoodsController.instance.goldList, cat.levelUpNeedGoldList );

        UIManager.instance.UpdateGoldText( cat.levelUpNeedGoldList, LevelUpNeedGold_Text );

        cat.CatLevelUp();
        UIManager.instance.catInformation.SetCatLevelText( cat );
    }
}
