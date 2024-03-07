using System;
using UnityEngine;
using UnityEngine.UI;

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
    private InputField errorLogText;

    void Start()
    {
        GlobalInstance = this;
        Application.logMessageReceived += HandleExceptions;
    }

    void Update()
    {
        
    }

    private void HandleExceptions(string log, string stack, LogType type)
    {
        if (type == LogType.Exception && !settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(true);
            SetSettingsPanelActive(errorLogPanel);

            errorLogText.text = log + "\n\n" + stack;
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
