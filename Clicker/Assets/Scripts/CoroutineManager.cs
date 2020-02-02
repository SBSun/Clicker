using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroCoroutine
{
    List<IEnumerator> _coroutines = new List<IEnumerator>();

    public void AddCoroutine( IEnumerator enumerator )
    {
        _coroutines.Add( enumerator );
    }

    public void Run()
    {
        int i = 0;
        while (i < _coroutines.Count)
        {
            if (!_coroutines[i].MoveNext())
            {
                _coroutines.RemoveAt( i );
                continue;
            }
            i++;
        }
    }
}

public class CoroutineManager : MonoBehaviour
{
    #region Singleton

    private static CoroutineManager m_instance;

    public static CoroutineManager instance
    {
        get
        {
            if (m_instance != null)
                return m_instance;

            m_instance = FindObjectOfType<CoroutineManager>();

            if (m_instance == null)
                m_instance = new GameObject( name: "CoroutineManager" ).AddComponent<CoroutineManager>();

            return m_instance;
        }
    }

    #endregion

    MicroCoroutine updateMicroCoroutine = new MicroCoroutine();
    MicroCoroutine fixedUpdateMicroCoroutine = new MicroCoroutine();
    MicroCoroutine endOfFrameMicroCoroutine = new MicroCoroutine();

    public void StartUpdateCoroutine( IEnumerator routine )
    {
        updateMicroCoroutine.AddCoroutine( routine );
        Debug.Log( "방가" );
    }

    public void StartFixedUpdateCoroutine( IEnumerator routine )
    {
        fixedUpdateMicroCoroutine.AddCoroutine( routine );
    }

    public void StartEndOfFrameCoroutine( IEnumerator routine )
    {
        endOfFrameMicroCoroutine.AddCoroutine( routine );
    }

    void Awake()
    {
        StartCoroutine( RunUpdateMicroCoroutine() );
        StartCoroutine( RunFixedUpdateMicroCoroutine() );
        StartCoroutine( RunEndOfFrameMicroCoroutine() );
        StartUpdateCoroutine( SomeCoroutine() );
    }

    IEnumerator RunUpdateMicroCoroutine()
    {
        while (true)
        {
            yield return null;
            updateMicroCoroutine.Run();
        }
    }

    IEnumerator RunFixedUpdateMicroCoroutine()
    {
        var fu = new WaitForFixedUpdate();
        while (true)
        {
            yield return fu;
            fixedUpdateMicroCoroutine.Run();
        }
    }

    IEnumerator RunEndOfFrameMicroCoroutine()
    {
        var eof = new WaitForEndOfFrame();
        while (true)
        {
            yield return eof;
            endOfFrameMicroCoroutine.Run();
        }
    }

    IEnumerator SomeCoroutine()
    {
        Debug.Log( "하이루1" );

        yield return new WaitForSeconds( 10f );

        Debug.Log( "하이루2" );

        yield return new WaitForSeconds( 10f );

        Debug.Log( "하이루3" );

        yield return new WaitForSeconds( 10f );

        Debug.Log( "하이루4" );

        yield return null;
    }
}
