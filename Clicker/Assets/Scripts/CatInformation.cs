using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CatInformation", menuName = "Create CatInformation")]
public class CatInformation : ScriptableObject
{
    public enum CatClass
    {
        Five,
        Four,
        Three,
        Two
    }
    public CatClass catClass;


    //고양이 이미지
    public Sprite catSprite;
    //고양이의 이름
    public string catName;
    //고양이 직업
    public string job;
    //고양이 소개
    public string introduction;
}
