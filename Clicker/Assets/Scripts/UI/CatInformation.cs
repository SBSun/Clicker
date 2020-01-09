using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatInformation : MonoBehaviour
{
    public GameObject go_CatInformation;

    public Image cat_Image;
    public Text catName_Text;
    public Text catLevel_Text;

    public void SetCatInformation(Cat newCat)
    {
        cat_Image.sprite = newCat.catSprite;
        catName_Text.text = newCat.catName;
        catLevel_Text.text = "Lv." + newCat.catLevel.ToString();

        go_CatInformation.SetActive( true );
    }

    public void DeactiveButton()
    {
        go_CatInformation.SetActive( false );
    }

    public void SetCatLevelText(Cat newCat)
    {
        catLevel_Text.text = "Lv." + newCat.catLevel.ToString();
    }
}
