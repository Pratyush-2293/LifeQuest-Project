using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskItem : MonoBehaviour
{
    public string taskTitle = "";
    public string taskDescription = "";
    public enum Difficulty { easy, medium, hard};
    public Difficulty taskDifficulty = Difficulty.easy;

    public TMP_Text titleText = null;
    public TMP_Text descriptionText = null;
    public int taskIndex;

    public GameObject diffCounterEasy = null;
    public GameObject diffCounterMedium = null;
    public GameObject diffCounterHard = null;

    public GameObject rewardSelectionPanel;
    public Button questKeyButton;
    public Button dungeonKeyButton;
    public TMP_Text expPointsAmountText;
    public TMP_Text goldCoinsAmountText;

    private void Start()
    {
        titleText.text = taskTitle;
        descriptionText.text = taskDescription;
        SetDifficultyCounters();
    }

    private void SetDifficultyCounters()
    {
        if(taskDifficulty == Difficulty.easy)
        {
            diffCounterEasy.gameObject.SetActive(true);
        }
        else if(taskDifficulty == Difficulty.medium)
        {
            diffCounterEasy.gameObject.SetActive(true);
            diffCounterMedium.gameObject.SetActive(true);
        }
        else
        {
            diffCounterEasy.gameObject.SetActive(true);
            diffCounterMedium.gameObject.SetActive(true);
            diffCounterHard.gameObject.SetActive(true);
        }
    }

    public void OnCompleteButton()
    {
        AudioManager.instance.PlayUISound("taskDoneTing1");

        // Add complete functionality
        Debug.Log("Task Completed!");

        // Load the reward selection buttons

        // Handling the Quest Key Reward Button
        if(TaskListMenu.instance.questKeyClaimCount == 2)
        {
            questKeyButton.interactable = false;
        }

        // Handling the Dungeon Key Reward Button
        if(GameData.instance.maxCompletedLevel <= 11)
        {
            dungeonKeyButton.interactable = false;
        }

        // Handling the EXP Points Reward Button
        if(taskDifficulty == Difficulty.easy)
        {
            expPointsAmountText.text = "X" + TaskListMenu.instance.expRewardEasyTask.ToString();
        }
        else if(taskDifficulty == Difficulty.medium)
        {
            expPointsAmountText.text = "X" + TaskListMenu.instance.expRewardMediumTask.ToString();
        }
        else
        {
            expPointsAmountText.text = "X" + TaskListMenu.instance.expRewardHardTask.ToString();
        }

        // Handling Gold Coins Reward Button
        if (taskDifficulty == Difficulty.easy)
        {
            goldCoinsAmountText.text = "X" + TaskListMenu.instance.goldRewardEasyTask.ToString();
        }
        else if (taskDifficulty == Difficulty.medium)
        {
            goldCoinsAmountText.text = "X" + (TaskListMenu.instance.goldRewardEasyTask * 2).ToString();
        }
        else
        {
            goldCoinsAmountText.text = "X" + (TaskListMenu.instance.goldRewardEasyTask * 3).ToString();
        }

        rewardSelectionPanel.gameObject.SetActive(true);
    }

    public void OnAbandonButton()
    {
        AudioManager.instance.PlayUISound("paperTaskAbandon");

        // Maybe give a warning before ?
        Debug.Log("Task Abandoned!");
        TaskListMenu.instance.RemoveTaskItemData(taskTitle);
        SaveManager.instance.SavePlayerTasksData();

        Destroy(gameObject);
    }

    public void OnEditButton()
    {
        TaskListMenu.instance.OnTaskEditButton(this);
    }

    public void OnQuestKeyButton()
    {
        AudioManager.instance.PlayUISound("keysReward");

        GameData.instance.levelKeys++;
        TaskListMenu.instance.questKeyClaimCount++;
        PlayerPrefs.SetInt("QuestKeyClaimCount", TaskListMenu.instance.questKeyClaimCount);
        PlayerPrefs.Save();

        AfterButtonHandler();
    }

    public void OnDungeonKeyButton()
    {
        AudioManager.instance.PlayUISound("keysReward");

        GameData.instance.dungeonKeys++;
        AfterButtonHandler();
    }

    public void OnExpPointsButton()
    {
        AudioManager.instance.PlayUISound("taskDoneTing2");

        if (taskDifficulty == Difficulty.easy)
        {
            GameData.instance.expPoints += TaskListMenu.instance.expRewardEasyTask;
        }
        else if (taskDifficulty == Difficulty.medium)
        {
            GameData.instance.expPoints += TaskListMenu.instance.expRewardMediumTask;
        }
        else
        {
            GameData.instance.expPoints += TaskListMenu.instance.expRewardHardTask;
        }

        AfterButtonHandler();
    }

    public void OnGoldCoinsRewardButton()
    {
        AudioManager.instance.PlayUISound("coinsReward");

        if (taskDifficulty == Difficulty.easy)
        {
            GameData.instance.goldCoins += TaskListMenu.instance.goldRewardEasyTask;
        }
        else if (taskDifficulty == Difficulty.medium)
        {
            GameData.instance.goldCoins += (TaskListMenu.instance.goldRewardEasyTask * 2);
        }
        else
        {
            GameData.instance.goldCoins += (TaskListMenu.instance.goldRewardEasyTask * 3);
        }

        AfterButtonHandler();
    }

    public void AfterButtonHandler()
    {
        TaskListMenu.instance.RemoveTaskItemData(taskTitle);
        SaveManager.instance.SavePlayerTasksData();
        SaveManager.instance.SaveGameData();

        Destroy(gameObject);
    }
}
