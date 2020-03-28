using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Camera mainCamera;

    public Coroutine cameraZoomCoroutine;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Start()
    {
        height = mainCamera.orthographicSize; //카메라의 세로 절반 길이
        width = height * Screen.width / Screen.height; //카메라의 가로 절반 길이
        size = new Vector2( width * 2, height * 2 );
    }

    void Update()
    {
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

                float mY = size.y * 0.5f - height;
                float clampY = Mathf.Clamp( transform.position.y, -mY + center.y, mY + center.y );

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

                float mY = size.y * 0.5f - height;
                float clampY = Mathf.Clamp( transform.position.y, -mY + center.y, mY + center.y );

                transform.position = new Vector3( 0, clampY, -10 );
            }
        }  
    }

    //가구 배치 상태일 때 실행
    public void SetCameraZoomMax(Transform floorPos)
    {
        cameraZoomCoroutine = StartCoroutine( CameraSmoothZoom( floorPos ) );
    }

    public IEnumerator CameraSmoothZoom( Transform floorPos )
    {
        isZooming = true;

        float time = 0f;

        Vector3 cameraPos = transform.position;
        float orthographicSize = mainCamera.orthographicSize;

        while (time < 0.3f)
        {
            time += Time.deltaTime;
            
            transform.position = Vector3.Lerp( cameraPos, floorPos.position, time / 0.3f );
            transform.position = new Vector3( transform.position.x, transform.position.y, -10 );
            mainCamera.orthographicSize = Mathf.Lerp( orthographicSize, zoomMax, time / 0.3f );
            Debug.Log( time + ", " + mainCamera.orthographicSize );


            yield return null;
        }

        //UIManager.instance.simpleCatInformationUI.SetInformation( floorPos.GetComponent<FloorInformation>().catConsume  );

        isZooming = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube( center, size );
    }
}
