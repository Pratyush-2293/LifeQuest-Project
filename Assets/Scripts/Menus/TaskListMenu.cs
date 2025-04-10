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
    public Text taskCreationPanelTitle = null;
    public InputField titleInput = null;
    public InputField descriptionInput = null;
    public Toggle easyToggle = null;
    public Toggle mediumToggle = null;
    public Toggle hardToggle = null;
    public GameObject createTaskButton;
    public GameObject cancelCreateTaskButton;
    public GameObject confirmEditTaskButton;
    public GameObject cancelEditTaskButton;

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

    // Private variables
    private TaskItem editingTaskItem;

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

    public void OnTaskEditButton(TaskItem task)
    {
        // Load the task data into the editing panel
        taskCreationPanelTitle.text = "Edit Task";
        titleInput.text = task.taskTitle;
        descriptionInput.text = task.taskDescription;

        if (task.taskDifficulty == TaskItem.Difficulty.easy)
        {
            easyToggle.isOn = true;
        }
        else if (task.taskDifficulty == TaskItem.Difficulty.medium)
        {
            mediumToggle.isOn = true;
        }
        else if (task.taskDifficulty == TaskItem.Difficulty.hard)
        {
            hardToggle.isOn = true;
        }

        // Turn off task creation buttons
        createTaskButton.gameObject.SetActive(false);
        cancelCreateTaskButton.gameObject.SetActive(false);

        // Turn on task editing buttons
        confirmEditTaskButton.gameObject.SetActive(true);
        cancelEditTaskButton.gameObject.SetActive(true);

        // Display the editing panel
        TaskListMenu.instance.OnCreateNewTaskButton();

        // Save the task reference for other functions
        editingTaskItem = task;
    }

    public void OnEditConfirmButton()
    {
        // Delete the old task using it's reference
        editingTaskItem.AfterButtonHandler();

        // Create the new task using the edited info in the editing/creation panel
        OnCreateTaskButton();

        ResetTaskCreationPanel();

        createTaskMenu.gameObject.SetActive(false);
    }

    public void OnEditCancelButton()
    {
        // On cancel, we do not delete the old task & just refresh the edit/create panel
        ResetTaskCreationPanel();

        createTaskMenu.gameObject.SetActive(false);
    }

    private void ResetTaskCreationPanel()
    {
        taskCreationPanelTitle.text = "Create New Task";
        titleInput.text = "";
        descriptionInput.text = "";

        easyToggle.isOn = true;

        // Disable the editing buttons in the panel
        confirmEditTaskButton.gameObject.SetActive(false);
        cancelEditTaskButton.gameObject.SetActive(false);

        // Enable the task creation buttons in the panel
        createTaskButton.gameObject.SetActive(true);
        cancelCreateTaskButton.gameObject.SetActive(true);
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