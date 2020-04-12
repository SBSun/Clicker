using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager m_instance;

    public static UIManager instance
    {
        get
        {
            if (m_instance != null)
                return m_instance;

            m_instance = FindObjectOfType<UIManager>();

            if (m_instance == null)
                m_instance = new GameObject( name: "UIManager" ).AddComponent<UIManager>();

            return m_instance;
        }
    }

    string[] units = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I" };

    public enum ViewUI
    {
        Recruitment, //고양이 모집
        CatInventory,
        Main, //메인 화면
        FurnitureDispose, //가구 배치
        Ranking,
        Shop,
        PopUp
    }

    [Header("ViewUI 관련 변수")]
    public ViewUI currentViewUI; //현재 무슨 UI를 보고있는지

    public ViewUI beforeViewUI;

    private GameObject go_BeforeViewUI;
    private GameObject go_CurrentViewUI;

    [HideInInspector]
    public GameObject go_PopUpUI;

    [Header("UI 스크립트")]
    public TopUI topUI;
    public BottomUI bottomUI;
    public SimpleCatInformationUI simpleCatInformationUI;
    public RecruitmentUI recruitmentUI;
    public CatInventoryUI catInventoryUI;
    public FurnitureDisposeUI furnitureDisposeUI;
    public ShopUI shopUI;
    public PopUpUI popUpUI;

    [Header("해상도 비율 구하기")]
    public CanvasScaler canvasScaler;
    public float scale;         // > 1 가로 비율이 커짐, 0 기준 비율, < 1 세로 비율이 커짐
    public float widthRatio;    // 화면 가로 비율
    public float heightRatio;   // 화면 세로 비율
    public float gcd = 0;       // 가로 세로의 최대 공약수
    public float widthMaxUI;
    public float heightMaxUI;
    public float multiple;

    void Awake()
    {
        scale = ((float)Screen.width / Screen.height) / ((float)9 / 16);

        if (scale != 0)
        {
            int width = Screen.width;
            int height = Screen.height;

            gcd = GetGCD( width, height );

            widthRatio = width / gcd;
            heightRatio = height / gcd;

            if (scale > 1)
            {
                //최대 width = 기준 height / height 비율 * width 비율
                widthMaxUI = canvasScaler.referenceResolution.y / heightRatio * widthRatio;
                multiple = widthMaxUI / canvasScaler.referenceResolution.x;
            }
            else if(scale < 1)
            {
                //최대 height = 기준 width / width 비율 * height 비율
                heightMaxUI = canvasScaler.referenceResolution.x / widthRatio * heightRatio;
                multiple = heightMaxUI / canvasScaler.referenceResolution.y;
            }        

            Debug.Log( "화면 비율 - " + widthRatio + " : " + heightRatio );
        }  
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown( KeyCode.Escape ))
            {
                switch (currentViewUI)
                {
                    case ViewUI.Recruitment:
                        recruitmentUI.go_RecruitmentUI.SetActive( false );
                        currentViewUI = ViewUI.Main;
                        break;

                    case ViewUI.CatInventory:
                        catInventoryUI.go_CatInventoryUI.SetActive( false );
                        currentViewUI = ViewUI.Main;
                        break;

                    case ViewUI.Main:
                        //게임 종료
                        break;

                    case ViewUI.FurnitureDispose:
                        furnitureDisposeUI.ResetFurnitureDisposeUI();
                        currentViewUI = ViewUI.Main;                   
                        break;

                    case ViewUI.Ranking:
                        //go_RankingUI.SetActive( false );
                        currentViewUI = ViewUI.Main;
                        break;

                    case ViewUI.Shop:
                        shopUI.go_ShopUI.SetActive( false );
                        currentViewUI = ViewUI.Main;
                        break;

                    case ViewUI.PopUp:
                        PopUpDeactivate();
                        break;

                }
            }
        }
    }

    public void UpdateGoldText()
    {
        topUI.gold_Text.text = GoldChangeString( GoodsController.instance.goldList );
    }

    public string GoldChangeString( List<int> newGoldList )
    {
        string str = "";

        for (int i = newGoldList.Count - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                str = newGoldList[0].ToString() + units[0];
            }
            else
            {
                if (newGoldList[i] != 0)
                {
                    if (newGoldList[i - 1] == 0)
                        str = string.Format( "{0}{1}", newGoldList[i], units[i] );
                    else
                        str = string.Format( "{0}.{1}{2}", newGoldList[i], newGoldList[i - 1], units[i] );

                    break;
                }
            }
        }

        return str;
    }

    public void OnClickBottomUIButton( int nextViewUINum )
    {
        go_CurrentViewUI = null;

        //Bottom UI의 버튼들을 누를 때 팝업이 출력되어 있는 상태이면 팝업을 꺼준다
        if (currentViewUI == ViewUI.PopUp)
            PopUpDeactivate();

        switch (nextViewUINum)
        {
            case (int)ViewUI.Recruitment:
                go_CurrentViewUI = recruitmentUI.go_RecruitmentUI;
                recruitmentUI.SetRecruitmentUI();
                currentViewUI = ViewUI.Recruitment;
                break;

            case (int)ViewUI.CatInventory:
                go_CurrentViewUI = catInventoryUI.go_CatInventoryUI;
                catInventoryUI.SetCatInventoryUI();
                currentViewUI = ViewUI.CatInventory;
                break;

            case (int)ViewUI.Main:
                currentViewUI = ViewUI.Main;
                break;

            case (int)ViewUI.FurnitureDispose:
                go_CurrentViewUI = furnitureDisposeUI.go_FurnitureDisposeUI;
                currentViewUI = ViewUI.FurnitureDispose;
                furnitureDisposeUI.SetFurnitureDisposeUI();
                Debug.Log( "가구" );
                break;

            case (int)ViewUI.Ranking:
                //go_CurrentViewUI = go_RankingUI;
                currentViewUI = ViewUI.Ranking;
                break;

            case (int)ViewUI.Shop:
                go_CurrentViewUI = shopUI.go_ShopUI;
                shopUI.SetShopUI();
                currentViewUI = ViewUI.Shop;
                break;
        }

        if (go_CurrentViewUI != null)
            go_CurrentViewUI.SetActive( true );
        if (go_BeforeViewUI != null && go_CurrentViewUI != go_BeforeViewUI)
            go_BeforeViewUI.SetActive( false );

        go_BeforeViewUI = go_CurrentViewUI;
    }

    //팝업 활성화
    public void PopUpActivation( GameObject _go_PopUpUI)
    {
        //팝업을 활성화 시키기 전에 보고있었던 UI 저장
        beforeViewUI = currentViewUI;

        //활성화 시킬 팝업 게임오브젝트를 go_PopUpUI 변수에 저장
        go_PopUpUI = _go_PopUpUI;

        //팝업 활성화
        go_PopUpUI.SetActive( true );

        //현재 보고 있는 UI를 팝업으로 설정
        currentViewUI = ViewUI.PopUp;
    }

    //팝업 비활성화
    public void PopUpDeactivate()
    {
        if (beforeViewUI == ViewUI.CatInventory)
        {
            catInventoryUI.rt_Scroll.sizeDelta = new Vector2( catInventoryUI.rt_Scroll.sizeDelta.x, catInventoryUI.beforeScrollHeight );
            catInventoryUI.rt_Scroll.anchoredPosition = new Vector2( 0, catInventoryUI.beforeScrollPosY );
        }

        //현재 보고 있는 UI를 팝업 전에 보고 있던 UI를 저장한 beforeViewUI로 설정
        currentViewUI = beforeViewUI;

        go_PopUpUI.SetActive( false ); //비활성화
    }

    public float GetGCD(float _width, float _height)
    {
        float max, min, temp = 0f;

        if(_width > _height)
        {
            max = _width;
            min = _height;
        }
        else
        {
            max = _height;
            min = _width;
        }

        while(max % min != 0)
        {
            temp = max % min;
            max = min;
            min = temp;
        }

        return min;
    }

    //기준 가로 비율보다 크면 UI의 사이즈와 포지션을 조정
    //0 - 사이즈, 포지션 변경, 1 - 사이즈 변경, 2 - 포지션 변경
    public void SetUIRT(RectTransform rt_UI, int setType = 0)
    {
        if(setType == 0)
        {
            rt_UI.sizeDelta = rt_UI.sizeDelta * multiple;
            rt_UI.anchoredPosition = rt_UI.anchoredPosition * multiple;
        }
        else if(setType == 1)
            rt_UI.sizeDelta = rt_UI.sizeDelta * multiple;
        else if(setType == 2)
            rt_UI.anchoredPosition = rt_UI.anchoredPosition * multiple;
    }
}
