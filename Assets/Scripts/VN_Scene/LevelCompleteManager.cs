using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCompleteManager : MonoBehaviour
{
    [Header("EXP Reward")]
    [Space(5)]
    public int expReward;

    [Header("Gold Reward")]
    [Space(5)]
    public int goldReward;

    [Header("Item Reward 1")]
    [Space(5)]
    public ItemSO itemReward1;
    public int itemRewardQuantity1;

    [Header("Item Reward 2")]
    [Space(5)]
    public ItemSO itemReward2;
    public int itemRewardQuantity2;

    [Header("Item Reward 3")]
    [Space(5)]
    public ItemSO itemReward3;
    public int itemRewardQuantity3;

    [Header("Level Complete Panel Components")]
    [Space(5)]
    public GameObject[] rewardItemsUI = new GameObject[5];
    public Image[] rewardIconDisplay = new Image[3];
    public TMP_Text[] rewardNamesDisplay = new TMP_Text[5];
    [Space(10)]
    public Animator levelCompletePanelAnimator;
    public GameObject touchBlockerPanel;

    public void LoadLevelCompletePanel(bool completingFirstTime)
    {
        // turn on the touch blocker
        touchBlockerPanel.gameObject.SetActive(true);

        // Trigger level complete panel intro animation
        levelCompletePanelAnimator.SetTrigger("Enter");

        // Loading in the quantity of experience obtained
        rewardNamesDisplay[0].text += " X" + expReward.ToString();

        // Loading in the quantity of gold obtained
        rewardNamesDisplay[1].text += " X" + goldReward.ToString();

        // load the reward items into the panel UI if first time completing level
        if(completingFirstTime == true)
        {
            // - Load item datas
            if (itemReward1 != null)
            {
                rewardItemsUI[2].gameObject.SetActive(true);
                rewardIconDisplay[0].sprite = itemReward1.itemIcon;
                rewardNamesDisplay[2].text = itemReward1.itemName + " X" + itemRewardQuantity1.ToString();
            }
            if (itemReward2 != null)
            {
                rewardItemsUI[3].gameObject.SetActive(true);
                rewardIconDisplay[1].sprite = itemReward2.itemIcon;
                rewardNamesDisplay[3].text = itemReward2.itemName + " X" + itemRewardQuantity2.ToString();
            }
            if (itemReward3 != null)
            {
                rewardItemsUI[4].gameObject.SetActive(true);
                rewardIconDisplay[2].sprite = itemReward3.itemIcon;
                rewardNamesDisplay[4].text = itemReward3.itemName + " X" + itemRewardQuantity3.ToString();
            }

            // Add the reward items to the player's inventory

            // - Add the exp
            GameData.instance.expPoints += expReward;

            // - Add the gold
            GameData.instance.goldCoins += goldReward;

            // - Add the reward items
            AddRewardItems();

            // Increment the completed story level counter in GameData
            GameData.instance.maxCompletedLevel++;

            // Save changes made to GameData
            // SaveManager.instance.SaveGameData(); // Uncomment this later
        }
        else
        {
            rewardItemsUI[0].gameObject.SetActive(false);
            rewardItemsUI[1].gameObject.SetActive(false);
        }
    }

    private void AddRewardItems()
    {
        if(itemReward1 != null)
        {
            if(itemReward1.isStackable == true)
            {
                bool itemStored = false;
                foreach(Item item in GameData.instance.inventory)
                {
                    if(item.itemName == itemReward1.itemName)
                    {
                        item.itemQuantity += itemRewardQuantity1;
                        itemStored = true;
                        break;
                    }
                }

                if(itemStored == false)
                {
                    Item addItem = new Item(itemReward1);
                    addItem.itemQuantity = itemRewardQuantity1;
                    GameData.instance.inventory.Add(addItem);
                }
            }
            else
            {
                GameData.instance.inventory.Add(new Item(itemReward1));
            }
        }

        if (itemReward2 != null)
        {
            if (itemReward2.isStackable == true)
            {
                bool itemStored = false;
                foreach (Item item in GameData.instance.inventory)
                {
                    if (item.itemName == itemReward2.itemName)
                    {
                        item.itemQuantity += itemRewardQuantity2;
                        itemStored = true;
                        break;
                    }
                }

                if (itemStored == false)
                {
                    Item addItem = new Item(itemReward2);
                    addItem.itemQuantity = itemRewardQuantity2;
                    GameData.instance.inventory.Add(addItem);
                }
            }
            else
            {
                GameData.instance.inventory.Add(new Item(itemReward2));
            }
        }

        if (itemReward3 != null)
        {
            if (itemReward3.isStackable == true)
            {
                bool itemStored = false;
                foreach (Item item in GameData.instance.inventory)
                {
                    if (item.itemName == itemReward3.itemName)
                    {
                        item.itemQuantity += itemRewardQuantity3;
                        itemStored = true;
                        break;
                    }
                }

                if (itemStored == false)
                {
                    Item addItem = new Item(itemReward3);
                    addItem.itemQuantity = itemRewardQuantity3;
                    GameData.instance.inventory.Add(addItem);
                }
            }
            else
            {
                GameData.instance.inventory.Add(new Item(itemReward3));
            }
        }
    }

    public void OnContinueButton()
    {
        // Load the level select scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelectMenu");
    }
}
