using System;
using UnityEngine;

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
    private GameObject mainMenu;

    [SerializeField]
    private GameObject settingsMenu;

    void Start()
    {
        GlobalInstance = this;
    }

    void Update()
    {
        
    }

    public void SetMainMenuActive(bool active)
    {
        mainMenu.SetActive(active);
    }

    public void OnClickRegenerate()
    {
        OnClickRegenerateEvent?.Invoke(this, new EventArgs());
    }

    public void OnClickQuit()
    {
        OnClickQuitEvent?.Invoke(this, new EventArgs());
    }

    public void OnClickSettings()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    public void OnClickExport()
    {
        OnClickExportEvent?.Invoke(this, new EventArgs());
    }
}
