using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CaveEditor : MonoBehaviour
{
    [SerializeField]
    private Toggle ImpassableToggle;

    [SerializeField]
    private Toggle CaveToggle;

    [SerializeField]
    private Toggle CaveForestToggle;

    [SerializeField]
    private Toggle CrystalCaveToggle;

    [SerializeField]
    private Toggle DripCaveToggle;

    [SerializeField]
    private Toggle SeaCaveToggle;

    [SerializeField]
    private Toggle LargeToggle;

    [SerializeField]
    private Toggle SmallToggle;

    [SerializeField]
    private Toggle FortToggle;

    [SerializeField]
    private Toggle ThroneToggle;

    [SerializeField]
    private Toggle MoreMagicSitesToggle;

    [SerializeField]
    private TMP_InputField PlaneGateNumberInput;

    [SerializeField]
    private TMP_InputField NameInput;

    private InteractiveNode selectedNode;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public void Activate(InteractiveNode node)
    {
        selectedNode = node;

        UpdatePanel();
    }

    public void ApplyChanges()
    {
        var terrain = Terrain.PLAINS;

        // Figure out which exclusive terrain flag to use first
        if (CaveToggle.isOn)
        {
            terrain = Terrain.CAVE;
        }
        if (CaveForestToggle.isOn)
        {
            terrain = Terrain.FOREST;
        }
        if (CrystalCaveToggle.isOn)
        {
            terrain = Terrain.HIGHLAND;
        }

        // Sea swamp not allowed for caves
        if (DripCaveToggle.isOn)
        {
            terrain = Terrain.SWAMP;
        }
        else if (SeaCaveToggle.isOn) 
        {
            terrain |= Terrain.SEA;
        }

        // Apply size modifiers
        if (LargeToggle.isOn)
        {
            terrain |= Terrain.LARGER;
        }
        else if (SmallToggle.isOn)
        {
            terrain |= Terrain.SMALLER;
        }

        // Apply misc modifiers
        if (ThroneToggle.isOn)
        {
            terrain |= Terrain.THRONE;
        }
        if (MoreMagicSitesToggle.isOn)
        {
            terrain |= Terrain.MANY_SITES;
        }

        selectedNode.Node.SetHasFort(FortToggle.isOn);
        selectedNode.Node.SetGateNumber(int.Parse(PlaneGateNumberInput.text));
        selectedNode.Node.SetNameOverride(NameInput.text);
        selectedNode.Node.SetTerrain(terrain);

        UpdatePanel();
    }

    private void UpdatePanel()
    {
        PlaneGateNumberInput.text = selectedNode.Node.GateNumber.ToString();
        NameInput.text = selectedNode.Node.NameOverride;

        ImpassableToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.CAVE_WALL);
        CaveToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.CAVE);
        CaveForestToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.FOREST);
        CrystalCaveToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.HIGHLAND);
        DripCaveToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.SWAMP);
        SeaCaveToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.SEA);

        LargeToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.LARGER);
        SmallToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.SMALLER);

        ThroneToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.THRONE);
        MoreMagicSitesToggle.isOn = selectedNode.Node.Terrain.HasFlag(Terrain.MANY_SITES);
        FortToggle.isOn = selectedNode.Node.Fort != Fort.NONE;
    }
}
