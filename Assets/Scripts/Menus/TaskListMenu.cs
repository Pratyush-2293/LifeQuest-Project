using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskListMenu : Menu
{
    public static TaskListMenu instance = null;

    public GameObject createTaskMenu = null;
    public TaskItem taskItemPrefab = null;
    public TaskItem taskItem;
    public Transform taskListTransform = null;

    // Task Creation Menu
    public InputField titleInput = null;
    public InputField descriptionInput = null;
    public Toggle easyToggle = null;
    public Toggle mediumToggle = null;
    public Toggle hardToggle = null;

    // Reward Claim Data
    public int questKeyClaimCount = 0;
    public string questKeyLastClaimDate;
    public string todayDate;
    public int expRewardEasyTask = 0;
    public int expRewardMediumTask = 0;
    public int expRewardHardTask = 0;
    public int goldRewardEasyTask = 0;

    // Save Data
    public PlayerTasksData playerTasksData = new PlayerTasksData();
    private bool tasksLoaded = false;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one TaskListMenu!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        // Updating the Quest Key Claim Data
        questKeyClaimCount = PlayerPrefs.GetInt("QuestKeyClaimCount", 0);
        questKeyLastClaimDate = PlayerPrefs.GetString("QuestKeyLastClaimDate", "");
        todayDate = System.DateTime.Now.ToString("yyyy-MM-dd");

        if (questKeyLastClaimDate != todayDate)
        {
            // It's a new day, reset count
            questKeyClaimCount = 0;
            PlayerPrefs.SetInt("QuestKeyClaimCount", 0);
            PlayerPrefs.SetString("QuestKeyLastClaimDate", todayDate);
            PlayerPrefs.Save();
        }
    }

    public void LoadPlayerTasks()
    {
        if (!tasksLoaded)
        {
            SaveManager.instance.LoadPlayerTasksData();

            for (int i = 0; i < playerTasksData.playerTaskItemDatas.Count; i++)
            {
                taskItem = Instantiate(taskItemPrefab);
                taskItem.transform.SetParent(taskListTransform, false);

                taskItem.taskTitle = playerTasksData.playerTaskItemDatas[i].taskTitle;
                taskItem.taskDescription = playerTasksData.playerTaskItemDatas[i].taskDescription;
                taskItem.taskDifficulty = playerTasksData.playerTaskItemDatas[i].difficulty;
            }

            tasksLoaded = true;
        }

        CalculateExpReward();
        CalculateGoldReward();
    }

    public void RemoveTaskItemData(string taskTitle)
    {
        for(int i = 0; i < playerTasksData.playerTaskItemDatas.Count; i++)
        {
            if(playerTasksData.playerTaskItemDatas[i].taskTitle == taskTitle)
            {
                playerTasksData.playerTaskItemDatas.RemoveAt(i);
            }
        }
    }

    public void OnCreateNewTaskButton()
    {
        createTaskMenu.gameObject.SetActive(true);
    }

    public void OnCreateTaskButton()
    {
        // Instantiating and Adding to layout
        taskItem = Instantiate(taskItemPrefab);
        taskItem.transform.SetParent(taskListTransform, false);

        // Filling in values
        taskItem.taskTitle = titleInput.text;
        taskItem.taskDescription = descriptionInput.text;

        // Setting Difficulty Counters
        if (easyToggle.isOn)
        {
            taskItem.taskDifficulty = TaskItem.Difficulty.easy;
        }
        else if (mediumToggle.isOn)
        {
            taskItem.taskDifficulty = TaskItem.Difficulty.medium;
        }
        else
        {
            taskItem.taskDifficulty = TaskItem.Difficulty.hard;
        }

        // Saving Task Item Data
        PlayerTaskItemData taskItemData = new PlayerTaskItemData();
        taskItemData.taskTitle = taskItem.taskTitle;
        taskItemData.taskDescription = taskItem.taskDescription;
        taskItemData.difficulty = taskItem.taskDifficulty;
        playerTasksData.playerTaskItemDatas.Add(taskItemData);

        SaveManager.instance.SavePlayerTasksData();

        // Resetting values of Task Creation Menu
        titleInput.text = "";
        descriptionInput.text = "";

        createTaskMenu.gameObject.SetActive(false);
    }

    public void OnCreateTaskCancelButton()
    {
        createTaskMenu.gameObject.SetActive(false);
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }

    private void CalculateExpReward()
    {
        if(GameData.instance.aldenLevel < 10)
        {
            expRewardEasyTask = 50;
        }
        else if(GameData.instance.aldenLevel >= 10 && GameData.instance.aldenLevel < 20)
        {
            expRewardEasyTask = 375;
        }

        expRewardMediumTask = expRewardEasyTask * 2;
        expRewardHardTask = expRewardEasyTask * 3;
    }

    private void CalculateGoldReward()
    {
        if (GameData.instance.aldenLevel < 10)
        {
            goldRewardEasyTask = 50;
        }
        else if (GameData.instance.aldenLevel >= 10 && GameData.instance.aldenLevel < 20)
        {
            goldRewardEasyTask = 150;
        }
    }
}


[System.Serializable]
public class PlayerTasksData
{
    public List<PlayerTaskItemData> playerTaskItemDatas = new List<PlayerTaskItemData>();
}

[System.Serializable]
public class PlayerTaskItemData
{
    public string taskTitle;
    public string taskDescription;
    public TaskItem.Difficulty difficulty;
}