using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public enum ErrorLogLevel
{
    ERROR,
    WARNING,
    INFO 
}

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager GlobalInstance
    {
        get;
        private set;
    }

    public event EventHandler OnClickRegenerateEvent;
    public event EventHandler OnClickExportEvent;
    public event EventHandler OnClickQuitEvent;

    [SerializeField]
    private GameObject uiRoot;

    [SerializeField]
    private GameObject settingsPanel;

    [SerializeField]
    private GameObject quitPanel;

    [SerializeField]
    private GameObject generatorSettingsPanel;

    [SerializeField]
    private GameObject gameSettingsPanel;

    [SerializeField]
    private GameObject errorLogPanel;

    [SerializeField]
    private GameObject errorLogListRoot;

    [SerializeField]
    private GameObject errorLogEntryPrefab;

    [SerializeField]
    private TMP_Dropdown strategyDropdown;

    [SerializeField]
    private TMP_Dropdown strategyConfigDropdown;

    [SerializeField]
    private GameObject playerInfoEntryPrefab;

    [SerializeField]
    private RectTransform playerInfoListRoot;

    [SerializeField]
    private TMP_Dropdown playerCountDropdown;

    [SerializeField]
    private Toggle disciplesToggle;

    private List<PlayerInfoEntry> playerInfoEntries = new();
    private List<ErrorLogEntry> errorLogEntries = new();
    private IDataManager dataManager;
    private ISettingsManager settingsManager;

    void Start()
    {
        GlobalInstance = this;
        Application.logMessageReceived += HandleExceptions;
        dataManager = DependencyInjector.Resolve<IDataManager>();
        settingsManager = DependencyInjector.Resolve<ISettingsManager>();

        InitializeDropdowns();
    }

    void Update()
    {
        
    }

    public void ClearErrorLog()
    {
        foreach (var entry in errorLogEntries)
        {
            Destroy(entry.gameObject);
        }

        errorLogEntries.Clear();
    }

    private void InitializeDropdowns()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(StrategyAttribute)));
        var strategyList = new List<TMP_Dropdown.OptionData>();
       
        foreach (var strategyType in types)
        {
            var attribute = (StrategyAttribute)Attribute.GetCustomAttribute(strategyType, typeof(StrategyAttribute));
            var option = new TMP_Dropdown.OptionData(attribute.DisplayName);
            strategyList.Add(option);
        }

        strategyDropdown.options = strategyList;
        settingsManager.SetActiveStrategy(types.First());
        OnStrategyChanged();

        playerCountDropdown.value = 7; // 9 player option is default
    }

    public void OnClickDisciplesToggle()
    {
        settingsManager.SetDisciples(disciplesToggle.isOn);

        for (int i = 0; i < playerInfoEntries.Count; i++)
        {
            var entry = playerInfoEntries[i];
            entry.SetShowTeam(disciplesToggle.isOn);
        }
    }

    public void OnPlayerCountChanged()
    {
        var count = playerCountDropdown.value + 2;

        if (count > playerInfoEntries.Count)
        {
            while (playerInfoEntries.Count < count)
            {
                var playerInfoEntry = Instantiate(playerInfoEntryPrefab, playerInfoListRoot).GetComponent<PlayerInfoEntry>();
                playerInfoEntries.Add(playerInfoEntry);
            }
        }
        else if (count < playerInfoEntries.Count)
        {
            while (playerInfoEntries.Count > count)
            {
                var entry = playerInfoEntries[playerInfoEntries.Count - 1];
                playerInfoEntries.Remove(entry);

                Destroy(entry.gameObject);
            }
        }

        settingsManager.UpdatePlayerCount(count);

        var playerInfoList = settingsManager.AllPlayerInfo.ToList();
        for (int i = 0; i < count; i++)
        {
            var entry = playerInfoEntries[i];
            entry.SetPlayerInfo(playerInfoList[i]);
            entry.SetShowTeam(settingsManager.IsDisciples);
        }
    }

    public void OnStrategyChanged()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(StrategyAttribute))).ToList();
        var index = strategyDropdown.value;
        var selectedType = types[index];
        var attribute = (StrategyAttribute)Attribute.GetCustomAttribute(selectedType, typeof(StrategyAttribute));
        var data = dataManager.AllStrategyData.FirstOrDefault(data => data.GetType() == attribute.DataClassType);

        if (data == null || !data.StrategyConfigDefinitions.Any())
        {
            Debug.LogError("Cannot get data for strategy type " + selectedType.Name);
        }
        else
        {
            settingsManager.SetActiveStrategyConfigDefinition(data.StrategyConfigDefinitions.First());
            var options = data.StrategyConfigDefinitions.Select(config => new TMP_Dropdown.OptionData(config.Name)).ToList();
            strategyConfigDropdown.options = options;
        }
    }

    public void OnStrategyConfigChanged()
    {
        var newValue = strategyConfigDropdown.value;
    }

    public void AddErrorLogEntry(string text, ErrorLogLevel logLevel, bool showPanel)
    {
        var errorLogEntry = Instantiate(errorLogEntryPrefab).GetComponent<ErrorLogEntry>();
        errorLogEntry.SetText(text, logLevel.ToString());

        errorLogEntries.Add(errorLogEntry);

        if (showPanel)
        {
            settingsPanel.SetActive(true);
            SetSettingsPanelActive(errorLogPanel);
        }
    }

    private void HandleExceptions(string log, string stack, LogType type)
    {
        if (type == LogType.Exception && !settingsPanel.activeSelf)
        {
            // TODO: Format text properly for the error log entry. only supports two lines. 
            // errorLogText.text = log + "\n\n" + stack;
        }
    }

    public void SetUIActive(bool active)
    {
        uiRoot.SetActive(active);
    }

    public void OnClickRegenerate()
    {
        OnClickRegenerateEvent?.Invoke(this, new EventArgs());
    }

    public void OnClickQuit()
    {
        OnClickQuitEvent?.Invoke(this, new EventArgs());
    }

    public void OnClickToggleQuitPanel()
    {
        quitPanel.SetActive(!quitPanel.activeSelf);
    }

    public void OnClickToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void OnClickExport()
    {
        OnClickExportEvent?.Invoke(this, new EventArgs());
    }

    public void SetSettingsPanelActive(GameObject panel)
    {
        gameSettingsPanel.SetActive(false);
        generatorSettingsPanel.SetActive(false);
        errorLogPanel.SetActive(false);
        panel.SetActive(true);
    }
}
