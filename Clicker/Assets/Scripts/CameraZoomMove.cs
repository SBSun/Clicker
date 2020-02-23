using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomMove : MonoBehaviour
{
    public float zoomSpeed = 0f;
    public float zoomMax = 0f;
    public float zoomMin = 0f;

    public Vector2 center; //중심점
    public Vector2 size; //크기

    float width;
    float height;
    // Start is called before the first frame update
    void Start()
    {
        height = Camera.main.orthographicSize; //카메라의 세로 절반 길이
        width = height * Screen.width / Screen.height; //카메라의 가로 절반 길이
        size = new Vector2( width * 2, height * 2 );
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        Move();
    }

    public void Zoom()
    {
        if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetAxis( "Mouse ScrollWheel" ) > 0)
            {
                Camera.main.orthographicSize -= zoomSpeed * Time.deltaTime;
            }
            else if (Input.GetAxis( "Mouse ScrollWheel" ) < 0)
            {
                Camera.main.orthographicSize += zoomSpeed * Time.deltaTime;
            }

            if (Input.GetAxis( "Mouse ScrollWheel" ) != 0)
            {
                Camera.main.orthographicSize = Mathf.Clamp( Camera.main.orthographicSize, zoomMin, zoomMax );

                float mX = size.x * 0.5f - width;
                float clampX = Mathf.Clamp( transform.position.x, -mX + center.x, mX + center.x );

                float mY = size.y * 0.5f - height;
                float clampY = Mathf.Clamp( transform.position.y, -mY + center.y, mY + center.y );

                transform.position = new Vector3( clampX, clampY, -10 );
            }
        }
        else if(Application.platform == RuntimePlatform.Android)
        {
            if(Input.touchCount == 2)
            {
                if(UIManager.instance.currentViewUI == UIManager.ViewUI.Main)
                {
                    if (Input.GetTouch( 0 ).phase == TouchPhase.Moved && Input.GetTouch( 1 ).phase == TouchPhase.Moved)
                    {

                    }
                }
            }
        }
    }

    public void Move()
    {
        if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetMouseButton( 0 ))
            {
                Debug.Log( "마우스 누르고있음" );

                float posX = Input.GetAxis( "Mouse X" );
                float posY = Input.GetAxis( "Mouse Y" );

                transform.position += new Vector3( posX, posY );

                float mX = size.x * 0.5f - width;
                float clampX = Mathf.Clamp( transform.position.x, -mX + center.x, mX + center.x );

                float mY = size.y * 0.5f - height;
                float clampY = Mathf.Clamp( transform.position.y, -mY + center.y, mY + center.y );

                transform.position = new Vector3( clampX, clampY, -10 );
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube( center, size );
    }
}
