using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerCombat2D : MonoBehaviour
{
    public int turnCounter = 30;
    public int turnSpeed = 2;
    public int fastTurnSpeed = 0;
    public int slowTurnSpeed = 0;
    public bool isPlayerCharacter = true;
    public string characterName = "Alden";
    public int characterPosition = 1;
    public AldenCombat2D aldenCombatController = null;
    public ValricCombat2D valricCombatController = null;
    public OsmirCombat2D osmirCombatController = null;
    public AssassinCombat2D assassinCombatController = null;
    public bool isDefeated = false;

    private void Start()
    {
        fastTurnSpeed = turnSpeed + Mathf.CeilToInt(turnSpeed * 0.25f);
        slowTurnSpeed = turnSpeed - Mathf.CeilToInt(turnSpeed * 0.25f);
    }

    public void TickTurnCounter()
    {
        if (characterName == "Alden")
        {
            if(aldenCombatController.activeStatuses.Any(status => status.statusName == Status.StatusName.Haste))
            {
                turnCounter += fastTurnSpeed;
            }
            else if(aldenCombatController.activeStatuses.Any(status => status.statusName == Status.StatusName.Slowed))
            {
                turnCounter += slowTurnSpeed;
            }
            else
            {
                turnCounter += turnSpeed;
            }
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
    }

    public void TickStatuses()
    {
        if(characterName == "Alden")
        {
            foreach (Status status in aldenCombatController.activeStatuses)
            {
                status.statusCurrentDuration--;
            }

            aldenCombatController.activeStatuses.RemoveAll(status => status.statusCurrentDuration <= 0);
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
    }

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
        else if(actionName == "Skill1")
        {
            Action_Skill1(targetPosition);
        }
        else if (actionName == "Skill2")
        {
            Action_Skill2(targetPosition);
        }
        else if (actionName == "Skill3")
        {
            Action_Skill3(targetPosition);
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

    public void AddManaTick()
    {
        if (characterName == "Alden")
        {
            if (aldenCombatController.mana < 100)
            {
                if(aldenCombatController.activeStatuses.Any(status => status.statusName == Status.StatusName.Clarity))
                {
                    aldenCombatController.mana += 2;
                }
                else if(aldenCombatController.activeStatuses.Any(status => status.statusName == Status.StatusName.Confusion))
                {
                    aldenCombatController.mana += 0;
                }
                else
                {
                    aldenCombatController.mana++;
                }
            }
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

    public void Action_Skill1(int targetPosition)
    {
        if (characterName == "Alden")
        {
            aldenCombatController.AldenSkill1(targetPosition, characterPosition);
            AddTimeCost(aldenCombatController.skill1TimeCost);
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

    public void Action_Skill2(int targetPosition)
    {
        if (characterName == "Alden")
        {
            aldenCombatController.AldenSkill2(targetPosition, characterPosition);
            AddTimeCost(aldenCombatController.skill2TimeCost);
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

    public void Action_Skill3(int targetPosition)
    {
        if (characterName == "Alden")
        {
            aldenCombatController.AldenSkill3(targetPosition, characterPosition);
            AddTimeCost(aldenCombatController.skill3TimeCost);
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

    public void UpdateSkillButtons()
    {
        if (characterName == "Alden")
        {
            aldenCombatController.EnableSkillButtons();
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

    // ------------------------ STATUS MANAGEMENT FUNCTIONS ------------------------

    public void DealFrailty(int duration)
    {
        CombatManager.instance.HandleDealtAffliction(Status.StatusName.Frailty, duration);
    }

    public void DealSlowed(int duration)
    {
        CombatManager.instance.HandleDealtAffliction(Status.StatusName.Slowed, duration);
    }
    public void DealConfusion(int duration)
    {
        CombatManager.instance.HandleDealtAffliction(Status.StatusName.Confusion, duration);
    }

    public void DealExposed(int duration)
    {
        CombatManager.instance.HandleDealtAffliction(Status.StatusName.Exposed, duration);
    }

    public void GrantMight(int duration)
    {
        CombatManager.instance.HandleGrantedBlessing(Status.StatusName.Might, duration);
    }

    public void GrantFocus(int duration)
    {
        CombatManager.instance.HandleGrantedBlessing(Status.StatusName.Focus, duration);
    }

    public void GrantHaste(int duration)
    {
        CombatManager.instance.HandleGrantedBlessing(Status.StatusName.Haste, duration);
    }

    public void GrantClarity(int duration)
    {
        CombatManager.instance.HandleGrantedBlessing(Status.StatusName.Clarity, duration);
    }

    public void GrantBarrier(int duration)
    {
        CombatManager.instance.HandleGrantedBlessing(Status.StatusName.Barrier, duration);
    }
}
