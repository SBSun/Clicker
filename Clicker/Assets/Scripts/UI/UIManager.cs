using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager m_instance;

    public static UIManager instance
    {
        get
        {
            if (m_instance != null)
                return m_instance;

            m_instance = FindObjectOfType<UIManager>();

            if (m_instance == null)
                m_instance = new GameObject( name: "UIManager" ).AddComponent<UIManager>();

            return m_instance;
        }
    }

    public Text gold_Text;

    string[] units = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I" };

    public CatInformation catInformation;

    public CatLevelUpButton catLevelButton;

    public void UpdateGoldText(List<int> newGoldList, Text newShowText)
    {
        string str;
 
        for (int i = newGoldList.Count - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                newShowText.text = newGoldList[0].ToString() + units[0];
            }
            else
            {
                if (newGoldList[i - 1] == 0)
                    str = string.Format( "{0}{1}", newGoldList[i], units[i] );
                else
                    str = string.Format( "{0}.{1}{2}", newGoldList[i], newGoldList[i - 1], units[i] );
                newShowText.text = str;
            }
        }
    }
}
