using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCatHouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(UIManager.instance.scale > 1)
        {
            transform.localScale *= UIManager.instance.multiple;
            transform.position = new Vector2( 0, (transform.lossyScale.y - 1) / 2 * 10);
        }

    }

   
}
