using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecruitmentScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const int SIZE = 2;
    private float[] posValues = new float[SIZE] { 0, 1 };
    private float targetPosValue = 0f, curPosValue = 0f;

    [SerializeField] private float passDragSpeed = 0f;
    private int index = 0;

    private int CurIndex
    {
        get
        {
            return index;
        }
        set
        {
            if(value == 0)
            {
                go_LeftButton.SetActive( false );
                go_RightButton.SetActive( true );
            }
            else if(value == posValues.Length - 1)
            {
                go_LeftButton.SetActive( true );
                go_RightButton.SetActive( false );
            }

            index = value;
        }
    }
    public bool isDrag = false;

    public Scrollbar scrollbar;
    public GameObject go_LeftButton;
    public GameObject go_RightButton;

    void Update()
    {
        if(!isDrag)
        {
            scrollbar.value = Mathf.Lerp( scrollbar.value, targetPosValue, 0.1f );
        }
    }

    private float SetPos()
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (scrollbar.value < posValues[i] + 0.5f && scrollbar.value > posValues[i] - 0.5f)
            {
                CurIndex = i;
                return posValues[i];
            }
        }

        return 0;
    }

    public void OnBeginDrag( PointerEventData eventData )
    {
        curPosValue = SetPos();
    }

    public void OnDrag( PointerEventData eventData )
    {
        isDrag = true;
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        isDrag = false;

        targetPosValue = SetPos();

        if (curPosValue == targetPosValue)
        {
            Debug.Log( eventData.delta.x );

            if (eventData.delta.x > passDragSpeed && CurIndex == 1)
            {
                targetPosValue = curPosValue - 1f;
                CurIndex = 0;
            }
            else if (eventData.delta.x < -passDragSpeed && CurIndex == 0)
            {
                targetPosValue = curPosValue + 1f;
                CurIndex = 1;
            }
        }
    }

    //방향 버튼, 매개변수 - 왼쪽 : -1, 오른쪽 : 1
    public void OnClickDirectionButton(int direction)
    {
        if(!isDrag)
        {
            CurIndex += direction;
            targetPosValue = posValues[CurIndex];
        }  
    }
}
