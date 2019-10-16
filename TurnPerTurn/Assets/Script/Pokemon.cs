using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Action
{


    public enum Type
    {
        ATTAQUE,
        SOIN,
        DEF,
        FREEZE,
        POISON,
        SWITCH,
        NONE
    }

    public Type type = Type.NONE;
    public float value = 0;
    public float currentPokemonID;
    public float currentPV;
    public byte Id { get; set; }

    public static object Deserialize(byte[] data)
    {
        var result = new Action();
        result.Id = data[0];
        result.type = (Action.Type)data[1];
        result.value = data[2];
        result.currentPokemonID = data[3];
        result.currentPV = data[4];
        return result;
    }

    public static byte[] Serialize(object customType)
    {
        var c = (Action)customType;
        return new byte[] { c.Id, (byte)c.type, (byte)c.value, (byte)c.currentPokemonID, (byte)c.currentPokemonID};
    }
}

[CreateAssetMenu]
[Serializable]
public class Pokemon : ScriptableObject
{
    public Action[] listActions = new Action[3];
    public float maxpv;
    public float speed;
    public string name;
    public string type;
}