
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerInfoEntry : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown nationDropdown;

    [SerializeField]
    private TMP_Dropdown teamDropdown;

    public PlayerInfo PlayerInfo { get; private set; }

    void Start()
    {
        var dataManager = DependencyInjector.Resolve<IDataManager>();
        var options = new List<TMP_Dropdown.OptionData>();

        foreach (var nation in dataManager.GetData<NationData>().Nations)
        {
            options.Add(new TMP_Dropdown.OptionData(nation.Name));
        }

        nationDropdown.options = options;
    }

    void Update()
    {
        
    }

    public void SetShowTeam(bool show)
    {
        teamDropdown.gameObject.SetActive(show);

        // resize?
    }

    public void OnNationChange()
    {
        var dataManager = DependencyInjector.Resolve<IDataManager>();
        var nations = dataManager.GetData<NationData>().Nations;
        PlayerInfo.SetNation(nations[nationDropdown.value]);
    }

    public void OnTeamChange()
    {
        var team = teamDropdown.value + 1;
        PlayerInfo.SetTeam(team);
    }

    public void SetPlayerInfo(PlayerInfo playerInfo)
    {
        PlayerInfo = playerInfo;
        nationDropdown.value = 5;

        if (playerInfo.Team > 0)
        {
            teamDropdown.value = playerInfo.Team - 1;
        }
    }
}
