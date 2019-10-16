using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type { FIRE, WATER, PLANT, WIND, GROUND, ELEKTRIK };

[CreateAssetMenu(fileName = "Pokemon 1", menuName = "ScriptableObjects/PkmnScriptableObject", order = 1)]
public class pkmnScriptObj : ScriptableObject
{  
    public float hp = 0.0f;
    public float speed = 0.0f;
    public float atk = 0.0f;
    public float def = 0.0f;
    public string pkmnName;
    public abilityScriptObj[] abilityList = new abilityScriptObj[3];
    public bool pkmnSwitchAbility;
    public Type pkmnType;
}
