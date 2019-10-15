using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status { HEAL, DAMAGE, BOOST };

[CreateAssetMenu(fileName = "ability 1", menuName = "ScriptableObjects/AbilityScriptableObject", order = 2)]
public class abilityScriptObj : ScriptableObject
{
    public string abilityName;
    public Type type;
    public Status status;
}
