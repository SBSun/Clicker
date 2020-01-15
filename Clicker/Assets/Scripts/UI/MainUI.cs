using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public GameObject go_CatInventory;

    public void OnClickCatInventory()
    {
        go_CatInventory.SetActive( true );
    }
}
