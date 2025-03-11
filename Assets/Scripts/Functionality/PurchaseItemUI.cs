using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseItemUI : MonoBehaviour
{
    public ItemSO item;

    public Image itemIcon = null;
    public TMP_Text itemName = null;
    public TMP_Text itemPrice = null;
    public TMP_Text itemQuantity = null;

    public Button minusButton;
    public Button buyButton;

    public Color tooExpensiveColor;

    private int itemPriceValue = 0;
    private int itemQuantityValue = 1;

    public void Initialise(ItemSO itemData)
    {
        item = itemData;

        // Load the item's icon onto the UI
        if (item.itemIcon != null)
        {
            itemIcon.sprite = item.itemIcon;
        }

        // Load the item's name on the UI
        itemName.text = item.itemName;

        // Load the item's price on the UI
        itemPrice.text = item.itemPrice.ToString();

        // Hold a local value of the price & quantity of final purchase 
        itemPriceValue = item.itemPrice;

        // Highlight the item's price if it is too expensive to purchase
        ColorPrice();

        // Enable or disable the buy button if the player can afford it
        CheckBuyAvailability();
    }

    public void ColorPrice()
    {
        if(itemPriceValue > GameData.instance.goldCoins)
        {
            itemPrice.color = tooExpensiveColor;
        }
        else
        {
            itemPrice.color = Color.white;
        }
    }

    private void CheckBuyAvailability()
    {
        if(itemPriceValue <= GameData.instance.goldCoins)
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }
    }

    private int GetPriceOfItems(int itemQuantity)
    {
        int totalPrice = 0;

        for(int i = 0; i < itemQuantity; i++)
        {
            totalPrice += item.itemPrice;
        }

        return totalPrice;
    }

    // ---------------------- BUTTON FUNCTIONS ----------------------

    public void OnMinusButton()
    {
        // Update the quantity display
        itemQuantityValue--;
        itemQuantity.text = itemQuantityValue.ToString();

        // Update the price display
        itemPriceValue = GetPriceOfItems(itemQuantityValue);
        itemPrice.text = itemPriceValue.ToString();

        // Color the price text if not affordable
        ColorPrice();

        // Disable the buy button if not affordable
        CheckBuyAvailability();

        // Check to see if minus button needs to be disabled
        if(itemQuantityValue <= 1)
        {
            minusButton.interactable = false;
        }
    }

    public void OnPlusButton()
    {
        // Update the quantity display
        itemQuantityValue++;
        itemQuantity.text = itemQuantityValue.ToString();

        // Update the price display
        itemPriceValue = GetPriceOfItems(itemQuantityValue);
        itemPrice.text = itemPriceValue.ToString();

        // Color the price text if not affordable
        ColorPrice();

        // Disable the buy button if not affordable
        CheckBuyAvailability();

        // Make the minus button interactable
        minusButton.interactable = true;
    }

    public void OnBuyButton()
    {
        // instantiate an item of the itemSO type
        Item purchasedItem = new Item(item);

        // check player's inventory to see if that item already exists in inventory
        bool itemExists = false;
        foreach(Item item in GameData.instance.inventory)
        {
            // if it exists, then get it's reference, and add the purchased items' quantity to the item in the inventory.
            if (item.itemName == purchasedItem.itemName)
            {
                item.itemQuantity += itemQuantityValue;
                itemExists = true;
                break;
            }
        }

        // if it doesn't exist, then modify the instantiated item's quantity and then add it to the inventory.
        if(itemExists == false)
        {
            purchasedItem.itemQuantity = itemQuantityValue;
            GameData.instance.inventory.Add(purchasedItem);
        }

        // deduct the total cost of items from the player's coins
        GameData.instance.goldCoins -= itemPriceValue;

        // update the gold coins display
        MarketMenuManager.instance.UpdateGoldCoinsDisplay();

        // Now, updating price and availability of all items in the purchase menu after changes
        foreach(PurchaseItemUI purchaseItem in BlacksmithManager.instance.purchaseItemUIs)
        {
            purchaseItem.ColorPrice();
            purchaseItem.CheckBuyAvailability();
        }

    }
}
