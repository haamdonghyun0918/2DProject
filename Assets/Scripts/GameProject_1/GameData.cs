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
    public string characterId;
    public string weaponId;
    public int hp;
    public int att;
    public int def;
}

public class WeaponData : GameDataBase
{
    public string weaponId;
    public string characterId;
}