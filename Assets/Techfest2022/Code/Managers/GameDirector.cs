using UnityEngine;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    [HideInInspector]
    public InventoryManager InventoryManager = null;
    [HideInInspector]
    public TileManager GameTileManager = null;
    [HideInInspector]
    public BuildingUpgradeDataManager UpgradeDataManager = null;
    [HideInInspector]
    public GameplayUIManager GameplayUIManager = null;
    [HideInInspector]
    public TileSelectionManager TileSelectionManager = null;
    [HideInInspector]
    public VFXManager VfxManager = null;
    [HideInInspector]
    public AudioManager AudioManager;
    [HideInInspector]
    public CalamityManager CalamityManager;

    public static GameDirector Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // initializing managers
        InventoryManager = GetComponent<InventoryManager>();
        InventoryManager.Init();
        InventoryManager.ResourcesStatsChanged += OnResourcesStatsChanged;

        UpgradeDataManager = GetComponent<BuildingUpgradeDataManager>();
        UpgradeDataManager.Init();

        GameTileManager = GetComponent<TileManager>();
        GameTileManager.Init();

        TileSelectionManager = GetComponent<TileSelectionManager>();
        TileSelectorRaycaster.TileSelected += OnTileSelected;

        GameplayUIManager = GetComponent<GameplayUIManager>();
        GameplayUIManager.Init();
        
        GameplayUIManager.BuildingCreationRequested += OnBuildingCreationRequest;

        CalamityManager = GetComponent<CalamityManager>();
        CalamityManager.Init();

        VfxManager = GetComponent<VFXManager>();

        AudioManager = GetComponent<AudioManager>();
        AudioManager.Init();
    }

    private void Start()
    {
        AudioManager.PlayAudio(AudioFileType.BackgroundMusic);
    }

    private void OnResourcesStatsChanged()
    {
        GameplayUIManager.RefreshTopBarUI();
    }

    private void OnTileSelected(Tile tile)
    {
        TileSelectionManager.SelectTile(tile);
    }

    private void OnDestroy()
    {
        GameplayUIManager.BuildingCreationRequested -= OnBuildingCreationRequest;
    }

    private void OnBuildingCreationRequest(BuildingMenuData buildingMenuData, Tile tile)
    {
        GameTileManager.CreateBuilding(buildingMenuData, tile);
    }
}