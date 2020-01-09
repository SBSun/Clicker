using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsController : MonoBehaviour
{
    private string[] goldListName = new string[] {"Gold", "GoldA", "GoldB", "GoldC", "GoldD", "GoldE",
                                                    "GoldF", "GoldG", "GoldH", "GoldI"};

    public string[] unit = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I" };

    public List<int> goldList;

    public bool isBuyPossible = false;

    private static GoodsController m_instance;

    public static GoodsController instance
    {
        get
        {
            if (m_instance != null)
                return m_instance;

            m_instance = FindObjectOfType<GoodsController>();

            if (m_instance == null)
                m_instance = new GameObject( name: "GoodsController" ).AddComponent<GoodsController>();

            return m_instance;
        }
    }

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            goldList.Add( 0 );
        }
    }

    //리스트를 받아서 
    public void AddGold( List<int> addGoldList )
    {
        if (addGoldList.Count != 10)
            return;

        for (int i = 0; i < goldList.Count; i++)
        {
            //1000 이상이면
            if (goldList[i] + addGoldList[i] >= 1000)
            {
                //다음 자리수에 1을 더해준다.
                goldList[i + 1]++;
                //현재 자리수에는 나머지 값을 대입해준다.
                int remainder = goldList[i] + addGoldList[i] - 1000;
                goldList[i] = remainder;
            }
            else
            {
                goldList[i] += addGoldList[i];
            }
        }
        UIManager.instance.UpdateGoldText(goldList, UIManager.instance.gold_Text);
        Debug.Log( "골드 추가" );
    }

    public void SubGold( List<int> subGoldList )
    {
        SubGoldCheck( subGoldList );

        if (!isBuyPossible)
            return;

        for (int i = goldList.Count - 1; i >= 0; i--)
        {
            if (goldList[i] == 0)
                continue;

            for (int j = i; j >= 0; j--)
            {
                if (goldList[j] - subGoldList[j] >= 0)
                {
                    goldList[j] -= subGoldList[j];
                }
                else
                {
                    int remainder = 1000 - ((goldList[j] - subGoldList[j]) * -1);

                    if ((j + 1) <= goldList.Count - 1 && goldList[j + 1] > 0)
                    {
                        goldList[j + 1]--;
                    }

                    goldList[j] = remainder;
                }
            }

            break;
        }

        UIManager.instance.UpdateGoldText( goldList, UIManager.instance.gold_Text );
        Debug.Log( "골드 빼기" );
    }

    public void SubGoldCheck( List<int> subGoldCheckList )
    {
        for (int i = goldList.Count - 1; i >= 0; i--)
        {
            if (goldList[i] == 0 && subGoldCheckList[i] == 0)
            {
                Debug.Log( "둘다 0" );
                continue;
            }
            //현재 단위에서 보유하고 있는 골드가 크거나 같다면
            else if (goldList[i] > subGoldCheckList[i])
            {
                Debug.Log( "구입 가능" );
                isBuyPossible = true;
                break;
            }
            else if (goldList[i] == subGoldCheckList[i])
            {
                if(i == 0)
                {
                    Debug.Log( "구입 가능" );
                    isBuyPossible = true;
                    break;
                }

                for (int j = i - 1; j >= 0; j--)
                {
                    if (goldList[j] > subGoldCheckList[j])
                    {
                        Debug.Log( "구입 가능" );
                        isBuyPossible = true;
                        break;
                    }
                    else if (goldList[j] == subGoldCheckList[j])
                    {
                        //아직 다 구현 안함
                        if (j == 0)
                        {
                            isBuyPossible = true;
                            Debug.Log( "구입 가능" );
                        }

                        continue;
                    }
                }
            }
            else if (subGoldCheckList[i] > goldList[i])
            {
                Debug.Log( "골드 부족" );
                isBuyPossible = false;
            }
        }
    }
}
