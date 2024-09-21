using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject ROOT = null;
    public Menu previousMenu = null;

    public virtual void TurnOn(Menu previous)
    {
        if (ROOT)
        {
            if(previous != null)
            {
                previousMenu = previous;
            }
            ROOT.SetActive(true);
        }
        else
        {
            Debug.LogError("ROOT object not set!");
        }
    }

    public virtual void TurnOff(bool returnToPrevious)
    {
        if (ROOT)
        {
            ROOT.SetActive(false);

            if(previousMenu && returnToPrevious)
            {
                previousMenu.TurnOn(null);
            }
        }
        else
        {
            Debug.LogError("ROOT object not set!");
        }
    }
}
