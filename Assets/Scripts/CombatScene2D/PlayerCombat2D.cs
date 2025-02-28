using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat2D : MonoBehaviour
{
    public int turnCounter = 30;
    public int turnSpeed = 2;
    public bool isPlayerCharacter = true;
    public string characterName = "Alden";
    public int characterPosition = 1;
    public AldenCombat2D aldenCombatController = null;
    public ValricCombat2D valricCombatController = null;
    public OsmirCombat2D osmirCombatController = null;
    public AssassinCombat2D assassinCombatController = null;
    public bool isDefeated = false;

    public void DoAction(string actionName, int targetPosition)
    {
        if(actionName == "Attack")
        {
            Action_Attack(targetPosition);
        }
        else if(actionName == "Defend")
        {
            Action_Defend();
        }
    }

    private void AddTimeCost(int timeCost)
    {
        turnCounter = 100 - timeCost;
    }

    public int GetHealthValue()
    {
        if (characterName == "Alden")
        {
            return aldenCombatController.health;
        }
        else if(characterName == "Valric")
        {
            // do the same as above
        }
        else if(characterName == "Osmir")
        {
            // do the same as above
        }
        else if(characterName == "Assassin")
        {
            // do the same as above
        }


        return -1;
    }

    public int GetMaxHealthValue()
    {
        if (characterName == "Alden")
        {
            return aldenCombatController.maxHealth;
        }
        else if (characterName == "Valric")
        {
            // do the same as above
        }
        else if (characterName == "Osmir")
        {
            // do the same as above
        }
        else if (characterName == "Assassin")
        {
            // do the same as above
        }


        return -1;
    }

    public int GetManaValue()
    {
        if (characterName == "Alden")
        {
            return aldenCombatController.mana;
        }
        else if (characterName == "Valric")
        {
            // do the same as above
        }
        else if (characterName == "Osmir")
        {
            // do the same as above
        }
        else if (characterName == "Assassin")
        {
            // do the same as above
        }


        return -1;
    }

    public void Action_Attack(int targetPosition)
    {
        if(characterName == "Alden")
        {
            aldenCombatController.AldenAttack(targetPosition, characterPosition);
            AddTimeCost(aldenCombatController.attackTimeCost);
        }
        else if(characterName == "Valric")
        {
            // same as above
        }
        else if(characterName == "Osmir")
        {
            // same as above
        }
        else if(characterName == "Assassin")
        {
            // same as above
        }
    }

    public void Action_Defend()
    {
        if (characterName == "Alden")
        {
            aldenCombatController.AldenDefend();
            AddTimeCost(aldenCombatController.defendTimeCost);
        }
        else if (characterName == "Valric")
        {
            // same as above
        }
        else if (characterName == "Osmir")
        {
            // same as above
        }
        else if (characterName == "Assassin")
        {
            // same as above
        }
    }

    public void Action_TakeDamage(int incomingDamage)
    {
        if (characterName == "Alden")
        {
            aldenCombatController.AldenTakeDamage(incomingDamage);
        }
        else if (characterName == "Valric")
        {
            // same as above
        }
        else if (characterName == "Osmir")
        {
            // same as above
        }
        else if (characterName == "Assassin")
        {
            // same as above
        }
    }
}
