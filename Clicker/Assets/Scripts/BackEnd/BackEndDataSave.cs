using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackEndDataSave : MonoBehaviour
{
    public void OnClickInsertData()
    {
        int level = Random.Range( 1, 100 );
        int exp = Random.Range( 0, 100 );

        Param param = new Param();

        param.Add( "LEVEL", level);
        param.Add( "EXP", exp );
    }
}
