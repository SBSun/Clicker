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

    void Start()
    {
        if (UIManager.instance.scale > 1)
        {
            SetSize();
            SetPosition();
        }
    }

    public void SetSize()
    {
        rt_TopImage.sizeDelta = rt_TopImage.sizeDelta * UIManager.instance.multiple;
        rt_DiaBlankImage.sizeDelta = rt_DiaBlankImage.sizeDelta * UIManager.instance.multiple;
        rt_DiaImage.sizeDelta = rt_DiaImage.sizeDelta * UIManager.instance.multiple;
        rt_GoldBlankImage.sizeDelta = rt_GoldBlankImage.sizeDelta * UIManager.instance.multiple;
        rt_GoldImage.sizeDelta = rt_GoldImage.sizeDelta * UIManager.instance.multiple;
        rt_SlideImage.sizeDelta = rt_SlideImage.sizeDelta * UIManager.instance.multiple;
        rt_RankingButton.sizeDelta = rt_RankingButton.sizeDelta * UIManager.instance.multiple;
        rt_AchievementButton.sizeDelta = rt_AchievementButton.sizeDelta * UIManager.instance.multiple;
        rt_SettingButton.sizeDelta = rt_SettingButton.sizeDelta * UIManager.instance.multiple;
        rt_QuestButton.sizeDelta = rt_QuestButton.sizeDelta * UIManager.instance.multiple;
    }

    public void SetPosition()
    {
        rt_SlideImage.anchoredPosition = rt_SlideImage.anchoredPosition * UIManager.instance.multiple;
        rt_DiaBlankImage.anchoredPosition = rt_DiaBlankImage.anchoredPosition * UIManager.instance.multiple;
        rt_DiaImage.anchoredPosition = rt_DiaImage.anchoredPosition * UIManager.instance.multiple;
        rt_GoldBlankImage.anchoredPosition = rt_GoldBlankImage.anchoredPosition * UIManager.instance.multiple;
        rt_GoldImage.anchoredPosition = rt_GoldImage.anchoredPosition * UIManager.instance.multiple;
        rt_RankingButton.anchoredPosition = rt_RankingButton.anchoredPosition * UIManager.instance.multiple;
        rt_AchievementButton.anchoredPosition = rt_AchievementButton.anchoredPosition * UIManager.instance.multiple;
        rt_SettingButton.anchoredPosition = rt_SettingButton.anchoredPosition * UIManager.instance.multiple;
        rt_QuestButton.anchoredPosition = rt_QuestButton.anchoredPosition * UIManager.instance.multiple;     
    }
}
