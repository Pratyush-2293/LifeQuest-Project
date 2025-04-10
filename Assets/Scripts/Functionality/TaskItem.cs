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

    public Text titleText = null;
    public Text descriptionText = null;
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
            expPointsAmountText.text = TaskListMenu.instance.expRewardEasyTask.ToString();
        }
        else if(taskDifficulty == Difficulty.medium)
        {
            expPointsAmountText.text = TaskListMenu.instance.expRewardMediumTask.ToString();
        }
        else
        {
            expPointsAmountText.text = TaskListMenu.instance.expRewardHardTask.ToString();
        }

        // Handling Gold Coins Reward Button
        if (taskDifficulty == Difficulty.easy)
        {
            goldCoinsAmountText.text = TaskListMenu.instance.goldRewardEasyTask.ToString();
        }
        else if (taskDifficulty == Difficulty.medium)
        {
            goldCoinsAmountText.text = (TaskListMenu.instance.goldRewardEasyTask * 2).ToString();
        }
        else
        {
            goldCoinsAmountText.text = (TaskListMenu.instance.goldRewardEasyTask * 3).ToString();
        }

        rewardSelectionPanel.gameObject.SetActive(true);
    }

    public void OnAbandonButton()
    {
        // Maybe give a warning before ?
        Debug.Log("Task Abandoned!");
        TaskListMenu.instance.RemoveTaskItemData(taskTitle);
        SaveManager.instance.SavePlayerTasksData();

        Destroy(gameObject);
    }

    public void OnQuestKeyButton()
    {
        GameData.instance.levelKeys++;
        TaskListMenu.instance.questKeyClaimCount++;
        PlayerPrefs.SetInt("QuestKeyClaimCount", TaskListMenu.instance.questKeyClaimCount);
        PlayerPrefs.Save();

        AfterRewardButtonHandler();
    }

    public void OnDungeonKeyButton()
    {
        GameData.instance.dungeonKeys++;
        AfterRewardButtonHandler();
    }

    public void OnExpPointsButton()
    {
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

        AfterRewardButtonHandler();
    }

    public void OnGoldCoinsRewardButton()
    {
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

        AfterRewardButtonHandler();
    }

    private void AfterRewardButtonHandler()
    {
        TaskListMenu.instance.RemoveTaskItemData(taskTitle);
        SaveManager.instance.SavePlayerTasksData();
        SaveManager.instance.SaveGameData();

        Destroy(gameObject);
    }
}
