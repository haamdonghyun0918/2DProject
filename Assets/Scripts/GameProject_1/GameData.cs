using System;
using System.Collections.Generic;

// json파일이 공통으로 가지고 있는 부문: ID
[System.Serializable]
public class GameDataBase
{
    public string Id;
}

// Character.json 파일에 있는 데이터들의 열 이름들 작성 (공통에 있는 것은 제외)
[System.Serializable]
public class CharacterData : GameDataBase
{
    public string Name;
    public string[] Card;
    public int Hp;
    public string CharacterAnimAddress;
    public string CharacterImageAddress;
    public string CharacterImageSpriteName;
}

// Card.json 파일에 있는 데이터들의 열 이름들 작성 (공통에 있는 것은 제외)
[System.Serializable]
public class CardData : GameDataBase
{
    public string Name;
    public int Damage;
    public int Heal;
    public int Bleed;
    public string Description;
    public string ImageIconAddress;
    public string ImageCardAddress;
    public string ImageDamageAddress;
}

//Monster.json 파일에 있는 데이터들의 열 이름들 작성 (공통에 있는 것은 제외)
[System.Serializable]
public class MonsterData : GameDataBase
{
    public string Name;
    public string MonsterAddress;
    public string MonsterSpriteName;
    public string MonsterAnim;
    public int MonsterHp;
    public int MonsterAtk;
}

//Map.json 파일에 있는 데이터들의 열 이름들 작성 (공통에 있는 것은 제외)
[System.Serializable]
public class MapData : GameDataBase
{
    public string[] Monster;
    public string BossMonster;
    public string MapImageAddress;
}

//ExampleData.json 파일에 있는 데이터들의 열 이름들 작성 (공통에 있는 것은 제외)
[System.Serializable]
public class ExampleData : GameDataBase
{
    public string Image;
    public string Text;
}