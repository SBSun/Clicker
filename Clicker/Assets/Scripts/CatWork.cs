using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWork : MonoBehaviour
{
    public CatSlot catSlot;

    public Coroutine coroutine;

    private void Start()
    {
        if(catSlot.slotStatus == CatSlot.SlotStatus.Open)
        {
            StartCatWork();
        }
    }

    public void StartCatWork()
    {
        coroutine = StartCoroutine( CatWorkCoroutine() );
    }

    public IEnumerator CatWorkCoroutine()
    {
        while(true)
        {

            yield return null;
        }
    }
}
