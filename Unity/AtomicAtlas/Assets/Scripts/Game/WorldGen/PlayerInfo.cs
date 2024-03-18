
public class PlayerInfo
{
    public int Team { get; private set; }
    public NationDefinition NationDefinition { get; private set; }

    public PlayerInfo(int team, NationDefinition nationDefinition)
    {
        Team = team;
        NationDefinition = nationDefinition;
    }

    public void SetTeam(int teamNum)
    {
        Team = teamNum;
    }

    public void SetNation(NationDefinition nationDef)
    {
        NationDefinition = nationDef;
    }
}
