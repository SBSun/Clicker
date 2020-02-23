using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using LitJson;

public class BackEndCustom : MonoBehaviour
{
    public InputField id_InputField;
    public InputField pw_InputField;

    public GameObject go_LoginUI;
   
    public void OnClickSignUp()
    {
        BackendReturnObject BRO = Backend.BMember.CustomSignUp( id_InputField.text, pw_InputField.text );

        if(BRO.IsSuccess())
        {
            Debug.Log( "회원가입 완료" );

            BackEndManager.instance.backEndDataSave.InsertGachaSystemData();
            BackEndManager.instance.backEndDataSave.InsertFurnitureItemData();
        }
        else
        {
            string error = BRO.GetStatusCode();

            switch (error)
            {
                case "409":
                    Debug.Log( "중복된 CustomId가 존재합니다." );
                    break;

                default:
                    Debug.Log( "서버 공통 에러 발생 - " + BRO.GetMessage() );
                    break;
            }
        }  
    }

    public void OnClickLogin()
    {
        BackendReturnObject BRO = Backend.BMember.CustomLogin( id_InputField.text, pw_InputField.text );

        if (BRO.IsSuccess())
        {
            Debug.Log( "로그인 완료" );
            BackEndManager.instance.backEndDataSave.GetGachaSystemData();
            BackEndManager.instance.backEndDataSave.GetFurnitureItemData();
            go_LoginUI.SetActive( false );
        }
        else
        {
            string error = BRO.GetStatusCode();

            switch (error)
            {
                case "401":
                    Debug.Log( "아이디가 존재하지 않거나 비밀번호가 틀렸습니다." );
                    break;

                case "403":
                    Debug.Log( "차단당한 Id입니다." );
                    break;

                default:
                    Debug.Log( "서버 공통 에러 발생 - " + BRO.GetMessage() );
                    break;
            }
        }
    } 
}
