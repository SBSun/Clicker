using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitmentUI : MonoBehaviour
{
    public GameObject go_RecruitmentUI;

    public RectTransform rt_BottomImage;

    public RecruitmentScroll recruitmentScroll;

    void Start()
    {
        if (UIManager.instance.scale < 1)
        {
            rt_BottomImage.sizeDelta = new Vector2( rt_BottomImage.sizeDelta.x, UIManager.instance.heightMaxUI / 2 - 98 );
        }
    }

    public void SetRecruitmentUI()
    {
        recruitmentScroll.scrollbar.value = 0f;
    }
}
