using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopUI : MonoBehaviour
{
    public RectTransform rt_TopImage;
    public RectTransform rt_DiaBlankImage;
    public RectTransform rt_DiaImage;
    public RectTransform rt_GoldBlankImage;
    public RectTransform rt_GoldImage;
    public RectTransform rt_SlideImage;
    public RectTransform rt_RankingButton;
    public RectTransform rt_AchievementButton;
    public RectTransform rt_SettingButton;
    public RectTransform rt_QuestButton;
    public RectTransform rt_SaveButton;

    public Text gold_Text;
    public Text dia_Text;

    void Start()
    {
        if (UIManager.instance.scale > 1)
        {
            UIManager.instance.SetUIRT( rt_TopImage, 0 );
            UIManager.instance.SetUIRT( rt_DiaBlankImage, 0 );
            UIManager.instance.SetUIRT( rt_DiaImage, 0 );
            UIManager.instance.SetUIRT( rt_GoldBlankImage, 0 );
            UIManager.instance.SetUIRT( rt_GoldImage, 0 );
            UIManager.instance.SetUIRT( rt_SlideImage, 0 );
            UIManager.instance.SetUIRT( rt_RankingButton, 0 );
            UIManager.instance.SetUIRT( rt_AchievementButton, 0 );
            UIManager.instance.SetUIRT( rt_SettingButton, 0 );
            UIManager.instance.SetUIRT( rt_QuestButton, 0 );
            UIManager.instance.SetUIRT( rt_SaveButton, 0 );
        }
    }

    public void TopUIActivation()
    {
        rt_TopImage.gameObject.SetActive( true );
    }

    public void TopUIDeactivate()
    {
        rt_TopImage.gameObject.SetActive( false );
    }
}
