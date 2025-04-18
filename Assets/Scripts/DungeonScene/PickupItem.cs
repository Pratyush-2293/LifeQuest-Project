using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Pickup Item Data")]
    [Space(5)]
    public ItemSO pickupItemReward;
    public int itemQuantity = 1;
    public string itemPickupSoundName = "";

    [Header("Object Components")]
    public Animator pickupItemAnimator;
    public GameObject interactPopup;

    public void PickUpItem()
    {
        // Turn off the interact popup
        interactPopup.gameObject.SetActive(false);

        // Play the pickup animation
        pickupItemAnimator.SetTrigger("PickupItem");

        // Add item to player inventory
        if(GameData.instance != null)
        {
            Item itemToAdd = GameData.instance.inventory.Find(item => item.itemName == pickupItemReward.itemName);

            if (itemToAdd != null)
            {
                itemToAdd.itemQuantity += itemQuantity;
            }
            else
            {
                itemToAdd = new Item(pickupItemReward);
                itemToAdd.itemQuantity = itemQuantity;

                GameData.instance.inventory.Add(itemToAdd);
            }
        }

        // Play item pickup sound
        if(itemPickupSoundName != "")
        {
            DungeonManager.instance.PlaySoundEffect(itemPickupSoundName);
        }

        // Start self destroy sequence
        StartCoroutine(DestorySelf());
    }

    private IEnumerator DestorySelf()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
