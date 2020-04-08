using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsController : MonoBehaviour
{
    private string[] goldListName = new string[] {"Gold", "GoldA", "GoldB", "GoldC", "GoldD", "GoldE",
                                                    "GoldF", "GoldG", "GoldH", "GoldI"};

    public string[] unit = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I" };

    public List<int> goldList = new List<int>();
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
        UIManager.instance.UpdateGoldText();
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

        UIManager.instance.UpdateGoldText();
    }

    public void SubGold( List<int> operandGoldList, List<int> subGoldList )
    {
        if(!SubGoldCheck( operandGoldList, subGoldList ))
            return;

        for (int i = operandGoldList.Count - 1; i >= 0; i--)
        {
            //골드가 존재하지 않는 원소라면 다시 반복
            if (operandGoldList[i] == 0)
                continue;

            //제일 높은 단위부터 제일 낮은 단위까지 계산
            for (int j = i; j >= 0; j--)
            {
                //보유하고 있는 골드의 j번째 단위에서 빼려고 하는 골드의 j번째 단위가 양수라면 뺀다.
                if (operandGoldList[j] - subGoldList[j] >= 0)
                {
                    operandGoldList[j] -= subGoldList[j];
                }
                //음수이면 j + 1 번째 단위에서 1을 뺀다.
                else
                {
                    int remainder = 1000 - ((operandGoldList[j] - subGoldList[j]) * -1);

                    if ((j + 1) <= operandGoldList.Count - 1)
                    {
                        if (operandGoldList[j + 1] > 0)
                        {
                            operandGoldList[j + 1]--;

                            operandGoldList[j] = remainder;
                        }
                        else
                        {
                            for (int k = j + 2; k < i + 1; i++)
                            {
                                if(operandGoldList[k] > 0)
                                {
                                    operandGoldList[k]--;

                                    for (int n = k-1; n > j; n--)
                                    {
                                        operandGoldList[n] = 999;
                                    }

                                    operandGoldList[j] = remainder;

                                    break;
                                }
                            }
                        }
                    }

                    
                }
            }
            break;
        }

        UIManager.instance.UpdateGoldText();
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
                break;
            }
        }

        return isBuyPossible;
    }
}