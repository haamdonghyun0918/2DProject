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
}

public class CardData : GameDataBase
{
    public string Name;
    public int Damage;
    public string Description;
    public string Address;
}