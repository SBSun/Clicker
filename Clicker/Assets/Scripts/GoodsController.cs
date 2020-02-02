using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsController : MonoBehaviour
{
    private string[] goldListName = new string[] {"Gold", "GoldA", "GoldB", "GoldC", "GoldD", "GoldE",
                                                    "GoldF", "GoldG", "GoldH", "GoldI"};

    public string[] unit = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I" };

    public List<int> goldList;
    public int carat; //보석

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
    public void AddGold( List<int> operandGoldList, List<int> addGoldList )
    {
        if (addGoldList.Count != 10)
            return;

        for (int i = 0; i < operandGoldList.Count; i++)
        {
            //1000 이상이면
            if (operandGoldList[i] + addGoldList[i] >= 1000)
            {
                //다음 자리수에 1을 더해준다.
                operandGoldList[i + 1]++;
                //현재 자리수에는 나머지 값을 대입해준다.
                int remainder = operandGoldList[i] + addGoldList[i] - 1000;
                operandGoldList[i] = remainder;
            }
            else
            {
                operandGoldList[i] += addGoldList[i];
            }
        }
    }

    public void SubGold( List<int> operandGoldList, List<int> subGoldList )
    {
        if(!SubGoldCheck( operandGoldList, subGoldList ))
            return;

        for (int i = operandGoldList.Count - 1; i >= 0; i--)
        {
            if (operandGoldList[i] == 0)
                continue;

            for (int j = i; j >= 0; j--)
            {
                if (operandGoldList[j] - subGoldList[j] >= 0)
                {
                    operandGoldList[j] -= subGoldList[j];
                }
                else
                {
                    int remainder = 1000 - ((operandGoldList[j] - subGoldList[j]) * -1);

                    if ((j + 1) <= operandGoldList.Count - 1 && operandGoldList[j + 1] > 0)
                    {
                        operandGoldList[j + 1]--;
                    }

                    operandGoldList[j] = remainder;
                }
            }
            break;
        }
    }

    public bool SubGoldCheck( List<int> operandGoldList, List<int> subGoldCheckList )
    {
        bool isBuyPossible = false;

        for (int i = operandGoldList.Count - 1; i >= 0; i--)
        {
            if (operandGoldList[i] == 0 && subGoldCheckList[i] == 0)
            {
                Debug.Log( "둘다 0" );
                continue;
            }
            //현재 단위에서 보유하고 있는 골드가 크거나 같다면
            else if (operandGoldList[i] > subGoldCheckList[i])
            {
                Debug.Log( "구입 가능" );
                isBuyPossible = true;
                break;
            }
            //해당 자리수가 같으면
            else if (operandGoldList[i] == subGoldCheckList[i])
            {
                if (i == 0)
                {
                    //보유하고 있는 골드와 subGold 양이 같으면 
                    Debug.Log( "구입 가능" );
                    isBuyPossible = true;
                    break;
                }

                for (int j = i - 1; j >= 0; j--)
                {
                    if (operandGoldList[j] > subGoldCheckList[j])
                    {
                        Debug.Log( "구입 가능" );
                        isBuyPossible = true;
                        break;
                    }
                    else if (operandGoldList[j] == subGoldCheckList[j])
                    {
                        //아직 다 구현 안함
                        //끝이면 양이 같으므로 구입 가능
                        if (j == 0)
                        {
                            isBuyPossible = true;
                            Debug.Log( "구입 가능" );
                            break;
                        }

                        continue;
                    }
                }

                break;
            }
            else if (subGoldCheckList[i] > goldList[i])
            {
                Debug.Log( "골드 부족" );
                isBuyPossible = false;
            }
        }

        return isBuyPossible;
    }
}