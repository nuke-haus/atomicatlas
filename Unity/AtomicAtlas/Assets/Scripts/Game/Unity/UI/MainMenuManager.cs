using System;
using UnityEngine;
using System.Collections.Generic;

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

    private List<ErrorLogEntry> errorLogEntries;

    void Start()
    {
        GlobalInstance = this;
        Application.logMessageReceived += HandleExceptions;
        errorLogEntries = new List<ErrorLogEntry>();
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
