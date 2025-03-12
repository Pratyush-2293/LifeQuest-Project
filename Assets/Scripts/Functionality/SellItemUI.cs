using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellItemUI : MonoBehaviour
{
    public Item item;

    public Image itemIcon = null;
    public TMP_Text itemName = null;
    public TMP_Text itemSellPrice = null;
    public TMP_Text itemSellQuantity = null;
    public TMP_Text itemAvailableQuantity = null;

    public Button minusButton;
    public Button plusButton;
    public Button sellButton;

    public GameObject sellQuantityControlPanel = null;
    public GameObject availableQuantityPanel = null;

    private int itemSellPriceValue = 0;
    private int itemSellQuantityValue = 1;
    private int itemAvailableQuantityValue = 0;

    public void Initialise(Item itemData)
    {
        item = itemData;

        // Load the item's icon onto the UI
        if (item.itemIcon != null)
        {
            itemIcon.sprite = item.itemIcon;
        }

        // Load the item's name on the UI
        itemName.text = item.itemName;

        // Add the item's enhancement level to the name
        if(item.enhancementLevel > 0)
        {
            itemName.text += " +" + item.enhancementLevel.ToString();
        }

        // Load the item's sell price on the UI
        itemSellPrice.text = item.sellValue.ToString();

        // Load the item's available quantity on the UI
        itemAvailableQuantity.text = item.itemQuantity.ToString();

        // Hold a local value of the price & available quantity for selling
        itemSellPriceValue = item.sellValue;
        itemAvailableQuantityValue = item.itemQuantity;

        // if the item is not stackable, hide the quantity control & quantity display
        if(item.isStackable == false)
        {
            sellQuantityControlPanel.gameObject.SetActive(false);
            availableQuantityPanel.gameObject.SetActive(false);
        }
    }

    private int GetSellPriceOfItems(int itemQuantity)
    {
        int totalPrice = 0;

        for (int i = 0; i < itemQuantity; i++)
        {
            totalPrice += item.sellValue;
        }

        return totalPrice;
    }

    // ---------------------- BUTTON FUNCTIONS ----------------------

    public void OnMinusButton()
    {
        // Update the quantity display
        itemSellQuantityValue--;
        itemSellQuantity.text = itemSellQuantityValue.ToString();

        // Update the price display
        itemSellPriceValue = GetSellPriceOfItems(itemSellQuantityValue);
        itemSellPrice.text = itemSellPriceValue.ToString();;

        // Check to see if minus button needs to be disabled
        if (itemSellQuantityValue <= 1)
        {
            minusButton.interactable = false;
        }

        // Make the plus button interactible
        plusButton.interactable = true;
    }

    public void OnPlusButton()
    {
        // Update the quantity display
        itemSellQuantityValue++;
        itemSellQuantity.text = itemSellQuantityValue.ToString();

        // Update the price display
        itemSellPriceValue = GetSellPriceOfItems(itemSellQuantityValue);
        itemSellPrice.text = itemSellPriceValue.ToString();

        // Make the minus button interactable
        minusButton.interactable = true;

        // Disable the plus button if the sell quantity has reached available quantity
        if(itemSellQuantityValue == itemAvailableQuantityValue)
        {
            plusButton.interactable = false;
        }
    }

    public void OnSellButton()
    {
        // check player's inventory to find the item
        foreach (Item itemInInventory in GameData.instance.inventory)
        {
            // if it exists, then get it's reference, and deduct the sold item's quantity from the item in the inventory.
            if (itemInInventory.itemName == item.itemName)
            {
                if(itemInInventory.enhancementLevel == item.enhancementLevel)
                {
                    item.itemQuantity -= itemSellQuantityValue;
                    break;
                }
            }
        }

        // Remove the item from the inventory if it's quantity has reached 0
        GameData.instance.inventory.RemoveAll(item => item.itemQuantity < 1);

        // add the total sell value of items to the player's coins
        GameData.instance.goldCoins += itemSellPriceValue;

        // update the gold coins display
        MarketMenuManager.instance.UpdateGoldCoinsDisplay();

        // Update the available quantity value
        itemAvailableQuantityValue -= itemSellQuantityValue;
        itemAvailableQuantity.text = itemAvailableQuantityValue.ToString();

        // Reset the item sell quantity to 1
        itemSellQuantityValue = 1;
        itemSellQuantity.text = itemSellQuantityValue.ToString();

        // Reset the item sell price to sell price of one object
        itemSellPriceValue = GetSellPriceOfItems(itemSellQuantityValue);
        itemSellPrice.text = itemSellPriceValue.ToString();

        // Disable the minus button and enable the plus button
        minusButton.interactable = false;
        plusButton.interactable = true;

        // Check if the available quantity has reached zero, if yes, then delete self from the list
        if (itemAvailableQuantityValue <= 0)
        {
            Destroy(gameObject);
        }
    }
}
