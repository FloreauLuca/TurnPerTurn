using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manyFunctions : MonoBehaviour
{
    private int criticalHitModifier = 1;
    private float typeModifier;
    public pkmnScriptObj actualPkmn;
    public pkmnScriptObj opponentPkmn;
    public abilityScriptObj actualAbility;
    

    void checkType()
    {
        switch (actualAbility.type)
        {
            case Type.FIRE:
                if (opponentPkmn.pkmnType == Type.PLANT)
                {
                    typeModifier = 2.0f;
                }
                else if (opponentPkmn.pkmnType == Type.WATER)
                {
                    typeModifier = 0.5f;
                }
                else
                {
                    typeModifier = 1.0f;
                }
                break;
            case Type.WATER:
                if (opponentPkmn.pkmnType == Type.FIRE || opponentPkmn.pkmnType == Type.GROUND)
                {
                    typeModifier = 2.0f;
                }
                else if (opponentPkmn.pkmnType == Type.ELEKTRIK || opponentPkmn.pkmnType == Type.PLANT)
                {
                    typeModifier = 0.5f;
                }
                else
                {
                    typeModifier = 1.0f;
                }
                break;
            case Type.PLANT:
                if (opponentPkmn.pkmnType == Type.WATER || opponentPkmn.pkmnType == Type.GROUND)
                {
                    typeModifier = 2.0f;
                }
                else if (opponentPkmn.pkmnType == Type.FIRE || opponentPkmn.pkmnType == Type.WIND)
                {
                    typeModifier = 0.5f;
                }
                else
                {
                    typeModifier = 1.0f;
                }
                break;
            case Type.ELEKTRIK:
                if (opponentPkmn.pkmnType == Type.WATER || opponentPkmn.pkmnType == Type.WIND)
                {
                    typeModifier = 2.0f;
                }
                else if (opponentPkmn.pkmnType == Type.PLANT)
                {
                    typeModifier = 0.5f;
                }
                else if (opponentPkmn.pkmnType == Type.GROUND)
                {
                    typeModifier = 0.0f;
                }
                else
                {
                    typeModifier = 1.0f;
                }
                break;
            case Type.GROUND:
                if (opponentPkmn.pkmnType == Type.FIRE || opponentPkmn.pkmnType == Type.ELEKTRIK)
                {
                    typeModifier = 2.0f;
                }
                else if (opponentPkmn.pkmnType == Type.WATER)
                {
                    typeModifier = 0.5f;
                }
                else if (opponentPkmn.pkmnType == Type.WIND)
                {
                    typeModifier = 0.0f;
                }
                else
                {
                    typeModifier = 1.0f;
                }
                break;
            case Type.WIND:
                if (opponentPkmn.pkmnType == Type.PLANT)
                {
                    typeModifier = 2.0f;
                }
                else if (opponentPkmn.pkmnType == Type.ELEKTRIK)
                {
                    typeModifier = 0.5f;
                }
                else
                {
                    typeModifier = 1.0f;
                }
                break;
            default:
                break;
        }
    }

    void damageCalculation()
    {
        float lostHp;
        lostHp = (actualPkmn.atk - opponentPkmn.def) * criticalHitModifier * typeModifier;
        opponentPkmn.hp = opponentPkmn.hp - lostHp;
        criticalHitModifier = 1;
        typeModifier = 1.0f;
    }

    void checkCriticalHit()
    {
        int randomNmb = Random.Range(1, 101);

        if (randomNmb < 15)
        {
            criticalHitModifier = 2;
        }
    }

    void pkmnSwitch()
    {
        //get pkmnTeamList instance and change actualPkmn by selectedPkmn;
    }
    private void Start()
    {
        checkCriticalHit();
        checkCriticalHit();
        checkCriticalHit();
        checkCriticalHit();
        checkCriticalHit();
    }
}
