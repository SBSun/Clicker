using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public enum CurrentViewUI
    {
        Recruitment, //고양이 모집
        CatInventory, 
        Main, //메인 화면
        Dispose, //가구 배치
        Ranking, 
        Shop,
        PopUp
    }
    public CurrentViewUI currentViewUI; //현재 무슨 UI를 보고있는지

    public GameObject go_BeforeViewUI;
    public GameObject go_CurrentViewUI;

    public GameObject go_RecruitmentUI;
    public GameObject go_CatInventoryUI;
    public GameObject go_DisposeUI;
    public GameObject go_RankingUI;
    public GameObject go_ShopUI;
    public GameObject go_PopUpUI;

    public void OnClickUIButton(int nextViewUINum)
    {
        go_CurrentViewUI = null;

        switch(nextViewUINum)
        {
            case (int)CurrentViewUI.Recruitment:
                go_CurrentViewUI = go_RecruitmentUI;
                currentViewUI = CurrentViewUI.Recruitment;
                break;

            case (int)CurrentViewUI.CatInventory:
                go_CurrentViewUI = go_CatInventoryUI;
                currentViewUI = CurrentViewUI.CatInventory;
                break;

            case (int)CurrentViewUI.Main:
                currentViewUI = CurrentViewUI.Main;
                break;

            case (int)CurrentViewUI.Dispose:
                go_CurrentViewUI = go_DisposeUI;
                currentViewUI = CurrentViewUI.Dispose;
                break;

            case (int)CurrentViewUI.Ranking:
                go_CurrentViewUI = go_RankingUI;
                currentViewUI = CurrentViewUI.Ranking;
                break;

            case (int)CurrentViewUI.Shop:
                go_CurrentViewUI = go_ShopUI;
                currentViewUI = CurrentViewUI.Shop;
                break;
        }

        if (go_CurrentViewUI != null)
            go_CurrentViewUI.SetActive( true );
        if (go_BeforeViewUI != null && go_CurrentViewUI != go_BeforeViewUI)
            go_BeforeViewUI.SetActive( false );

        go_BeforeViewUI = go_CurrentViewUI;
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {

            }
        }
    }
}
