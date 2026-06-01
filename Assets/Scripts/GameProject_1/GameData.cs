using System;
using System.Collections.Generic;

[System.Serializable]
public class GameDataBase
{
    public string Id;
}

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

[System.Serializable]
public class MapData : GameDataBase
{
    public string[] Monster;
    public string MapImageAddress;
}