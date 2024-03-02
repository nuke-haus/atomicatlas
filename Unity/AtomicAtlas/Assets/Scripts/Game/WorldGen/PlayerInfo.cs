
public class PlayerInfo
{
    public int Team => team;
    public NationDefinition NationDefinition => nation;

    private int team;
    private NationDefinition nation;

    public void SetTeam(int teamNum)
    {
        team = teamNum;
    }

    public void SetNation(NationDefinition nationDef)
    {
        nation = nationDef;
    }
}
