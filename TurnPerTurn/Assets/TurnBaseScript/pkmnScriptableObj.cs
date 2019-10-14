using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon 1", menuName = "ScriptableObjects/PkmnScriptableObject", order = 1)]
public class pkmnScriptableObj : ScriptableObject
{
    public int hp = 0;
    public int speed = 0;
    public int atk = 0;
    public int def = 0;
    public string ability1;
    public string ability2;
    public string abilitySwitch;
}
