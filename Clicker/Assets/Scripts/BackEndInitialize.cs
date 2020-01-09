using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackEndInitialize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Backend.Initialize( Bro =>
         {
             Debug.Log( "뒤끝 초기화 진행 " + Bro );

             if (Bro.IsSuccess())
             {
                 Debug.Log( Backend.Utils.GetGoogleHash() );
             }
             else
             {
                 Debug.Log( "초기화 실패 : " + Bro.GetErrorCode() );
             }
         } );
    }

}
