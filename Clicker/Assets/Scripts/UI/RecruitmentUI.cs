using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitmentUI : MonoBehaviour
{
    public GameObject go_RecruitmentUI;
    
    public RectTransform rt_BottomImage;

    public RecruitmentScroll recruitmentScroll;
    private GachaSystem gachaSystem;

    [Header("모집 애니메이션 관련 변수")]
    public GameObject go_RecruitmentAnimationUI;
    public Image cutScene1_1;
    public Image cutScene1_2;
    public Image cutScene2_1;
    public Image cutScene2_2;
    public Image cutScene3_1;
    public Image cutScene3_2;
    public Image cutScene3_3;

    public Coroutine animationCoroutine;

    [Header( "모집 결과 관련 변수" ), Space(10)]
    public GameObject go_RecruitmentResultUI;
    public GameObject go_Minislots;
    public GameObject go_Effect;
    public GameObject go_Result;
    public Image cat_Image;
    public Image star_Image;
    public Image new_Image;
    public Text name_Text;
    public Sprite slotInside_Sprite;
    public Sprite slotInside5_Sprite;
    public MiniCatSlot[] miniCatSlots;

    //제일 등급이 높고 우선순위가 높은 고양이가 몇번째 있는지
    private int highestCatNum = 0;

    void Start()
    {
        gachaSystem = GetComponent<GachaSystem>();

        if (UIManager.instance.scale < 1)
        {
            rt_BottomImage.sizeDelta = new Vector2( rt_BottomImage.sizeDelta.x, UIManager.instance.heightMaxUI / 2 - 98 );
        }
    }

    public void SetRecruitmentUI()
    {
        recruitmentScroll.scrollbar.value = 0f;
    }

    public IEnumerator RecruitmentAnimation()
    {
        SetPickHighestCatSprite();
        go_RecruitmentAnimationUI.SetActive( true );

        float time = 0f;
        Color color;

        color = new Color(1,1,1,0);

        while (time < 0.5f)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp( 0, 1, time / 0.5f );

            cutScene1_1.color = color;
            cutScene1_2.color = color;
            yield return null;
        }

        time = 0f;

        Vector2 originPos = cutScene1_2.transform.position;
        while(time < 0.5f)
        {
            time += Time.deltaTime;

            cutScene1_2.transform.position = originPos;
            cutScene1_2.transform.position = new Vector2( cutScene1_2.transform.position.x + Random.Range( -5f, 5f ), cutScene1_2.transform.position.y );
            yield return null;
        }

        time = 0f;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp( 0, 1, time / 0.5f );

            cutScene2_1.color = color;
            cutScene2_2.color = color;
            yield return null;
        }

        time = 0f;

        int direction = -1;

        while (time < 2f)
        {
            time += Time.deltaTime;

            cutScene2_2.transform.Rotate( new Vector3( 0, 0, 30 * direction * Time.deltaTime ) );

            if (cutScene2_2.transform.eulerAngles.z >= 350 && cutScene2_2.transform.eulerAngles.z <= 355)
                direction = 1;
            else if(cutScene2_2.transform.eulerAngles.z >= 5 && cutScene2_2.transform.eulerAngles.z <=10)
                direction = -1;

            yield return null;
        }

        time = 0f;
        while(time < 0.5f)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp( 0, 1, time / 0.5f );

            cutScene3_1.color = color;
            yield return null;
        }

        time = 0f;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp( 0, 1, time / 0.5f );

            cutScene3_2.color = color;
            yield return null;
        }

        time = 0f;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp( 0, 1, time / 0.5f );

            cutScene3_3.color = color;
            yield return null;
        }

        yield return new WaitForSeconds( 2f );

        go_RecruitmentAnimationUI.SetActive( false );
        go_RecruitmentResultUI.SetActive( true );

        ResetAnimation();
    }

    public void StartAnimation()
    {
        animationCoroutine = StartCoroutine( RecruitmentAnimation() );
    }

    public void StopAnimation()
    {
        StopCoroutine( animationCoroutine );
        ResetAnimation();
    }

    //애니메이션에 사용되는 리소소들 초기화
    public void ResetAnimation()
    {
        Color color = new Color( 1, 1, 1, 0 );

        cutScene1_1.color = color;
        cutScene1_2.color = color;
        cutScene2_1.color = color;
        cutScene2_2.color = color;
        cutScene3_1.color = color;
        cutScene3_2.color = color;
        cutScene3_3.color = color;

        cutScene1_2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        cutScene2_2.transform.localRotation = Quaternion.identity;
    }

    //애니메이션 스킵 버튼
    public void OnClickAnimationSkip()
    {
        StopAnimation();

        go_RecruitmentAnimationUI.SetActive( false );
        go_RecruitmentResultUI.SetActive( true );
    }

    //뽑기 결과로 가는 버튼
    public void OnClickGoResult()
    {
        go_Effect.SetActive( false );

        go_Result.SetActive( true );

        if (gachaSystem.gachaType == GachaType.One)
        {
            SetPickCatInfo( gachaSystem.pickCatList[0] );
        }
        else
        {
            SetPickCatInfo( gachaSystem.pickCatList[highestCatNum] );
            go_Minislots.gameObject.SetActive( true );
        }
    }

    //다시 모집 UI로
    public void OnClickRecruitmentUI()
    {
        go_Effect.SetActive( true );

        go_Result.SetActive( false );

        go_RecruitmentResultUI.gameObject.SetActive( false );

        if(gachaSystem.gachaType == GachaType.Ten)
            go_Minislots.gameObject.SetActive( false );

        UIManager.instance.topUI.TopUIActivation();
        UIManager.instance.bottomUI.BottomUIActivation();
    }

    //연속뽑기를 했을 때 호출
    public void SetPickHighestCatSprite()
    {
        if(gachaSystem.gachaType == GachaType.One)
        {
            cat_Image.sprite = gachaSystem.pickCatList[0].catInformation.catSprite;
        }
        else
        {
            int highestClass = 4;

            for (int i = 0; i < miniCatSlots.Length; i++)
            {
                if ((int)miniCatSlots[i].cat.catInformation.catClass < highestClass)
                {
                    highestClass = (int)miniCatSlots[i].cat.catInformation.catClass;
                    highestCatNum = i;
                }
                else if ((int)miniCatSlots[i].cat.catInformation.catClass == highestClass)
                {
                    if (miniCatSlots[i].cat.catInformation.catName.CompareTo( miniCatSlots[highestCatNum].cat.catInformation.catName ) < 0)
                    {
                        highestCatNum = i;
                    }
                }
            }

            cat_Image.sprite = miniCatSlots[highestCatNum].cat.catInformation.catSprite;
        }  
    }

    //뽑은 고양이 정보 출력
    public void SetPickCatInfo(Cat _cat)
    {
        cat_Image.sprite = _cat.catInformation.catSprite;

        name_Text.text = _cat.catInformation.catName;

        if (_cat.count == 1)
            new_Image.enabled = true;
        else
            new_Image.enabled = false;

        star_Image.sprite = UIManager.instance.star_Sprites[(int)_cat.catInformation.catClass];
        star_Image.rectTransform.sizeDelta = new Vector2( 120 * (5 - (int)_cat.catInformation.catClass), star_Image.rectTransform.sizeDelta.y );
    }
}
    