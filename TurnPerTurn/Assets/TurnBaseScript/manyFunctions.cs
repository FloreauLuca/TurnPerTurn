using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manyFunctions : MonoBehaviour
{
    private int poisonDamage = 0;
    private bool poisoned = false;
    private bool blocked = false;
    private int missHitModifier = 1;
    private int criticalHitModifier = 1;
    private float typeModifier = 1.0f;
    public pkmnScriptObj actualPkmn;
    public pkmnScriptObj opponentPkmn;
    public abilityScriptObj actualAbility;
    

    public void checkType(PokeType abilitiy, PokeType opponent)
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
        if (poisoned)
        {
            poisonDamage = 5;
        }

        if (blocked)
        {
            missHitModifier = 0;
        }
        float lostHp;
        lostHp = (actualPkmn.atk - opponentPkmn.def) * criticalHitModifier * typeModifier * missHitModifier;
        opponentPkmn.hp = opponentPkmn.hp - lostHp - poisonDamage;
        criticalHitModifier = 1;
        typeModifier = 1.0f;
        missHitModifier = 1;
        poisonDamage = 0;
    }

    void checkCriticalHit()
    {
        int randomNmb = Random.Range(1, 101);

        if (randomNmb < 15)
        {
            criticalHitModifier = 2;
        }
    }

    void addPkmnStats()
    {
        actualPkmn.hp = Random.Range(20.0f, 30.0f);
        actualPkmn.speed = Random.Range(5.0f, 10.0f);
        actualPkmn.atk = Random.Range(5.0f, 8.0f);
        actualPkmn.def = Random.Range(10.0f, 15.0f);
    }

    void checkMissHit()
    {
        int randomMissNmb = Random.Range(1, 101);

        if (randomMissNmb < 50)
        {
            missHitModifier = 0;
        }
    }

    void pkmnSwitch()
    {
        //get pkmnTeamList instance and change actualPkmn by selectedPkmn;
    }



}
