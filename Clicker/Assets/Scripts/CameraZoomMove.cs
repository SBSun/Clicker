using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraZoomMove : MonoBehaviour
{
    public float moveSpeed = 0f;

    public float zoomSpeed = 0f;
    public float zoomMax = 0f;
    public float zoomMin = 0f;

    public bool isZooming = false; //확대 중인지 아닌지, 코루틴에 사용

    public Vector2 center; //중심점
    public Vector2 size; //크기

    float width;
    float height;
    float mY = 0f;
    float clampY = 0f;

    [HideInInspector]
    public Camera mainCamera;
    //Graphic Raycaster를 사용하기 위해
    public Canvas canvas;
    private GraphicRaycaster graphicRay;
    private PointerEventData ped;

    public Coroutine cameraZoomCoroutine;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        graphicRay = canvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData( null );
    }

    void Start()
    {
        height = mainCamera.orthographicSize; //카메라의 세로 절반 길이
        width = height * Screen.width / Screen.height; //카메라의 가로 절반 길이
        size = new Vector2( width * 2, height * 2 );
    }

    void Update()
    {
        if(Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            ped.position = Input.mousePosition;
            List<RaycastResult> resultList = new List<RaycastResult>();
            graphicRay.Raycast( ped, resultList );

            if(resultList.Count > 0)
            {
                Debug.Log( "UI 터치 중" );
                return;
            }
        }       

        Zoom();
        Move();
    }

    public void Zoom()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount == 2)
            {
                if (UIManager.instance.currentViewUI == UIManager.ViewUI.Main)
                {
                    if (Input.GetTouch( 0 ).phase == TouchPhase.Moved && Input.GetTouch( 1 ).phase == TouchPhase.Moved)
                    {
                        Touch touchZero = Input.GetTouch( 0 );
                        Touch touchOne = Input.GetTouch( 1 );

                        Vector2 touchZeroPastPos = touchZero.position - touchZero.deltaPosition;
                        Vector2 touchOnePastPos = touchOne.position - touchOne.deltaPosition;

                        float pastTouchMag = (touchZeroPastPos - touchOnePastPos).magnitude;
                        float touchMag = (touchZero.position - touchOne.position).magnitude;

                        float magnitudeDiff = pastTouchMag - touchMag;
                    }
                }
            }
        }
        else
        {
            if (Input.GetAxis( "Mouse ScrollWheel" ) > 0)
            {
                mainCamera.orthographicSize -= zoomSpeed * Time.deltaTime;              
            }
            else if (Input.GetAxis( "Mouse ScrollWheel" ) < 0)
            {
                mainCamera.orthographicSize += zoomSpeed * Time.deltaTime;
            }

            if (Input.GetAxis( "Mouse ScrollWheel" ) != 0)
            {
                mainCamera.orthographicSize = Mathf.Clamp( mainCamera.orthographicSize, zoomMax, zoomMin );

                height = mainCamera.orthographicSize;
                width = height * Screen.width / Screen.height;

                mY = size.y * 0.5f - height;
                clampY = Mathf.Clamp( transform.position.y, -mY + center.y, mY + center.y );

                transform.position = new Vector3( 0, clampY, -10 );
            }
        }
        
    }

    public void Move()
    {
        if (Application.platform == RuntimePlatform.Android)
        {

        }
        else
        {
            if (Input.GetMouseButton( 0 ))
            {
                float posY = Input.GetAxis( "Mouse Y" );

                transform.position += new Vector3( 0, posY ) * moveSpeed;

                mY = size.y * 0.5f - height;
                clampY = Mathf.Clamp( transform.position.y, -mY + center.y, mY + center.y );

                transform.position = new Vector3( 0, clampY, -10 );
            }
        }  
    }

    //가구 배치일 때는 
    public void SetFurnitureDispose()
    {
        mainCamera.orthographicSize = zoomMax;

        height = mainCamera.orthographicSize;
        width = height * Screen.width / Screen.height;

        mY = size.y * 0.5f - height;
        clampY = Mathf.Clamp( transform.position.y, -mY + center.y, mY + center.y );

        transform.position = new Vector3( 0, clampY, -10 );
    }

    public void ResetFurnitureDispose()
    {
        mainCamera.orthographicSize = zoomMin;

        height = mainCamera.orthographicSize;
        width = height * Screen.width / Screen.height;

        mY = size.y * 0.5f - height;
        clampY = Mathf.Clamp( transform.position.y, -mY + center.y, mY + center.y );

        transform.position = new Vector3( 0, clampY, -10 );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube( center, size );
    }
}
