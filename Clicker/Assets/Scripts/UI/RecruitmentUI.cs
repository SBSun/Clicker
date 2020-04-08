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
    public Image black_Image;
    public Image effect1_Image;
    public Image effect2_Image;
    public Image cat_Image;
    public Image nameBackground_Image;   
    public Image star_Image;
    public Button goResult_Button;
    public Button goRecruitmentUI_Button;
    public Text name_Text;

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
        black_Image.enabled = false;

        //애니메이션은 오브젝트를 비활성화해야지 멈춤
        effect1_Image.gameObject.SetActive( false );
        effect2_Image.gameObject.SetActive( false );

        nameBackground_Image.enabled = true;
        name_Text.enabled = true;

        goRecruitmentUI_Button.gameObject.SetActive( true );
    }

    //다시 모집 UI로
    public void OnClickRecruitmentUI()
    {
        black_Image.enabled = true;

        nameBackground_Image.enabled = false;
        name_Text.enabled = false;

        effect1_Image.gameObject.SetActive( true );
        effect2_Image.gameObject.SetActive( true );

        goRecruitmentUI_Button.gameObject.SetActive(false);

        go_RecruitmentResultUI.gameObject.SetActive( false );

        UIManager.instance.topUI.TopUIActivation();
        UIManager.instance.bottomUI.BottomUIActivation();
    }

    //뽑은 고양이 정보 출력
    public void PickCatInformation(CatInformation _catInformation)
    {
        cat_Image.sprite = _catInformation.catSprite;

        name_Text.text = _catInformation.catName;
    }
}
    