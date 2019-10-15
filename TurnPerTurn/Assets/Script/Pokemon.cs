﻿using System;
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
    public int currentPokemonID;
    public int currentPV;
    public byte Id { get; set; }

    public static object Deserialize(byte[] data)
    {
        var result = new Action();
        result.Id = data[0];
        result.type = (Action.Type)data[2];
        result.value = data[7];
        result.currentPokemonID = data[13];
        result.currentPV = data[18];
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