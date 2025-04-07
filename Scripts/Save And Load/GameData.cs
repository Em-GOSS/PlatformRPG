using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int currency;
    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, int> stash;

    public GameData()
    {
        this.currency = 0;
        inventory = new SerializableDictionary<string, int>();
        stash = new SerializableDictionary<string, int>();
    }
}
