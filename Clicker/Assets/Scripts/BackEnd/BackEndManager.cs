using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using BackEnd;
using LitJson;

public class BackEndManager : MonoBehaviour
{
    private static BackEndManager m_instance;

    public static BackEndManager instance
    {
        get
        {
            if (m_instance != null)
                return m_instance;

            m_instance = FindObjectOfType<BackEndManager>();

            if (m_instance == null)
                m_instance = new GameObject( name: "BackEndManager" ).AddComponent<BackEndManager>();

            return m_instance;
        }
    }

    public BackEndDataSave backEndDataSave;
    public BackEndCustom backEndCustom;

    /*
    private void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode( false )
            .RequestEmail() //이메일 요청
            .RequestIdToken() //토큰 요청
            .Build();

        PlayGamesPlatform.InitializeInstance( config );
        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();
        GoogleLogIn();
    }

    public void GoogleLogIn()
    {
        if(!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Social.localUser.Authenticate( ( bool success ) =>
            {
                if (!success)
                {
                    Debug.Log( "구글 로그인 실패" );
                    return;
                }

                Debug.Log( "GetIdToken - " + PlayGamesPlatform.Instance.GetIdToken() );
                Debug.Log( "Email - " + ((PlayGamesLocalUser)Social.localUser).Email );
                Debug.Log( "GoogleId - " + Social.localUser.id );
                Debug.Log( "UserName - " + Social.localUser.userName );

                BackEndServerLogIn();
            } );
        }
    }

    
    private string GetToken()
    {
        if(PlayGamesPlatform.Instance.localUser.authenticated)
        {
            //유저 토큰 받기
            string idToken = PlayGamesPlatform.Instance.GetIdToken();

            return idToken;
        }
        else
        {
            Debug.Log( "접속되어있지 않습니다. 잠시 후 다시 시도하세요." );
            GoogleLogIn();
            return null;
        }
    }

        
    public void BackEndServerLogIn()
    {
        //구글 로그인이 성공했다면 뒤끝 서버에 가입 요청
        BackendReturnObject BRO = Backend.BMember.AuthorizeFederation( GetToken(), FederationType.Google, "gpgs로 만든 계정" );

        if(BRO.IsSuccess())
        {
            Debug.Log( "구글 토큰으로 뒤끝서버 로그인 성공" );
            backEndDataSave.GetGachaSystemData();
            backEndDataSave.GetFurnitureItemData();
        }
        else
        {
            switch(BRO.GetStatusCode())
            {
                case "200":
                    Debug.Log( "이미 회원가입된 회원" );
                    break;

                case "403":
                    Debug.Log( "차단된 사용자 입니다. 차단 사유 - " + BRO.GetErrorCode() );
                    break;

                default:
                    Debug.Log( "서버 공통 에러 발생 - " + BRO.GetMessage() );
                    break;
            }
        }
    }*/
}

