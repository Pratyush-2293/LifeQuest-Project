using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat2D : MonoBehaviour
{
    public int turnCounter = 30;
    public int turnSpeed = 2;
    public bool isPlayerCharacter = true;
    public string characterName = "Alden";
    public AldenCombat2D aldenCombatController = null;
    public ValricCombat2D valricCombatController = null;
    public OsmirCombat2D osmirCombatController = null;
    public AssassinCombat2D assassinCombatController = null;

    public void DoAction(string actionName, int targetPosition)
    {
        if(actionName == "Attack")
        {
            Action_Attack(targetPosition);
        }
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
            aldenCombatController.AldenAttack(targetPosition);
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
}
