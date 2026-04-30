using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool drawClouds= true;

    // --- Map & Grid --
    public Map currentMap = null;
    private float hexWidth = 1.732f;
    private float hexHeight = 2f;
    
    // --- Players & Turn ---
    public Player currentPlayer;
    public Player[] players = new Player[1];
    public Player humanPlayer;

    //AI
    bool isAITurn = false;
    public int aiDifficulty;

    //Tribe types
    public List<Civ> civs = new List<Civ>();

    [Header("Gameplay UI")]
    public TMPro.TextMeshProUGUI moneyText;
    public TMPro.TextMeshProUGUI cultureText;
    public int turn = 0;
    public TMPro.TextMeshProUGUI turnText;
    public TMP_Text tileInfoText;
    public GameObject tileOptionClickedImage;

    [Header("Game Setup UI")]
    public TMP_Dropdown civDropdown;
    public Slider aiCountSlider;
    public Slider aiDifficultySlider;
    public TextMeshProUGUI aiCountText;
    public TextMeshProUGUI aiDifficultyText;

    //Techs
    public Tech currentTech;
    public List<Tech> techList = new List<Tech>();

    //Civics
    public Civic currentCivic;
    public List<Civic> civicList;// = new List<Civic>();

    //Govenements
    public Policy currentPolicy;
    public List<Policy> policyList = new List<Policy>();
    public List<Govenments> govenmentList = new List<Govenments>();
    public List<Ministrys> ministrysList = new List<Ministrys>();
    public List<GovernmentBuildings> governmentBuildingsList = new List<GovernmentBuildings>();

    // --- Tiles & Selection ---
    public int approxY;
    public int approxX;
    public Tile currentTile;
    private Tile mainTile;
    private int selectedX;
    private int selectedY;
    public Tile capitalTile;

    private enum SelectionState { None, TileSelected, TroopSelected, ActionSelected }
    private SelectionState selectionState = SelectionState.None;

    [SerializeField] private Camera mainCamera;
    public CameraController cameraController;
    public GameObject textParent;

    //Return Btn
    public GameObject ReturnBtn;

    // --- Panels ---
    public GameObject singlePlayerPanel;
    public GameObject gamePanel;
    public GameObject tileClickedPanel;
    public GameObject TroopClickPanel;
    public GameObject techTree;
    public GameObject socialTechnologyTree;
    public GameObject statsPanel;
    public GameObject menuPanel;
    public GameObject government;
    public GameObject policyClickedPanel;

    //Add Troops buttons
    public GameObject AddWarriorBtn;
    public GameObject AddHorsemanBtn;
    public GameObject AddSpearmanBtn;
    public GameObject AddShipBtn;
    public GameObject AddRammingShipBtn;
    public GameObject AddShieldBtn;
    public GameObject AddArcherBtn;
    public GameObject AddChariotBtn;
    public GameObject AddSwordsmanBtn;
    public GameObject AddKnightBtn;
    public GameObject AddCatapultBtn;
    public GameObject AddFrigateBtn;
    public GameObject AddCaravelBtn;
    public GameObject AddMusketeerBtn;
    public GameObject AddCannonBtn;
    public GameObject AddCavalryBtn;
    public GameObject AddMachineGunBtn;
    public GameObject AddInfantryBtn;
    public GameObject AddArtilleryBtn;
    public GameObject AddTankBtn;
    public GameObject AddZeppelinBtn;
    public GameObject AddBiplaneBtn;
    public GameObject AddCruiserBtn;
    public GameObject AddDreadnortBtn;

    //Add Action btns
    public GameObject BurnForestBtn;
    public GameObject DestroyBtn;
    public GameObject ChopForestBtn;
    public GameObject HuntAnimalBtn;
    public GameObject HuntFishBtn;
    
    //Troop clicked UI
    public GameObject ClaimTileBtn;
    public GameObject HealTroopBtn;
    public GameObject TroopClickPanelImage;

    //District Btns
    public GameObject AddCityBtn;
    public GameObject AddHarbourBtn;
    public GameObject AddCommercialBtn;
    public GameObject AddIndustrialBtn;

    //Add building btns
    public GameObject AddFishingBoatsBtn;
    public GameObject AddPastureBtn;
    public GameObject AddFarmBtn;
    public GameObject AddMineBtn;
    public GameObject AddMonumentBtn;
    public GameObject AddQuarryBtn;
    public GameObject AddWallBtn;
    public GameObject AddMarketBtn;
    public GameObject AddWaterwheelBtn;
    public GameObject AddFurTradingPostBtn;
    public GameObject AddLumberHutBtn;
    public GameObject AddCustomsHouseBtn;
    public GameObject AddTowerBtn;
    public GameObject AddForgeBtn;
    public GameObject AddFortBtn;
    public GameObject AddWindmillBtn;
    public GameObject AddSawmillBtn;
    public GameObject AddLightHouseBtn;
    public GameObject AddShipyardBtn;
    public GameObject AddWhalingShipBtn;
    public GameObject AddCarpentryWorkshopBtn;
    public GameObject AddBankBtn;
    public GameObject AddPaperMillBtn;
    public GameObject AddNavalBaseBtn;
    public GameObject AddSkyScrapersBtn;
    public GameObject AddMeatProcessingPlantBtn;
    public GameObject AddWarehousesBtn;
    public GameObject AddAirportBtn;
    public GameObject AddUniversityBtn;
    public GameObject AddTankFactoryBtn;
    public GameObject AddRoadBtn;
    public GameObject AddBridgeBtn;
    public GameObject AddTrainTracksBtn;
    public GameObject AddTrainTrackBridgeBtn;
    public GameObject AddTradeRouteBtn;

    [Header("Prefabs")]

    //Tile Selection Prefabs
    public GameObject tileSelectedPrefab;
    public GameObject troopSelectedPrefab;
    public GameObject troopMovementPrefab;
    public GameObject troopAttackPrefab;
    public GameObject healthTextPrefab;

    //Border prefabs
    public GameObject borderPrefab;

    //Tile Prefabs
    public GameObject cloudPrefab;
    public GameObject fieldTilePrefab;
    public GameObject oceanTilePrefab;
    public GameObject desertTilePrefab;
    public GameObject snowTilePrefab;
    public GameObject mountainTilePrefab;
    public GameObject coastTilePrefab;
    public GameObject riverTilePrefab;

    //Forest Prefabs
    public GameObject BorealForestPrefab;
    public GameObject DryForestPrefab;
    public GameObject RainforestPrefab;
    public GameObject TemperateForestPrefab;
    public GameObject seaIcePrefab;
    public GameObject riverIcePrefab;

    //Reasource Prefabs
    public GameObject cropResourcePrefab;
    public GameObject fishResourcePrefab;
    public GameObject whaleResourcePrefab;
    public GameObject horseResourcePrefab;
    public GameObject boarResourcePrefab;
    public GameObject deerResourcePrefab;
    public GameObject metalResourcePrefab;
    public GameObject penguinResourcePrefab;

    //Troop Prefabs
    public GameObject warriorPrefab;
    public GameObject horsemanPrefab;
    public GameObject spearmanPrefab;
    public GameObject boatPrefab;
    public GameObject shipPrefab;
    public GameObject rammingShipPrefab;
    public GameObject archerPrefab;
    public GameObject chariotPrefab;
    public GameObject shieldPrefab;
    public GameObject swordsManPrefab;
    public GameObject knightPrefab;
    public GameObject catapultPrefab;
    public GameObject frigatePrefab;
    public GameObject caravelPrefab;
    public GameObject musketeerPrefab;
    public GameObject cannonPrefab;
    public GameObject cavalryPrefab;
    public GameObject machineGunPrefab;
    public GameObject infantryPrefab;
    public GameObject artilleryPrefab;
    public GameObject tankPrefab;
    public GameObject zeppelinPrefab;
    public GameObject biplanePrefab;
    public GameObject dreadnortPrefab;
    public GameObject cruiserPrefab;

    //District Prefabs
    public GameObject districtTilePrefab;
    public GameObject cityPrefab;
    public GameObject harbourPrefab;
    public GameObject commercialDistrict;
    public GameObject industrialZone;

    //Building Prefabs
    public GameObject palacePrefab;
    public GameObject monumentPrefab;
    public GameObject wallPrefab;
    public GameObject pasturePrefab;
    public GameObject farmPrefab;
    public GameObject fishingBoatsPrefab;
    public GameObject minePrefab;
    public GameObject quarryPrefab;
    public GameObject marketPrefab;
    public GameObject waterwheelPrefab;
    public GameObject furTradingPostPrefab;
    public GameObject lumberHutPrefab;
    public GameObject customsHousePrefab;
    public GameObject towerPrefab;
    public GameObject forgePrefab;
    public GameObject fortPrefab;
    public GameObject windmillPrefab;
    public GameObject sawmillPrefab;
    public GameObject lightHousePrefab;
    public GameObject shipyardPrefab;
    public GameObject whalingShipPrefab;
    public GameObject carpentryWorkshopPrefab;
    public GameObject bankPrefab;
    public GameObject paperMillPrefab;
    public GameObject navalBasePrefab;
    public GameObject skyScrapersPrefab;
    public GameObject meatProcessingPlantPrefab;
    public GameObject warehousesPrefab;
    public GameObject airportPrefab;
    public GameObject universityPrefab;
    public GameObject tankFactoryPrefab;
    public GameObject roadPrefab;
    public GameObject trainTracksPrefab;
    public GameObject BridgePrefab;
    public GameObject BridgeConnectorPrefab;
    public GameObject TrainTrackBridgePrefab;
    public GameObject tradeRoutePrefab;

    //Govenment assets
    public enum PolicyType { Military, Economic, Social, Industrial, Wildcard}
    public GameObject policySlotPrefab;// btn prefab
    public Transform slotParent;// the UI container
    public PolicySlot currentSlot;
    public Transform optionsSlotParent;
    public GameObject govenmentBtn;
    public GameObject governmentPrefab;

    //back grounds for policys
    public Sprite MilitaryBG;
    public Sprite EconomicBG;
    public Sprite SocialBG;
    public Sprite IndustrialBG;
    public Sprite WildcardBG;

    //More Govenment Sprites
    public Sprite GovenmentSprite;
    public Sprite MinistrySprite;
    public Transform MinistryParent;
    public GameObject ministryPrefab;
    public Ministrys CurrentMinistry;
    public Sprite GovernmentBuildingSprite;
    public Transform GovernmentBuildingParent;
    public GameObject GovernmentBuildingPrefab;
    public GovernmentBuildings CurrentGovernmentBuilding;

    [Header("train models")]

    public GameObject trainTrackRightPrefab;
    public GameObject trainTrackTopRightPrefab;
    public GameObject trainTrackTopLeftPrefab;
    public GameObject trainTrackLeftPrefab;
    public GameObject trainTrackBottomLeftPrefab;
    public GameObject trainTrackBottomRightPrefab;
    public GameObject turnStilePrefab;

    // Train track bridge prefabs
    public GameObject trainTrackBridgeRightPrefab;
    public GameObject trainTrackBridgeTopRightPrefab;
    public GameObject trainTrackBridgeTopLeftPrefab;
    public GameObject trainTrackBridgeLeftPrefab;
    public GameObject trainTrackBridgeBottomLeftPrefab;
    public GameObject trainTrackBridgeBottomRightPrefab;
    public GameObject trainTrackBridgeConnectorPrefab;

    [Header("Ministry Sprites")]
    public Sprite MinistryWarSprite;
    public Sprite MinistryTreasurySprite;
    public Sprite MinistryJusticeSprite;
    public Sprite MinistryCultureSprite;
    public Sprite MinistryInteriorSprite;
    public Sprite MinistryStateSprite;

    [Header("Government Sprites")]
    public Sprite MonarchySprite;
    public Sprite RepublicSprite;
    public Sprite TheocracySprite;
    public Sprite OligarchySprite;

    [Header("Government Building Sprites")]
    public Sprite CourtSprite;
    public Sprite AcademySprite;
    public Sprite TaxOfficeSprite;
    public Sprite MintSprite;
    public Sprite TheaterSprite;
    public Sprite ArchivesSprite;
    public Sprite NewsPaperOfficeSprite;

    [Header("Civic UI")]
    public List<GameObject> civicButtons; // drag from inspector

    public Sprite CivicNormalSprite;
    public Sprite CivicUnlockedSprite;
    public Sprite CivicNotEnoughSprite;
    public Sprite CivicLockedSprite; 

    [Header("Civic Display Panel")]
    public GameObject civicDisplayPanel;
    public TextMeshProUGUI civicTitleText;
    public TextMeshProUGUI civicUnlocksText;
    public Button civicUnlockButton;
    public Button civicCloseButton;

    [Header("Tech UI")]
    public List<GameObject> techButtons;   // drag buttons like civicButtons

    public Sprite TechNormalSprite;
    public Sprite TechUnlockedSprite;
    public Sprite TechNotEnoughSprite;
    public Sprite TechLockedSprite;

    public Sprite normalSprite;
    public Sprite notEnoughSprite;

    [Header("Wonders")]
    public GameObject eygptPopWonder;
    public GameObject eygptTradeWonder;
    public GameObject eygptExplorerWonder;
    public GameObject romePopWonder;
    public GameObject romeTradeWonder;
    public GameObject romeExplorerWonder;
    public GameObject greecePopWonder;
    public GameObject greeceTradeWonder;
    public GameObject greeceExplorerWonder;
    public GameObject persiaPopWonder;
    public GameObject persiaTradeWonder;
    public GameObject persiaExplorerWonder;

    public GameObject popWonderBtn;
    public GameObject tradeWonderBtn;
    public GameObject explorerWonderBtn;

    [Header("Troop info UI")]
    public Sprite ShieldSprite;
    public Sprite ColorSprite;
    public Sprite WallSprite;
    public GameObject damageTextPrefab;
    [SerializeField] private Material highlightMaterial;

    public GameObject endGamePanel;

    [Header("Random stuff")]
    public RectTransform contentRT;
    public GameObject btnContent;
    bool ignoreClicks = false;
    public Sprite loseImage;
    public Sprite victoryImage;
    public GameObject aiIsPlaying;
    public GameObject TribeDestroyedPanel;
    public GameObject desertCity;
    public GameObject desertdistrictTilePrefab;
    public Sprite buildSprite;

    //Coliders
    public GameObject colliderPrefab;
    private ClickHex[,] clickHexes;

    //New Game
    public void NewGame(){
        endGamePanel.SetActive(false);
        singlePlayerPanel.SetActive(true);
        gamePanel.SetActive(false);
        menuPanel.SetActive(false);
        players = new Player[1];
        
    }

    //Finds a tech with corrisponding btn name
    Tech GetTechFromButton(GameObject btn)
    {
        return techList.Find(t => t.Name == btn.name);
    }

    //Draws techs
    public void DrawTechs()
    {
        foreach (GameObject btnGO in techButtons)
        {
            Tech tech = GetTechFromButton(btnGO);
            if (tech == null)
                continue;

            Image img = btnGO.GetComponent<Image>();
            TextMeshProUGUI[] texts = btnGO.GetComponentsInChildren<TextMeshProUGUI>(true);

            TextMeshProUGUI nameText = texts.Length > 0 ? texts[0] : null;
            TextMeshProUGUI costText = texts.Length > 1 ? texts[1] : null;

            if (nameText != null)
                nameText.text = tech.Name;

            if (costText != null)
                costText.text = tech.Cost(currentPlayer, currentMap).ToString();

            bool unlocked = currentPlayer.unlockedTechs.Contains(tech);
            bool prereqsMet = tech.CanUnlock(currentPlayer);
            bool canAfford = currentPlayer.money >= tech.Cost(currentPlayer, currentMap);

            // RESET
            if (nameText != null) nameText.color = Color.white;
            if (costText != null)
            {
                costText.color = Color.white;
                costText.gameObject.SetActive(true);
            }

            if (unlocked)
            {
                img.sprite = TechUnlockedSprite;
                if (costText != null)
                    costText.gameObject.SetActive(false);
            }
            else if (!prereqsMet)
            {
                img.sprite = TechLockedSprite;
                if (nameText != null)
                    nameText.color = Color.grey;
                if (costText != null)
                    costText.gameObject.SetActive(false);
            }
            else if (!canAfford)
            {
                img.sprite = TechNotEnoughSprite;
                if (costText != null)
                    costText.color = Color.red;
            }
            else
            {
                img.sprite = TechNormalSprite;
            }
    
            Transform objectParent = btnGO.transform.Find("ObjectParent");
            if (objectParent != null)
            {
                objectParent.gameObject.SetActive(prereqsMet);
            }
            Button btn = btnGO.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                OnTechClicked(tech);
            });
        }
    }

    //Handles tech clicks
    public void OnTechClicked(Tech tech)
    {
        if (tech == null) return;

        civicDisplayPanel.SetActive(true);
        civicTitleText.text = tech.Name;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (!string.IsNullOrEmpty(tech.description))
        {
            sb.AppendLine(tech.description);
        }
        else
        {
            sb.Append("No description available");
        }

        civicUnlocksText.text = sb.ToString();

        bool canUnlock = !currentPlayer.unlockedTechs.Contains(tech) && tech.CanUnlock(currentPlayer) && currentPlayer.money >= tech.Cost(currentPlayer, currentMap);

        civicUnlockButton.interactable = canUnlock;

        civicUnlockButton.onClick.RemoveAllListeners();
        civicUnlockButton.onClick.AddListener(() =>
        {
            TryUnlockTech(tech);
            civicDisplayPanel.SetActive(false);
            ignoreClicks = false;
        });
    }

    //Trys to unlock the tech
    void TryUnlockTech(Tech tech)
    {
        if (tech == null) return;
        if (currentPlayer.unlockedTechs.Contains(tech)) return;
        if (!tech.CanUnlock(currentPlayer)) return;
        if (currentPlayer.money < tech.Cost(currentPlayer, currentMap)) return;

        currentPlayer.money -= tech.Cost(currentPlayer, currentMap);
        currentPlayer.unlockedTechs.Add(tech);

        DrawTechs();
        UpdateStats();
    }

    //Gets Civic from button
    Civic GetCivicFromButton(GameObject btn)
    {
        return civicList.Find(c => c.name == btn.name);
    }

    //Draws the civics
    public void DrawCivics()
    {
        foreach (GameObject btnGO in civicButtons)
        {
            Civic civic = GetCivicFromButton(btnGO);
            if (civic == null)
                continue;

            Image img = btnGO.GetComponent<Image>();
            TextMeshProUGUI[] texts = btnGO.GetComponentsInChildren<TextMeshProUGUI>(true);

            TextMeshProUGUI nameText = texts.Length > 0 ? texts[0] : null;
            TextMeshProUGUI costText = texts.Length > 1 ? texts[1] : null;

            if (nameText != null)
                nameText.text = civic.name;

            if (costText != null)
                costText.text = civic.Cost(currentPlayer).ToString();

            bool unlocked = currentPlayer.unlockedCivics.Contains(civic);
            bool prereqsMet = civic.CanUnlock(currentPlayer);
            Transform[] allChildren = btnGO.GetComponentsInChildren<Transform>(true);

    foreach (Transform t in allChildren)
    {
        if (t.name == "image1")
        {
            t.gameObject.SetActive(prereqsMet);
        }
    }
    RectTransform nameRT = nameText != null ? nameText.GetComponent<RectTransform>() : null;

    // store default Y once for govenment
    float defaultNameY = 55;   // set this to whatever your NORMAL Y is
    float lockedOffset = -15f;   // how much lower it moves when locked

    bool canAfford = currentPlayer.culture >= civic.Cost(currentPlayer);

    // RESET
    if (nameText != null) nameText.color = Color.white;
    if (costText != null)
    {
        costText.color = Color.white;
        costText.gameObject.SetActive(true);
    }
    if (!prereqsMet)
    {
        nameText.color = Color.grey;
    }
    else
    {
        if (nameText != null)
            nameText.color = Color.white;
    }
    if (unlocked)
    {
        img.overrideSprite = CivicUnlockedSprite;
        if (costText != null)
            costText.gameObject.SetActive(false);
    }
else if (!prereqsMet)
{
    img.sprite = CivicLockedSprite;
    if (costText != null)
        costText.gameObject.SetActive(false);
}
else if (!canAfford)
{
    img.sprite = CivicNotEnoughSprite;
    if (costText != null)
    {
        costText.gameObject.SetActive(true);
        costText.color = Color.red;
    }
}
else // prereqs met + can afford
{
    img.sprite = CivicNormalSprite;
    if (costText != null)
    {
        costText.gameObject.SetActive(true);
        costText.color = Color.white;
    }
}


        Button btn = btnGO.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            OnCivicClicked(civic);
        });
    }
}

public void OnCivicClicked(Civic civic)
{
    civicDisplayPanel.SetActive(true);
    civicTitleText.text = civic.name;

    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    if (civic.unlockedPolicies != null && civic.unlockedPolicies.Count > 0)
    {
        sb.AppendLine("Policies:");
        foreach (Policy p in civic.unlockedPolicies)
            sb.AppendLine($"• {p.name} — {p.description}");
        sb.AppendLine();
    }

    if (civic.unlockedGovernments != null && civic.unlockedGovernments.Count > 0)
    {
        sb.AppendLine("Governments:");
        foreach (Govenments g in civic.unlockedGovernments)
            sb.AppendLine($"• {g.name} — {g.description}");
        sb.AppendLine();
    }

    if (civic.unlockedMinistrys != null && civic.unlockedMinistrys.Count > 0)
    {
        sb.AppendLine("Ministries:");
        foreach (Ministrys m in civic.unlockedMinistrys)
            sb.AppendLine($"• {m.name} — {m.description}");
        sb.AppendLine();
    }

    if (civic.unlockedGovernmentBuildings != null && civic.unlockedGovernmentBuildings.Count > 0)
    {
        sb.AppendLine("Government Buildings:");
        foreach (GovernmentBuildings b in civic.unlockedGovernmentBuildings)
            sb.AppendLine($"• {b.name} — {b.description}");
    }

    if (sb.Length == 0)
        sb.Append("No direct unlocks");

    civicUnlocksText.text = sb.ToString();

    bool canUnlock =
        !currentPlayer.unlockedCivics.Contains(civic) &&
        civic.CanUnlock(currentPlayer) &&
        currentPlayer.culture >= civic.Cost(currentPlayer);

    civicUnlockButton.interactable = canUnlock;

    civicUnlockButton.onClick.RemoveAllListeners();
    civicUnlockButton.onClick.AddListener(() =>
    {
        UnlockCivic(civic);
        civicDisplayPanel.SetActive(false);
        ignoreClicks = false;
    });
}

void UnlockCivic(Civic civic)
{
    if (!civic.CanUnlock(currentPlayer)) return;
    if (currentPlayer.culture < civic.Cost(currentPlayer)) return;
    if (currentPlayer.unlockedCivics.Contains(civic)) return;

    currentPlayer.culture -= civic.Cost(currentPlayer);
    currentPlayer.unlockedCivics.Add(civic);

    if (civic.unlockedPolicies != null)
        currentPlayer.unlockedPolicys.AddRange(civic.unlockedPolicies);

    if (civic.unlockedGovernments != null)
        currentPlayer.unlockedGovernments.AddRange(civic.unlockedGovernments);

    if (civic.unlockedMinistrys != null)
        currentPlayer.unlockedMinistrys.AddRange(civic.unlockedMinistrys);

    if (civic.unlockedGovernmentBuildings != null)
        currentPlayer.unlockedGovernmentBuildings.AddRange(civic.unlockedGovernmentBuildings);

    DrawCivics();
    drawPolicy();
    checkIfExpMonument();
    checkIfPopMonument();
    checkIfTradeMonument();
}
public void CloseCivicPanel()
{
    civicDisplayPanel.SetActive(false);
    ignoreClicks = false;
}

public void drawPolicy()
{
    // Clear old slots
    foreach (Transform t in slotParent)
        Destroy(t.gameObject);
int policyCount = currentPlayer.currentGovernment.policySlots.Count;
float xOffset= 0f;
xOffset -= ((100f * policyCount) / 2)-50;

    float spacingX = 100f;

    foreach (var slot in currentPlayer.currentGovernment.policySlots)
    {
        GameObject slotGO = Instantiate(policySlotPrefab);
        slotGO.transform.SetParent(slotParent, false);

        RectTransform rt = slotGO.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(xOffset, 50);
        xOffset += spacingX;

        // Background sprite
        Image img = slotGO.GetComponent<Image>();
        if (img != null)
        {
            switch (slot.type)
            {
                case PolicySlot.PolicyType.Military: img.sprite = MilitaryBG; break;
                case PolicySlot.PolicyType.Economic: img.sprite = EconomicBG; break;
                case PolicySlot.PolicyType.Social: img.sprite = SocialBG; break;
                case PolicySlot.PolicyType.Industrial: img.sprite = IndustrialBG; break;
                case PolicySlot.PolicyType.Wildcard: img.sprite = WildcardBG; break;

            }
        }

        // Text
        TextMeshProUGUI text = slotGO.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = slot.activePolicy != null ? slot.activePolicy.name : "Empty";
        TextMeshProUGUI[] texts = slotGO.GetComponentsInChildren<TextMeshProUGUI>(true);

        TextMeshProUGUI nameText = texts.Length > 0 ? texts[0] : null;
        TextMeshProUGUI decText = texts.Length > 1 ? texts[1] : null;
        nameText.text = slot.activePolicy != null ? slot.activePolicy.name : "Empty";
        decText.text = slot.activePolicy != null ? slot.activePolicy.description : "";

        
        // Button click
        Button btn = slotGO.GetComponent<Button>();
        btn.onClick.AddListener(() => OnPolicySlotClicked(slot));
    }

    Image img1 = govenmentBtn.GetComponent<Image>();
RectTransform rtGov = govenmentBtn.GetComponent<RectTransform>();

img1.sprite = GetGovernmentSprite(currentPlayer.currentGovernment);
rtGov.sizeDelta = GetGovernmentSize(currentPlayer.currentGovernment);
rtGov.anchoredPosition = new Vector2(0,GetGovernmentY(currentPlayer.currentGovernment));

    TextMeshProUGUI text1 = slotParent.GetComponentInChildren<TextMeshProUGUI>();
    if (text1 != null)
        text1.text = currentPlayer.currentGovernment != null ? currentPlayer.currentGovernment.name : "Empty";

    int ministryCount = currentPlayer.currentGovernment.ministrysPolicies.Count;
      //  int ministryCount = 1;

    float ministryOffset= 0f;
    ministryOffset -= ((400f * ministryCount) / 2)-200;


    foreach (Transform t in MinistryParent)
        Destroy(t.gameObject);

    foreach (Transform t in GovernmentBuildingParent)
        Destroy(t.gameObject);

    foreach (Ministrys ministry in currentPlayer.currentGovernment.ministrysPolicies)
    {
        GameObject ministryGO = Instantiate(ministryPrefab);
        ministryGO.transform.SetParent(MinistryParent, false);

        RectTransform rt = ministryGO.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(ministryOffset, -50f);
        
        Image img2 = ministryGO.GetComponent<Image>();
        img2.sprite = GetMinistrySprite(ministry);

        rt.sizeDelta = GetMinistrySize(ministry);

        TextMeshProUGUI text2 = ministryGO.GetComponentInChildren<TextMeshProUGUI>();
        if (text2 != null)
            text2.text = ministry != null ? ministry.name : "Empty";
        
        Button btn = ministryGO.GetComponent<Button>();
        btn.onClick.AddListener(() => OnMinistryClicked(ministry));
        //xOffset = 0f;
        
        policyCount = ministry.policySlots.Count;
        xOffset= ministryOffset;
        xOffset -= ((100f * policyCount) / 2)-50;

        foreach (var slot in ministry.policySlots)
        {
            GameObject slotGO = Instantiate(policySlotPrefab);
            slotGO.transform.SetParent(MinistryParent, false);

            RectTransform rt1 = slotGO.GetComponent<RectTransform>();
            rt1.anchoredPosition = new Vector2(xOffset, -67f);
            xOffset += spacingX;

            // Background sprite
            Image img = slotGO.GetComponent<Image>();
            if (img != null)
            {
                switch (slot.type)
                {
                    case PolicySlot.PolicyType.Military: img.sprite = MilitaryBG; break;
                    case PolicySlot.PolicyType.Economic: img.sprite = EconomicBG; break;
                    case PolicySlot.PolicyType.Social: img.sprite = SocialBG; break;
                    case PolicySlot.PolicyType.Industrial: img.sprite = IndustrialBG; break;
                    case PolicySlot.PolicyType.Wildcard: img.sprite = WildcardBG; break;

                }
            }

            // Text
            TextMeshProUGUI text4 = slotGO.GetComponentInChildren<TextMeshProUGUI>();
            if (text4 != null)
                text4.text = slot.activePolicy != null ? slot.activePolicy.name : "Empty";
        TextMeshProUGUI[] texts = slotGO.GetComponentsInChildren<TextMeshProUGUI>(true);

        TextMeshProUGUI nameText1 = texts.Length > 0 ? texts[0] : null;
        TextMeshProUGUI decText1 = texts.Length > 1 ? texts[1] : null;
        nameText1.text = slot.activePolicy != null ? slot.activePolicy.name : "Empty";
        decText1.text = slot.activePolicy != null ? slot.activePolicy.description : "";
            // Button click
            Button btn4 = slotGO.GetComponent<Button>();
            btn4.onClick.AddListener(() => OnPolicySlotClicked(slot));
        }
        //xOffset = 0f;
        int govermentBuildingCount = ministry.activeGovernmentBuildings.Count;
        float govermentBuildingOffset= ministryOffset;
        govermentBuildingOffset -= ((150f * govermentBuildingCount) / 2)-75;
        foreach(GovernmentBuildings governmentBuilding in ministry.activeGovernmentBuildings){
            GameObject GovernmentBuildingsGO = Instantiate(GovernmentBuildingPrefab);
            GovernmentBuildingsGO.transform.SetParent(GovernmentBuildingParent, false);
            
            RectTransform rt3 = GovernmentBuildingsGO.GetComponent<RectTransform>();
rt3.anchoredPosition = new Vector2(
    govermentBuildingOffset,
    GetGovernmentBuildingY(governmentBuilding)
);
        
            Image img5 = GovernmentBuildingsGO.GetComponent<Image>();
img5.sprite = GetGovernmentBuildingSprite(governmentBuilding);
rt3.sizeDelta = GetGovernmentBuildingSize(governmentBuilding);

            TextMeshProUGUI text5 = GovernmentBuildingsGO.GetComponentInChildren<TextMeshProUGUI>();
            if (text5 != null)
            text5.text = governmentBuilding != null ? governmentBuilding.name : "Empty";
        
            Button btn5 = GovernmentBuildingsGO.GetComponent<Button>();
            //btn5.onClick.AddListener(() => OnGovernmentBuildingClicked(governmentBuilding));
            btn5.onClick.AddListener(() =>
            {
                CurrentMinistry = ministry; //  THIS WAS MISSING
                OnGovernmentBuildingClicked(governmentBuilding);
            });

            policyCount = governmentBuilding.policySlots.Count;
            xOffset= govermentBuildingOffset;
            xOffset -= ((100f * policyCount) / 2)-50;
            foreach (var slot in governmentBuilding.policySlots)
            {
                GameObject slotGO = Instantiate(policySlotPrefab);
                slotGO.transform.SetParent(GovernmentBuildingParent, false);

                RectTransform rt1 = slotGO.GetComponent<RectTransform>();
                rt1.anchoredPosition = new Vector2(xOffset, -50f);
                xOffset += spacingX;

                // Background sprite
                Image img = slotGO.GetComponent<Image>();
                if (img != null)
                {
                    switch (slot.type)
                    {
                        case PolicySlot.PolicyType.Military: img.sprite = MilitaryBG; break;
                        case PolicySlot.PolicyType.Economic: img.sprite = EconomicBG; break;
                        case PolicySlot.PolicyType.Social: img.sprite = SocialBG; break;
                        case PolicySlot.PolicyType.Industrial: img.sprite = IndustrialBG; break;
                        case PolicySlot.PolicyType.Wildcard: img.sprite = WildcardBG; break;

                    }
                }

                // Text
                TextMeshProUGUI text4 = slotGO.GetComponentInChildren<TextMeshProUGUI>();
                if (text4 != null)
                    text4.text = slot.activePolicy != null ? slot.activePolicy.name : "Empty";
        TextMeshProUGUI[] texts = slotGO.GetComponentsInChildren<TextMeshProUGUI>(true);

        TextMeshProUGUI nameText2 = texts.Length > 0 ? texts[0] : null;
        TextMeshProUGUI decText2 = texts.Length > 1 ? texts[1] : null;
        nameText2.text = slot.activePolicy != null ? slot.activePolicy.name : "Empty";
        decText2.text = slot.activePolicy != null ? slot.activePolicy.description : "";
                // Button click
                Button btn4 = slotGO.GetComponent<Button>();
                btn4.onClick.AddListener(() => OnPolicySlotClicked(slot));
            }
            govermentBuildingOffset += 150;

        }
        ministryOffset += 400;

    }
    //Draw();
    UpdateStats();

}
public void OnGovernmentBuildingClicked(GovernmentBuildings governmentBuilding){

    foreach (Transform t in optionsSlotParent)
        Destroy(t.gameObject);
    CurrentGovernmentBuilding = governmentBuilding;
int optionCount = currentPlayer.unlockedGovernmentBuildings
    .Where(b => !IsGovernmentBuildingAlreadyUsed(b))
    .Count();

float spacingX = 150f;
float xOffset = GetCenteredOffset(optionCount, spacingX);

    foreach (GovernmentBuildings governmentBuildings in currentPlayer.unlockedGovernmentBuildings)
    {
    if (IsGovernmentBuildingAlreadyUsed(governmentBuildings))
        continue;
        //GameObject optionGO = Instantiate(GovernmentBuildingParent);
            GameObject optionGO = Instantiate(GovernmentBuildingPrefab, optionsSlotParent, false);

       // optionGO.transform.SetParent(optionsSlotParent, false);

        RectTransform rt = optionGO.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(xOffset, 0f);
        xOffset += spacingX;

        Image img = optionGO.GetComponent<Image>();
img.sprite = GetGovernmentBuildingSprite(governmentBuildings);
rt.sizeDelta = GetGovernmentBuildingSize(governmentBuildings);
 img = optionGO.GetComponent<Image>();
 rt = optionGO.GetComponent<RectTransform>();

ApplyVisual(
    img,
    rt,
    GetGovernmentBuildingSprite(governmentBuildings),
    GetGovernmentBuildingSize(governmentBuildings),
    GetGovernmentBuildingY(governmentBuildings),
    0.8f
);

        TextMeshProUGUI text = optionGO.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = governmentBuildings.name;

        TextMeshProUGUI[] texts = optionGO.GetComponentsInChildren<TextMeshProUGUI>(true);

        TextMeshProUGUI nameText3 = texts.Length > 0 ? texts[0] : null;
        TextMeshProUGUI costText3 = texts.Length > 1 ? texts[1] : null;
        nameText3.text = governmentBuildings.name ;
        costText3.gameObject.SetActive(true);

        costText3.text = governmentBuildings.cost.ToString();

        Button btn = optionGO.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
                if (IsGovernmentBuildingAlreadyUsed(governmentBuildings)) return;
    if (currentPlayer.culture < governmentBuildings.cost) return;

    currentPlayer.culture -= governmentBuildings.cost;
    int index = CurrentMinistry.activeGovernmentBuildings
        .IndexOf(CurrentGovernmentBuilding);

    if (index != -1)
    {
        CurrentMinistry.activeGovernmentBuildings[index] = governmentBuildings;
    }            drawPolicy();
                OnGovernmentBuildingClicked(governmentBuildings);
        });
    }
}
public void OnMinistryClicked(Ministrys ministry){
    foreach (Transform t in optionsSlotParent)
        Destroy(t.gameObject);
    CurrentMinistry = ministry;
int optionCount = currentPlayer.unlockedMinistrys
    .Where(m => !IsMinistryAlreadyUsed(m))
    .Count();

float spacingX = 300f;
float xOffset = GetCenteredOffset(optionCount, spacingX);

    foreach (Ministrys ministrys in currentPlayer.unlockedMinistrys)
    {
    if (IsMinistryAlreadyUsed(ministrys))
        continue;
        GameObject optionGO = Instantiate(ministryPrefab);
        optionGO.transform.SetParent(optionsSlotParent, false);

        RectTransform rt = optionGO.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(xOffset, 0f);
        xOffset += spacingX;

        Image img = optionGO.GetComponent<Image>();
        img.sprite = MinistrySprite;
 img = optionGO.GetComponent<Image>();
 rt = optionGO.GetComponent<RectTransform>();

ApplyVisual(
    img,
    rt,
    GetMinistrySprite(ministrys),
    GetMinistrySize(ministrys),
    0f,
    0.75f
);

        TextMeshProUGUI text = optionGO.GetComponentInChildren<TextMeshProUGUI>();
        
        if (text != null)
            text.text = ministrys.name;
        TextMeshProUGUI[] texts = optionGO.GetComponentsInChildren<TextMeshProUGUI>(true);

        TextMeshProUGUI nameText3 = texts.Length > 0 ? texts[0] : null;
        TextMeshProUGUI costText3 = texts.Length > 1 ? texts[1] : null;
        nameText3.text = ministrys.name ;
        costText3.gameObject.SetActive(true);

        costText3.text = ministrys.cost.ToString();

        Button btn = optionGO.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
                if (IsMinistryAlreadyUsed(ministrys)) return;
    if (currentPlayer.culture < ministrys.cost) return;
ministrys.activeGovernmentBuildings.Clear();
for (int i = 0; i < ministrys.buildingSlots; i++)
{
    ministrys.activeGovernmentBuildings.Add(
        new GovernmentBuildings("Empty Building", 0, "No Building assigned")
    );
}
    currentPlayer.culture -= ministrys.cost;
            //CurrentMinistry= ministrys;
                int index = currentPlayer.currentGovernment.ministrysPolicies.IndexOf(CurrentMinistry);
    if (index != -1)
    {
        currentPlayer.currentGovernment.ministrysPolicies[index] = ministrys;
    }
            drawPolicy();
            OnMinistryClicked(ministry);
        });
    }
}
public void ChangeGovernemt(){
    foreach (Transform t in optionsSlotParent)
        Destroy(t.gameObject);

int optionCount = currentPlayer.unlockedGovernments
    .Where(g => g != currentPlayer.currentGovernment)
    .Count();

float spacingX = 450f;
float xOffset = GetCenteredOffset(optionCount, spacingX);


    foreach (Govenments goverment in currentPlayer.unlockedGovernments)
    {
        if(goverment == currentPlayer.currentGovernment) continue;
        GameObject optionGO = Instantiate(governmentPrefab);
        optionGO.transform.SetParent(optionsSlotParent, false);

        RectTransform rt = optionGO.GetComponent<RectTransform>();
rt.anchoredPosition = new Vector2(xOffset, 0f);
xOffset += spacingX;

        Image img = optionGO.GetComponent<Image>();
        img.sprite = GovenmentSprite;

        TextMeshProUGUI text = optionGO.GetComponentInChildren<TextMeshProUGUI>();
         img = optionGO.GetComponent<Image>();
 rt = optionGO.GetComponent<RectTransform>();

        TextMeshProUGUI[] texts = optionGO.GetComponentsInChildren<TextMeshProUGUI>(true);

        TextMeshProUGUI nameText3 = texts.Length > 0 ? texts[0] : null;
        TextMeshProUGUI costText3 = texts.Length > 1 ? texts[1] : null;
        nameText3.text = goverment.name ;
        costText3.gameObject.SetActive(true);

        costText3.text = goverment.cost.ToString();
        
ApplyVisual(
    img,
    rt,
    GetGovernmentSprite(goverment),
    GetGovernmentSize(goverment),
    0f,
    0.7f  
);

        if (text != null)
            text.text = goverment.name;

        Button btn = optionGO.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            if (currentPlayer.culture < goverment.cost) return;

            currentPlayer.culture -= goverment.cost;
            currentPlayer.currentGovernment.ministrysPolicies.Clear();
            for (int i = 0; i <currentPlayer.currentGovernment.ministrySlots; i++)
            {
                currentPlayer.currentGovernment.ministrysPolicies.Add(currentPlayer.currentGovernment.CreateEmptyMinistry());
            }
            currentPlayer.currentGovernment = goverment;
            drawPolicy();
            ChangeGovernemt();
        });
    }
}
void OnPolicySlotClicked(PolicySlot slot)
{
    currentSlot = slot;

    switch (slot.type)
    {
        case PolicySlot.PolicyType.Military:
            DisplayMilitaryPolicyOptions();
            break;

        case PolicySlot.PolicyType.Economic:
            DisplayEconomicPolicyOptions();
            break;

        case PolicySlot.PolicyType.Social:
            DisplaySocialPolicyOptions();
            break;

        case PolicySlot.PolicyType.Industrial:
            DisplayIndustrialPolicyOptions();
            break;
        
        case PolicySlot.PolicyType.Wildcard:
            DisplayWildCardPolicyOptions();
            break;
    }
}
public void DisplayMilitaryPolicyOptions()
{
    DisplayPolicyOptions(Policy.PolicyType.Military);
}

public void DisplayEconomicPolicyOptions()
{
    DisplayPolicyOptions(Policy.PolicyType.Economic);
}

public void DisplaySocialPolicyOptions()
{
    DisplayPolicyOptions(Policy.PolicyType.Social);
}

public void DisplayIndustrialPolicyOptions()
{
    DisplayPolicyOptions(Policy.PolicyType.Industrial);
}
public void DisplayWildCardPolicyOptions()
{
    DisplayPolicyOptions(Policy.PolicyType.Wildcard);

}
void DisplayPolicyOptions(Policy.PolicyType type)
{
    foreach (Transform t in optionsSlotParent)
        Destroy(t.gameObject);

int optionCount = currentPlayer.unlockedPolicys
    .Where(p => (type == Policy.PolicyType.Wildcard || p.type == type) && !IsPolicyAlreadyUsed(p))
    .Count();

float spacingX = 100f;
float xOffset = GetCenteredOffset(optionCount, spacingX);

    foreach (Policy policy in currentPlayer.unlockedPolicys)
    {
        if (type != Policy.PolicyType.Wildcard && policy.type != type)
            continue;
        if (IsPolicyAlreadyUsed(policy))
            continue;
        GameObject optionGO = Instantiate(policySlotPrefab);
        optionGO.transform.SetParent(optionsSlotParent, false);

        RectTransform rt = optionGO.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(xOffset, 0f);
        xOffset += spacingX;

        Image img = optionGO.GetComponent<Image>();
        if (img != null)
        {
            switch (policy.type)
            {
                case Policy.PolicyType.Military: img.sprite = MilitaryBG; break;
                case Policy.PolicyType.Economic: img.sprite = EconomicBG; break;
                case Policy.PolicyType.Social: img.sprite = SocialBG; break;
                case Policy.PolicyType.Industrial: img.sprite = IndustrialBG; break;
                case Policy.PolicyType.Wildcard: img.sprite = WildcardBG; break;

            }
        }

        TextMeshProUGUI text = optionGO.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = policy.name;

        TextMeshProUGUI[] texts = optionGO.GetComponentsInChildren<TextMeshProUGUI>(true);

        TextMeshProUGUI nameText3 = texts.Length > 0 ? texts[0] : null;
        TextMeshProUGUI decText3 = texts.Length > 1 ? texts[1] : null;
        nameText3.text = policy.name ;
        decText3.text =  policy.description;
        TextMeshProUGUI costText = texts.Length > 2 ? texts[2] : null;
        costText.gameObject.SetActive(true);

        costText.text = policy.cost.ToString();
        
        Button btn = optionGO.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
                if (IsPolicyAlreadyUsed(policy)) return;
    if (currentPlayer.culture < policy.cost) return;

    currentPlayer.culture -= policy.cost;
            currentSlot.activePolicy = policy;
            drawPolicy();
            DisplayPolicyOptions(type);
        });
    }
}
Sprite GetMinistrySprite(Ministrys ministry)
{
    switch (ministry.name)
    {
        case "Ministry of War": return MinistryWarSprite;
        case "Treasury": return MinistryTreasurySprite;
        case "Ministry of Justice": return MinistryJusticeSprite;
        case "Ministry of Culture": return MinistryCultureSprite;
        case "Ministry of Interior": return MinistryInteriorSprite;
        case "Ministry of State": return MinistryStateSprite;
        default: return MinistrySprite; // fallback
    }
}
Vector2 GetMinistrySize(Ministrys ministry)
{
    switch (ministry.name)
    {
        case "Ministry of War":
            return new Vector2(210, 135);

        case "Treasury":
            return new Vector2(300, 135);

        case "Ministry of Justice":
            return new Vector2(300, 135);

        case "Ministry of Culture":
            return new Vector2(300, 135);

        case "Ministry of Interior":
            return new Vector2(210, 135);

        case "Ministry of State":
            return new Vector2(300, 135);

        default:
            return new Vector2(210, 135);
    }
}
Sprite GetGovernmentSprite(Govenments government)
{
    switch (government.name)
    {
        case "Monarchy": return MonarchySprite;
        case "Republic": return RepublicSprite;
        case "Theocracy": return TheocracySprite;
        case "Oligarchy": return OligarchySprite;
        default: return GovenmentSprite;
    }
}

Vector2 GetGovernmentSize(Govenments government)
{
    switch (government.name)
    {
        case "Monarchy": return new Vector2(400, 168);
        case "Republic": return new Vector2(200, 150);
        case "Theocracy": return new Vector2(400, 250);
        case "Oligarchy": return new Vector2(700, 250);
        default: return new Vector2(463, 229);
    }
}
Sprite GetGovernmentBuildingSprite(GovernmentBuildings building)
{
    switch (building.name)
    {
        case "Court": return CourtSprite;
        case "Academy": return AcademySprite;
        case "Tax office": return TaxOfficeSprite;
        case "Mint": return MintSprite;
        case "Theater": return TheaterSprite;
        case "Archives": return ArchivesSprite;
        case "Press Office": return NewsPaperOfficeSprite;
        default: return GovernmentBuildingSprite;
    }
}

Vector2 GetGovernmentBuildingSize(GovernmentBuildings building)
{
    switch (building.name)
    {
        case "Court": return new Vector2(100, 136);
        case "Academy": return new Vector2(100, 157);
        case "Tax office": return new Vector2(100, 138);
        case "Mint": return new Vector2(100, 134);
        case "Theater": return new Vector2(100, 120);
        case "Archives": return new Vector2(100, 130);
        case "Press Office": return new Vector2(100, 163);
        default: return new Vector2(100, 120);
    }
}
float GetGovernmentY(Govenments government)
{
    switch (government.name)
    {
        case "Monarchy":
            return 230f;   // tall, sits a bit higher

        case "Republic":
            return 221f;   // balanced

        case "Theocracy":
            return 270f;   // slightly lower

        case "Oligarchy":
            return 270f;   // compact but wide

        default:
            return 260;
    }
}

float GetGovernmentBuildingY(GovernmentBuildings building)
{
    switch (building.name)
    {
        case "Court":
            return -31f;

        case "Academy":
            return -22f;

        case "Tax office":
            return -31f;

        case "Mint":
            return -30f;

        case "Theater":
            return -38f;

        case "Archives":
            return -33f;

        case "Press Office":
            return -18f;

        default:
            return -38f;
    }
}
bool IsPolicyAlreadyUsed(Policy policy)
{
    foreach (var slot in currentPlayer.currentGovernment.policySlots)
        if (slot.activePolicy == policy) return true;

    foreach (var ministry in currentPlayer.currentGovernment.ministrysPolicies)
        foreach (var slot in ministry.policySlots)
            if (slot.activePolicy == policy) return true;

    foreach (var ministry in currentPlayer.currentGovernment.ministrysPolicies)
        foreach (var building in ministry.activeGovernmentBuildings)
            foreach (var slot in building.policySlots)
                if (slot.activePolicy == policy) return true;

    return false;
}

bool IsMinistryAlreadyUsed(Ministrys ministry)
{
    return currentPlayer.currentGovernment.ministrysPolicies.Contains(ministry);
}

bool IsGovernmentBuildingAlreadyUsed(GovernmentBuildings building)
{
    foreach (var ministry in currentPlayer.currentGovernment.ministrysPolicies)
        if (ministry.activeGovernmentBuildings.Contains(building))
            return true;

    return false;
}
void ApplyVisual(
    Image img,
    RectTransform rt,
    Sprite sprite,
    Vector2 baseSize,
    float baseY,
    float scale = 1f
)
{
    img.sprite = sprite;
    rt.sizeDelta = baseSize * scale;
    rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, baseY * scale);
}
float GetCenteredOffset(int count, float spacing)
{
    return -((spacing * count) / 2f) + (spacing / 2f);
}
    
    // ------------------- Game Setup -------------------
    public void StartGame()
    {
        singlePlayerPanel.SetActive(false);
        gamePanel.SetActive(true);
       //players.Clear();
        players[0] = new Player("Eygpt", 10000, true, new Color(0.76f, 0.70f, 0.50f), Color.blue, 10000);   // Human player
        //players[1] = new Player("China", 10000, false, Color.red, new Color(1.0f, 0.84f, 0.0f), 5);  // AI player
        GenerateTechTree();

        currentMap = new Map(10, 10, players);
        currentMap.SpawnMap();
        currentPlayer = players[0]; // Start with first player
                GenerateCivicsTree();
        cameraController.SetMapBounds(currentMap.width, currentMap.height);
        foreach(Player p in players){
            p.intializeClouds(currentMap.width, currentMap.height);
        }
        NextTurn();
        SpawnHexColliders();

        Draw(true, true, true, true, true, true, true, true, true, true, true);
    }

    public void Start(){
        //players.Clear();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        ReturnBtn.SetActive(false);
        gamePanel.SetActive(false);
        //tileClickedPanel.SetActive(false);
        AddWarriorBtn.SetActive(false);
        AddHorsemanBtn.SetActive(false);
        AddPastureBtn.SetActive(false);
        techTree.SetActive(false);
        socialTechnologyTree.SetActive(false);
        government.SetActive(false);
        menuPanel.SetActive(false);
        statsPanel.SetActive(false);
        singlePlayerPanel.SetActive(true);

        StartCoroutine(PeriodicCleanup());

        civicCloseButton.onClick.AddListener(() =>
        {
            civicDisplayPanel.SetActive(false);
            ignoreClicks = false;
        });
        intializeCivs();
        SetupGameSetupUI();
    }
    IEnumerator PeriodicCleanup()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f); // wait 60 seconds
            System.GC.Collect();
            Resources.UnloadUnusedAssets();
            Debug.Log("Memory cleanup executed at: " + Time.time);
        }
    }
    void SetupGameSetupUI()
    {
        // CIV DROPDOWN
        civDropdown.ClearOptions();
        civDropdown.AddOptions(civs.Select(c => c.name).ToList());

        // AI SLIDER
        aiCountSlider.minValue = 1;
        aiCountSlider.maxValue = 3;
        aiCountSlider.wholeNumbers = true;

        aiCountSlider.onValueChanged.AddListener(v =>
        {
            aiCountText.text = $"AI Players: {(int)v}";
        });
        aiDifficultySlider.onValueChanged.AddListener(v =>
{
    int difficulty = Mathf.RoundToInt(v);
    string difficultyName = difficulty switch
    {
        1 => "Easy",
        2 => "Normal",
        3 => "Hard",
        4 => "Expert",
        5 => "Extreme",
        _ => "Unknown"
    };

    aiDifficultyText.text = $"Difficulty: {difficultyName}";
});
    aiDifficultyText.text = $"Difficulty: Easy";

    aiCountText.text = $"AI Players: {(int)aiCountSlider.value}";
}
public void StartGameFromMenu()
{
    //players.Clear();
    singlePlayerPanel.SetActive(false);
    gamePanel.SetActive(true);
    turn = 0;
    int aiCount = (int)aiCountSlider.value;
     aiDifficulty = (int)aiDifficultySlider.value;
    players = new Player[aiCount + 1];

    // --- HUMAN PLAYER ---
    Civ selectedCiv = civs[civDropdown.value];
    players[0] = new Player(
        selectedCiv.name,
        3,
        true,
        selectedCiv.primaryColor,
        selectedCiv.secondaryColor,
        4
    );

    // --- AI PLAYERS (NO DUPLICATES) ---
    List<Civ> availableCivs = new List<Civ>(civs);
    availableCivs.Remove(selectedCiv);

    for (int i = 1; i <= aiCount; i++)
    {
        int r = Random.Range(0, availableCivs.Count);
        Civ aiCiv = availableCivs[r];
        availableCivs.RemoveAt(r);

        players[i] = new Player(
            aiCiv.name,
            3,
            false,
            aiCiv.primaryColor,
            aiCiv.secondaryColor,
            4
        );
    }

    // --- START GAME ---
    GenerateTechTree();
    GenerateCivicsTree();

    currentMap = new Map(5 + (players.Length*5), 5 + (players.Length*5), players);
    currentMap.SpawnMap();

    currentPlayer = players[0];
    humanPlayer= players[0];
    cameraController.SetMapBounds(currentMap.width, currentMap.height);

    foreach (Player p in players)
        p.intializeClouds(currentMap.width, currentMap.height);

    FirstTurn();
    Draw(true, true, true, true, true, true, true, true, true, true, true);
        SpawnHexColliders();

}
private void FirstTurn(){

            currentPlayer.money += countMoney();
            currentPlayer.moneyMade += countMoney();
        currentPlayer.culture += countCulture();
                currentPlayer.cultureMade += countCulture();

        // Reset move/attack status of all current player's troops
        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile tile = currentMap.tiles[x, y];
                if (tile.unit != null && tile.unit.owner == currentPlayer)
                {
                    tile.unit.hasMoved = false;
                    tile.unit.hasAttacked = false;
                }
                if (tile.district != null&&tile.district.building != null &&tile.district.building is Palace && tile.owner == currentPlayer){
                    capitalTile = tile;
                }
            }
        }
        if(currentPlayer.isPlayer == true)
        ExploreDouble(currentPlayer, capitalTile);
int currentIndex1 = System.Array.IndexOf(players, currentPlayer);

// If last player just finished → new turn
if (currentIndex1 == players.Length - 1)
{
    turn++;
   // UpdateTurnText();
}
cameraController.FocusOnCapital(capitalTile);

    if (turnText != null)
        turnText.text = "Turn: " + turn;

        if(currentPlayer.isPlayer == false){
              StartCoroutine(RunAi());
                    StartCoroutine(PauseAndResumeRoutine());

        }
        Draw(true, true, true, true, true,true, true, true, true, true, true);
}
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && ignoreClicks == false)
        {
if (EventSystem.current.IsPointerOverGameObject())
    return;
    var coords = GetClickedHexCoordinates();

if (coords is (int x, int y))
{
    currentTile = currentMap.tiles[x, y];//null on this line
}
            DetectTileClick();
            
        }
    }
    public void intializeCivs(){
        civs.Add(new Civ("Eygpt", new Color(0.76f, 0.70f, 0.50f), Color.blue));

    civs.Add(new Civ(
        "Rome",
        new Color(0.60f, 0.10f, 0.10f),   // imperial red
        new Color(0.85f, 0.75f, 0.20f)    // gold
    ));

    civs.Add(new Civ(
        "Greece",
        new Color(0.15f, 0.35f, 0.75f),   // Aegean blue
        Color.white                      // marble
    ));

    civs.Add(new Civ(
        "Persia",
        new Color(0.45f, 0.10f, 0.55f),   // royal purple
        new Color(0.90f, 0.75f, 0.30f)    // imperial gold
    ));
    }
    // ------------------- Turn Management -------------------
    public void NextTurn()
    {
        if (isAITurn&&currentPlayer.isPlayer==true)
            return;
        int currentIndex = System.Array.IndexOf(players, currentPlayer);
        currentIndex = (currentIndex + 1) % players.Length;
        currentPlayer = players[currentIndex];


        currentPlayer.money += countMoney();
                currentPlayer.moneyMade += countMoney();

        currentPlayer.culture += countCulture();
        currentPlayer.cultureMade += countCulture();

        // Reset move/attack status of all current player's troops
        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile tile = currentMap.tiles[x, y];
                if (tile.unit != null && tile.unit.owner == currentPlayer)
                {
                    tile.unit.hasMoved = false;
                    tile.unit.hasAttacked = false;
                }
                if (tile.district != null&&tile.district.building != null &&tile.district.building is Palace && tile.owner == currentPlayer){
                    capitalTile = tile;
                }
            }
        }
        if(currentPlayer.isPlayer == true)
        ExploreDouble(currentPlayer, capitalTile);
int currentIndex1 = System.Array.IndexOf(players, currentPlayer);

// If last player just finished → new turn
if (currentIndex1 == players.Length - 1)
{
    turn++;
    System.GC.Collect(); 
    Resources.UnloadUnusedAssets();
   // UpdateTurnText();
}
//if(turn==1)        cameraController.FocusOnCapital(capitalTile);

    if (turnText != null)
        turnText.text = "Turn: " + turn;

        if(currentPlayer.isPlayer == false){
            //isAITurn = true;
              StartCoroutine(RunAi());
                    StartCoroutine(PauseAndResumeRoutine());

        }    else{
        //isAITurn = false;
    }
       Draw(        true, true, true, true, true,
        true, true, true, true, true, true);
    }

    IEnumerator PauseAndResumeRoutine()
    {
        // Wait for 3 seconds of real time
        yield return new WaitForSeconds(1f);
    }

IEnumerator ShowAIBuildVisual(Tile tile)
{

    // ---------- VISUAL ----------
    Vector3 pos = CalculateHexPosition(tile.x, tile.y) + Vector3.up * 0.5f;

    GameObject buildObj = new GameObject("BuildVisual");
    SpriteRenderer sr = buildObj.AddComponent<SpriteRenderer>();
    sr.sprite = buildSprite;

    buildObj.transform.position = pos;
    buildObj.transform.localScale = Vector3.one * 0.25f;
    buildObj.transform.rotation = Quaternion.Euler(45f, 0, 0);

    SetLayerRecursively(buildObj, LayerMask.NameToLayer("TroopLayer"));

    // ---------- FLOAT ANIMATION ----------
    float duration = 0.4f;
    float t = 0f;
    Vector3 start = pos;
    Vector3 end = pos + Vector3.up * 0.5f;

    while (t < 1f)
    {
        t += Time.deltaTime / duration;
        buildObj.transform.position = Vector3.Lerp(start, end, t);
        yield return null;
    }

    yield return new WaitForSeconds(0.15f);

    Destroy(buildObj);
}

    
    public IEnumerator RunAi()
    {
        isAITurn = true;
        aiIsPlaying.SetActive(true);

        int AIActions = turn*aiDifficulty/2;
        int AIPhase = Mathf.Clamp(((turn*aiDifficulty) - 1) / 12 + 1, 1, 7);//1 = stone age 2 = bronze age, 3 = classical age, 4 = mediaval age, 5 explorationa age, 6 = industrial age, 7 industal age pt 2

        yield return AIAttack(AIActions, AIPhase);
        yield return AIBuild(AIActions, AIPhase);

        yield return new WaitForSeconds(1);

        isAITurn = false;
        aiIsPlaying.SetActive(false);

        NextTurn();
    }

    public IEnumerator AIBuild(int AIActions, int AIPhase){
    for (int x = 0; x < currentMap.width; x++)
    {
        for (int y = 0; y < currentMap.height; y++)
        {
            Tile tile = currentMap.tiles[x, y];
            float aiRoll = Random.value;

            if (tile.owner != currentPlayer||(tile.unit != null&& tile.unit.owner != currentPlayer))
                continue;

         if(AIPhase == 1){
            aiRoll = Random.value;
                if (aiRoll > 0.2f)
               AiAddDisciplinePolicy(currentPlayer);

                unlockClimbing();
            if (tile.forestResource != "")
            {
                if (tile.tileType == "Desert" || tile.tileType == "Plains" && 0.5< aiRoll && AIActions > 1)
                {
                    tile.forestResource = "";
                    tile.resource = "Crop";
                    AIActions --;
                    
                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawTile : true);
                        yield return new WaitForSeconds(0.3f);
                    }
                    //StartCoroutine(ShowAIBuildVisual(tile));
                }
            }

            if (tile.building == null && tile.district == null&& tile.forestResource == "")
            {
                aiRoll = Random.value;
                if (aiRoll > 0.6f &&
                    (tile.tileType == "Snow" ||
                     tile.tileType == "Desert" ||
                     tile.tileType == "Plains") &&
                    HasAdjacentBuilding(x, y) && AIActions > 2)
                {
                    tile.district = new City();
                    tile.tileType = "District";
                    tile.resource = "";
                    currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                    AIActions -=2;

                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
                else if (tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.1f)
                {
                    tile.building = new Farm();
                    tile.resource = "";
                    AIActions --;

                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }else if(tile.tileType == "Plains" && AIActions > 1&&aiRoll > 0.05f&& tile.building == null && tile.forestResource == ""){
                    tile.building = new Pasture();
                    tile.resource = "";
                    AIActions --;
                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }

            if (tile.district != null)
            {//tile.district.building = new Monument();

                aiRoll = Random.value;
                if(tile.unit==null && AIActions > 1){
                    aiRoll = Random.value;
                    if(aiRoll > 0.3f){
                        tile.unit = new Warrior(currentPlayer);

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawUnit : true);
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    else{
                        tile.unit = new Spearman(currentPlayer);

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawUnit : true);
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    AIActions --;
                }
            }
            }

            if(AIPhase == 2){
                if (aiRoll > 0.2f)
               AiAddDisciplinePolicy(currentPlayer);
                if (aiRoll > 0.2f)
               AiAddMtPolicy(currentPlayer);
                unlockSailing();
            if (tile.forestResource != "")
            {
                if (tile.tileType == "Desert" || tile.tileType == "Plains" && 0.5< aiRoll && AIActions > 1)
                {
                    tile.forestResource = "";
                    tile.resource = "Crop";
                    AIActions --;

                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawTile : true);
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }

            if (tile.building == null && tile.district == null && tile.forestResource == "")
            {
                aiRoll = Random.value;
                if (aiRoll > 0.6f &&
                    (tile.tileType == "Snow" ||
                     tile.tileType == "Desert" ||
                     tile.tileType == "Plains") &&
                    HasAdjacentBuilding(x, y) && AIActions > 2)
                {
                    tile.district = new City();
                    tile.tileType = "District";
                    tile.resource = "";
                    currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                    AIActions -=2;

                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
                else if (tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.1f)
                {
                    tile.building = new Farm();
                    tile.resource = "";
                    AIActions --;

                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }else if(tile.tileType == "Plains" && AIActions > 1&&aiRoll > 0.05f&& tile.building == null && tile.forestResource == ""){
                    tile.building = new Pasture();
                    tile.resource = "";
                    AIActions --;

                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }

            if (tile.district != null&& !(tile.district is Harbour ))
            {//tile.district.building = new Monument();
                aiRoll = Random.value;
                if(aiRoll > 0.3f){
                    tile.hasWall = true;

                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }

                aiRoll = Random.value;
                if(tile.unit==null && AIActions > 1){
                    aiRoll = Random.value;
                    if(aiRoll > 0.7f){
                        tile.unit = new Warrior(currentPlayer);
                         
                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawUnit : true);
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    else if(aiRoll > 0.5f){
                        tile.unit = new Spearman(currentPlayer);

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawUnit : true);
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(aiRoll > 0.2f){
                        tile.unit = new Horseman(currentPlayer);
                         
                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawUnit : true);
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else{
                        tile.unit = new Archer(currentPlayer);

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawUnit : true);
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    AIActions --;
                }
            }
            }
            if(AIPhase == 3){
            aiRoll = Random.value;
                if (aiRoll > 0.2f)
               AiAddMtPolicy(currentPlayer);
                if (aiRoll > 0.2f)
               AiAddPressGangsPolicy(currentPlayer);
                if (tile.forestResource != "")
                {
                    if (tile.tileType == "Desert" || tile.tileType == "Plains" && 0.5< aiRoll && AIActions > 1)
                    {
                        tile.forestResource = "";
                        tile.resource = "Crop";
                        AIActions --;
                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawTile : true);
                        yield return new WaitForSeconds(0.3f);
                    }
                    }
                    else if (tile.tileType == "Snow" &&0.5< aiRoll)
                    {
                        tile.building = new LumberHut();
                        tile.resource = "";
                        AIActions --;
                    }
                }
                aiRoll = Random.value;

                if(tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.5f&&tile.building == null && tile.district == null){
                        tile.building = new Farm();
                        tile.resource = "";
                        AIActions --;
                        
                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
                aiRoll = Random.value;
                if(tile.tileType == "Snow" && tile.resource == "Penguin"&& 0.5< aiRoll){
                        tile.building = new FurTradingPost();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
                aiRoll = Random.value;
                if(tile.resource == "Fish" && 0.7< aiRoll){
                        tile.building = new FishingBoats();
                        tile.resource = "";
                        AIActions --;
                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
                aiRoll = Random.value;
                
                if(tile.resource == "Metal" && 0.6< aiRoll){
                        tile.building = new Mine();
                        tile.resource = "";
                        AIActions --;
                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }

                }
                if (tile.building == null && tile.district == null && tile.forestResource == "")
                {
                    aiRoll = Random.value;
                    if(aiRoll > 0.8f && (tile.tileType == "Coast" || tile.tileType == "River")&& HasAdjacentBuilding(x, y) && AIActions > 2){
                        tile.district = new Harbour();
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;
                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                    }else if(aiRoll > 0.5f&&tile.tileType == "River"&& AIActions > 1){
                        tile.building = new Waterwheel();
                        tile.resource = "";
                        AIActions --;
                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                    }
                    if (aiRoll > 0.8f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentBuilding(x, y) && AIActions > 2)
                    {
                        tile.district = new City();
                        tile.tileType = "District";
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                    }
                    else if (tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.1f)
                    {
                        tile.building = new Farm();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                    }else if(tile.tileType == "Plains" && AIActions > 1&&aiRoll > 0.05f && tile.building == null && tile.forestResource == ""){
                        tile.building = new Pasture();
                        tile.resource = "";
                        AIActions --;
                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                    }
                }
                if (tile.district != null && tile.district is City)
                {                //tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(tile.unit==null && AIActions > 1){
                        aiRoll = Random.value;
                        if(aiRoll > 0.8f){
                            tile.unit = new Swordsman(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else if(aiRoll > 0.6f){
                            tile.unit = new Shield(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else if(aiRoll > 0.4f){
                            tile.unit = new Chariot(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else if(aiRoll > 0.3f){
                            tile.unit = new Horseman(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else if(aiRoll > 0.1f){
                            tile.unit = new Catapult(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else{
                            tile.unit = new Archer(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        AIActions --;
                    }
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                        if(aiRoll > 0.8){
                            tile.district.building = new Monument();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                        }else{
                            tile.district.building = new Market();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                        }
                                                                            AIActions --;

                    }
                }
                if (tile.district != null && tile.district is Harbour)
                {                //tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(tile.unit==null && AIActions > 1){
                        aiRoll = Random.value;
                        if(aiRoll > 0.5f){
                            tile.unit = new Ship(currentPlayer);
                            
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else{
                            tile.unit = new RammingShip(currentPlayer);
                            
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }

                        }
                        AIActions --;
                    }
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                        if(aiRoll > 0.5){
                            tile.district.building = new LightHouse();
                        }else{
                            tile.district.building = new CustomHouse();
                        }
                                                                            AIActions --;

                    }
                }
            }
            if(AIPhase == 4){
                aiRoll = Random.value;
                if (aiRoll > 0.2f)
               AiAddMtPolicy(currentPlayer);
                if (aiRoll > 0.2f)
               AiAddPressGangsPolicy(currentPlayer);
                if (tile.forestResource != "")
                {
                    if (tile.tileType == "Desert" || tile.tileType == "Plains" && 0.5< aiRoll && AIActions > 1)
                    {
                        tile.forestResource = "";
                        tile.resource = "Crop";
                        AIActions --;

                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawTile : true);
                        yield return new WaitForSeconds(0.3f);
                    }
                    }
                    else if (tile.tileType == "Snow" &&0.5< aiRoll)
                    {
                        tile.building = new LumberHut();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                    }
                }
                aiRoll = Random.value;

                if(tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.5f&&tile.building == null && tile.district == null){
                        tile.building = new Farm();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
                aiRoll = Random.value;
                if(tile.tileType == "Snow" && tile.resource == "Penguin"&& 0.5< aiRoll){
                        tile.building = new FurTradingPost();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
                aiRoll = Random.value;
                if(tile.resource == "Fish" && 0.7< aiRoll){
                        tile.building = new FishingBoats();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
                aiRoll = Random.value;
                
                if(tile.resource == "Metal" && 0.6< aiRoll){
                        tile.building = new Mine();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                }

                if (tile.building == null && tile.district == null && tile.forestResource == "")
                {
                    aiRoll = Random.value;
                    if(aiRoll > 0.8f && (tile.tileType == "Coast" || tile.tileType == "River")&& HasAdjacentBuilding(x, y) && AIActions > 2){
                        tile.district = new Harbour();
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }
                    }else if(aiRoll > 0.5f&&tile.tileType == "River"&& AIActions > 1){
                        tile.building = new Waterwheel();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawBuildings : true,drawTile : true);
                        StartCoroutine(ShowAIBuildVisual(tile));
                        yield return new WaitForSeconds(0.3f);
                    }

                    }
                    if (aiRoll > 0.7f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentBuilding(x, y) && AIActions > 2)
                    {
                        tile.district = new City();
                        tile.tileType = "District";
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(aiRoll > 0.5f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentCity(x, y) && AIActions > 2){
                        tile.district = new Commercial();
                        tile.tileType = "District";
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if (aiRoll > 0.2f && (tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentFarm(tile) && AIActions > 1){
                        tile.building = new Windmill();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    else if (tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.1f)
                    {
                        tile.building = new Farm();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(tile.tileType == "Plains" && AIActions > 1&&aiRoll > 0.05f && tile.building == null && tile.forestResource == ""){
                        tile.building = new Pasture();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                }
                if (tile.district != null && tile.district is City)
                {                //tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(tile.unit==null && AIActions > 1){
                        aiRoll = Random.value;
                        if(aiRoll > 0.7f){
                            tile.unit = new Swordsman(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                            
                        }else if(aiRoll > 0.6f){
                            tile.unit = new Shield(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else if(aiRoll > 0.5f){
                            tile.unit = new Knight(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else if(aiRoll > 0.3f){
                            tile.unit = new Horseman(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else if(aiRoll > 0.1f){
                            tile.unit = new Catapult(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else{
                            tile.unit = new Archer(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        AIActions --;
                    }
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                        if(aiRoll > 0.5){
                            tile.district.building = new Monument();
                                                    AIActions --;
                                                                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                        }else{
                            tile.district.building = new Market();
                                                    AIActions --;

                                                                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                        }
                    }
                }
                if (tile.district != null && tile.district is Harbour)
                {                //tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(tile.unit==null && AIActions > 1){
                        aiRoll = Random.value;
                        if(aiRoll > 0.7f){
                            tile.unit = new Ship(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else if(aiRoll > 0.3f){
                            tile.unit = new Caraval(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else{
                            tile.unit = new RammingShip(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }

                        }
                        AIActions --;
                    }
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                                            aiRoll = Random.value;

                        if(aiRoll > 0.7){
                            tile.district.building = new LightHouse();
                                                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else if(aiRoll > 0.3f){
                            tile.district.building = new Shipyard();
                                                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else{
                            tile.district.building = new CustomHouse();
                                                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }
                                                AIActions --;

                    }
                }
                if (tile.district != null && tile.district is Commercial)
                {
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                                            aiRoll = Random.value;

                        if(aiRoll > 0.5){
                            tile.district.building = new Tower();
                        }
                                                AIActions --;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                    }
                }
                if (tile.district != null&& !(tile.district is Harbour ))
                {//tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(aiRoll > 0.2f){
                        tile.hasWall = true;
                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                }
            }
            if(AIPhase == 5){
                aiRoll = Random.value;
                if (aiRoll > 0.2f)
               AiAddAdmiralsPolicy(currentPlayer);
                if (aiRoll > 0.2f)
               AiAddColonizationPolicy(currentPlayer);
                if (tile.forestResource != "")
                {
                    if (tile.tileType == "Desert" || tile.tileType == "Plains" && 0.5< aiRoll && AIActions > 1)
                    {
                        tile.forestResource = "";
                        tile.resource = "Crop";
                        AIActions --;


                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawTile : true);
                        yield return new WaitForSeconds(0.3f);
                    }
                    }
                    else if (tile.tileType == "Snow" &&0.5< aiRoll)
                    {
                        tile.building = new LumberHut();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                }
                aiRoll = Random.value;

                if(tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.5f&&tile.building == null && tile.district == null){
                        tile.building = new Farm();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }

                aiRoll = Random.value;
                if(tile.tileType == "Snow" && tile.resource == "Penguin"&& 0.5< aiRoll){
                        tile.building = new FurTradingPost();
                        tile.resource = "";
                        AIActions --;
                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                aiRoll = Random.value;
                if(tile.resource == "Fish" && 0.7< aiRoll){
                        tile.building = new FishingBoats();
                        tile.resource = "";
                        AIActions --;
                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                aiRoll = Random.value;
                
                if(tile.resource == "Metal" && 0.6< aiRoll){
                        tile.building = new Mine();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                if(tile.resource == "Whale" && 0.6< aiRoll){
                        tile.building = new WhalingShip();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                if (tile.building == null && tile.district == null && tile.forestResource == "")
                {
                    aiRoll = Random.value;
                    if(aiRoll > 0.8f && (tile.tileType == "Coast" || tile.tileType == "River")&& HasAdjacentBuilding(x, y) && AIActions > 2){
                        tile.district = new Harbour();
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);


                        AIActions -=2;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(aiRoll > 0.5f&&tile.tileType == "River"&& AIActions > 1){
                        tile.building = new Waterwheel();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    if (aiRoll > 0.7f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentBuilding(x, y) && AIActions > 2)
                    {
                        tile.district = new City();
                        tile.tileType = "District";
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                    }else if(aiRoll > 0.5f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentCity(x, y) && AIActions > 2){
                        tile.district = new Commercial();
                        tile.tileType = "District";
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if (aiRoll > 0.45f && (tile.tileType == "Snow" ||tile.tileType == "Desert" || tile.tileType == "Plains") && AIActions > 1){
                        tile.building = new Fort();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                    }
                    else if (aiRoll > 0.2f && (tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentFarm(tile) && AIActions > 1){
                        tile.building = new Windmill();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                    }
                    else if (tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.1f)
                    {
                        tile.building = new Farm();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                    }else if(tile.tileType == "Plains" && AIActions > 1&&aiRoll > 0.05f && tile.building == null && tile.forestResource == ""){
                        tile.building = new Pasture();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                    }else if(aiRoll > 0.95f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && AIActions > 1){
                        tile.building = new University();

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                    }
                }
                if (tile.district != null && tile.district is City)
                {                //tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(tile.unit==null && AIActions > 1){
                        aiRoll = Random.value;
                        if(aiRoll > 0.6f){
                            tile.unit = new Musketman(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                            
                        }else if(aiRoll > 0.5f){
                            tile.unit = new Archer(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else if(aiRoll > 0.4f){
                            tile.unit = new Horseman(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else if(aiRoll > 0.3f){
                            tile.unit = new Cavalry(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else if(aiRoll > 0.1f){
                            tile.unit = new Cannon(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else{
                            tile.unit = new Swordsman(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        AIActions --;
                    }
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                        if(aiRoll > 0.5){
                            tile.district.building = new Monument();
                                                    AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }


                        }else{
                            tile.district.building = new Market();
                                                    AIActions --;
                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }



                        }
                    }
                }
                if (tile.district != null && tile.district is Harbour)
                {                //tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(tile.unit==null && AIActions > 1){
                        aiRoll = Random.value;
                        if(aiRoll > 0.6f){
                            tile.unit = new Frigate(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else{
                            tile.unit = new Caraval(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        AIActions --;
                    }
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                                            aiRoll = Random.value;

                        if(aiRoll > 0.7){
                            tile.district.building = new LightHouse();

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else if(aiRoll > 0.5f){
                            tile.district.building = new Shipyard();

                                                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else if(aiRoll > 0.4f){
                            tile.district.building = new NavelBase();

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }
                        else{
                            tile.district.building = new CustomHouse();

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                        }
                                                AIActions --;

                    }
                }
                if (tile.district != null && tile.district is Commercial)
                {
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                                            aiRoll = Random.value;

                        if(aiRoll > 0.5){
                            tile.district.building = new Tower();

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else{
                            tile.district.building = new Bank();

                                                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                        }

                                                AIActions --;

                    }
                }
                if (tile.district != null&& !(tile.district is Harbour))
                {//tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(aiRoll > 0.3f){
                        tile.hasWall = true;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                }
            }
            if(AIPhase == 6){
                aiRoll = Random.value;
                if (aiRoll > 0.2f)
               AiAddDisciplinePolicy(currentPlayer);
                if (aiRoll > 0.2f)
               AiAddSkisPolicy(currentPlayer);
                if (tile.forestResource != "")
                {
                    if (tile.tileType == "Desert" || tile.tileType == "Plains" && 0.5< aiRoll && AIActions > 1)
                    {
                        tile.forestResource = "";
                        tile.resource = "Crop";
                        AIActions --;

                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawTile : true);
                        yield return new WaitForSeconds(0.3f);
                    }
                    }
                    else if (tile.tileType == "Snow" &&0.5< aiRoll)
                    {
                        tile.building = new LumberHut();
                        tile.resource = "";
                        AIActions --;

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                }
                aiRoll = Random.value;

                if(tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.5f&&tile.building == null && tile.district == null){
                        tile.building = new Farm();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                aiRoll = Random.value;
                if(tile.tileType == "Snow" && tile.resource == "Penguin"&& 0.5< aiRoll){
                        tile.building = new FurTradingPost();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                aiRoll = Random.value;
                if(tile.resource == "Fish" && 0.7< aiRoll){
                        tile.building = new FishingBoats();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                aiRoll = Random.value;
                
                if(tile.resource == "Metal" && 0.6< aiRoll){
                        tile.building = new Mine();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                if(tile.resource == "Whale" && 0.6< aiRoll){
                        tile.building = new WhalingShip();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                if (tile.building == null && tile.district == null && tile.forestResource == "")
                {
                    aiRoll = Random.value;
                    if(aiRoll > 0.8f && (tile.tileType == "Coast" || tile.tileType == "River")&& HasAdjacentBuilding(x, y) && AIActions > 2){
                        tile.district = new Harbour();
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    if (aiRoll > 0.8f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentBuilding(x, y) && AIActions > 2)
                    {
                        tile.district = new City();
                        tile.tileType = "District";
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(aiRoll > 0.7f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentCity(x, y) && AIActions > 2){
                        tile.district = new Commercial();
                        tile.tileType = "District";
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(aiRoll > 0.5f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentCity(x, y) && AIActions > 2){
                        tile.district = new Industrial();
                        tile.tileType = "District";
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if (aiRoll > 0.45f && (tile.tileType == "Snow" ||tile.tileType == "Desert" || tile.tileType == "Plains") && AIActions > 1){
                        tile.building = new Fort();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    else if (aiRoll > 0.2f && (tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentFarm(tile) && AIActions > 1){
                        tile.building = new Windmill();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    else if (tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.1f)
                    {
                        tile.building = new Farm();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(tile.tileType == "Plains" && AIActions > 1&&aiRoll > 0.05f && tile.building == null && tile.forestResource == ""){
                        tile.building = new Pasture();
                        tile.resource = "";
                        AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(aiRoll > 0.01f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && AIActions > 1){
                        tile.building = new University();
                        tile.resource = "";

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                }
                if (tile.district != null && tile.district is City)
                {                //tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(tile.unit==null && AIActions > 1){
                        aiRoll = Random.value;
                        if(aiRoll > 0.7f){
                            tile.unit = new Musketman(currentPlayer);
                            
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else if(aiRoll > 0.5f){
                            tile.unit = new Cavalry(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else if(aiRoll > 0.3f){
                            tile.unit = new Cannon(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else{
                            tile.unit = new Swordsman(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        AIActions --;
                    }
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                        if(aiRoll > 0.5){
                            tile.district.building = new Monument();
                                                    AIActions --;

                                                                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                        }else{
                            tile.district.building = new Market();
                                                    AIActions --;

                                                if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }
                    }
                }
                if (tile.district != null && tile.district is Harbour)
                {                //tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(tile.unit==null && AIActions > 1){
                        aiRoll = Random.value;
                        if(aiRoll > 0.6f){
                            tile.unit = new Frigate(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else if(aiRoll > 0.3f){
                            tile.unit = new Dreadnort(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else{
                            tile.unit = new Cruiser(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        AIActions --;
                    }
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                                            aiRoll = Random.value;

                        if(aiRoll > 0.7){
                            tile.district.building = new LightHouse();

                                                                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else if(aiRoll > 0.5f){
                            tile.district.building = new Shipyard();

                                                                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else if(aiRoll > 0.4f){
                            tile.district.building = new NavelBase();

                                                                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }
                        else{
                            tile.district.building = new CustomHouse();

                                                                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }
                                                AIActions --;

                    }
                }
                if (tile.district != null && tile.district is Commercial)
                {
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                                            aiRoll = Random.value;

                        if(aiRoll > 0.5){
                            tile.district.building = new Tower();

                                                                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else{
                            tile.district.building = new Bank();

                                                                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                        }
                                                AIActions --;
                    }
                }
                if (tile.district != null && tile.district is Industrial)
                {
                    aiRoll = Random.value;

                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                                            aiRoll = Random.value;

                        if(aiRoll > 0.5){
                            tile.district.building = new CarpentryWorkshop();

                                                                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else{
                            tile.district.building = new Warehouses();

                                                                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                        }
                                                AIActions --;
                    }
                }
                /*if (tile.building != null && tile.building is Airport)
                {
                    aiRoll = Random.value;

                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                        aiRoll = Random.value;

                        if(aiRoll > 0.5){
                            tile.district.building = new Zeppelin();
                            AIActions --;

                        }
                    }
                }*/
            }
            if(AIPhase >= 7){
                aiRoll = Random.value;
                if (aiRoll > 0.2f)
               AiAddAdmiralsPolicy(currentPlayer);
                if (aiRoll > 0.2f)
               AiAddPressGangsPolicy(currentPlayer);
                if (tile.forestResource != "")
                {
                    if (tile.tileType == "Desert" || tile.tileType == "Plains" && 0.5< aiRoll && AIActions > 1)
                    {
                        tile.forestResource = "";
                        tile.resource = "Crop";
                        AIActions --;


                    if(humanPlayer.exploredTiles[tile.x, tile.y])
                    {
                        Draw(drawTile : true);
                        yield return new WaitForSeconds(0.3f);
                    }
                    }
                    else if (tile.tileType == "Snow" &&0.5< aiRoll)
                    {
                        tile.building = new LumberHut();
                        tile.resource = "";
                        AIActions --;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                }
                aiRoll = Random.value;

                if(tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.5f&&tile.building == null && tile.district == null){
                        tile.building = new Farm();
                        tile.resource = "";
                        AIActions --;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                aiRoll = Random.value;
                if(tile.tileType == "Snow" && tile.resource == "Penguin"&& 0.5< aiRoll){
                        tile.building = new FurTradingPost();
                        tile.resource = "";
                        AIActions --;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                aiRoll = Random.value;
                if(tile.resource == "Fish" && 0.7< aiRoll){
                        tile.building = new FishingBoats();
                        tile.resource = "";
                        AIActions --;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                aiRoll = Random.value;
                
                if(tile.resource == "Metal" && 0.6< aiRoll){
                        tile.building = new Mine();
                        tile.resource = "";
                        AIActions --;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                if(tile.resource == "Whale" && 0.6< aiRoll){
                        tile.building = new WhalingShip();
                        tile.resource = "";
                        AIActions --;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                }
                if (tile.building == null && tile.district == null&& tile.forestResource == "")
                {
                    aiRoll = Random.value;
                    if(aiRoll > 0.8f && (tile.tileType == "Coast" || tile.tileType == "River")&& HasAdjacentBuilding(x, y) && AIActions > 2){
                        tile.district = new Harbour();
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    if (aiRoll > 0.85f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentBuilding(x, y) && AIActions > 2)
                    {
                        tile.district = new City();
                        tile.tileType = "District";
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(aiRoll > 0.7f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentCity(x, y) && AIActions > 2){
                        tile.district = new Commercial();
                        tile.tileType = "District";
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(aiRoll > 0.5f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && HasAdjacentCity(x, y) && AIActions > 2){
                        tile.district = new Industrial();
                        tile.tileType = "District";
                        tile.resource = "";
                        currentMap.ClaimAdjacentTiles(x, y, currentPlayer);
                        AIActions -=2;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    else if (aiRoll > 0.2f && (tile.tileType == "Snow" ||tile.tileType == "Desert" || tile.tileType == "Plains") && AIActions > 1){
                        tile.building = new Airport();
                        tile.resource = "";
                        AIActions --;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    else if (tile.resource == "Crop" && AIActions > 1 &&aiRoll > 0.1f)
                    {
                        tile.building = new Farm();
                        tile.resource = "";
                        AIActions --;

                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(tile.tileType == "Plains" && AIActions > 1&&aiRoll > 0.05f && tile.building == null && tile.forestResource == ""){
                        tile.building = new Pasture();
                        tile.resource = "";
                        AIActions --;
                                                                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }else if(aiRoll > 0.01f && (tile.tileType == "Snow" || tile.tileType == "Desert" || tile.tileType == "Plains") && AIActions > 1){
                        tile.resource = "";
                        tile.building = new University();

                        if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                }
                if (tile.district != null && tile.district is City)
                {                //tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(tile.unit==null && AIActions > 1){
                        aiRoll = Random.value;
                        if(aiRoll > 0.6f){
                            tile.unit = new Infantry(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                            
                        }
                        else if(aiRoll > 0.4f){
                            tile.unit = new Artillary(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }else if(aiRoll > 0.2f){
                            tile.unit = new MachineGun(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else{
                            tile.unit = new Cavalry(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        AIActions --;
                    }
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                        if(aiRoll > 0.5){
                            tile.district.building = new Monument();
                                                    AIActions --;

                        }else{
                            tile.district.building = new Market();
                                                    AIActions --;

                        }
                    }
                }
                if (tile.district != null && tile.district is Harbour)
                {                //tile.district.building = new Monument();
                    aiRoll = Random.value;
                    if(tile.unit==null && AIActions > 1){
                        aiRoll = Random.value;
                        if(aiRoll > 0.5f){
                            tile.unit = new Dreadnort(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        else{
                            tile.unit = new Cruiser(currentPlayer);

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        AIActions --;
                    }
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                                            aiRoll = Random.value;

                        if(aiRoll > 0.7){
                            tile.district.building = new LightHouse();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else if(aiRoll > 0.5f){
                            tile.district.building = new Shipyard();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else if(aiRoll > 0.4f){
                            tile.district.building = new NavelBase();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }
                        else{
                            tile.district.building = new CustomHouse();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }
                                                AIActions --;

                    }
                }
                if (tile.district != null && tile.district is Commercial)
                {
                    aiRoll = Random.value;
                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                                            aiRoll = Random.value;

                        if(aiRoll > 0.5){
                            tile.district.building = new SkyScrapers();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                        }else if(aiRoll > 0.4){
                            tile.district.building = new Tower();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }
                        else{
                            tile.district.building = new Bank();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                        }
                                                AIActions --;
                    }
                }
                if (tile.district != null && tile.district is Industrial)
                {
                    aiRoll = Random.value;

                    if(tile.district.building == null && AIActions > 1 && aiRoll > 0.5){
                                            aiRoll = Random.value;

                        if(aiRoll > 0.9){
                            tile.district.building = new CarpentryWorkshop();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }else if(aiRoll > 0.2){
                            tile.district.building = new Factorys();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }
                        }
                        else{
                            tile.district.building = new Warehouses();
                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                        {
                            Draw(drawBuildings : true,drawTile : true);
                            StartCoroutine(ShowAIBuildVisual(tile));
                            yield return new WaitForSeconds(0.3f);
                        }

                        }
                                                AIActions --;
                        
                    }
                    if(tile.district.building != null && AIActions > 1 && aiRoll > 0.3 && tile.district.building is Factorys && tile.unit == null){
                        tile.unit = new Tank(currentPlayer);
                    }
                }
                if (tile.building != null && tile.building is Airport)
                {
                    aiRoll = Random.value;

                    if( AIActions > 1 && aiRoll > 0.5&& tile.unit == null){
                        aiRoll = Random.value;

                        if(aiRoll > 0.7){
                            tile.unit = new Zeppelin(currentPlayer);
                            AIActions --;

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }

                        }else{
                            tile.unit = new Biplane(currentPlayer);
                            AIActions --;

                            if(humanPlayer.exploredTiles[tile.x, tile.y])
                            {
                                Draw(drawUnit : true);
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                    }
                }
            }
            
        }
    }
            yield return null;

    }
        
    ///
    /// Handles AI attack
    /// 
    /// 
    
    public IEnumerator AIAttack(int AIActions, int AIPhase){
        List<(Troops unit, Tile tile)> aiUnits = new List<(Troops, Tile)>();

        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile tile = currentMap.tiles[x, y];
                if (tile.unit != null && tile.unit.owner == currentPlayer)
                {
                    aiUnits.Add((tile.unit, tile));
                }
            }
        }

        foreach (var entry in aiUnits)
        {
            Troops unit = entry.unit;
            Tile unitTile = entry.tile;

            if (unit.hasMoved && unit.hasAttacked)
                continue;

            if(unitTile.owner != currentPlayer && !(unitTile.tileType == "Ocean" && unitTile.resource==""))
            {
                currentTile = unitTile;
                ClaimTile();
                unitTile = currentMap.tiles[unitTile.x, unitTile.y];
                currentTile = unitTile;
            }

            Tile target = FindNearestEnemy(unitTile, currentPlayer);

            if (target != null  && target.unit != null && target.unit.owner != unit.owner&& CubeDistance(unitTile, target) <= unit.range && !unit.hasAttacked)
            {
                mainTile = unitTile;
                currentTile = target;
                if(target.unit != null)

                yield return  StartCoroutine(HandleAttack());
                yield return new WaitForSeconds(1f);
                AIActions --;
                
                if(!(mainTile.unit is MachineGun))
                    unit.hasAttacked = true;
                continue;
            }

            // Otherwise move
            if (!unit.hasMoved)
            {
                List<Tile> moves = GetMovableTilesForAI(unitTile, unit);
                if (moves.Count > 0)
                {
                    Tile bestMove = null;
                    int bestDist = int.MaxValue;

                    if (target != null)
                    {
                        foreach (Tile m in moves)
                        {
                            int d = CubeDistance(m, target);
                            if (d < bestDist)
                            {
                                bestDist = d;
                                bestMove = m;
                            }
                        }
                    }

                    if (bestMove == null){
                        bestMove = moves[Random.Range(0, moves.Count)];
                        AIActions ++;
                    }
                
                    yield return StartCoroutine(MoveUnitAIVisual(unitTile, bestMove, unit,false));

                    unitTile = bestMove;
                    AIActions --;
                }
            }
        }   
        yield return null;
    }

void AiAddDisciplinePolicy(Player ai)
{
    // If AI already has Discipline active → stop
    if (HasActivePolicy("Discipline"))
        return;

    // Create policy (AI-only instance)
    Policy discipline = new Policy(
        "Discipline",
        Policy.PolicyType.Military,
        0,
        "Defence bonus on mountains"
    );

    // Add to unlocked policies if missing
    if (!ai.unlockedPolicys.Exists(p => p.name == "Discipline"))
        ai.unlockedPolicys.Add(discipline);

    // Equip it into the first free military slot
    foreach (var slot in ai.currentGovernment.policySlots)
    {
        if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
        {
            slot.activePolicy = discipline;
            return;
        }
    }

    // Also check ministry slots
    foreach (var ministry in ai.currentGovernment.ministrysPolicies)
    {
        foreach (var slot in ministry.policySlots)
        {
            if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
            {
                slot.activePolicy = discipline;
                return;
            }
        }
    }
}
void AiAddMtPolicy(Player ai)
{
    // If AI already has Discipline active → stop
    if (HasActivePolicy("Martial Tradition"))
        return;

    // Create policy (AI-only instance)
    Policy discipline = new Policy(
        "Martial Tradition",
        Policy.PolicyType.Military,
        0,
        "Defence bonus on mountains"
    );

    // Add to unlocked policies if missing
    if (!ai.unlockedPolicys.Exists(p => p.name == "Martial Tradition"))
        ai.unlockedPolicys.Add(discipline);

    // Equip it into the first free military slot
    foreach (var slot in ai.currentGovernment.policySlots)
    {
        if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
        {
            slot.activePolicy = discipline;
            return;
        }
    }

    // Also check ministry slots
    foreach (var ministry in ai.currentGovernment.ministrysPolicies)
    {
        foreach (var slot in ministry.policySlots)
        {
            if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
            {
                slot.activePolicy = discipline;
                return;
            }
        }
    }
}
void AiAddSkisPolicy(Player ai)
{
    // If AI already has Discipline active → stop
    if (HasActivePolicy("Skis"))
        return;

    // Create policy (AI-only instance)
    Policy discipline = new Policy(
        "Skis",
        Policy.PolicyType.Military,
        0,
        "Defence bonus on mountains"
    );

    // Add to unlocked policies if missing
    if (!ai.unlockedPolicys.Exists(p => p.name == "Skis"))
        ai.unlockedPolicys.Add(discipline);

    // Equip it into the first free military slot
    foreach (var slot in ai.currentGovernment.policySlots)
    {
        if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
        {
            slot.activePolicy = discipline;
            return;
        }
    }

    // Also check ministry slots
    foreach (var ministry in ai.currentGovernment.ministrysPolicies)
    {
        foreach (var slot in ministry.policySlots)
        {
            if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
            {
                slot.activePolicy = discipline;
                return;
            }
        }
    }

}
void AiAddPressGangsPolicy(Player ai)
{
    // If AI already has Discipline active → stop
    if (HasActivePolicy("Press Gangs"))
        return;

    // Create policy (AI-only instance)
    Policy discipline = new Policy(
        "Press Gangs",
        Policy.PolicyType.Military,
        0,
        "Defence bonus on mountains"
    );

    // Add to unlocked policies if missing
    if (!ai.unlockedPolicys.Exists(p => p.name == "Press Gangs"))
        ai.unlockedPolicys.Add(discipline);

    // Equip it into the first free military slot
    foreach (var slot in ai.currentGovernment.policySlots)
    {
        if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
        {
            slot.activePolicy = discipline;
            return;
        }
    }

    // Also check ministry slots
    foreach (var ministry in ai.currentGovernment.ministrysPolicies)
    {
        foreach (var slot in ministry.policySlots)
        {
            if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
            {
                slot.activePolicy = discipline;
                return;
            }
        }
    }
}
void AiAddAdmiralsPolicy(Player ai)
{
    // If AI already has Discipline active → stop
    if (HasActivePolicy("Admirals"))
        return;

    // Create policy (AI-only instance)
    Policy discipline = new Policy(
        "Admirals",
        Policy.PolicyType.Military,
        0,
        "Defence bonus on mountains"
    );

    // Add to unlocked policies if missing
    if (!ai.unlockedPolicys.Exists(p => p.name == "Admirals"))
        ai.unlockedPolicys.Add(discipline);

    // Equip it into the first free military slot
    foreach (var slot in ai.currentGovernment.policySlots)
    {
        if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
        {
            slot.activePolicy = discipline;
            return;
        }
    }

    // Also check ministry slots
    foreach (var ministry in ai.currentGovernment.ministrysPolicies)
    {
        foreach (var slot in ministry.policySlots)
        {
            if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
            {
                slot.activePolicy = discipline;
                return;
            }
        }
    }
}
void AiAddColonizationPolicy(Player ai)
{
    // If AI already has Discipline active → stop
    if (HasActivePolicy("Colonization"))
        return;

    // Create policy (AI-only instance)
    Policy discipline = new Policy(
        "Colonization",
        Policy.PolicyType.Military,
        0,
        "Defence bonus on mountains"
    );

    // Add to unlocked policies if missing
    if (!ai.unlockedPolicys.Exists(p => p.name == "Colonization"))
        ai.unlockedPolicys.Add(discipline);

    // Equip it into the first free military slot
    foreach (var slot in ai.currentGovernment.policySlots)
    {
        if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
        {
            slot.activePolicy = discipline;
            return;
        }
    }

    // Also check ministry slots
    foreach (var ministry in ai.currentGovernment.ministrysPolicies)
    {
        foreach (var slot in ministry.policySlots)
        {
            if (slot.type == PolicySlot.PolicyType.Military && slot.activePolicy == null)
            {
                slot.activePolicy = discipline;
                return;
            }
        }
    }
}
private bool HasAdjacentBuilding(int x, int y)
{
    foreach (Tile n in GetNeighbors(currentMap.tiles[x, y]))
    {
        if (n.building != null)
            return true;
    }
    return false;
}
private bool HasAdjacentCity(int x, int y)
{
    foreach (Tile n in GetNeighbors(currentMap.tiles[x, y]))
    {
        if (n.district is City)
            return true;
    }
    return false;
}

    IEnumerator MoveUnitAIVisual(Tile from, Tile to, Troops unit, bool isAttack) 
    { 
        if (unit == null || from == null || to == null)
            yield break; 
        
        GetAIVisuals();

        Troops finalUnit = unit; 

        bool needsBoat = (to.tileType == "Coast" || to.tileType == "River" || to.tileType == "Ocean") && !unit.isBoat && !unit.isAir && !to.hasRoad && !to.hasTrainTrack;
        if (needsBoat) 
        {
            finalUnit = new Boat(unit.owner, unit); 
        } 
        else if (unit is Boat && (to.tileType == "Plains" || to.tileType == "Desert" || to.tileType == "Snow" || to.tileType == "Mountain"|| to.tileType == "District")) 
        {
            Boat boat = unit as Boat;
            if (boat != null && boat.unitToCarry != null) 
            {
                finalUnit = boat.unitToCarry;
            }
        } 

        // ---------- NO VISUAL → LOGIC ONLY ---------- 
        if (unit.visual == null && (to.unit == null || isAttack) || !humanPlayer.exploredTiles[to.x, to.y]) 
        {
            from.unit = null;
            to.unit = finalUnit; 
            finalUnit.hasMoved = true; 
            DoAIExploration(finalUnit, to);

            // ---------- POST-MOVE ATTACK ----------
            Tile enemy = FindNearestEnemyTroop(to, finalUnit.owner);
            if (enemy != null && CubeDistance(to, enemy) <= finalUnit.range && !finalUnit.hasAttacked)
            {
                mainTile = to;
                currentTile = enemy;
                yield return StartCoroutine(HandleAttack());
                yield return new WaitForSeconds(0.1f);
                finalUnit.hasAttacked = true;
            }

            yield break; 
        } 

        if (to.unit == null || isAttack) 
        { 
            from.unit = null;
            Draw(drawUnit: true); 

            GameObject animObj = Instantiate(unit.visual, unit.visual.transform.position, unit.visual.transform.rotation); 
            SetLayerRecursively(animObj,LayerMask.NameToLayer("TroopLayer"));

            animObj.tag = "Untagged"; 
            Vector3 start = animObj.transform.position;
            Vector3 end = CalculateHexPosition(to.x, to.y);

            float duration = 0.2f; 
            float t = 0f; 
            while (t < 1f) 
            { 
                t += Time.deltaTime / duration;
                animObj.transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            } 

            to.unit = finalUnit;
            finalUnit.hasMoved = true;
            DoAIExploration(finalUnit, to);

            Tile enemyAnim = FindNearestEnemyTroop(to, finalUnit.owner);
            if (enemyAnim != null && CubeDistance(to, enemyAnim) <= finalUnit.range && !finalUnit.hasAttacked)
            {
                mainTile = to;
                currentTile = enemyAnim;
                yield return StartCoroutine(HandleAttack());
                yield return new WaitForSeconds(0.1f);
                finalUnit.hasAttacked = true;
            }

            animObj.tag = "Destroyable";
            animObj.tag = "Unit";

            yield break; 
        }

        yield break; 
    }

private Tile FindNearestEnemyTroop(Tile from, Player aiPlayer)
{
    Tile closest = null;
    int bestDist = int.MaxValue;

    for (int x = 0; x < currentMap.width; x++)
    {
        for (int y = 0; y < currentMap.height; y++)
        {
            Tile t = currentMap.tiles[x, y];
            // Only consider enemy troops
            if (t.unit != null && t.unit.owner != aiPlayer)
            {
                int d = CubeDistance(from, t);
                if (d < bestDist)
                {
                    bestDist = d;
                    closest = t;
                }
            }
        }
    }

    return closest;
}
private void DoAIExploration(Troops unit, Tile tile)
{
    if (tile.tileType == "Mountain" ||
        unit is Caraval ||
        unit is Frigate ||
        unit is Dreadnort ||
        unit is Cruiser ||
        unit is Biplane ||
        unit is Zeppelin)
    {
        ExploreDouble(unit.owner, tile);
    }
    else
    {
        ExploreTile(unit.owner, tile);
    }

}

private void MoveUnitAI(Tile from, Tile to, Troops unit)
{
    from.unit = null;

    bool needsBoat =
        (to.tileType == "Coast" || to.tileType == "River" ||to.tileType == "Ocean") &&
        !unit.isBoat && !unit.isAir &&
        !to.hasRoad && !to.hasTrainTrack;

    if (needsBoat)
        to.unit = new Boat(unit.owner, unit);
    else
        if(unit is Boat && (to.tileType == "Plains" || to.tileType == "Desert" ||to.tileType == "Snow"||to.tileType == "Mountain")){
            Boat boat = unit as Boat;
            if (boat != null && boat.unitToCarry != null)
            {
                to.unit = boat.unitToCarry;        
                boat.unitToCarry = null;               

            }
        }else{
            to.unit = unit;
        }

    unit.hasMoved = true;

    if (to.tileType == "Mountain" ||
        unit is Caraval || unit is Frigate ||
        unit is Dreadnort ||
        unit is Cruiser||unit is Biplane||unit is Zeppelin)
        ExploreDouble(unit.owner, to);
    else
        ExploreTile(unit.owner, to);
}


    private Tile FindNearestEnemy(Tile from, Player aiPlayer)
{
    Tile closest = null;
    int bestDist = int.MaxValue;

    for (int x = 0; x < currentMap.width; x++)
    {
        for (int y = 0; y < currentMap.height; y++)
        {
            Tile t = currentMap.tiles[x, y];
            //if(x == from.x && y == from.y){}else{
            if ((t.unit != null && t.unit.owner != aiPlayer) || (t.owner!=aiPlayer /*&&!(t.tileType == "Ocean"&&t.resource=="")*/))
            {
                int d = CubeDistance(from, t);
                if (d < bestDist)
                {
                    bestDist = d;
                    closest = t;
                }
            }//}
        }
    }

    return closest;
}

    private List<Tile> GetMovableTilesForAI(Tile start, Troops unit)
{
    List<Tile> result = new List<Tile>();
    Queue<Tile> frontier = new Queue<Tile>();
    Dictionary<Tile, int> cost = new Dictionary<Tile, int>();

    frontier.Enqueue(start);
    cost[start] = 0;

    while (frontier.Count > 0)
    {
        Tile current = frontier.Dequeue();
        int currentCost = cost[current];

        foreach (Tile neighbor in GetNeighbors(current))
        {
if (neighbor.unit != null && neighbor.unit.owner != unit.owner)
    continue;
            if (!unit.CanMoveTo(neighbor)) continue;

int newCost = currentCost + Mathf.FloorToInt(neighbor.movementCost);
            if (newCost > unit.movement) continue;

    if (!cost.ContainsKey(neighbor))
    {
    cost[neighbor] = newCost;
    result.Add(neighbor);
    bool ignoresZOC =  unit is Chariot || unit.isAir;
    if (!ignoresZOC && IsInZoneOfControl(neighbor, unit.owner))
    {
        // AI unit stops here
    }
    else
    {
        frontier.Enqueue(neighbor);
    }
            }
        }
    }

    return result;
}

    private List<Tile> GetValidMoveTiles(Troops unit, Tile fromTile)
{
    List<Tile> valid = new List<Tile>();

    foreach (Tile neighbor in GetNeighbors(fromTile))
    {
if (neighbor.unit != null && neighbor.unit.owner != unit.owner)
    continue;
        if (!unit.CanMoveTo(neighbor)) continue;
        if (neighbor.movementCost > unit.movement) continue;

        valid.Add(neighbor);
    }
    return valid;
}
private bool IsInZoneOfControl(Tile tile, Player movingPlayer)
{
    foreach (Tile neighbor in GetNeighbors(tile))
    {
        if (neighbor.unit != null && 
            neighbor.unit.owner != movingPlayer &&
            !neighbor.unit.isBoat &&
            !neighbor.unit.isAir)
        {
            return true;
        }
    }
    return false;
}
    public int countMoney(){
        int money = 0;
        bool hasRomeTransMonument =  hasWonder("Rome", typeof(TradeWonder),currentPlayer);

        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile tile = currentMap.tiles[x, y];

                if((tile.forestResource != "")&& tile.owner == currentPlayer && hasWonder("Greece", typeof(ExplorerWonder),currentPlayer)){
                    money += 2;
                }
                if(tile.tileType == "Ocean"&& tile.owner == currentPlayer && hasWonder("Greece", typeof(TradeWonder),currentPlayer)){
                    money += 1;
                }
                if (tile.building != null && tile.owner == currentPlayer)
                {
                    money += tile.building.returnRevenue(x,y,currentMap);
                if (HasActivePolicy("State monopolies") && tile.building is Pasture)
                {
                    money += 1;
                }   
                if (HasActivePolicy("Free Market") && tile.building is FishingBoats)
                {
                    money += 1;
                }   
                }
                if(tile.district != null && tile.owner == currentPlayer){
                    money += tile.district.returnRevenue(x,y,currentMap);
                }

                if(tile.district != null && tile.owner == currentPlayer && tile.hasRoad){
                    if(currentMap.IsConnectedToCapitalByRoad(tile, currentPlayer)){
                    money ++; 
                    if(hasRomeTransMonument) money ++;
                }
                    if(currentPlayer.currentGovernment.name == "Monarchy"){
                        money ++;
                        
                    }
                }
                if(tile.district is City && tile.owner == currentPlayer && tile.hasTrainTrack){
                    if(currentMap.IsConnectedToCapitalByTrain(tile, currentPlayer))
                    money ++;
                    if(currentPlayer.currentGovernment.name == "Monarchy"){
                        money ++;
                    }
                }
                if (hasWonder("Eygpt", typeof(PopWonder),currentPlayer) && tile.building is Farm && tile.tileType == "Desert"&& tile.owner == currentPlayer )
                {
                    money += 2;
                }
            }
        }
        if (currentPlayer.currentGovernment != null)
        {
            foreach (Ministrys ministry in currentPlayer.currentGovernment.ministrysPolicies)
            {
                foreach (GovernmentBuildings building in ministry.activeGovernmentBuildings)
                {
                    if (building.name == "Mint")
                    {
                        money += 10;
                    }
                }
            }
        }
        return money;
    }
    public int countCulture(){
        int culture = 0;

        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile tile = currentMap.tiles[x, y];

                if(tile.tileType == "Ocean"&& tile.owner == currentPlayer && currentPlayer.currentGovernment.name == "Republic"){
                    culture += 1;
                }
                if(tile.hasRoad == true && tile.owner == currentPlayer && HasActivePolicy("Trade Routes")){
                    culture += 1;
                }
                if((tile.forestResource != ""/* || tile.forestResource != null */)&& tile.owner == currentPlayer && HasActivePolicy("National Parks")){
                    culture += 2;
                }
                if(tile.hasWall == true && tile.owner == currentPlayer && HasActivePolicy("Heritage")){
                    culture += 2;
                }
                if (tile.building != null && tile.owner == currentPlayer)
                {
                    culture += tile.building.returnCulture(x,y,currentMap);
                }
                if(tile.district != null && tile.owner == currentPlayer){
                    culture += tile.district.returnCulture(x,y,currentMap);
                }
                if (hasWonder("Eygpt", typeof(PopWonder),currentPlayer) && tile.building is Farm&& tile.tileType == "Desert"&& tile.owner == currentPlayer )
                {
                    culture += 2;
                }
                if (hasWonder("Eygpt", typeof(TradeWonder),currentPlayer) && tile.tileType == "Desert"&& tile.owner == currentPlayer )
                {
                    culture += 1;
                }
                if (HasActivePolicy("Free Market") && tile.building is FishingBoats)
                {
                    culture += 1;
                } 
            }
        }
        if (currentPlayer.currentGovernment != null)
        {
            foreach (Ministrys ministry in currentPlayer.currentGovernment.ministrysPolicies)
            {
                foreach (GovernmentBuildings building in ministry.activeGovernmentBuildings)
                {
                    if (building.name == "Theater")
                    {
                        culture += 10;
                    }   
                }
            }
        }
        if(hasWonder("Rome", typeof(ExplorerWonder),currentPlayer)){
            culture *= 2;
        }

        return culture;
    }

    public bool hasWonder(string wonderVarant, System.Type wonderType,Player wonderPlayer){
        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile tile = currentMap.tiles[x, y];

                if(tile.wonder != null && tile.owner == wonderPlayer && tile.wonder.typeOwner.tribeType == wonderVarant && tile.wonder.GetType() == wonderType){
                    return true;
                }
            }
        }
        return false;
    }

    public bool HasActivePolicy(string policyName)
    {
    // Government slots
    foreach (var slot in currentPlayer.currentGovernment.policySlots)
        if (slot.activePolicy != null && slot.activePolicy.name == policyName)
            return true;

    // Ministry slots
    foreach (var ministry in currentPlayer.currentGovernment.ministrysPolicies)
        foreach (var slot in ministry.policySlots)
            if (slot.activePolicy != null && slot.activePolicy.name == policyName)
                return true;

    // Government building slots
    foreach (var ministry in currentPlayer.currentGovernment.ministrysPolicies)
        foreach (var building in ministry.activeGovernmentBuildings)
            foreach (var slot in building.policySlots)
                if (slot.activePolicy != null && slot.activePolicy.name == policyName)
                    return true;

    return false;
}

    public void GetAIVisuals(){
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Visual"))
        {
            Destroy(obj);
        }
        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {        
                Tile tile = currentMap.tiles[x, y];
if (tile.unit == null) continue;
        GameObject troopPrefab = null;
        if (tile.unit.GetType().Name == "Warrior") troopPrefab = warriorPrefab;
        if (tile.unit.GetType().Name == "Horseman") troopPrefab = horsemanPrefab;
        if (tile.unit.GetType().Name == "Spearman") troopPrefab = spearmanPrefab;
        if (tile.unit.GetType().Name == "Boat") troopPrefab = boatPrefab;
        if (tile.unit.GetType().Name == "Ship") troopPrefab = shipPrefab;
        if (tile.unit is RammingShip) troopPrefab = rammingShipPrefab;
        if (tile.unit is Archer) troopPrefab = archerPrefab;
        if (tile.unit is Chariot) troopPrefab = chariotPrefab;
        if (tile.unit is Shield) troopPrefab = shieldPrefab;
        if (tile.unit is Swordsman) troopPrefab = swordsManPrefab;
        if (tile.unit is Knight) troopPrefab = knightPrefab;
        if (tile.unit is Catapult) troopPrefab = catapultPrefab;
        if (tile.unit is Frigate) troopPrefab = frigatePrefab;
        if (tile.unit is Caraval) troopPrefab = caravelPrefab;
        if (tile.unit is Musketman) troopPrefab = musketeerPrefab;
        if (tile.unit is Cannon) troopPrefab = cannonPrefab;
        if (tile.unit is Cavalry) troopPrefab = cavalryPrefab;
        if (tile.unit is MachineGun) troopPrefab = machineGunPrefab;
        if (tile.unit is Infantry) troopPrefab = infantryPrefab;
        if (tile.unit is Artillery) troopPrefab = artilleryPrefab;
        if (tile.unit is Tank) troopPrefab = tankPrefab;
        if (tile.unit is Zeppelin) troopPrefab = zeppelinPrefab;
        if (tile.unit is Biplane) troopPrefab = biplanePrefab;
        if (tile.unit is Cruiser) troopPrefab = cruiserPrefab;
        if (tile.unit is Dreadnort) troopPrefab = dreadnortPrefab;


        if (troopPrefab != null)
        {

                Vector3 pos = CalculateHexPosition(x, y)/* + Vector3.up * 0.1f*/;
                GameObject troopObj = Instantiate(troopPrefab, pos, Quaternion.identity);
                troopObj.transform.rotation = Quaternion.Euler(0, -45f, 0);
                tile.unit.visual = troopObj;
                troopObj.tag = "Visual";
                SetLayerRecursively(troopObj,LayerMask.NameToLayer("InvisibleLayer"));
                            ColorUnit(troopObj, tile.unit.owner.playerColor,tile.unit.owner.SecondaryColor);

            }}
        }
    }

    // ------------------- Drawing -------------------
    public void Draw(bool drawClouds = false, bool drawTile = false,bool drawUnit = false,bool drawBuildings = false,bool drawMovementAttackIndicators = false,bool drawBorders = false,bool drawWalls = false,bool drawRoads = false,bool drawTrainTracks = false,bool drawWonders = false,bool drawTradeRoutes = false)
    {           
            // Destroy all objects tagged "Destroyable"
            if(drawClouds){
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Destroyable"))
                {
                    Destroy(obj);
                }
            }
            if(drawTile){
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Tile"))
            {checkIfPopMonument();

                Destroy(obj);
            }
            }
            if(drawUnit){
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Unit"))
            {
                Destroy(obj);
            }}
                        if(drawBuildings){

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Buildings"))
            {
                Destroy(obj);
            }}
                        if(drawMovementAttackIndicators){

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("MovementAttackIndicators"))
            {
                Destroy(obj);
                
            }}
                        if(drawBorders){

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Borders"))
            {checkIfPopMonument();
                Destroy(obj);
            }}
                        if(drawWalls){

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Walls"))
            {
                Destroy(obj);
            }}
                        if(drawRoads){
checkIfTradeMonument();
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Roads"))
            {
                Destroy(obj);
            }}
                        if(drawTrainTracks){
checkIfTradeMonument();

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TrainTracks"))
            {
                Destroy(obj);
            }}
                        if(drawWonders){

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Wonders"))
            {checkIfPopMonument();
                Destroy(obj);
            }}
            if(drawTradeRoutes)
            {
                checkIfTradeMonument();

                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TradeRoutes"))
                {
                    Destroy(obj);
                }
            }

        // Spawn tiles and units
        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile tile = currentMap.tiles[x, y];

                if (humanPlayer != null && !humanPlayer.exploredTiles[x, y] /*&&drawClouds == true*/ )
                {
                    if(drawClouds)
                    {
                        Vector3 pos = CalculateHexPosition(x, y); // slightly above tile
                        GameObject cloud = Instantiate(cloudPrefab, pos, Quaternion.identity);
                        cloud.tag = "Destroyable"; // so it will be cleared in next Draw()
                    }
                }
                else
                {
                    if(drawTile)
                    SpawnTile(tile, x, y);
                    if(drawUnit)
                    SpawnUnit(tile, x, y);
                    if(drawMovementAttackIndicators)
                    SpawnMovementAttackIndicators(tile, x, y);
                    if(drawBorders)
                    SpawnBorders(tile, x, y);
                    if(drawBuildings)
                    SpawnBuildings(tile, x, y);
                    if(drawWalls)
                    SpawnWalls(tile, x, y);
                    if(drawRoads)
                    SpawnRoads(tile, x, y);
                    if(drawTrainTracks)
                    SpawnTrainTracks(tile, x, y);
                    if(drawWonders)
                    SpawnWonders(tile, x, y);
                    if(drawTradeRoutes)
                    SpawnTradeRoutes(tile, x, y);
                }
            }
        }

        if(drawMovementAttackIndicators)
            SpawnSelectionIndicators();        // Highlight selected tile/troop



        // Display current player info
        if (moneyText != null && currentPlayer != null)
        {
            moneyText.text = $"{currentPlayer.money} (+{countMoney()})" ;
            cultureText.text = $"{currentPlayer.culture} (+{countCulture()})";
        }
    }

    public void UpdateStats()
    {
        if (moneyText != null && currentPlayer != null)
        {
            moneyText.text = $"{humanPlayer.money} (+{countMoney()})" ;
            cultureText.text = $"{humanPlayer.culture} (+{countCulture()})";
        }
    }
private void SpawnWonders(Tile tile, int x, int y)
{
    if (tile.wonder == null) return;

    GameObject wonderPrefab = null;
    Wonder wonder = tile.wonder;

    Player player = wonder.typeOwner;

    if(wonder is PopWonder){
        if(player.tribeType == "Eygpt"){
            wonderPrefab = eygptPopWonder;
        }
        if(player.tribeType == "Rome"){
            wonderPrefab = romePopWonder;
        }
        if(player.tribeType == "Greece"){
            wonderPrefab = greecePopWonder;
        }
        if(player.tribeType == "Persia"){
            wonderPrefab = persiaPopWonder;
        }
    }else if(wonder is TradeWonder){
        if(player.tribeType == "Eygpt"){
            wonderPrefab = eygptTradeWonder;
        }
        if(player.tribeType == "Rome"){
            wonderPrefab = romeTradeWonder;
        }
        if(player.tribeType == "Greece"){
            wonderPrefab = greeceTradeWonder;
        }
        if(player.tribeType == "Persia"){
            wonderPrefab = persiaTradeWonder;
        }
    }else if(wonder is ExplorerWonder){
        if(player.tribeType == "Eygpt"){
            wonderPrefab = eygptExplorerWonder;
        }
        if(player.tribeType == "Rome"){
            wonderPrefab = romeExplorerWonder;
        }
        if(player.tribeType == "Greece"){
            wonderPrefab = greeceExplorerWonder;
        }
        if(player.tribeType == "Persia"){
            wonderPrefab = persiaExplorerWonder;
        }
    }

    if (wonderPrefab == null) return;

    Vector3 pos = CalculateHexPosition(x, y);
    GameObject obj = Instantiate(wonderPrefab, pos, Quaternion.identity);
    obj.tag = "Destroyable";
    obj.tag = "Wonders";

    ColorUnit(obj, tile.owner.playerColor, tile.owner.SecondaryColor);
    SetLayerRecursively(obj, LayerMask.NameToLayer("MainLayer"));
}


    private void ColorUnit(GameObject obj, Color c, Color s)
    {
        Renderer[] rends = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in rends)
        {
            foreach (Material m in r.materials)
            {            if (!m.HasProperty("_Color"))
                continue;
                if (m.color == Color.black)
                {
                    m.color = c;
                }
                if(m.color == Color.white)
                {
                    m.color = s;
                }
            }
        }
    }

    private void HighlightUnit(GameObject obj, Color c)
    {
        Renderer[] rends = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in rends)
        {
            foreach (Material m in r.materials)
            {            if (!m.HasProperty("_Color"))
                continue;

                    m.color = c;

            }
        }
    }
    public void GenerateCivicsTree(){
civicList = new List<Civic>
{
    new Civic(
        "Hierarchy",
        5,
        null,
        policies: new List<Policy>
        {
            new Policy("Plantations", Policy.PolicyType.Industrial, 5, "Farms get +1 food output")
        }
    ),

    new Civic(
        "Divine Right",
        10,
        new List<string> { "Hierarchy" },
        governments: new List<Govenments>
        {
            new Govenments("Monarchy", 6, "+1 gold from every trade route to your capital", 2,   new List<PolicySlot.PolicyType>
    {
        PolicySlot.PolicyType.Military,
        PolicySlot.PolicyType.Military,
        PolicySlot.PolicyType.Economic,
        PolicySlot.PolicyType.Wildcard
    })
        },
        policies: new List<Policy>
        {
            new Policy("Martial Tradition", Policy.PolicyType.Military, 5, "Defense bonus on forests")
        }
    ),   new Civic(
    "Law and Order",
    10,
    new List<string> { "Hierarchy" },
    policies: null,
    governments: null,
    ministrys: new List<Ministrys>
    {
        new Ministrys("Ministry of Justice", 5, "It contains a social, economic and military policy slot. Also 2 building slots", 2,new List<PolicySlot.PolicyType>
        {   
            PolicySlot.PolicyType.Social,
            PolicySlot.PolicyType.Economic,
            PolicySlot.PolicyType.Military
        })
    },
    governmentBuildings: null
),new Civic(
        "Judges",
        20,
        new List<string> { "Law and Order" },

        governmentBuildings: new List<GovernmentBuildings>
        {
            new GovernmentBuildings("Court", 7, "Wildcard policy slot", new List<PolicySlot.PolicyType>{PolicySlot.PolicyType.Wildcard})
        },
        policies: new List<Policy>
        {
            new Policy("Mining Rights", Policy.PolicyType.Industrial, 5, "+5 gold from every mine")
        }
    ),
    new Civic("Research", 40, new List<string> { "Judges" }, policies: new List<Policy>{new Policy("Research Grants", Policy.PolicyType.Industrial, 5, "20% of every tech"),}),
    new Civic("Bureaucracy", 40, new List<string> { "Judges" },
    ministrys: new List<Ministrys>
    {
        new Ministrys("Ministry of Interior", 5, "Gives 2 building slots and 2 industrial policy slots", 3,new List<PolicySlot.PolicyType>
        {   
            PolicySlot.PolicyType.Industrial,
            PolicySlot.PolicyType.Industrial
        }),
    },governmentBuildings: new List<GovernmentBuildings>
        {
            new GovernmentBuildings("Press Office", 5, "Social policy slot", new List<PolicySlot.PolicyType>{PolicySlot.PolicyType.Social}),
        }
    ),
    new Civic("Public Transport", 80, new List<string> { "Bureaucracy" }, policies: new List<Policy>{new Policy("Subway", Policy.PolicyType.Economic, 5, "+2 gold and +1 culture to each city tile with a building"),}),
    new Civic("Currency", 5, null, policies: new List<Policy> {new Policy("Free Market", Policy.PolicyType.Economic , 5, "+1 gold and +1 culture from each fishing boat"),new Policy("Wood Working", Policy.PolicyType.Industrial , 5, "+2 culture from each lumber hut")}),
    new Civic("Taxation", 20, new List<string> { "Economics" }, policies: new List<Policy>{new Policy("Tariffs", Policy.PolicyType.Economic, 5, "Double revenue from harbour levels"),},governmentBuildings: new List<GovernmentBuildings>
        {new GovernmentBuildings("Tax office", 5, "Econmomic policy slot",new List<PolicySlot.PolicyType>{PolicySlot.PolicyType.Economic}),}),

     new Civic(
        "Capitalism",40,
        new List<string> { "Taxation" },
        governments: new List<Govenments>
        {
            new Govenments("Oligarchy", 6, "Doubles revenue from levels for commercial and industrial districts", 1,   new List<PolicySlot.PolicyType>
    {
        PolicySlot.PolicyType.Economic,
        PolicySlot.PolicyType.Economic,
        PolicySlot.PolicyType.Industrial,
        PolicySlot.PolicyType.Industrial,
        PolicySlot.PolicyType.Wildcard,
        PolicySlot.PolicyType.Wildcard,
        PolicySlot.PolicyType.Military,


    })
        }),
    new Civic("Industrialization", 40, new List<string> { "Taxation" }, policies: new List<Policy> {new Policy("Child labour", Policy.PolicyType.Economic , 5, "Citys provide an adjancy bonus of 3 for industrial zones"),new Policy("Public Works", Policy.PolicyType.Economic , 5, "Every time you build a road or a train track you get +2 culture")}),
    new Civic("Mass Media", 80, new List<string> { "Industrialization" }, policies: new List<Policy> {new Policy("Radio", Policy.PolicyType.Industrial , 5, "Commercial districts get +1 culture per level"),new Policy("Propaganda", Policy.PolicyType.Social , 5, "Every time you kill an enemy you get +4 culture")}),

    new Civic("Economics", 10, new List<string> { "Currency" }, null, null, ministrys: new List<Ministrys>
    {
        new Ministrys("Treasury", 5, "3 economic slots and 1 building slot", 1,new List<PolicySlot.PolicyType>
        {   
            PolicySlot.PolicyType.Economic,
            PolicySlot.PolicyType.Economic,
            PolicySlot.PolicyType.Economic
        })
    },        governmentBuildings: new List<GovernmentBuildings>
        {new GovernmentBuildings("Mint", 5, "Prints +5 gold")}
    ),
    new Civic("Caravans", 10, new List<string> { "Currency" }, policies: new List<Policy>{new Policy("Trade Routes", Policy.PolicyType.Economic, 5, "Each road provide +1 culture"),}),
    new Civic("Navel Impressment", 20, new List<string> { "Caravans" }, policies: new List<Policy>{new Policy("Press Gangs", Policy.PolicyType.Military, 5, "Defence bonus on coasts and rivers"),}),
    new Civic("Centralised Power", 5, null, policies: new List<Policy> {new Policy("State monopolies", Policy.PolicyType.Economic , 5, "Pastures generate +1 gold"),new Policy("Discipline", Policy.PolicyType.Military , 5, "Defence bonus on mountains")}),
    new Civic("Mobilization", 10, new List<string> { "Centralised Power" }, policies: new List<Policy>{new Policy("Conscription", Policy.PolicyType.Military, 5, "All troops cost 2 less dollars to build"),},
    ministrys: new List<Ministrys>
    {
        new Ministrys("Ministry of War", 5, "2 military slots, 2 building slots", 2,new List<PolicySlot.PolicyType>
        {   
            PolicySlot.PolicyType.Military,
            PolicySlot.PolicyType.Military
        }),
    }),new Civic(
        "Wartime economy",
        20,
        new List<string> { "Mobilization" },

        governmentBuildings: new List<GovernmentBuildings>
        {
            new GovernmentBuildings("Academy", 5, "Military policy slot", new List<PolicySlot.PolicyType>{PolicySlot.PolicyType.Military})
        },
        policies: new List<Policy>
        {
            new Policy("Maritime Industry", Policy.PolicyType.Industrial, 5, "Ships are 5 gold cheaper")
        }
    ),
    new Civic("Winter Warfare", 40, new List<string> { "Wartime economy" }, policies: new List<Policy>{new Policy("Skis", Policy.PolicyType.Military, 5, "+1 movement on snow"),}),
    new Civic("Expansion", 40, new List<string> { "Wartime economy" }, policies: new List<Policy> {new Policy("Colonization", Policy.PolicyType.Economic , 5, "Claim tile now claims adjacent ones too"),new Policy("Immigration", Policy.PolicyType.Social , 5, "Citys are half cost")}),
    new Civic("Nationalism", 80, new List<string> { "Expansion" }, policies: new List<Policy>{new Policy("Nationalism", Policy.PolicyType.Military, 5, "1/2 of unit cost refunded as culture per unit built"),new Policy("Admirals", Policy.PolicyType.Military, 5, "Defence bonus on ocean"),new Policy("Workers Rights", Policy.PolicyType.Industrial, 5, "Industrial districts now produce +1 culture per level")}),
    new Civic(
        "Art",
        5,
        null,
        policies: new List<Policy>
        {
            new Policy("Sculptures", Policy.PolicyType.Social, 5, "Palace Provides +2 culture and +2 Gold")
        }
    ),
    new Civic("Writing", 10, new List<string> { "Art" }, policies: new List<Policy> {new Policy("Education", Policy.PolicyType.Social , 5, "20% off all social technologys"),new Policy("Literature", Policy.PolicyType.Social , 5, "+1 culture per city tile")}),
new Civic(
    "Philosophy",
    10,
    new List<string> { "Art" },

    governments: new List<Govenments>
    {
        new Govenments(
            "Republic",
            6,
            "+1 culture per ocean tile in your empire",
            3,
            new List<PolicySlot.PolicyType>
            {
                PolicySlot.PolicyType.Economic,
                PolicySlot.PolicyType.Social
            }
        )
    },

    ministrys: new List<Ministrys>
    {
        new Ministrys(
            "Ministry of Culture",
            5,
            "+3 social policy slots and 1 building slot",
            1,
            new List<PolicySlot.PolicyType>
            {
                PolicySlot.PolicyType.Social,
                PolicySlot.PolicyType.Social,
                PolicySlot.PolicyType.Social
            }
        )
    }
),

new Civic(
    "Scripture",
    20,
    new List<string> { "Writing" },

    governments: new List<Govenments>
    {
        new Govenments(
            "Theocracy",
            6,
            "+2 gold and +2 culture on each city tile",
            2,
            new List<PolicySlot.PolicyType>
            {
                PolicySlot.PolicyType.Wildcard,
                PolicySlot.PolicyType.Social,
                PolicySlot.PolicyType.Wildcard,
                PolicySlot.PolicyType.Social
            }
        )
    },

    governmentBuildings: new List<GovernmentBuildings>
    {
        new GovernmentBuildings(
            "Theater",
            5,
            "Gives +5 culture per turn"
        )
    }
),

    new Civic(
    "Records",
    40,
    new List<string> { "Scripture" },

    policies: new List<Policy>
    {
        new Policy(
            "Heritage",
            Policy.PolicyType.Social,
            5,
            "Walls give +2 culture"
        )
    },

    ministrys: new List<Ministrys>
        {
            new Ministrys(
                "Ministry of State",
                5,
                "3 wild cards slots",
                0,
                new List<PolicySlot.PolicyType>
                {
                    PolicySlot.PolicyType.Wildcard,
                    PolicySlot.PolicyType.Wildcard,
                    PolicySlot.PolicyType.Wildcard,

                }
            )
        },

        governmentBuildings: new List<GovernmentBuildings>
        {
            new GovernmentBuildings(
                "Archives",
                5,
                "Adds an industrial policy slot",
                new List<PolicySlot.PolicyType>
                {
                PolicySlot.PolicyType.Industrial
                }
            )
        }
        ),
        new Civic("Environmentalism", 80, new List<string> { "Records" }, policies: new List<Policy>{new Policy("National Parks", Policy.PolicyType.Social, 5, "+2 culture on all forest tiles"),}),

        /*policyList.Add(new Policy("State monopolies", Policy.PolicyType.Economic, 5, "Doubles output of upperclass"));
        currentPlayer.unlockedPolicys.Add(new Policy("State monopolies", Policy.PolicyType.Economic, 5, "Doubles output of upperclass"));
        currentPlayer.unlockedPolicys.Add(new Policy("Free market", Policy.PolicyType.Economic, 5, " increased revenue from commercial zones doubled adjacency bonuses"));
        currentPlayer.unlockedGovernments.Add(new Govenments("Monarchy", 5, "Palce gives more money", 2));
        currentPlayer.unlockedGovernments.Add(new Govenments("Republic", 5, "Ocean gives culture", 3));
        currentPlayer.unlockedMinistrys.Add(new Ministrys("Monarchy", 5, "Palce gives more money", 2));
        currentPlayer.unlockedMinistrys.Add(new Ministrys("Ministry of war", 5, "Ministry of war", 2));
        currentPlayer.unlockedGovernmentBuildings.Add(new GovernmentBuildings("Academy", 5, "Palce gives more money"));
        currentPlayer.unlockedGovernmentBuildings.Add(new GovernmentBuildings("Theater", 5, "Palce gives more money"));*/
    };}
   /* public void unlockHierarchy()
    {
        foreach (Civic c in civicList)
        {
            if (c.name == "Hierarchy")
            {
                currentCivic = c;
            }
        }

        if (currentCivic != null && currentPlayer.culture >= currentCivic.cost &&currentCivic.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedCivics.Add(currentCivic);
            currentPlayer.culture -= currentCivic.cost;
        
            if (currentCivic.unlockedPolicies != null)
            {
                foreach (Policy policy in currentCivic.unlockedPolicies)
                {
                    currentPlayer.unlockedPolicys.Add(policy);
                }
            }
            if (currentCivic.unlockedGovernments != null)
            {
                foreach (Govenments gov in currentCivic.unlockedGovernments)
                {
                    currentPlayer.unlockedGovernments.Add(gov);
                }
            }
            if (currentCivic.unlockedMinistrys != null)
            {
                foreach (Ministrys ministry in currentCivic.unlockedMinistrys)
                {
                    currentPlayer.unlockedMinistrys.Add(ministry);
                }
            }
            if (currentCivic.unlockedGovernmentBuildings != null)
            {
                foreach (GovernmentBuildings building in currentCivic.unlockedGovernmentBuildings)
                {
                    currentPlayer.unlockedGovernmentBuildings.Add(building);
                }
            }
            Draw();

        }
    }*/
    public void unlockHierarchy()          { UnlockCivicByName("Hierarchy"); }
public void unlockDivineRight()        { UnlockCivicByName("Divine Right"); }
public void unlockLawAndOrder()        { UnlockCivicByName("Law and Order"); }
public void unlockJudges()             { UnlockCivicByName("Judges"); }
public void unlockResearch()           { UnlockCivicByName("Research"); }
public void unlockBurocracy()          { UnlockCivicByName("Bureaucracy"); }
public void unlockPublicWorks()        { UnlockCivicByName("Public Transport"); }
public void unlockCurrency()           { UnlockCivicByName("Currency"); }
public void unlockTaxation()           { UnlockCivicByName("Taxation"); }
public void unlockCapitalism()         { UnlockCivicByName("Capitalism"); }
public void unlockIndustrialization()  { UnlockCivicByName("Industrialization"); }
public void unlockMassMedia()          { UnlockCivicByName("Mass Media"); }
public void unlockEconomics()          { UnlockCivicByName("Economics"); }
public void unlockCaravans()           { UnlockCivicByName("Caravans"); }
public void unlockNavalImpressment()   { UnlockCivicByName("Navel Impressment"); }
public void unlockCentralisedPower()   { UnlockCivicByName("Centralised Power"); }
public void unlockMobilization()       { UnlockCivicByName("Mobilization"); }
public void unlockWartimeEconomy()     { UnlockCivicByName("Wartime economy"); }
public void unlockWinterWarfare()      { UnlockCivicByName("Winter Warfare"); }
public void unlockExpansion()          { UnlockCivicByName("Expansion"); }
public void unlockNationalism()        { UnlockCivicByName("Nationalism"); }
public void unlockArt()                { UnlockCivicByName("Art"); }
public void unlockWriting()            { UnlockCivicByName("Writing"); }
public void unlockPhilosophy()         { UnlockCivicByName("Philosophy"); }
public void unlockScripture()          { UnlockCivicByName("Scripture"); }
public void unlockRecords()            { UnlockCivicByName("Records"); }
public void unlockEnvironmentalism()   { UnlockCivicByName("Environmentalism"); }

private void UnlockCivicByName(string civicName)
{
    if (civicList == null || currentPlayer == null)
        return;

    currentCivic = null;

    foreach (Civic c in civicList)
    {
        if (c.name == civicName)
        {
            currentCivic = c;
            break;
        }
    }

    if (currentCivic == null)
        return;

    if (currentPlayer.unlockedCivics.Contains(currentCivic))
        return;

    if (currentPlayer.culture < currentCivic.Cost(currentPlayer))
        return;

    if (!currentCivic.CanUnlock(currentPlayer))
        return;

    // Unlock civic
    currentPlayer.unlockedCivics.Add(currentCivic);
    currentPlayer.culture -= currentCivic.Cost(currentPlayer);

    // Unlock contents
    if (currentCivic.unlockedPolicies != null)
        currentPlayer.unlockedPolicys.AddRange(currentCivic.unlockedPolicies);

    if (currentCivic.unlockedGovernments != null)
        currentPlayer.unlockedGovernments.AddRange(currentCivic.unlockedGovernments);

    if (currentCivic.unlockedMinistrys != null)
        currentPlayer.unlockedMinistrys.AddRange(currentCivic.unlockedMinistrys);

    if (currentCivic.unlockedGovernmentBuildings != null)
        currentPlayer.unlockedGovernmentBuildings.AddRange(currentCivic.unlockedGovernmentBuildings);

    //Draw();
    UpdateStats();
}

    public void GenerateTechTree(){
        techList.Add(new Tech("Animal Husbandry",5, null,"Horseman\n•Health: 12\n•Attack: 2\n•Defence: 2\n•Range: 1\n•Movement: 4\n\nPasture - Generates 1 gold and 0.5 food. Must be built on field"));
        techList.Add(new Tech("Farming", 5, null,"Farm - Generates 2 food. Must be built on crop\n\nBurn forest - Allows you to turn forest into crop"));
        techList.Add(new Tech("Climbing",5, null,"Chop - allows chopping of forsts, provide 2 gold\n\nClimbing - allows movement on mountains"));
        techList.Add(new Tech("Sailing",5, null,"Allows harvesting of fish, provides 3 gold\n\nAllows movement on water\n\nFishing Boats - Generates 1 gold and 1.5 food"));
        techList.Add(new Tech("Hunting",5, null,"Spearman\n•Health: 12\n•Attack: 3\n•Defence: 2\n•Range: 1\n•Movement: 2\n\nAllows hunting of animals - gives 3 gold"));
        techList.Add(new Tech("Ship Building", 12,new List<string>{"Sailing"},"Harbour - Provides 1 gold per level, +2 levels per adjacent fishing boat or whaling ship, allows the construction of boats \n\n Ship\n•Health: 20\n•Attack: 4\n•Defence: 3\n•Range: 2\n•Movement: 4"));
        //Tech sailing = techList.Find(t => t.Name == "Sailing");
        //Tech shipBuilding = techList.Find(t => t.Name == "Ship Building");
        //shipBuilding.prerequisites.Add(sailing);
        techList.Add(new Tech("Mining", 12,new List<string>{"Climbing"}, "Mine - Generates 4 gold, must be built on metal"));
        //Tech climbing = techList.Find(t => t.Name == "Climbing");
        //Tech mining = techList.Find(t => t.Name == "Mining");
        //mining.prerequisites.Add(climbing);
        techList.Add(new Tech("Masonary", 12,new List<string>{"Climbing"}, "Quarry - Generates 1 gold, must be built on mountain \n\n Monument - Generates 2 culture, must be built on city"));
        //Tech masonary = techList.Find(t => t.Name == "Masonary");
        //masonary.prerequisites.Add(climbing);
        techList.Add(new Tech("Defence", 12,new List<string>{"Farming"}, "Wall - Provides Defence Bonus, must be built on city, \n\n Shield\n•Health: 15\n•Attack: 1\n•Defence: 3\n•Range: 1\n•Movement: 2 "));
        techList.Add(new Tech("Wheels", 12,new List<string>{"Farming"}, "WaterWheel - Generates 2 food per adjacent farm\n\n Chariot\n•Health: 15\n•Attack: 4\n•Defence: 3\n•Range: 1\n•Movement: 4 "));
        techList.Add(new Tech("Travel", 12,new List<string>{"Animal Husbandry"}, "Roads - Generates 1 gold per connection to your capital, tile has a move cost of 0.5 \n\n Bridges - allows roads over rivers"));
        techList.Add(new Tech("Archery", 12,new List<string>{"Hunting"}, "\n\n Archer\n•Health: 10\n•Attack: 2\n•Defence: 1\n•Range: 2\n•Movement: 2 \n\n Fur Trading Post - Generates 3 gold, 1 food and 1 culture"));
        techList.Add(new Tech("Logging", 12,new List<string>{"Hunting"},"Lumber huts - generates 2 gold per turn"));
        techList.Add(new Tech("Navel Warfare", 12,new List<string>{"Ship Building"}, " Ramming Ship \n•Health: 30\n•Attack: 5\n•Defence: 3\n•Range: 1\n•Movement: 5"));
        //Tech NavelWarfare  = techList.Find(t => t.Name == "Navel Warfare");
        //NavelWarfare.prerequisites.Add(shipBuilding);
        techList.Add(new Tech("Trade", 25,new List<string>{"Ship Building"},"Trade Routes - Generates 1 gold per connection to your capital, tile has a move cost of 0.5 \n\n Customs house - Generates 1 gold for ever harbour level "));
        techList.Add(new Tech("Smelting", 25,new List<string>{ "Mining" }, " Swords Man \n•Health: 15\n•Attack: 4\n•Defence: 3\n•Range: 1\n•Movement: 2, \n\n Forge - Generates 3 gold per adjacent mine"));
        techList.Add(new Tech("Fortifications", 25,new List<string>{"Masonary"}, "Fort - Provides a wall bonus"));
        techList.Add(new Tech("Engineering", 25,new List<string>{"Defence"}, "Windmill - Generates 2 gold per adjacent farm"));
        techList.Add(new Tech("Commerce", 25,new List<string>{"Travel"}, "Commercial District - Generates 1 gold per adjacent city and city building, Generates 3 gold per adjacent harbour \n\n Tower - Generate 2 gold per adjcent tile"));
        techList.Add(new Tech("Stirrups", 25,new List<string>{"Travel", "Knight \n•Health: 20\n•Attack: 4\n•Defence: 4\n•Range: 1\n•Movement: 6"}));
        techList.Add(new Tech("Machinary", 25,new List<string>{"Logging"}, "Saw mill - Generates 2 gold per adjacent forest"));
        techList.Add(new Tech("Navigation", 60,new List<string>{"Navel Warfare"}, "Frigate\n•Health: 35\n•Attack: 8\n•Defence: 6\n•Range: 2\n•Movement: 8\n\nLight house - Generates 3 food, gold and culture"));
        techList.Add(new Tech("Shipyard", 60,new List<string>{"Trade"}, "Caraval \n•Health: 30\n•Attack: 7\n•Defence: 5\n•Range: 2\n•Movement: 6 \n\n Shipyard - Generates 6 gold"));
        techList.Add(new Tech("Whaling", 60,new List<string>{"Trade"},"Whaling Ship - Generates 5 gold"));
        techList.Add(new Tech("Industry", 120,new List<string>{"Coal", "Ware House - Generates 2 gold per adjacent building"}));
        techList.Add(new Tech("Metallurgy", 60,new List<string>{"Smelting"}, "Cannon \n•Health: 10\n•Attack: 6\n•Defence: 2\n•Range: 4\n•Movement: 2,"));
        techList.Add(new Tech("Tactics", 60,new List<string>{"Fortifications"}));
        techList.Add(new Tech("Gunpowder", 60,new List<string>{"Engineering"}, "Destroy - Pay 5 to remove your building \n\n Musket man \n•Health: 12\n•Attack: 5\n•Defence: 2\n•Range: 2\n•Movement: 2,"));
        techList.Add(new Tech("Finance", 60,new List<string>{"Commerce"}, "Bank - Generates 1 Gold per adjacent district and 3 gold per adjacent district building"));
        techList.Add(new Tech("Printing press", 60,new List<string>{"Machinary"}, "Papermill - Generates 2 culture per adjacent lumber hut"));
        techList.Add(new Tech("SteamPower", 120, new List<string>{"Navigation"}, "Destoryer \n•Health: 70\n•Attack: 35\n•Defence: 5\n•Range: 2\n•Movement: 20\n\nNaval base - Provides a wall bonus on that tile"));
        techList.Add(new Tech("Steel", 120, new List<string>{"Shipyard"}, "Dreadnort \n•Health: 120\n•Attack: 20\n•Defence: 15\n•Range: 3\n•Movement: 16\n\nSky Scrapers - Generates 1 dollar for each Commercial district level"));
        techList.Add(new Tech("Refrigeration", 120, new List<string>{"Whaling"}, "Refrigeration Plant - Generates 4 gold per adjacent pasture"));
        techList.Add(new Tech("Coal", 60, new List<string>{"Smelting"}, "Industrial Zone - Generates 1 gold per adjacent mine and Quarry. Generates 5 Gold per adjacent harbour, windmill, sawmill or forge \n\n Carpantry workshop - Generates 2 culture per adjacent lumber hut"));
        techList.Add(new Tech("Military science", 120, new List<string>{"Gunpowder"}, "Cavalry \n•Health: 20 \n•Attack: 5\n•Defence: 3\n•Range: 2\n•Movement: 4"));
        techList.Add(new Tech("Mechanics", 120, new List<string>{"Military science"}, "Machine Guns \n•Health: 10\n•Attack: 4\n•Defence: 1\n•Range: 3\n•Movement: 2"));

        techList.Add(new Tech("Railways", 120, new List<string>{"Finance"},"Railways - You get 1 gold per railway connection to my captial. Give tiles a movement cost of 0\n\nRailway bridges allow you to build railways on rivers"));
        techList.Add(new Tech("AirTravel", 120, new List<string>{"Finance"},"Airports, Generaters 5 stars , Allows you to build air units"));
        techList.Add(new Tech("Science", 120, new List<string>{"Printing press"}, "Universty - gives 20% off each tech"));
        techList.Add(new Tech("Torpedos", 240, new List<string>{"SteamPower"}));
        techList.Add(new Tech("Electricity", 240, new List<string>{"Steel"}));
        techList.Add(new Tech("Tanks", 240, new List<string>{"Industry"}, "Tank factory - Allows you to build tanks\n\nTank \n•Health: 40\n•Attack: 8\n•Defence: 6\n•Range: 2\n•Movement: 1"));
        techList.Add(new Tech("Infantry", 240, new List<string>{"Military science"},"Infantry \n•Health: 12\n•Attack: 5\n•Defence: 2\n•Range: 3\n•Movement: 2"));
        techList.Add(new Tech("Aeroplane", 240, new List<string>{"AirTravel"},"Biplane\n•Health: 20\n•Attack: 6\n•Defence: 3\n•Range: 2\n•Movement: 20"));
        techList.Add(new Tech("Artillary", 240, new List<string>{"Science"},"Artillary\n•Health: 10\n•Attack: 8\n•Defence: 1\n•Range: 4\n•Movement: 2"));
    }

    public void DisplayTechTree(){
                if (isAITurn)
            return;
        techTree.SetActive(true);
        DrawTechs();
        ReturnBtn.SetActive(true);
    }
    public void DisplaySocialTechnologyTree(){
                if (isAITurn)
            return;
        socialTechnologyTree.SetActive(true);
        DrawCivics();
        ReturnBtn.SetActive(true);
    }
    public void DisplayGovernment(){
                if (isAITurn)
            return;
        government.SetActive(true);
        drawPolicy();
        ReturnBtn.SetActive(true);
    }
    public void unlockAnimalHusbandry()
    {
        foreach (Tech t in techList)
        {
            if (t.Name == "Animal Husbandry")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockClimbing()
    {
        foreach (Tech t in techList)
        {
            if (t.Name == "Climbing")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockSailing()
    {
        foreach (Tech t in techList)
        {
            if (t.Name == "Sailing")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockHunting()
    {
        foreach (Tech t in techList)
        {
            if (t.Name == "Hunting")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
        public void unlockFarming()
    {
        foreach (Tech t in techList)
        {
            if (t.Name == "Farming")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockShipBuilding()
    {
        foreach (Tech t in techList)
        {
            if (t.Name == "Ship Building")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockMine()
    {
        foreach (Tech t in techList)
        {
            if (t.Name == "Mining")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockMasonary()
    {
        foreach (Tech t in techList)
        {
            if (t.Name == "Masonary")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockDefence()
    {
        foreach (Tech t in techList)
        {
            if (t.Name == "Defence")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockWheel(){
        foreach (Tech t in techList)
        {
            if (t.Name == "Wheels")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockTravel(){
        foreach (Tech t in techList)
        {
            if (t.Name == "Travel")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockArchery(){
        foreach (Tech t in techList)
        {
            if (t.Name == "Archery")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockLogging(){
        foreach (Tech t in techList)
        {
            if (t.Name == "Logging")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
    public void unlockNavelWarfare(){
        foreach (Tech t in techList)
        {
            if (t.Name == "Navel Warfare")
            {
                currentTech = t;
            }
        }

        if (currentTech != null &&currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) &&currentTech.CanUnlock(currentPlayer))
        {
            currentPlayer.unlockedTechs.Add(currentTech);
            currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
            //Draw();
            UpdateStats();
        }
    }
public void unlockTrade() { UnlockTechByName("Trade"); }
public void unlockSmelting() { UnlockTechByName("Smelting"); }
public void unlockFortifications() { UnlockTechByName("Fortifications"); }
public void unlockEngineering() { UnlockTechByName("Engineering"); }
public void unlockCommerce() { UnlockTechByName("Commerce"); }
public void unlockStirrups() { UnlockTechByName("Stirrups"); }
public void unlockMachinary() { UnlockTechByName("Machinary"); }
public void unlockNavigation() { UnlockTechByName("Navigation"); }
public void unlockShipyard() { UnlockTechByName("Shipyard"); }
public void unlockWhaling() { UnlockTechByName("Whaling"); }
public void unlockIndustry() { UnlockTechByName("Industry"); }
public void unlockMetallurgy() { UnlockTechByName("Metallurgy"); }
public void unlockTactics() { UnlockTechByName("Tactics"); }
public void unlockGunpowder() { UnlockTechByName("Gunpowder"); }
public void unlockFinance() { UnlockTechByName("Finance"); }
public void unlockPrintingPress() { UnlockTechByName("Printing press"); }
public void unlockSteamPower() { UnlockTechByName("SteamPower"); }
public void unlockSteel() { UnlockTechByName("Steel"); }
public void unlockRefrigeration() { UnlockTechByName("Refrigeration"); }
public void unlockCoal() { UnlockTechByName("Coal"); }
public void unlockMechanics() { UnlockTechByName("Mechanics"); }
public void unlockMilitaryScience() { UnlockTechByName("Military science"); }
public void unlockRailways() { UnlockTechByName("Railways"); }
public void unlockAirTravel() { UnlockTechByName("AirTravel"); }
public void unlockScience() { UnlockTechByName("Science"); }
public void unlockTorpedos() { UnlockTechByName("Torpedos"); }
public void unlockElectricity() { UnlockTechByName("Electricity"); }
public void unlockTanks() { UnlockTechByName("Tanks"); }
public void unlockInfantry() { UnlockTechByName("Infantry"); }
public void unlockAeroplane() { UnlockTechByName("Aeroplane"); }
public void unlockArtillary() { UnlockTechByName("Artillary"); }

private void UnlockTechByName(string techName)
{
    foreach (Tech t in techList)
    {
        if (t.Name == techName)
        {
            currentTech = t;
            break;
        }
    }

    if (currentTech != null && currentPlayer.money >= currentTech.Cost(currentPlayer, currentMap) && currentTech.CanUnlock(currentPlayer))
    {
        currentPlayer.unlockedTechs.Add(currentTech);
        currentPlayer.money -= currentTech.Cost(currentPlayer, currentMap);
       //Draw();
       UpdateStats();
    }
}

    public void Return()
    {
        techTree.SetActive(false);
        socialTechnologyTree.SetActive(false);
        government.SetActive(false);
        ReturnBtn.SetActive(false);
        civicDisplayPanel.SetActive(false);
        ignoreClicks = false;
        menuPanel.SetActive(false);
        statsPanel.SetActive(false);
        endGamePanel.SetActive(false);
    }
Quaternion FindWaterRotation(Tile tile)
{
    float rot = 0f;
    int x = tile.x;
    int y = tile.y;

    if (y % 2 == 0) // even row
    {
        if (HasLandTile(x + 1, y)) rot = 180f;
        else if (HasLandTile(x, y + 1)) rot = 120f;
        else if (HasLandTile(x - 1, y + 1)) rot = 60f;
        else if (HasLandTile(x - 1, y)) rot = 0f;
        else if (HasLandTile(x - 1, y - 1)) rot = 300f;
        else if (HasLandTile(x, y - 1)) rot = 240f;
    }
    else // odd row
    {
        if (HasLandTile(x + 1, y)) rot = 180f;
        else if (HasLandTile(x + 1, y + 1)) rot = 120f;
        else if (HasLandTile(x, y + 1)) rot = 60f;
        else if (HasLandTile(x - 1, y)) rot = 0f;
        else if (HasLandTile(x, y - 1)) rot = 300f;
        else if (HasLandTile(x + 1, y - 1)) rot = 240f;
    }

    return Quaternion.Euler(0, rot, 0);
}

// Helper to check if the tile is land
bool HasLandTile(int nx, int ny)
{
    if (nx < 0 || ny < 0 || nx >= currentMap.width || ny >= currentMap.height)
        return false;

    Tile t = currentMap.tiles[nx, ny];
    if (t == null) return false;

    return t.tileType == "District" || t.tileType == "Plains" || t.tileType == "Desert" ||
           t.tileType == "Snow" || t.tileType == "Mountain";
}

    private void SpawnTile(Tile tile, int x, int y)
    {
        GameObject prefab = null;

        if (tile.tileType == "District"/*&& !(tile.district is Harbour)*/)
        {
            if(tile.owner.tribeType == "Eygpt" || tile.owner.tribeType == "Persia"){
                prefab = desertdistrictTilePrefab;
            }else{
                prefab = districtTilePrefab; // Draw district tile
            }
        }
        else if (tile.tileType == "Plains")
        {
            prefab = fieldTilePrefab;
        }else if(tile.tileType == "Ocean"){
            prefab = oceanTilePrefab;
        }else if(tile.tileType == "Desert"){
            prefab = desertTilePrefab;
        }else if(tile.tileType == "Snow"){
            prefab = snowTilePrefab;
        }else if(tile.tileType == "Mountain"){
            prefab = mountainTilePrefab;
        }else if(tile.tileType == "Coast"){
            prefab = coastTilePrefab;
        }else if(tile.tileType == "River"){
            prefab = riverTilePrefab;
        }

        if (prefab != null)
        {
            GameObject spawned = Instantiate(prefab, CalculateHexPosition(x, y), Quaternion.identity);
            spawned.tag = "Destroyable";
                        spawned.tag = "Tile";

        }
            Vector3 spawnPos = CalculateHexPosition(x, y); // slightly above the tile
    GameObject forestPrefab = null;

    if (tile.forestResource == "BorealForest")
        forestPrefab = BorealForestPrefab;
    else if (tile.forestResource == "DryForest")
        forestPrefab = DryForestPrefab;
    else if (tile.forestResource == "TemperateForest")
        forestPrefab = TemperateForestPrefab;
    else if (tile.forestResource == "Rainforest")
        forestPrefab = RainforestPrefab;
    else if (tile.forestResource == "Ice")
    {
        if (tile.tileType == "River")
            forestPrefab = riverIcePrefab;
        else if (tile.tileType == "Ocean" || tile.tileType == "Coast")
            forestPrefab = seaIcePrefab;
    }

    if (forestPrefab != null)
    {
        GameObject forestGO = Instantiate(forestPrefab, spawnPos, Quaternion.identity);
        forestGO.tag = "Destroyable";
        forestGO.tag = "Tile";
        //SetLayerRecursively(forestGO, LayerMask.NameToLayer("MainLayer"));
    }
    // ------------------- Resource Drawing -------------------
GameObject resourcePrefab = null;
bool layer = true;
switch(tile.resource)
{
    case "Crop":
        resourcePrefab = cropResourcePrefab;
        break;
    case "Fish":
        resourcePrefab = fishResourcePrefab;
         layer = false;
        break;
    case "Horse":
        resourcePrefab = horseResourcePrefab;
        break;
    case "Boar":
        resourcePrefab = boarResourcePrefab;
        break;
    case "Deer":
        resourcePrefab = deerResourcePrefab;
        break;
    case "Metal":
        resourcePrefab = metalResourcePrefab;
        break;
    case "Penguin":
        resourcePrefab = penguinResourcePrefab;
        break;
    case "Whale":
        resourcePrefab = whaleResourcePrefab;
        layer = false;

        break;
}

if (resourcePrefab != null)
{
    Vector3 resourcePos = CalculateHexPosition(x, y); // slightly above tile
    GameObject resGO = Instantiate(resourcePrefab, resourcePos, Quaternion.identity);
    resGO.tag = "Destroyable";
        resGO.tag = "Tile";

    if(layer) SetLayerRecursively(resGO, LayerMask.NameToLayer("MainLayer"));
}

        if (tile.district is City)
        {
            Vector3 capitalPos = CalculateHexPosition(x, y)/* + new Vector3(0f, 0.72f, 0f)*/; // slightly above tile
            GameObject capitalGO = null;
            if(tile.owner.tribeType == "Eygpt" || tile.owner.tribeType == "Persia"){
                capitalGO = Instantiate(desertCity, capitalPos, Quaternion.identity);
            }else{
                capitalGO = Instantiate(cityPrefab, capitalPos, Quaternion.identity);
            }
            capitalGO.tag = "Destroyable";
            capitalGO.tag = "Tile";
            ColorUnit(capitalGO, tile.owner.playerColor,tile.owner.SecondaryColor);
            //SetLayerRecursively(capitalGO,LayerMask.NameToLayer("MainLayer"));


        }
        if (tile.district is Harbour)
        {
            Vector3 capitalPos = CalculateHexPosition(x, y)/* + new Vector3(0f, 0.72f, 0f)*/; // slightly above tile
            Quaternion rot = FindWaterRotation(tile);

            GameObject capitalGO = Instantiate(harbourPrefab, capitalPos, rot);
            capitalGO.tag = "Destroyable";
            capitalGO.tag = "Tile";
            //SetLayerRecursively(capitalGO,LayerMask.NameToLayer("MainLayer"));
        }
        if (tile.district is Commercial)
        {
            Vector3 capitalPos = CalculateHexPosition(x, y)/* + new Vector3(0f, 0.72f, 0f)*/; // slightly above tile
            GameObject capitalGO = Instantiate(commercialDistrict, capitalPos, Quaternion.identity);
            capitalGO.tag = "Destroyable";
            capitalGO.tag = "Tile";
            //SetLayerRecursively(capitalGO,LayerMask.NameToLayer("MainLayer"));
        }        
        if (tile.district is Industrial)
        {
            Vector3 capitalPos = CalculateHexPosition(x, y)/* + new Vector3(0f, 0.72f, 0f)*/; // slightly above tile
            GameObject capitalGO = Instantiate(industrialZone, capitalPos, Quaternion.identity);
            capitalGO.tag = "Destroyable";
            capitalGO.tag = "Tile";
            //SetLayerRecursively(capitalGO,LayerMask.NameToLayer("MainLayer"));
        }

    }


    private void SpawnBuildings(Tile tile, int x, int y){
        GameObject transportPrefab = null;
       // if(tile.hasTrainTrack) transportPrefab = trainTracksPrefab;
        //if(tile.hasTrainTrack && tile.tileType == "River") transportPrefab = TrainTrackBridgePrefab;
        if(transportPrefab != null){
            Vector3 pos = CalculateHexPosition(x, y)/* + Vector3.up * 0.1f*/;
            GameObject troopObj = Instantiate(transportPrefab, pos, Quaternion.identity);
            troopObj.tag = "Destroyable";
            troopObj.tag = "Buildings";
            //ColorUnit(troopObj, tile.owner.playerColor,tile.owner.SecondaryColor);
            //SetLayerRecursively(troopObj,LayerMask.NameToLayer("MainLayer"));
        }
        transportPrefab = null;
       // if(tile.hasRoad) transportPrefab = roadPrefab;
      //  if(tile.hasRoad && tile.tileType == "River") transportPrefab = BridgePrefab;
        if(transportPrefab != null){
            Vector3 pos = CalculateHexPosition(x, y)/* + Vector3.up * 0.1f*/;
            GameObject troopObj = Instantiate(transportPrefab, pos, Quaternion.identity);
            troopObj.tag = "Destroyable";
            troopObj.tag = "Buildings";

            //ColorUnit(troopObj, tile.owner.playerColor,tile.owner.SecondaryColor);
            //SetLayerRecursively(troopObj,LayerMask.NameToLayer("MainLayer"));
        }
        GameObject buildingPrefab = null;

        bool setLayer = false;
        if(tile.district != null&& tile.district.building != null){
            if(tile.district.building is Palace) buildingPrefab = palacePrefab;
            if(tile.district.building is Monument) buildingPrefab = monumentPrefab;
            if(tile.district.building is Market) buildingPrefab = marketPrefab;
            if(tile.district.building is CustomHouse) buildingPrefab = customsHousePrefab;
            if(tile.district.building is Tower) buildingPrefab = towerPrefab;
            if(tile.district.building is LightHouse) buildingPrefab = lightHousePrefab;
    if(tile.district.building is Shipyard) buildingPrefab = shipyardPrefab;
    if(tile.district.building is CarpentryWorkshop) buildingPrefab = carpentryWorkshopPrefab;
    if(tile.district.building is Bank) buildingPrefab = bankPrefab;
    if(tile.district.building is NavelBase) buildingPrefab = navalBasePrefab;
    if(tile.district.building is SkyScrapers) buildingPrefab = skyScrapersPrefab;
    if(tile.district.building is Warehouses) buildingPrefab = warehousesPrefab;
    if(tile.district.building is Factorys) buildingPrefab = tankFactoryPrefab;

        }
        if(tile.building != null){
            if(tile.building is Pasture) buildingPrefab = pasturePrefab;
            if(tile.building is Farm) buildingPrefab = farmPrefab;
            if(tile.building is FishingBoats) buildingPrefab = fishingBoatsPrefab;
            if(tile.building is Mine) buildingPrefab = minePrefab;
            if(tile.building is Quarry)buildingPrefab = quarryPrefab;
            if(tile.building is Waterwheel)buildingPrefab = waterwheelPrefab;
            if(tile.building is FurTradingPost){ buildingPrefab = furTradingPostPrefab;setLayer = true;}
    if(tile.building is LumberHut){ buildingPrefab = lumberHutPrefab;setLayer = true;}
    if(tile.building is Forge) buildingPrefab = forgePrefab;
    if(tile.building is Fort) buildingPrefab = fortPrefab;
    if(tile.building is Windmill) buildingPrefab = windmillPrefab;
    if(tile.building is Sawmill) buildingPrefab = sawmillPrefab;
    if(tile.building is WhalingShip) buildingPrefab = whalingShipPrefab;
    if(tile.building is Papermill) buildingPrefab = paperMillPrefab;
    if(tile.building is MeatProcessingPlant) buildingPrefab = meatProcessingPlantPrefab;
    if(tile.building is Airport) buildingPrefab = airportPrefab;
    if(tile.building is University) buildingPrefab = universityPrefab;
        }


        if(buildingPrefab != null){

            Vector3 pos = CalculateHexPosition(x, y)/* + Vector3.up * 0.1f*/;
            GameObject troopObj;
            if(tile.building is Waterwheel||(tile.district != null && tile.district.building is CustomHouse) ||
    (tile.district != null && tile.district.building is LightHouse) ||
    (tile.district != null && tile.district.building is Shipyard) ||
    (tile.district != null && tile.district.building is NavelBase)){
                Quaternion rot = FindWaterRotation(tile);
                 troopObj = Instantiate(buildingPrefab, pos, rot);

            }else{
                 troopObj = Instantiate(buildingPrefab, pos, Quaternion.identity);

            }
            if(troopObj != null){
            troopObj.tag = "Destroyable";
            troopObj.tag = "Buildings";
            ColorUnit(troopObj, tile.owner.playerColor,tile.owner.SecondaryColor);
            if(setLayer)
            SetLayerRecursively(troopObj,LayerMask.NameToLayer("MainLayer"));
            }
        }
    }

    private void SpawnUnit(Tile tile, int x, int y)
    {
        if (tile.unit == null) return;

        GameObject troopPrefab = null;
        if (tile.unit.GetType().Name == "Warrior") troopPrefab = warriorPrefab;
        if (tile.unit.GetType().Name == "Horseman") troopPrefab = horsemanPrefab;
        if (tile.unit.GetType().Name == "Spearman") troopPrefab = spearmanPrefab;
        if (tile.unit.GetType().Name == "Boat") troopPrefab = boatPrefab;
        if (tile.unit.GetType().Name == "Ship") troopPrefab = shipPrefab;
        if (tile.unit is RammingShip) troopPrefab = rammingShipPrefab;
        if (tile.unit is Archer) troopPrefab = archerPrefab;
        if (tile.unit is Chariot) troopPrefab = chariotPrefab;
        if (tile.unit is Shield) troopPrefab = shieldPrefab;
        if (tile.unit is Swordsman) troopPrefab = swordsManPrefab;
        if (tile.unit is Knight) troopPrefab = knightPrefab;
        if (tile.unit is Catapult) troopPrefab = catapultPrefab;
        if (tile.unit is Frigate) troopPrefab = frigatePrefab;
        if (tile.unit is Caraval) troopPrefab = caravelPrefab;
        if (tile.unit is Musketman) troopPrefab = musketeerPrefab;
        if (tile.unit is Cannon) troopPrefab = cannonPrefab;
        if (tile.unit is Cavalry) troopPrefab = cavalryPrefab;
        if (tile.unit is MachineGun) troopPrefab = machineGunPrefab;
        if (tile.unit is Infantry) troopPrefab = infantryPrefab;
        if (tile.unit is Artillery) troopPrefab = artilleryPrefab;
        if (tile.unit is Tank) troopPrefab = tankPrefab;
        if (tile.unit is Zeppelin) troopPrefab = zeppelinPrefab;
        if (tile.unit is Biplane) troopPrefab = biplanePrefab;
        if (tile.unit is Cruiser) troopPrefab = cruiserPrefab;
        if (tile.unit is Dreadnort) troopPrefab = dreadnortPrefab;


        if (troopPrefab != null)
        {

            Vector3 pos = CalculateHexPosition(x, y)/* + Vector3.up * 0.1f*/;
            GameObject troopObj = Instantiate(troopPrefab, pos, Quaternion.identity);
            troopObj.transform.rotation = Quaternion.Euler(0, -45f, 0);
            tile.unit.visual = troopObj;



           troopObj.tag = "Destroyable";
           troopObj.tag = "Unit";
            //if(tile.unit.isBoat == false)
            SetLayerRecursively(troopObj,LayerMask.NameToLayer("TroopLayer"));

            ColorUnit(troopObj, tile.unit.owner.playerColor,tile.unit.owner.SecondaryColor);

Vector3 posH = CalculateHexPosition(x, y) + new Vector3(0f, -0f, 0f);
if(tile.unit.hasMoved == false && tile.unit.owner.isPlayer == true)
SpawnHighlightRing(troopPrefab, posH);
/*/GameObject troopObjH = Instantiate(troopPrefab, posH, Quaternion.identity);
troopObjH.transform.rotation = Quaternion.Euler(0, -45f, 0);
troopObjH.transform.localScale *= 1.05f;

troopObjH.tag = "Destroyable";

HighlightUnit(troopObjH, Color.cyan);

SetLayerRecursively(troopObjH, LayerMask.NameToLayer("MainLayer"));*/

            // Health text
            Vector3 textPos = pos + Vector3.up * 1.3f;
            Quaternion rotation = Quaternion.Euler(45f, -0.03f, 0f);
            GameObject textObj = Instantiate(healthTextPrefab, textPos, rotation);

         /*   Outline outline = troopObj.GetComponent<Outline>();
if (outline == null)
    outline = troopObj.AddComponent<Outline>();
outline.SetOutline(true);

if(tile.unit.hasMoved == true)
outline.SetOutline(false);
            // Cache components*/

var uiRoot = textObj.transform;

var healthText = uiRoot.GetComponentInChildren<TextMeshPro>();
var colorSprite = uiRoot.Find("ColorSprite").GetComponent<SpriteRenderer>();
var shieldIcon = uiRoot.Find("ShieldIcon").GetComponent<SpriteRenderer>();
var wallIcon = uiRoot.Find("WallIcon").GetComponent<SpriteRenderer>();

// --- Health text ---
healthText.text = tile.unit.health.ToString();

// --- Tribe primary color ---
colorSprite.sprite = ColorSprite;
colorSprite.color = tile.unit.owner.playerColor;

// --- Bonuses ---
int defenceBonus = tile.defenceBonus(tile.unit.owner);
if(tile.tileType == "Desert"&&hasWonder("Eygpt", typeof(ExplorerWonder),tile.unit.owner))
    defenceBonus = 1;
if(tile.tileType == "Mountain"&&hasWonder("Persia", typeof(PopWonder),tile.unit.owner))
    defenceBonus = 2;
// Shield = any defence bonus
shieldIcon.sprite = ShieldSprite;
shieldIcon.enabled = defenceBonus ==1;

// Wall icon = wall only
wallIcon.sprite = WallSprite;
wallIcon.enabled = defenceBonus ==2;

            textObj.tag = "Destroyable";
            textObj.tag = "Unit";
            SetLayerRecursively(textObj,LayerMask.NameToLayer("nLayer"));
textObj.transform.SetParent(troopObj.transform);
            //textObj.GetComponent<TextMeshPro>().text = tile.unit.health.ToString();
        }
    }
private void SpawnHighlightRing(GameObject troopPrefab, Vector3 centerPos)
{
    int count = 10;
    float radius = 0.02f;   // how far around unit
    float yOffset = 0f; // slightly below
    float scaleMultiplier = 1f;

    for (int i = 0; i < count; i++)
    {
        float angle = i * Mathf.PI * 2f / count;
        float offsetX = Mathf.Cos(angle) * radius;
        float offsetZ = Mathf.Sin(angle) * radius;

        Vector3 spawnPos = centerPos + new Vector3(offsetX, yOffset, offsetZ);

        GameObject clone = Instantiate(troopPrefab, spawnPos, Quaternion.Euler(0, -45f, 0));
        clone.transform.localScale *= scaleMultiplier;

        clone.tag = "Destroyable";
        clone.tag = "Unit";
        SetLayerRecursively(clone, LayerMask.NameToLayer("MainLayer"));
        HighlightUnit(clone, Color.cyan);

        // Remove colliders & scripts
        foreach (var c in clone.GetComponentsInChildren<Collider>())
            Destroy(c);

        foreach (var mb in clone.GetComponentsInChildren<MonoBehaviour>())
            Destroy(mb);

        // Force solid unlit cyan material
        Renderer[] rends = clone.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            /*Material mat = new Material(Shader.Find("Unlit/Color"));
            mat.color = Color.cyan;
            r.material = mat;*/
            r.material = highlightMaterial;
        }
    }
}
private void CreateHighlightClone(GameObject troopObj, Color highlightColor)
{
    // Clone the visual
    GameObject highlight = Instantiate(troopObj, troopObj.transform);

    highlight.name = "HighlightClone";

    // Push slightly behind
    highlight.transform.localPosition += new Vector3(-0.01f, -0.01f, -0.01f);
    highlight.transform.localScale *= 1.1f;

    // Remove unwanted components
    foreach (var c in highlight.GetComponentsInChildren<Collider>())
        Destroy(c);

    foreach (var c in highlight.GetComponentsInChildren<MonoBehaviour>())
        Destroy(c);

    // Color everything
    Renderer[] rends = highlight.GetComponentsInChildren<Renderer>();
    foreach (Renderer r in rends)
    {
        foreach (Material m in r.materials)
        {
            if (m.HasProperty("_Color"))
                m.color = highlightColor;
            if (m.HasProperty("_BaseColor"))
                m.SetColor("_BaseColor", highlightColor);
        }
    }
}

    private void SpawnMovementAttackIndicators(Tile tile, int x, int y)
    {
        Vector3 pos = CalculateHexPosition(x, y);
        if (tile.isMoveable)
        {
            GameObject obj = Instantiate(troopMovementPrefab, pos, Quaternion.identity);
            obj.tag = "Destroyable";
            obj.tag = "MovementAttackIndicators";

            SetLayerRecursively(obj,LayerMask.NameToLayer("UILayer"));
        }
        if (tile.isAttackable)
        {
            GameObject obj = Instantiate(troopAttackPrefab, pos, Quaternion.identity);
            obj.tag = "Destroyable";        
            obj.tag = "MovementAttackIndicators";

            SetLayerRecursively(obj,LayerMask.NameToLayer("UILayer"));
        }
    }

    private void SpawnSelectionIndicators()
    {
        if(currentTile!= null){
        Vector3 pos = CalculateHexPosition(currentTile.x, currentTile.y);
        if (selectionState == SelectionState.TileSelected)
        {
            GameObject obj = Instantiate(tileSelectedPrefab, pos, Quaternion.identity);
            obj.tag = "Destroyable";
            obj.tag = "MovementAttackIndicators";

            SetLayerRecursively(obj,LayerMask.NameToLayer("UILayer"));

        }
        else if (selectionState == SelectionState.TroopSelected)
        {
            GameObject obj = Instantiate(troopSelectedPrefab, pos, Quaternion.identity);
            obj.tag = "Destroyable";
            obj.tag = "MovementAttackIndicators";
            SetLayerRecursively(obj,LayerMask.NameToLayer("UILayer"));


        }
        }
    }

    private Vector3 CalculateHexPosition(int x, int y)
    {
        float xOffset = (y % 2 == 0) ? 0 : hexWidth / 2f;
        float xPos = x * hexWidth + xOffset;
        float yPos = y * (hexHeight * 0.75f);
        return new Vector3(xPos, 0, yPos);
    }

    // ------------------- Input -------------------
    private void DetectTileClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        return;

        if (currentMap == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldPos = ray.GetPoint(distance);

            approxY = Mathf.RoundToInt(worldPos.z / (hexHeight * 0.75f));
            float xOffset = (approxY % 2 == 0) ? 0 : hexWidth / 2f;
            approxX = Mathf.RoundToInt((worldPos.x - xOffset) / hexWidth);

            approxX = Mathf.Clamp(approxX, 0, currentMap.width - 1);
            approxY = Mathf.Clamp(approxY, 0, currentMap.height - 1);

            selectedX = approxX;
            selectedY = approxY;

            if (!currentPlayer.exploredTiles[selectedX, selectedY])
            {
                Debug.Log("Tile is unexplored. Cannot click.");
                ResetMovementAttackFlags();

                return; // ignore this click
            }
// Initialize the list
List<Tile> adjacentTiles = new List<Tile>();

// Loop through adjacent tiles
for (int x = -1; x <= 1; x++) {
    for (int y = -1; y <= 1; y++) {

        int checkX = selectedX + x;
        int checkY = selectedY + y;

        // Optional: bounds check to avoid IndexOutOfRange
        if (checkX >= 0 && checkX < currentMap.tiles.GetLength(0) &&
            checkY >= 0 && checkY < currentMap.tiles.GetLength(1)) {
            adjacentTiles.Add(currentMap.tiles[checkX, checkY]);
        }
    }
}

// Find the closest tile to the mouse click
Tile closestTile = null;
float closestDist = float.MaxValue;

foreach (Tile t in adjacentTiles) {
    // Use worldPos instead of undefined mouseclick
    Vector3 tilePos = CalculateHexPosition(t.x, t.y);
    float newDist = Vector3.Distance(worldPos, tilePos); // distance from click to tile

    if (newDist < closestDist) {
        closestDist = newDist;
        closestTile = t;
    }
}


// closestTile now holds the tile nearest to the mouse click
           // currentTile = closestTile; //currentMap.tiles[selectedX, selectedY];

            //Debug.Log($"Selected Tile: ({selectedX}, {selectedY}) - Type: {currentTile.tileType}");
        tileOptionClickedImage.SetActive(true);
                                    UpdateTileInfo(currentTile);


        }

        HandleTileClick();
}

IEnumerator MoveUnitVisual(Tile from, Tile to)
{
    Troops movingUnit = from.unit;
    Troops finalUnit = movingUnit; // what ends up on target tile
    //from.unit = null;

    finalUnit.hasMoved = true;
     Draw(drawUnit : true);
    // ---------- EMBARK / DISEMBARK LOGIC ----------
    // Land → Coast/River (create boat)
    if ((to.tileType == "Coast" || to.tileType == "River") &&
        movingUnit.isBoat == false &&
        movingUnit.isAir == false &&
        to.hasRoad == false &&
        to.hasTrainTrack == false)
    {
        finalUnit = new Boat(currentPlayer, movingUnit);
    }
    // Boat → Land (unload unit)
    else if (!(to.tileType == "Coast" || to.tileType == "River" || to.tileType == "Ocean") &&
             movingUnit.isBoat == true &&
             movingUnit.isAir == false)
    {
        Boat boat = movingUnit as Boat;
        if (boat != null && boat.unitToCarry != null)
        {
            finalUnit = boat.unitToCarry;
            boat.unitToCarry = null;
        }
    }
    //Draw();
    // ---------- VISUAL MOVE ----------
    //ERROR IS HERE
    GameObject obj = movingUnit.visual;

    Vector3 start = obj.transform.position;
    Vector3 end = CalculateHexPosition(to.x, to.y);

    float duration = 0.2f;
    float t = 0f;
    while (t < 1f)
    {
        t += Time.deltaTime / duration;
        if(obj !=null)
        obj.transform.position = Vector3.Lerp(start, end, t);
        yield return null;
    }

    // ---------- FINALIZE TILE DATA ----------
    to.unit = finalUnit;
    from.unit = null;

    finalUnit.hasMoved = true;
    if(finalUnit is Artillery || finalUnit is Catapult ||finalUnit is MachineGun || finalUnit is Shield|| finalUnit is Zeppelin|| finalUnit is Tank)
        finalUnit.hasAttacked = true;

    // ---------- EXPLORATION ----------
    if (to.tileType == "Mountain" ||
        finalUnit is Caraval ||
        finalUnit is Frigate ||
        finalUnit is Dreadnort ||
        finalUnit is Cruiser || finalUnit is Biplane|| finalUnit is Zeppelin)
    {
        ExploreDouble(currentPlayer, to);
    }
    else
    {
        ExploreTile(currentPlayer, to);
    }

    // ---------- CLEANUP ----------
    ResetMovementAttackFlags();
    selectionState = SelectionState.None;
    currentTile = null;
    mainTile = null;

    Draw(drawUnit : true , drawClouds: true, drawMovementAttackIndicators: true); // redraw AFTER animation
}


    // ------------------- Movement & Attack -------------------
    private void HandleMovementAttackOptions()
    {    if (EventSystem.current.IsPointerOverGameObject())
        return;
        if (mainTile == null) return;

        ResetMovementAttackFlags();
        if(currentTile.unit.hasMoved == false&&currentTile.unit.hasAttacked == false){
            TroopClickPanel.SetActive(true);
            TroopClickPanelImage.SetActive(true);
            if(currentTile.owner != currentPlayer){
                ClaimTileBtn.SetActive(true);
            }
            if(currentTile.unit.health < currentTile.unit.maxHealth){
                HealTroopBtn.SetActive(true);
            }

        }
        Troops unit = mainTile.unit;
        Queue<Tile> frontier = new Queue<Tile>();
        Dictionary<Tile, int> cost = new Dictionary<Tile, int>();

        frontier.Enqueue(mainTile);
        cost[mainTile] = 0;
        if(currentTile.unit.hasMoved == false){
        while (frontier.Count > 0)
        {
            Tile current = frontier.Dequeue();
            int currentCost = cost[current];

                foreach (Tile neighbor in GetNeighbors(current))
{
    if (!unit.CanMoveTo(neighbor))
        continue;

    int movementPenalty = 0;

    // --- Sailing rule ---
    if (currentPlayer.unlockedTechs.Any(t => t.Name == "Sailing"))
    {
        // If moving *onto* a coast or river tile, add huge penalty (100)
        if ((neighbor.tileType == "Coast" || neighbor.tileType == "River")&&currentTile.unit.isBoat == false&&mainTile.unit.isAir == false&&neighbor.hasRoad==false&&neighbor.hasTrainTrack==false)
        {
            movementPenalty = 100;
        }
        else if(!(neighbor.tileType == "Coast" || neighbor.tileType == "River"||neighbor.tileType == "Ocean")&&currentTile.unit.name == "Boat")
        {
            movementPenalty = 100;
        }
    }

    float newCost = currentCost + neighbor.movementCost + movementPenalty;
    bool isAdjacent = (current == mainTile);

    // --- RULE 1: Adjacent tiles are always move-allowed ---
    if (isAdjacent && neighbor.unit == null)
    {
        neighbor.isMoveable = true;
        // Do NOT enqueue unless cost allows it (Rule 2)
        if (newCost <= unit.movement)
        {
            cost[neighbor] = Mathf.FloorToInt(newCost);
                    bool ignoresZOC = unit is Chariot || unit.isAir;
        if (ignoresZOC || !IsInZoneOfControl(neighbor, currentPlayer))
        {
            frontier.Enqueue(neighbor);
        };
        }
        continue;
    }
            
                if (!cost.ContainsKey(neighbor) && neighbor.unit == null && newCost <= unit.movement)
                {
                    cost[neighbor] = Mathf.FloorToInt(newCost);
                    
                    neighbor.isMoveable = true;
                        bool ignoresZOC = unit is Chariot || unit.isAir;
    if (!ignoresZOC && IsInZoneOfControl(neighbor, currentPlayer))
    {
        // can enter the tile but stops here
    }
    else
    {
        frontier.Enqueue(neighbor);
    }
                }
            }
        }
        }
        if(currentTile.unit.hasAttacked == false){
        MarkAttackTiles(mainTile, unit.range);}
        Draw(drawMovementAttackIndicators : true); // unsure if this is correct
    }

    private void ResetMovementAttackFlags()
    {
        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile t = currentMap.tiles[x, y];
                t.isMoveable = false;
                t.isAttackable = false;
            }
        }
        foreach (Transform child in btnContent.transform)
        {
            child.gameObject.SetActive(false);
        }
        tileClickedPanel.SetActive(false);
        foreach (Transform child in TroopClickPanel.transform)
        {
            child.gameObject.SetActive(false);
        }
        TroopClickPanel.SetActive(false);
        TroopClickPanelImage.SetActive(false);
    }

    private void MarkAttackTiles(Tile center, int range)
    {
        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile t = currentMap.tiles[x, y];
                if (t.unit != null && t.unit.owner != center.unit.owner && CubeDistance(center, t) <= range)
                {
                    t.isAttackable = true;
                }
            }
        }
    }

    private (int x, int y, int z) OffsetToCube(Tile tile)
    {
        int x = tile.x - (tile.y - (tile.y & 1)) / 2;
        int z = tile.y;
        int y = -x - z;
        return (x, y, z);
    }

    private int CubeDistance(Tile a, Tile b)
    {
        var ac = OffsetToCube(a);
        var bc = OffsetToCube(b);
        return Mathf.Max(Mathf.Abs(ac.x - bc.x), Mathf.Abs(ac.y - bc.y), Mathf.Abs(ac.z - bc.z));
    }

    private List<Tile> GetNeighbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();
        int[][] offsetsEven = new int[][] { new int[]{1,0}, new int[]{0,1}, new int[]{-1,1}, new int[]{-1,0}, new int[]{-1,-1}, new int[]{0,-1} };
        int[][] offsetsOdd = new int[][] { new int[]{1,0}, new int[]{1,1}, new int[]{0,1}, new int[]{-1,0}, new int[]{0,-1}, new int[]{1,-1} };
        int[][] offsets = (tile.y % 2 == 0) ? offsetsEven : offsetsOdd;

        foreach (var offset in offsets)
        {
            int nx = tile.x + offset[0];
            int ny = tile.y + offset[1];
            if (nx >= 0 && nx < currentMap.width && ny >= 0 && ny < currentMap.height)
                neighbors.Add(currentMap.tiles[nx, ny]);
        }

        return neighbors;
    }

    // ------------------- Tile Click Handling -------------------
    private void HandleTileClick()
    {
        if (isAITurn)
            return;
        if (currentTile == null) return;
        tileOptionClickedImage.SetActive(true);
        UpdateTileInfo(currentTile);

        if (selectionState == SelectionState.TroopSelected && currentTile.isMoveable)
        {
                StartCoroutine(MoveUnitVisual(mainTile, currentTile));

            /*if((currentTile.tileType == "Coast" ||currentTile.tileType == "River")&&mainTile.unit.isBoat == false&&mainTile.unit.isAir == false&&currentTile.hasRoad==false&&currentTile.hasTrainTrack==false){
                currentTile.unit = new Boat(currentPlayer, mainTile.unit);
            }else if(!(currentTile.tileType == "Coast" ||currentTile.tileType == "River" || currentTile.tileType == "Ocean") && mainTile.unit.isBoat == true&&mainTile.unit.isAir == false){
    Boat boat = mainTile.unit as Boat;     // ← SAFE CAST
    if (boat != null && boat.unitToCarry != null)
    {
        currentTile.unit = boat.unitToCarry;   // Place carried unit
        boat.unitToCarry = null;               // Clear cargo
    }            }
            else{
                currentTile.unit = mainTile.unit;
            }
            mainTile.unit = null;

            if(currentTile.tileType == "Mountain"|| currentTile.unit is Caraval || currentTile.unit is Dreadnort|| currentTile.unit is Cruiser){
                ExploreDouble(currentPlayer, currentTile);
            }else{
                ExploreTile(currentPlayer, currentTile);
            }

            ResetMovementAttackFlags();
            
            selectionState = SelectionState.None;

            currentTile.unit.hasMoved = true;
            currentTile = null;
            mainTile = null;
            Draw();*/
        }
        else if (selectionState == SelectionState.TroopSelected && currentTile.isAttackable)
        {
            if(!(mainTile.unit is MachineGun))
            mainTile.unit.hasAttacked = true;
            mainTile.unit.hasMoved = true;
            StartCoroutine(HandleAttack());
           

            ResetMovementAttackFlags();
            selectionState = SelectionState.None;


            currentTile = null;
            mainTile = null;
            Draw(drawMovementAttackIndicators:true, drawUnit : true); // EFN go back to this
        }
        else
        {
            if (currentTile != mainTile)
                selectionState = SelectionState.None;

            if ((selectionState != SelectionState.TroopSelected && currentTile.unit != null && currentTile != mainTile) ||
                (currentTile == mainTile && selectionState == SelectionState.None && currentTile.unit != null))
            {
                mainTile = currentTile;
                selectionState = SelectionState.TroopSelected;
                if (currentTile.unit != null && currentTile.unit.owner == currentPlayer)
                {
                    HandleMovementAttackOptions();
                }
             //   currentTile.isTroopSelected = true;
                Draw(drawMovementAttackIndicators:true);
            }
            else if (selectionState != SelectionState.TileSelected)
            {

                mainTile = currentTile;
                selectionState = SelectionState.TileSelected;
                currentTile.isSelected = true;
                ResetMovementAttackFlags();
                if (currentTile.owner == currentPlayer)
                {
                    HandleTileSelected();
                }else{                    HandleTileSelected();
}
                
                Draw(drawMovementAttackIndicators:true);

            }
            else
            {
                selectionState = SelectionState.None;
                ResetMovementAttackFlags();
                Draw(drawMovementAttackIndicators:true);
            }
        }
    }
    /*private void ExploreTile(Player player, Tile tile)
    {
        // Mark current tile
        player.exploredTiles[tile.x, tile.y] = true;

        // Mark adjacent tiles
        foreach (Tile neighbor in GetNeighbors(tile))
        {
            player.exploredTiles[neighbor.x, neighbor.y] = true;
        }
    }*/
    private void ExploreTile(Player player, Tile tile)
{
    bool anyNewTilesExplored = false;

    // Current tile
    if (!player.exploredTiles[tile.x, tile.y])
    {
        player.exploredTiles[tile.x, tile.y] = true;
        anyNewTilesExplored = true;
    }

    // Adjacent tiles
    foreach (Tile neighbor in GetNeighbors(tile))
    {
        if (!player.exploredTiles[neighbor.x, neighbor.y])
        {
            player.exploredTiles[neighbor.x, neighbor.y] = true;
            anyNewTilesExplored = true;
        }
    }

    // Only redraw if something actually changed
    if (anyNewTilesExplored)
    {
        Draw(
            drawClouds: true,
            drawTile: true,
            drawUnit: true,
            drawBuildings: true,
            drawBorders: true,
            drawWalls: true,
            drawRoads: true,
            drawTrainTracks: true,
            drawWonders: true,
            drawTradeRoutes: true
        );
    }
}
/*private void ExploreDouble(Player player, Tile tile)
{
    Queue<Tile> queue = new Queue<Tile>();
    HashSet<Tile> visited = new HashSet<Tile>();

    queue.Enqueue(tile);
    visited.Add(tile);

    int depth = 0;
    int tilesInCurrentRing = 1;
    int tilesInNextRing = 0;

    while (queue.Count > 0 && depth <= 2)   // 0 = self, 1 = neighbors, 2 = neighbors-of-neighbors
    {
        Tile current = queue.Dequeue();

        // Mark explored
        player.exploredTiles[current.x, current.y] = true;

        foreach (Tile neighbor in GetNeighbors(current))
        {
            if (!visited.Contains(neighbor))
            {
                visited.Add(neighbor);
                queue.Enqueue(neighbor);
                tilesInNextRing++;
            }
        }

        tilesInCurrentRing--;

        if (tilesInCurrentRing == 0)
        {
            depth++;
            tilesInCurrentRing = tilesInNextRing;
            tilesInNextRing = 0;
        }
    }
}*/
private void ExploreDouble(Player player, Tile tile)
{
    Queue<Tile> queue = new Queue<Tile>();
    HashSet<Tile> visited = new HashSet<Tile>();

    bool anyNewTilesExplored = false;

    queue.Enqueue(tile);
    visited.Add(tile);

    int depth = 0;
    int tilesInCurrentRing = 1;
    int tilesInNextRing = 0;

    while (queue.Count > 0 && depth <= 2)
    {
        Tile current = queue.Dequeue();

        // Only mark if not already explored
        if (!player.exploredTiles[current.x, current.y])
        {
            player.exploredTiles[current.x, current.y] = true;
            anyNewTilesExplored = true;
        }

        foreach (Tile neighbor in GetNeighbors(current))
        {
            if (!visited.Contains(neighbor))
            {
                visited.Add(neighbor);
                queue.Enqueue(neighbor);
                tilesInNextRing++;
            }
        }

        tilesInCurrentRing--;

        if (tilesInCurrentRing == 0)
        {
            depth++;
            tilesInCurrentRing = tilesInNextRing;
            tilesInNextRing = 0;
        }
    }

    // Only redraw if something actually changed
    if (anyNewTilesExplored)
    {
        checkIfExpMonument();
        Draw(
            drawClouds: true,
            drawTile: true,
            drawUnit: true,
            drawBuildings: true,
            drawBorders: true,
            drawWalls: true,
            drawRoads: true,
            drawTrainTracks: true,
            drawWonders: true,
            drawTradeRoutes: true
        );
    }
}
void UpdateTileInfo(Tile tile)
{
    if (tile == null)
    {
        tileInfoText.text = "";
        return;
    }

    List<string> parts = new List<string>();
    if (tile.owner != null)
        parts.Add(tile.owner.tribeType);
        parts.Add(tile.tileType);



    if (!string.IsNullOrEmpty(tile.resource))
        parts.Add(tile.resource);

    if (!string.IsNullOrEmpty(tile.forestResource))
        parts.Add(tile.forestResource);

    //parts.Add($"Move Cost: {tile.movementCost}");

    if (tile.district != null){
        parts.Add(tile.district.GetType().Name);
        parts.Add($"Level: {tile.district.returnLevel(currentTile.x, currentTile.y, currentMap)}");}

    if (tile.district?.building != null)
        parts.Add(tile.district.building.GetType().Name);

    if (tile.building != null)
        parts.Add(tile.building.GetType().Name);

    if (tile.wonder != null)
        parts.Add(tile.wonder.GetType().Name);

    if (tile.unit != null)
        parts.Add(tile.unit.GetType().Name);

    if (tile.hasWall)
        parts.Add("Wall");

    if (tile.hasRoad)
        parts.Add("Road");

    if (tile.hasTrainTrack)
        parts.Add("Rail");
    if (tile.hasTradeRoute)
        parts.Add("Trade Route");
    tileInfoText.text = string.Join(", ", parts);
}

/*void UpdateTileInfo(Tile tile)
{
    if (tile == null)
    {
        tileInfoText.text = "";
        return;
    }

    tileInfoText.text =
        $"{tile.tileType}, " +
        $"{(tile.owner != null ? tile.owner.tribeType : "No Owner")}, " +
        $"{tile.resource}, " +
        $"{tile.forestResource}, " +
        $"MoveCost:{tile.movementCost}, " +
        $"{(tile.district != null ? tile.district.GetType().Name : "")}, " +
        $"{tile.district?.building?.GetType().Name ?? ""}, " +
        $"{(tile.building != null ? tile.building.GetType().Name : "")}, " +
        $"{(tile.wonder != null ? tile.wonder.GetType().Name : "")}, " +
        $"{(tile.unit != null ? tile.unit.GetType().Name : "")}" +
        $"{(tile.hasWall ? ", Wall" : "")}" +
        $"{(tile.hasRoad ? ", Road" : "")}" +
        $"{(tile.hasTrainTrack ? ", Rail" : "")}";
}*/

    //Handles what buttons show
    private void HandleTileSelected(){
        tileClickedPanel.SetActive(true);
        TroopClickPanelImage.SetActive(true);

        //Wonder Btns
        if(currentTile.owner == currentPlayer && currentTile.building == null && currentTile.district == null &&(currentTile.tileType == "Snow"||currentTile.tileType == "Desert"||currentTile.tileType == "Plains") && currentTile.forestResource=="")
        {
            if(currentPlayer.popWonderBuilt == false && currentPlayer.unlockedCivics.Any(t => t.name == "Law and Order"))
            {
                int pop = 0;

                for (int x = 0; x < currentMap.width; x++)
                {
                    for (int y = 0; y < currentMap.height; y++)
                    {
                        Tile tile = currentMap.tiles[x, y];
                        if(tile.owner == currentPlayer && tile.district != null &&tile.district is City){
                            pop += tile.district.returnLevel(x, y, currentMap);
                        }
                    }
                }

                if(pop >= 15){
                    popWonderBtn.SetActive(true);
                }
            }

            if(currentPlayer.tradeWonderBuilt == false && currentPlayer.unlockedCivics.Any(t => t.name == "Caravans"))
            {
                int conected = 0;

                for (int x = 0; x < currentMap.width; x++)
                {
                    for (int y = 0; y < currentMap.height; y++)
                    {
                        Tile tile = currentMap.tiles[x, y];
                        if(tile.owner == currentPlayer && tile.district != null &&tile.district is City){
                            if(currentMap.IsConnectedToCapitalByRoad(tile, currentPlayer))
                            conected ++;  
                            if(currentMap.IsConnectedToCapitalByTrain(tile, currentPlayer))
                            conected ++;                          
                        }
                    }
                }

                if(conected >= 10){
                    tradeWonderBtn.SetActive(true);
                }
            }

            if(currentPlayer.expWonderBuilt == false && currentPlayer.unlockedCivics.Any(t => t.name == "Environmentalism"))
            {
                bool clouds = true;

                for (int x = 0; x < currentMap.width; x++)
                {
                    for (int y = 0; y < currentMap.height; y++)
                    {
                        Tile tile = currentMap.tiles[x, y];
                        if (!currentPlayer.exploredTiles[x, y]/* &&drawClouds == true*/)
                        {
                            clouds = false;
                        }
                    }
                }

                if(clouds){
                    explorerWonderBtn.SetActive(true);
                }
            } 
        }

        //City unit btns
        if (currentTile.district is City && currentTile.owner == currentPlayer)
        {
            AddWarriorBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Animal Husbandry"))
                AddHorsemanBtn.SetActive(true);
            
            
            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Hunting"))
                AddSpearmanBtn.SetActive(true);
            
            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Archery"))
                AddArcherBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Defence"))
                AddShieldBtn.SetActive(true);
            
            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Wheels"))
                AddChariotBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Smelting"))
                AddSwordsmanBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Stirrups"))
                AddKnightBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Machinary"))
                AddCatapultBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Gunpowder"))
                AddMusketeerBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Metallurgy"))
                AddCannonBtn.SetActive(true);

            if(currentPlayer.unlockedTechs.Any(t=> t.Name == "Military science"))
                AddCavalryBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Mechanics"))
                AddMachineGunBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Infantry"))
                AddInfantryBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Artillary"))
                AddArtilleryBtn.SetActive(true);
        }

        //Airport unit btns
        if(currentTile.building is Airport && currentTile.owner == currentPlayer)
        {
            if (currentPlayer.unlockedTechs.Any(t => t.Name == "AirTravel"))
                AddZeppelinBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Aeroplane"))
                AddBiplaneBtn.SetActive(true);
        }

        //Industrial zone unit btns
        if(currentTile.district != null &&currentTile.district.building is Factorys && currentTile.owner == currentPlayer){
            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Tanks"))
                AddTankBtn.SetActive(true);
        }
        
        //Harbour unit btns
        if (currentTile.district is Harbour && currentTile.owner == currentPlayer){
            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Ship Building"))
                AddShipBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Navel Warfare"))
                AddRammingShipBtn.SetActive(true);
            
            
            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Navigation"))
                AddFrigateBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Shipyard"))
                AddCaravelBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "SteamPower"))
                AddCruiserBtn.SetActive(true);

            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Steel"))
                AddDreadnortBtn.SetActive(true);
        }
        
        //Action btns
        if(currentTile.owner == currentPlayer)
        {
            if(currentTile.forestResource != "" && currentPlayer.unlockedTechs.Any(t => t.Name == "Farming")){
                BurnForestBtn.SetActive(true);
            }
            if((currentTile.building != null || currentTile.district != null) && currentPlayer.unlockedTechs.Any(t => t.Name == "Gunpowder")){
                DestroyBtn.SetActive(true);
            }
            if(currentTile.forestResource != "" && currentPlayer.unlockedTechs.Any(t => t.Name == "Climbing")){
                ChopForestBtn.SetActive(true);
            }
        }

        if(currentTile.district != null && currentTile.owner == currentPlayer &&currentPlayer.unlockedTechs.Any(t => t.Name == "Defence") && !(currentTile.tileType == "Coast"||currentTile.tileType == "River"||currentTile.tileType == "Ocean"))
                AddWallBtn.SetActive(true);

        // district buildings
        if(currentTile.district != null && currentTile.district.building == null && currentTile.owner == currentPlayer){
            if(currentTile.district is City){
                if(currentPlayer.unlockedTechs.Any(t => t.Name == "Masonary"))
                    AddMonumentBtn.SetActive(true);
                
                if(currentPlayer.unlockedTechs.Any(t => t.Name == "Defence"))
                    AddMarketBtn.SetActive(true);
            }
            
            if(currentTile.district is Harbour)
            {
                if(currentPlayer.unlockedTechs.Any(t => t.Name == "Trade"))
                    AddCustomsHouseBtn.SetActive(true);

                if (currentPlayer.unlockedTechs.Any(t => t.Name == "Navigation"))
                    AddLightHouseBtn.SetActive(true); 

                if (currentPlayer.unlockedTechs.Any(t => t.Name == "Shipyard"))
                    AddShipyardBtn.SetActive(true); 

                if (currentPlayer.unlockedTechs.Any(t => t.Name == "SteamPower"))
                    AddNavalBaseBtn.SetActive(true); 
            }
        
            if(currentTile.district is Commercial)
            {
                if(currentPlayer.unlockedTechs.Any(t => t.Name == "Commerce"))
                    AddTowerBtn.SetActive(true);

                if(currentPlayer.unlockedTechs.Any(t => t.Name == "Finance"))
                    AddBankBtn.SetActive(true); 

                if (currentPlayer.unlockedTechs.Any(t => t.Name == "Steel"))
                    AddSkyScrapersBtn.SetActive(true); 
            }
                            
            if (currentTile.district is Industrial)
            {
                if(currentPlayer.unlockedTechs.Any(t => t.Name == "Coal"))
                    AddCarpentryWorkshopBtn.SetActive(true);

                if(currentPlayer.unlockedTechs.Any(t => t.Name == "Industry"))
                    AddWarehousesBtn.SetActive(true); 
        
                if (currentPlayer.unlockedTechs.Any(t => t.Name == "Tanks"))
                    AddTankFactoryBtn.SetActive(true); 
            }


        }

        if(currentTile.owner == currentPlayer && (currentTile.tileType == "Snow"||currentTile.tileType == "Desert"||currentTile.tileType == "Plains"||currentTile.tileType == "District"))
        {
            if (currentPlayer.unlockedTechs.Any(t => t.Name == "Travel") && currentTile.hasRoad == false)
                AddRoadBtn.SetActive(true);

            if(currentPlayer.unlockedTechs.Any(t => t.Name == "Railways")&& currentTile.hasTrainTrack == false)
                AddTrainTracksBtn.SetActive(true);
        }

        if(currentTile.owner == currentPlayer && (currentTile.tileType == "Ocean"||currentTile.tileType == "Coast"||currentTile.tileType == "River") && currentPlayer.unlockedTechs.Any(t => t.Name == "Trade"))
            AddTradeRouteBtn.SetActive(true);

        string[] huntableAnimals = { "Horse", "Boar", "Deer", "Penguin" };

        //Building btns
        if(currentTile.building == null && currentTile.district == null && currentTile.owner == currentPlayer && currentTile.wonder == null)
        {
            if(currentPlayer.unlockedTechs.Any(t => t.Name == "Archery") && huntableAnimals.Contains(currentTile.resource))
                AddFurTradingPostBtn.SetActive(true);
        
            if(currentPlayer.unlockedTechs.Any(t => t.Name == "Logging") && currentTile.forestResource != "")
                AddLumberHutBtn.SetActive(true);
            
            if(currentPlayer.unlockedTechs.Any(t => t.Name == "Hunting") && huntableAnimals.Contains(currentTile.resource))
                HuntAnimalBtn.SetActive(true);
            
            if(currentTile.forestResource=="")
            {
                if(currentTile.tileType == "Plains" && currentPlayer.unlockedTechs.Any(t => t.Name == "Animal Husbandry"))
                    AddPastureBtn.SetActive(true);
            
                if(currentTile.resource=="Crop" && currentPlayer.unlockedTechs.Any(t => t.Name == "Farming"))
                    AddFarmBtn.SetActive(true);
        

                if((currentTile.tileType == "Snow"||currentTile.tileType == "Desert"||currentTile.tileType == "Plains"))
                {
                    AddCityBtn.SetActive(true);
      
                    if(currentPlayer.unlockedTechs.Any(t => t.Name == "Commerce"))
                        AddCommercialBtn.SetActive(true);
                
                    if(currentPlayer.unlockedTechs.Any(t => t.Name == "Coal"))
                        AddIndustrialBtn.SetActive(true);

                    if (currentPlayer.unlockedTechs.Any(t => t.Name == "Fortifications"))
                        AddFortBtn.SetActive(true);

                    if (currentPlayer.unlockedTechs.Any(t => t.Name == "AirTravel"))
                        AddAirportBtn.SetActive(true);
        
                    if (currentPlayer.unlockedTechs.Any(t => t.Name == "Science"))
                        AddUniversityBtn.SetActive(true);

                    if(currentPlayer.unlockedTechs.Any(t => t.Name == "Smelting") && HasAdjacentMine(currentTile))
                        AddForgeBtn.SetActive(true); 
        
                    if(currentPlayer.unlockedTechs.Any(t => t.Name == "Engineering") && HasAdjacentFarm(currentTile))
                        AddWindmillBtn.SetActive(true); 
        
                    if(currentPlayer.unlockedTechs.Any(t => t.Name == "Machinary") && HasAdjacentLumberHut(currentTile))
                        AddSawmillBtn.SetActive(true); 

                    if(currentPlayer.unlockedTechs.Any(t => t.Name == "Printing press") && HasAdjacentLumberHut(currentTile))
                        AddPaperMillBtn.SetActive(true); 

                    if(currentPlayer.unlockedTechs.Any(t => t.Name == "Refrigeration") && HasAdjacentPasture(currentTile))
                        AddMeatProcessingPlantBtn.SetActive(true); 
                }

                if((currentTile.tileType == "Coast" ||  currentTile.tileType == "River") && currentPlayer.unlockedTechs.Any(t => t.Name == "Ship Building"))  
                    AddHarbourBtn.SetActive(true);

                if(currentTile.resource == "Fish" && currentPlayer.unlockedTechs.Any(t => t.Name == "Sailing"))
                    AddFishingBoatsBtn.SetActive(true);
                
                if(currentTile.resource == "Metal" && currentPlayer.unlockedTechs.Any(t => t.Name == "Mining"))
                    AddMineBtn.SetActive(true);
        
                if(currentTile.tileType == "Mountain" && currentPlayer.unlockedTechs.Any(t => t.Name == "Masonary"))
                    AddQuarryBtn.SetActive(true);
        
                if(currentTile.tileType == "River" && currentPlayer.unlockedTechs.Any(t => t.Name == "Wheels"))
                    AddWaterwheelBtn.SetActive(true);

                if(currentTile.resource == "Whale" && currentPlayer.unlockedTechs.Any(t => t.Name == "Whaling"))
                    AddWhalingShipBtn.SetActive(true); 
                
                if((currentTile.tileType == "River") && currentPlayer.unlockedTechs.Any(t => t.Name == "Travel") && currentTile.hasRoad == false)
                    AddBridgeBtn.SetActive(true);

                if((currentTile.tileType == "River") && currentPlayer.unlockedTechs.Any(t => t.Name == "Railways") && currentTile.hasTrainTrack == false)
                    AddTrainTrackBridgeBtn.SetActive(true);
        
                if(currentPlayer.unlockedTechs.Any(t => t.Name == "Sailing") && currentTile.resource == "Fish")
                    HuntFishBtn.SetActive(true);
            }
        }
        tileOptionClickedImage.SetActive(true);
        LayoutTileButtons();
    }

    bool HasAdjacentMine(Tile tile)
    {
        foreach (Tile adj in GetAdjacentTiles(tile.x, tile.y))
        {
            if (adj != null && adj.building is Mine)
                return true;
        }
        return false;
    }

    bool HasAdjacentFarm(Tile tile)
    {
        foreach (Tile adj in GetAdjacentTiles(tile.x, tile.y))
        {
            if (adj != null && adj.building is Farm)
                return true;
        }
        return false;
    }

    bool HasAdjacentPasture(Tile tile)
    {
        foreach (Tile adj in GetAdjacentTiles(tile.x, tile.y))
        {
            if (adj != null && adj.building is Pasture)
                return true;
        }
        return false;
    }

    bool HasAdjacentLumberHut(Tile tile)
    {
        foreach (Tile adj in GetAdjacentTiles(tile.x, tile.y))
        {
            if (adj != null && adj.building is LumberHut)
                return true;
        }
        return false;
    }

    protected List<Tile> GetAdjacentTiles(int x, int y)
    {
        List<Tile> list = new List<Tile>();

        int[,] evenOffsets =
        {
            { +1,  0 },
            {  0, +1 },
            { -1, +1 },
            { -1,  0 },
            { -1, -1 },
            {  0, -1 }
        };

        int[,] oddOffsets =
        {
            { +1,  0 },
            { +1, +1 },
            {  0, +1 },
            { -1,  0 },
            {  0, -1 },
            { +1, -1 }
        };

        int[,] offsets = (y % 2 == 0) ? evenOffsets : oddOffsets;

        for (int i = 0; i < 6; i++)
        {
            int nx = x + offsets[i, 0];
            int ny = y + offsets[i, 1];

            if (nx >= 0 && ny >= 0 && nx < currentMap.width && ny < currentMap.height)
            {
                list.Add(currentMap.tiles[nx, ny]);
            }
        }

        return list;
    }
    private void LayoutTileButtons()
    {
        // All tile option buttons
        List<GameObject> buttons = new List<GameObject>()
        {
            AddArcherBtn,
AddChariotBtn,
AddSwordsmanBtn,
AddKnightBtn,
AddCatapultBtn,
AddFrigateBtn,
AddCaravelBtn,
AddMusketeerBtn,
AddCannonBtn,
AddCavalryBtn,
AddMachineGunBtn,
AddInfantryBtn,
AddArtilleryBtn,
AddTankBtn,
AddZeppelinBtn,
AddBiplaneBtn,
            AddWarriorBtn,
            AddHorsemanBtn,
            AddSpearmanBtn,
            AddShipBtn,
            AddRammingShipBtn,
            AddPastureBtn,
            AddFarmBtn,
            AddCityBtn,
            AddCommercialBtn,
            AddIndustrialBtn,
            BurnForestBtn,
            DestroyBtn,
            ChopForestBtn,
            AddHarbourBtn,
            AddFishingBoatsBtn,
            AddMineBtn,
            AddMonumentBtn,
            AddQuarryBtn,
            HuntAnimalBtn,
            HuntFishBtn,
            AddShieldBtn,
            AddWallBtn,
            AddMarketBtn,
AddWaterwheelBtn,
AddFurTradingPostBtn,
AddLumberHutBtn,
AddCustomsHouseBtn,
AddTowerBtn,
AddForgeBtn,
AddFortBtn,
AddWindmillBtn,
AddSawmillBtn,
AddLightHouseBtn,
AddShipyardBtn,
AddWhalingShipBtn,
AddCarpentryWorkshopBtn,
AddBankBtn,
AddPaperMillBtn,
AddNavalBaseBtn,
AddSkyScrapersBtn,
AddMeatProcessingPlantBtn,
AddWarehousesBtn,
AddAirportBtn,
AddUniversityBtn,
AddTankFactoryBtn,
            AddCruiserBtn,
            AddDreadnortBtn,
            AddRoadBtn,
            AddTradeRouteBtn,
            AddBridgeBtn,
            AddTrainTracksBtn,
            AddTrainTrackBridgeBtn,
            popWonderBtn,
            tradeWonderBtn,
            explorerWonderBtn,
        };

        // Filter active ones
        List<GameObject> activeBtns = buttons.FindAll(b => b.activeSelf);

        // Position them in a row at bottom
        float startX = -((activeBtns.Count - 1) * 150) / 2f; // center the row
        float y = +40f;
    
        for (int i = 0; i < activeBtns.Count; i++)
        {
            RectTransform rt = activeBtns[i].GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(startX + i * 150, y);
                    int cost = GetButtonCost(activeBtns[i]); // custom function, see below
        TMP_Text[] texts = activeBtns[i].GetComponentsInChildren<TMP_Text>(true);
        TMP_Text costText = texts[1];
        Image img = activeBtns[i].GetComponent<Image>();

        costText.text = cost.ToString();
        if (currentPlayer.money < cost)
        {
            costText.color = Color.red;
            img.sprite = notEnoughSprite;
        }
        else
        {
            costText.color = Color.white;
            img.sprite = normalSprite;
        }
        }
        float buttonWidth = 150f;
float padding = 20f; // optional extra space

float totalWidth = activeBtns.Count * buttonWidth + padding;

contentRT.sizeDelta = new Vector2(totalWidth, contentRT.sizeDelta.y);
    }
private int GetButtonCost(GameObject btn)
{
    if (btn == AddWarriorBtn) return new Warrior(currentPlayer).Cost;
    if (btn == AddHorsemanBtn) return new Horseman(currentPlayer).Cost;
    if (btn == AddSpearmanBtn) return new Spearman(currentPlayer).Cost;
    if (btn == AddArcherBtn) return new Archer(currentPlayer).Cost;
    if (btn == AddChariotBtn) return new Chariot(currentPlayer).Cost;
    if (btn == AddSwordsmanBtn) return new Swordsman(currentPlayer).Cost;
    if (btn == AddKnightBtn) return new Knight(currentPlayer).Cost;
    if (btn == AddCatapultBtn) return new Catapult(currentPlayer).Cost;
    if (btn == AddMusketeerBtn) return new Musketman(currentPlayer).Cost;
    if (btn == AddCannonBtn) return new Cannon(currentPlayer).Cost;
    if (btn == AddCavalryBtn) return new Cavalry(currentPlayer).Cost;
    if (btn == AddMachineGunBtn) return new MachineGun(currentPlayer).Cost;
    if (btn == AddInfantryBtn) return new Infantry(currentPlayer).Cost;
    if (btn == AddArtilleryBtn) return new Artillery(currentPlayer).Cost;
    if (btn == AddTankBtn) return new Tank(currentPlayer).Cost;
    if (btn == AddZeppelinBtn) return new Zeppelin(currentPlayer).Cost;
    if (btn == AddBiplaneBtn) return new Biplane(currentPlayer).Cost;
    if (btn == AddCityBtn) return new City().Cost(currentPlayer);
    if (btn == AddIndustrialBtn) return Industrial.cost;
    if (btn == AddCommercialBtn) return Commercial.cost;
    if (btn == AddHarbourBtn) return Harbour.cost;
    if (btn == AddForgeBtn) return Forge.cost;
    if (btn == AddWindmillBtn) return Windmill.cost;
    if (btn == AddSawmillBtn) return Sawmill.cost;
    if (btn == AddPaperMillBtn) return Papermill.cost;
    if (btn == AddLightHouseBtn) return LightHouse.cost;
    if (btn == AddShipyardBtn) return Shipyard.cost;
    if (btn == AddNavalBaseBtn) return NavelBase.cost;
    if (btn == AddAirportBtn) return Airport.cost;
    if (btn == AddUniversityBtn) return University.cost;
    if (btn == AddTankFactoryBtn) return Factorys.cost;
    if (btn == AddCruiserBtn) return new Cruiser(currentPlayer).Cost;
    if (btn == AddDreadnortBtn) return new Dreadnort(currentPlayer).Cost;
    if (btn == AddRoadBtn) return 2;
    if (btn == AddBridgeBtn) return 10;
    if (btn == AddTrainTracksBtn) return 5;
    if (btn == AddTrainTrackBridgeBtn) return 10;
    if (btn == AddTradeRouteBtn) return 2;

if (btn == AddFurTradingPostBtn) return FurTradingPost.cost;
if (btn == AddFishingBoatsBtn) return FishingBoats.cost;   
 if (btn == AddPastureBtn) return Pasture.cost;
    if (btn == AddFarmBtn) return Farm.cost;
    if (btn == AddWallBtn) return 10;
    if (btn == AddShieldBtn) return new Shield(currentPlayer).Cost;
    if (btn == BurnForestBtn) return 2;
    if (btn == DestroyBtn) return 5;
    if (btn == ChopForestBtn) return 0;
    if (btn == AddWaterwheelBtn) return Waterwheel.cost;
    if (btn == AddFurTradingPostBtn) return FurTradingPost.cost;
    if (btn == AddLumberHutBtn) return LumberHut.cost;
    if (btn == AddCustomsHouseBtn) return CustomHouse.cost;
    if (btn == AddTowerBtn) return Tower.cost;
    if (btn == AddFortBtn) return Fort.cost;
    if (btn == AddCarpentryWorkshopBtn) return CarpentryWorkshop.cost;
    if (btn == AddBankBtn) return Bank.cost;
    if (btn == AddSkyScrapersBtn) return SkyScrapers.cost;
    if (btn == AddMeatProcessingPlantBtn) return MeatProcessingPlant.cost;
    if (btn == AddWarehousesBtn) return Warehouses.cost;
        if (btn == AddFrigateBtn) return new Frigate(currentPlayer).Cost;
    if (btn == AddCaravelBtn) return new Caraval(currentPlayer).Cost;
    if (btn == AddShipBtn) return new Ship(currentPlayer).Cost;
    if (btn == AddRammingShipBtn) return new RammingShip(currentPlayer).Cost;
    if (btn == AddWhalingShipBtn) return  WhalingShip.cost;
    if (btn == AddMonumentBtn) return  Monument.cost;
    if (btn == AddQuarryBtn) return  Quarry.cost;
    if (btn == AddMarketBtn) return  Market.cost; 
    if (btn == AddMineBtn) return  Mine.cost;
    //if (btn == AddFishingBoatsBtn) return new FishingBoats(currentPlayer).Cost;
    // Default if unassigned
    return 0;
}

    IEnumerator RunFightAnimation(Tile tile, int damage)
    {
        if (tile == null || tile.unit == null || !humanPlayer.exploredTiles[tile.x, tile.y]) 
            yield break;

        Vector3 pos = CalculateHexPosition(tile.x, tile.y) + Vector3.up * 0.6f;
        Quaternion rotation = Quaternion.Euler(45f, 0, 0f);

        GameObject dmgObj = Instantiate(damageTextPrefab, pos, rotation);
        dmgObj.tag = "Untagged";

        SetLayerRecursively(dmgObj, LayerMask.NameToLayer("nLayer"));

        TextMeshPro text = dmgObj.GetComponentInChildren<TextMeshPro>();
        text.text = damage.ToString();

        Color startColor = text.color;

        // --- Phase 1: stay fully visible (0.5s) ---
        yield return new WaitForSeconds(0.5f);

        // --- Phase 2: fade out (0.5s) ---
        float fadeDuration = 0.5f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;

            text.color = new Color(
            startColor.r,
            startColor.g,
            startColor.b,
            1f - t
        );

        yield return null;
        }

        Destroy(dmgObj);
    }


    IEnumerator HandleAttack()
    {
        int damageToDefender = CalculateDamage(mainTile.unit, currentTile.unit, currentTile);

        currentTile.unit.health -= damageToDefender;
        StartCoroutine(RunFightAnimation(currentTile, damageToDefender));

        int distance = CubeDistance(mainTile, currentTile);
        if (distance <= currentTile.unit.range)
        {
            int counterDamage = CalculateDamageToAttacker(mainTile.unit, currentTile.unit, currentTile);

            mainTile.unit.health -= counterDamage;
            if(counterDamage > 0)
                StartCoroutine(RunFightAnimation(mainTile, counterDamage));
        }        
            
        currentTile.unit.health = Mathf.Max(0, currentTile.unit.health);
        mainTile.unit.health = Mathf.Max(0, mainTile.unit.health);

        //If Attacker dies
        if(mainTile.unit.health<=0)
            mainTile.unit = null;

        if (currentTile.unit.health <= 0)
        {
            currentPlayer.enemeysKilled ++;

            if(HasActivePolicy("Propaganda")){
                    currentPlayer.culture += 4;
            }

            if( hasWonder("Greece", typeof(PopWonder),mainTile.unit.owner)){
                mainTile.unit.owner.culture += 5;
            }

            if (mainTile.unit != null&&mainTile.unit.range == 1 &&mainTile.unit.health > 0 && !(mainTile.unit.isBoat == true && (currentTile.tileType == "Plains"||currentTile.tileType == "Desert"||currentTile.tileType == "Snow"||currentTile.tileType == "Mountain"||currentTile.tileType == "District")&& !(mainTile.unit.isBoat == false && (currentTile.tileType == "Coast"||currentTile.tileType == "Ocean"||(currentTile.tileType == "River"&&currentTile.hasRoad==false&&currentTile.hasTrainTrack==false)))))
            {
                StartCoroutine(MoveUnitAIVisual(mainTile, currentTile, mainTile.unit,true));
                ExploreTile(currentPlayer, currentTile);
            }
            currentTile.unit = null;
        }

        if(mainTile.unit != null && mainTile.unit.health <=0){
            mainTile.unit = null;
        }

        Draw(drawUnit : true, drawClouds: true, drawMovementAttackIndicators: true);

        yield return null;
    }

    public void AddPopMonument()
    {
        if (currentTile.wonder != null) return;

        Wonder localWonder = new PopWonder(currentPlayer);

        if (currentPlayer.culture >= localWonder.Cost)
        {
            currentTile.wonder = localWonder;
            currentTile.resource = "";

            currentPlayer.culture -= localWonder.Cost;
            currentPlayer.popWonderBuilt = true;
        }

        Draw(drawWonders : true);
        ResetMovementAttackFlags();
    }
public void AddTradeMonument()
{
    // Prevent overwriting an existing wonder
    if (currentTile.wonder != null) return;

    Wonder localWonder = new TradeWonder(currentPlayer);

    if (currentPlayer.culture >= localWonder.Cost)
    {

        currentTile.wonder = localWonder;
        currentTile.resource = "";

        currentPlayer.culture -= localWonder.Cost;
        if(currentPlayer.tribeType == "Persia"){
        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile t = currentMap.tiles[x, y];
                if (t.owner == currentPlayer && t.tileType == "Mountain")
                {
                    currentPlayer.money +=10;
                }
            }
        }      

    }
        currentPlayer.tradeWonderBuilt = true;  
    }
    Draw(drawWonders : true);
    ResetMovementAttackFlags();
}
public void AddExpMonument()
{
    // Prevent overwriting an existing wonder
    if (currentTile.wonder != null) return;

    Wonder localWonder = new ExplorerWonder(currentPlayer);

    if (currentPlayer.culture >= localWonder.Cost)
    {
        currentTile.wonder = localWonder;
        currentTile.resource = "";

        currentPlayer.culture -= localWonder.Cost;
        currentPlayer.expWonderBuilt = true;
    }

    Draw(drawWonders : true);
    ResetMovementAttackFlags();
    
}
    public void ClaimTile(){
        if(currentPlayer.culture > 0){
                            Player oldOwner = currentTile.owner; 

            currentTile.owner = currentTile.unit.owner;
            if(hasWonder("Persia", typeof(ExplorerWonder),currentPlayer)){
                currentPlayer.money+= 3;
            }
            if(HasActivePolicy("Colonization")){
                currentMap.ClaimAdjacentTilesTroops(currentTile.x,currentTile.y,currentPlayer);
            }
            if(currentTile.district!=null)currentMap.ClaimAdjacentTilesTroops(currentTile.x,currentTile.y,currentPlayer);

            currentTile.unit.hasMoved = true;
            currentTile.unit.hasAttacked = true;
            currentPlayer.culture --;

    // --- CHECK IF WE JUST CLAIMED A CITY ---
    bool claimedCity = currentTile.district is City;
bool humanDestroyed = false;
    if (claimedCity && oldOwner != null && oldOwner != currentPlayer)
    {
        // Count remaining cities of the defeated civ
        bool hasOtherCities = false;

        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile t = currentMap.tiles[x, y];
                if (t.owner == oldOwner && t.district is City && t != currentTile)
                {
                    hasOtherCities = true;
                    break;
                }
            }
            if (hasOtherCities) break;
        }

        // --- IF NO OTHER CITIES → CIV IS DESTROYED ---
        if (!hasOtherCities)
        {
             humanDestroyed = oldOwner.isPlayer;

            // Transfer ALL tiles to the conqueror
            for (int x = 0; x < currentMap.width; x++)
            {
                for (int y = 0; y < currentMap.height; y++)
                {
                    Tile t = currentMap.tiles[x, y];
                    if (t.owner == oldOwner)
                    {
                        t.owner = currentPlayer;
                                currentPlayer.exploredTiles[t.x, t.y] = true;

                        //if (t.unit != null)
                          //  t.unit = null; // destroy troops
                    }
                    if(t.unit != null&&t.unit.owner == oldOwner)
                                            t.unit = null; // destroy troops

                }
            }


bool AIremaining = false;


    for (int x = 0; x < currentMap.width; x++)
    {
        for (int y = 0; y < currentMap.height; y++)
        {
            Tile t = currentMap.tiles[x, y];

            if (t.owner!=null&& t.owner.isPlayer == false)
            {
                AIremaining = true;
            }
        }
    }



if (humanDestroyed || !AIremaining)
{
    endGamePanel.SetActive(true);

    bool humanWon = !humanDestroyed; // True if human won
    Sprite resultSprite = humanWon ? victoryImage : loseImage;

    // Get the Image components
    Image img1 = endGamePanel.transform.GetChild(1).GetComponent<Image>();
    Image img2 = endGamePanel.transform.GetChild(2).GetComponent<Image>();

    img1.sprite = resultSprite;
    img2.sprite = resultSprite;

    // Change their size depending on victory/defeat
    Vector2 newSize = humanWon ? new Vector2(50, 37) : new Vector2(35, 47);
    img1.rectTransform.sizeDelta = newSize;
    img2.rectTransform.sizeDelta = newSize;

    // Update the TextMeshProUGUI text
    endGamePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text =
        humanWon ? "Victory" : "Defeat";
    endGamePanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Money Made: " + (humanWon ? currentPlayer.moneyMade : oldOwner.moneyMade);
    endGamePanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Culture Made: " + (humanWon ? currentPlayer.cultureMade : oldOwner.cultureMade);
    endGamePanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Enemies Killed: " + (humanWon ? currentPlayer.enemeysKilled : oldOwner.enemeysKilled);
    //endGamePanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "Turns: " + turn;

}else{
    TribeDestroyedPanel.SetActive(true);
    int playerIndex = System.Array.IndexOf(players, oldOwner);

    TribeDestroyedPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "The empire of " + oldOwner.tribeType + " ruled by player " +(playerIndex + 1)+ " has fallen";
        Button closeButton = TribeDestroyedPanel.transform.GetChild(4).GetComponent<Button>();

    // Add a listener to close the panel
    closeButton.onClick.AddListener(() =>
    {
        TribeDestroyedPanel.SetActive(false);
    });
}

        }
    }

            Draw(drawBorders : true, drawUnit : true);
            ResetMovementAttackFlags();
        }
    }
    public void HealTroop(){
        currentTile.unit.health += 4;
        currentTile.unit.hasAttacked = true;
        currentTile.unit.hasMoved = true;

        if(currentTile.unit.health > currentTile.unit.maxHealth)
            currentTile.unit.health = currentTile.unit.maxHealth;
        Draw( drawUnit : true);
    }
    public void AddCity(){
        City localCity = new City();
        if(currentPlayer.money >= localCity.Cost(currentPlayer)){
            currentTile.district = localCity;
            currentTile.tileType = "District";
            currentTile.resource = "";
            currentPlayer.money -= localCity.Cost(currentPlayer);
            currentMap.ClaimAdjacentTiles(currentTile.x,currentTile.y,currentPlayer);
            ExploreDouble(currentPlayer, currentTile);
        }
        Draw(drawTile : true, drawBorders : true, drawClouds: true); // IHN
        ResetMovementAttackFlags();

    }    
    public void AddHarbour(){
        if(currentPlayer.money >= Harbour.cost){
            currentTile.district = new Harbour();
            //currentTile.tileType = "District";
            currentTile.resource = "";
            currentPlayer.money -= Harbour.cost;
            currentMap.ClaimAdjacentTiles(currentTile.x,currentTile.y,currentPlayer);
            ExploreDouble(currentPlayer, currentTile);
        }
        Draw(drawTile : true, drawBorders : true, drawClouds: true);
        ResetMovementAttackFlags();

    }
    public void AddCommercial(){
        if(currentPlayer.money >= Commercial.cost){
            currentTile.district = new Commercial();
            currentTile.tileType = "District";
            currentTile.resource = "";
            currentPlayer.money -= Commercial.cost;
            currentMap.ClaimAdjacentTiles(currentTile.x,currentTile.y,currentPlayer);
            ExploreDouble(currentPlayer, currentTile);
        }
        Draw(drawTile : true, drawBorders : true, drawClouds: true);
        ResetMovementAttackFlags();
    }
    public void AddIndustrial(){
        if(currentPlayer.money >= Industrial.cost){
            currentTile.district = new Industrial();
            currentTile.tileType = "District";
            currentTile.resource = "";
            currentPlayer.money -= Industrial.cost;
            currentMap.ClaimAdjacentTiles(currentTile.x,currentTile.y,currentPlayer);
            ExploreDouble(currentPlayer, currentTile);
        }
        Draw(drawTile : true, drawBorders : true, drawClouds: true);
        ResetMovementAttackFlags();
    }
    public void BurnForest(){
        if(currentPlayer.money >= 2){
            currentTile.forestResource = "";
            if(currentTile.tileType !="Snow")
                currentTile.resource = "Crop";
            currentPlayer.money -= 2;
        }
        Draw(drawTile : true);
        ResetMovementAttackFlags();

    }
    public void Destroy(){
        if(currentPlayer.money >= 5){
            currentTile.building = null;
            currentTile.district = null;
            if(currentTile.tileType == "District")
            currentTile.tileType = "Plains";

            currentPlayer.money -= 5;
        }
        Draw(drawTile : true, drawBuildings : true);
        ResetMovementAttackFlags();
    }
    public void ChopForest(){
        currentTile.forestResource = "";
        currentPlayer.money += 2;
        Draw(drawTile : true);
        ResetMovementAttackFlags();
    }
    /*private int CalculateDamage(Troops attacker, Troops defender)
    {
        int defenderHealth = ((attacker.attack*4)/defender.defence) * (attacker.health/attacker.maxHealth);

        //int attackerHealth = ((defender.defence*4)/attacker.attack) * (defender.health/defender.maxHealth);
        return defenderHealth;
    }
    private int CalculateDamageToAttacker(Troops attacker, Troops defender)
    {
        //int defenderHealth = ((attacker.attack*4)/defender.defence) * (attacker.health/attacker.maxHealth);

        int attackerHealth = ((defender.defence*4)/attacker.attack) * (defender.health/defender.maxHealth);
        return attackerHealth;
    }*/
private int CalculateDamage(Troops attacker, Troops defender, Tile defendingTile)
{
    int defenceBonus = defendingTile.defenceBonus(defender.owner);
if(defendingTile.tileType == "Desert"&&hasWonder("Eygpt", typeof(ExplorerWonder),defender.owner))
    defenceBonus = 1;
if(defendingTile.tileType == "Mountain"&&hasWonder("Persia", typeof(PopWonder),defender.owner))
    defenceBonus = 2;
    float attackFactor = (float)attacker.attack / (defender.defence + defenceBonus);
    float healthFactor = (float)attacker.health / attacker.maxHealth;

    float damage = attackFactor * 5f * healthFactor;

    return Mathf.Max(1, Mathf.RoundToInt(damage));
}

private int CalculateDamageToAttacker(Troops attacker, Troops defender, Tile defendingTile)
{
    if (defender.health <= 0) return 0; // No counterattack if defender is dead
    int defenceBonus = defendingTile.defenceBonus(defender.owner);
if(defendingTile.tileType == "Desert"&&hasWonder("Eygpt", typeof(ExplorerWonder),defender.owner))
    defenceBonus = 1;
if(defendingTile.tileType == "Mountain"&&hasWonder("Persia", typeof(PopWonder),defender.owner))
    defenceBonus = 2;
    float defenceFactor = (float)(defender.defence + defenceBonus) / attacker.attack;
    float healthFactor = (float)defender.health / defender.maxHealth;

    float damage = defenceFactor * 5f * healthFactor;

    return Mathf.Max(0, Mathf.RoundToInt(damage)); // Clamp to 0
}

    /*public void AddWarrior()
    {
        if (currentTile.district is City){
            if(currentTile.unit == null&&currentPlayer.money >= Warrior.Cost){
                currentTile.unit = new Warrior(currentPlayer);
                currentPlayer.money -= Warrior.Cost;
            }
        }
        Draw();
    }

    public void AddHorseman()
    {
        if (currentTile.district is City){
            if(currentTile.unit == null && currentPlayer.money >= Horseman.Cost){
                currentTile.unit = new Horseman(currentPlayer);
                currentPlayer.money -= Horseman.Cost;
            }
        }
        Draw();
    }
    public void AddSpearman()
    {
        if (currentTile.district is City){
            if(currentTile.unit == null && currentPlayer.money >= Spearman.Cost){
                currentTile.unit = new Spearman(currentPlayer);
                currentPlayer.money -= Spearman.Cost;
            }
        }
        Draw();
    }
    public void AddShield()
    {
        //if (currentTile.district is City){
            if(currentTile.unit == null && currentPlayer.money >= Shield.Cost){
                currentTile.unit = new Shield(currentPlayer);
                currentPlayer.money -= Shield.Cost;
            }
        //}
        Draw();
    }
    public void AddArcher()
{
    if (currentTile.district is City){
        if(currentTile.unit == null && currentPlayer.money >= Archer.Cost){
            currentTile.unit = new Archer(currentPlayer);
            currentPlayer.money -= Archer.Cost;
        }
    }
    Draw();
}

public void AddChariot()
{
    if (currentTile.district is City){
        if(currentTile.unit == null && currentPlayer.money >= Chariot.Cost){
            currentTile.unit = new Chariot(currentPlayer);
            currentPlayer.money -= Chariot.Cost;
        }
    }
    Draw();
}

public void AddSwordsman()
{
    if (currentTile.district is City){
        if(currentTile.unit == null && currentPlayer.money >= Swordsman.Cost){
            currentTile.unit = new Swordsman(currentPlayer);
            currentPlayer.money -= Swordsman.Cost;
        }
    }
    Draw();
}

public void AddKnight()
{
    if (currentTile.district is City){
        if(currentTile.unit == null && currentPlayer.money >= Knight.Cost){
            currentTile.unit = new Knight(currentPlayer);
            currentPlayer.money -= Knight.Cost;
        }
    }
    Draw();
}

public void AddCatapult()
{
    if (currentTile.district is City){
        if(currentTile.unit == null && currentPlayer.money >= Catapult.Cost){
            currentTile.unit = new Catapult(currentPlayer);
            currentPlayer.money -= Catapult.Cost;
        }
    }
    Draw();
}

public void AddFrigate()
{
    if (currentTile.district is Harbour){
        if(currentTile.unit == null && currentPlayer.money >= Frigate.Cost){
            currentTile.unit = new Frigate(currentPlayer);
            currentPlayer.money -= Frigate.Cost;
        }
    }
    Draw();
}

public void AddCaraval()
{
    if (currentTile.district is Harbour){
        if(currentTile.unit == null && currentPlayer.money >= Caraval.Cost){
            currentTile.unit = new Caraval(currentPlayer);
            currentPlayer.money -= Caraval.Cost;
        }
    }
    Draw();
}

public void AddMusketman()
{
    if (currentTile.district is City){
        if(currentTile.unit == null && currentPlayer.money >= Musketman.Cost){
            currentTile.unit = new Musketman(currentPlayer);
            currentPlayer.money -= Musketman.Cost;
        }
    }
    Draw();
}

public void AddCannon()
{
    if (currentTile.district is City){
        if(currentTile.unit == null && currentPlayer.money >= Cannon.Cost){
            currentTile.unit = new Cannon(currentPlayer);
            currentPlayer.money -= Cannon.Cost;
        }
    }
    Draw();
}
public void AddCavalry(){
    if(currentTile.unit == null && currentPlayer.money >= Cavalry.Cost){
        currentTile.unit = new Cavalry(currentPlayer);
        currentPlayer.money -= Cavalry.Cost;
    }
    Draw();
}
public void AddMachineGun()
{
    if (currentTile.district is City){
        if(currentTile.unit == null && currentPlayer.money >= MachineGun.Cost){
            currentTile.unit = new MachineGun(currentPlayer);
            currentPlayer.money -= MachineGun.Cost;
        }
    }
    Draw();
}

public void AddInfantry()
{
    if (currentTile.district is City){
        if(currentTile.unit == null && currentPlayer.money >= Infantry.Cost){
            currentTile.unit = new Infantry(currentPlayer);
            currentPlayer.money -= Infantry.Cost;
        }
    }
    Draw();
}

public void AddArtillery()
{
    if (currentTile.district is City){
        if(currentTile.unit == null && currentPlayer.money >= Artillery.Cost){
            currentTile.unit = new Artillery(currentPlayer);
            currentPlayer.money -= Artillery.Cost;
        }
    }
    Draw();
}

public void AddTank()
{
    //if (currentTile.district is City){
        if(currentTile.unit == null && currentPlayer.money >= Tank.Cost){
            currentTile.unit = new Tank(currentPlayer);
            currentPlayer.money -= Tank.Cost;
        }
    //}
    Draw();
}

public void AddZeppelin()
{
        if(currentTile.unit == null && currentPlayer.money >= Zeppelin.Cost){
            currentTile.unit = new Zeppelin(currentPlayer);
            currentPlayer.money -= Zeppelin.Cost;
        }
    
    Draw();
}

public void AddBiplane()
{
        if(currentTile.unit == null && currentPlayer.money >= Biplane.Cost){
            currentTile.unit = new Biplane(currentPlayer);
            currentPlayer.money -= Biplane.Cost;
        }
    
    Draw();
}
    public void AddDreadnort()
    {
        if(currentTile.unit == null && currentPlayer.money >= Dreadnort.Cost){
            currentTile.unit = new Dreadnort(currentPlayer);
            currentPlayer.money -= Dreadnort.Cost;
        }
    
        Draw();
    }

    public void AddCruiser()
    {
        if(currentTile.unit == null && currentPlayer.money >= Cruiser.Cost){
            currentTile.unit = new Cruiser(currentPlayer);
            currentPlayer.money -= Cruiser.Cost;
        }
    
        Draw();
    }


    public void AddShip()
    {
        if (currentTile.district is Harbour){
            if(currentTile.unit == null && currentPlayer.money >= Ship.Cost){
                currentTile.unit = new Ship(currentPlayer);
                currentPlayer.money -= Ship.Cost;
            }
        }
        Draw();
    }
    public void AddRammingShip()
    {
        if (currentTile.district is Harbour){
            if(currentTile.unit == null && currentPlayer.money >= RammingShip.Cost){
                currentTile.unit = new RammingShip(currentPlayer);
                currentPlayer.money -= RammingShip.Cost;
            }
        }
        Draw();
    }*/
    //OK Chat gpt here is the code u need to copy into every function
    // ================= PANEL FUNCTION =================
void ShowTroopPanel(Troops troop, System.Action trainAction, bool requireCity = true, bool requireHarbour = false)
{
    civicDisplayPanel.SetActive(true);
    ignoreClicks = true;

    civicTitleText.text = troop.name;

    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    sb.AppendLine("Stats:");
    sb.AppendLine("Attack: " + troop.attack);
    sb.AppendLine("Defence: " + troop.defence);
    sb.AppendLine("Movement: " + troop.movement);
    sb.AppendLine("Range: " + troop.range);
    sb.AppendLine("Health: " + troop.health + "/" + troop.maxHealth);
    sb.AppendLine("Cost: " + troop.Cost);

    civicUnlocksText.text = sb.ToString();

    bool canTrain =
        
        currentTile.unit == null &&
        currentPlayer.money >= troop.Cost;

    civicUnlockButton.interactable = canTrain;

    civicUnlockButton.onClick.RemoveAllListeners();
    civicUnlockButton.onClick.AddListener(() =>
    {
        trainAction();
        civicDisplayPanel.SetActive(false);
        ignoreClicks = false;
    });
}

// ================= TRAIN FUNCTION =================
void TrainTroop(Troops troop)
{
    if (currentTile.unit == null && currentPlayer.money >= troop.Cost)
    {
        currentTile.unit = troop;
        currentPlayer.money -= troop.Cost;

        if (HasActivePolicy("Nationalism"))
            currentPlayer.culture += troop.Cost / 2;
    }

    Draw(drawUnit: true);
}

// ================= TROOP BUTTONS =================

// Warrior
public void AddWarrior(){ ShowTroopPanel(new Warrior(currentPlayer), AddWarrior1); }
public void AddWarrior1(){ TrainTroop(new Warrior(currentPlayer)); }

// Horseman
public void AddHorseman(){ ShowTroopPanel(new Horseman(currentPlayer), AddHorseman1); }
public void AddHorseman1(){ TrainTroop(new Horseman(currentPlayer)); }

// Spearman
public void AddSpearman(){ ShowTroopPanel(new Spearman(currentPlayer), AddSpearman1); }
public void AddSpearman1(){ TrainTroop(new Spearman(currentPlayer)); }

// Shield
public void AddShield(){ ShowTroopPanel(new Shield(currentPlayer), AddShield1); }
public void AddShield1(){ TrainTroop(new Shield(currentPlayer)); }

// Archer
public void AddArcher(){ ShowTroopPanel(new Archer(currentPlayer), AddArcher1); }
public void AddArcher1(){ TrainTroop(new Archer(currentPlayer)); }

// Chariot
public void AddChariot(){ ShowTroopPanel(new Chariot(currentPlayer), AddChariot1); }
public void AddChariot1(){ TrainTroop(new Chariot(currentPlayer)); }

// Swordsman
public void AddSwordsman(){ ShowTroopPanel(new Swordsman(currentPlayer), AddSwordsman1); }
public void AddSwordsman1(){ TrainTroop(new Swordsman(currentPlayer)); }

// Knight
public void AddKnight(){ ShowTroopPanel(new Knight(currentPlayer), AddKnight1); }
public void AddKnight1(){ TrainTroop(new Knight(currentPlayer)); }

// Catapult
public void AddCatapult(){ ShowTroopPanel(new Catapult(currentPlayer), AddCatapult1); }
public void AddCatapult1(){ TrainTroop(new Catapult(currentPlayer)); }

// Frigate (Harbour)
public void AddFrigate(){ ShowTroopPanel(new Frigate(currentPlayer), AddFrigate1, false, true); }
public void AddFrigate1(){ TrainTroop(new Frigate(currentPlayer)); }

// Caraval (Harbour)
public void AddCaraval(){ ShowTroopPanel(new Caraval(currentPlayer), AddCaraval1, false, true); }
public void AddCaraval1(){ TrainTroop(new Caraval(currentPlayer)); }

// Musketman
public void AddMusketman(){ ShowTroopPanel(new Musketman(currentPlayer), AddMusketman1); }
public void AddMusketman1(){ TrainTroop(new Musketman(currentPlayer)); }

// Cannon
public void AddCannon(){ ShowTroopPanel(new Cannon(currentPlayer), AddCannon1); }
public void AddCannon1(){ TrainTroop(new Cannon(currentPlayer)); }

// Cavalry
public void AddCavalry(){ ShowTroopPanel(new Cavalry(currentPlayer), AddCavalry1); }
public void AddCavalry1(){ TrainTroop(new Cavalry(currentPlayer)); }

// Machine Gun
public void AddMachineGun(){ ShowTroopPanel(new MachineGun(currentPlayer), AddMachineGun1); }
public void AddMachineGun1(){ TrainTroop(new MachineGun(currentPlayer)); }

// Infantry
public void AddInfantry(){ ShowTroopPanel(new Infantry(currentPlayer), AddInfantry1); }
public void AddInfantry1(){ TrainTroop(new Infantry(currentPlayer)); }

// Artillery
public void AddArtillery(){ ShowTroopPanel(new Artillery(currentPlayer), AddArtillery1); }
public void AddArtillery1(){ TrainTroop(new Artillery(currentPlayer)); }

// Tank
public void AddTank(){ ShowTroopPanel(new Tank(currentPlayer), AddTank1); }
public void AddTank1(){ TrainTroop(new Tank(currentPlayer)); }

// Zeppelin
public void AddZeppelin(){ ShowTroopPanel(new Zeppelin(currentPlayer), AddZeppelin1); }
public void AddZeppelin1(){ TrainTroop(new Zeppelin(currentPlayer)); }

// Biplane
public void AddBiplane(){ ShowTroopPanel(new Biplane(currentPlayer), AddBiplane1); }
public void AddBiplane1(){ TrainTroop(new Biplane(currentPlayer)); }

// Dreadnort
public void AddDreadnort(){ ShowTroopPanel(new Dreadnort(currentPlayer), AddDreadnort1); }
public void AddDreadnort1(){ TrainTroop(new Dreadnort(currentPlayer)); }

// Cruiser
public void AddCruiser(){ ShowTroopPanel(new Cruiser(currentPlayer), AddCruiser1); }
public void AddCruiser1(){ TrainTroop(new Cruiser(currentPlayer)); }

// Ship (Harbour)
public void AddShip(){ ShowTroopPanel(new Ship(currentPlayer), AddShip1, false, true); }
public void AddShip1(){ TrainTroop(new Ship(currentPlayer)); }

// Ramming Ship (Harbour)
public void AddRammingShip(){ ShowTroopPanel(new RammingShip(currentPlayer), AddRammingShip1, false, true); }
public void AddRammingShip1(){ TrainTroop(new RammingShip(currentPlayer)); }
    /*public void AddWarrior(){
          Troops warrior = new Warrior(currentPlayer);

    civicDisplayPanel.SetActive(true);
    civicTitleText.text = warrior.name;

    System.Text.StringBuilder sb = new System.Text.StringBuilder();

sb.AppendLine("Stats:");
sb.AppendLine("Attack: " + warrior.attack);
sb.AppendLine("Defence: " + warrior.defence);
sb.AppendLine("Movement: " + warrior.movement);
sb.AppendLine("Range: " + warrior.range);
sb.AppendLine("Health: " + warrior.health + "/" + warrior.maxHealth);
sb.AppendLine("Cost: " + warrior.Cost);

    civicUnlocksText.text = sb.ToString();

    bool canTrain =
        currentTile.district is City &&
        currentTile.unit == null &&
        currentPlayer.money >= warrior.Cost;

    civicUnlockButton.interactable = canTrain;

    civicUnlockButton.onClick.RemoveAllListeners();
    civicUnlockButton.onClick.AddListener(() =>
    {
        AddWarrior1();
        civicDisplayPanel.SetActive(false);
    });
    }
    public void AddWarrior1()
    {
      if (currentTile.district is City){
          Troops troop = new Warrior(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
              if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddHorseman()
  {
      if (currentTile.district is City){
          Troops troop = new Horseman(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
             if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddSpearman()
  {
      if (currentTile.district is City){
          Troops troop = new Spearman(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
            if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddShield()
  {
      Troops troop = new Shield(currentPlayer);
      if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
          currentTile.unit = troop;
          currentPlayer.money -= troop.Cost;
            if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
      }
      Draw(drawUnit : true);
  }

  public void AddArcher()
  {
      if (currentTile.district is City){
          Troops troop = new Archer(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddChariot()
  {
      if (currentTile.district is City){
          Troops troop = new Chariot(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddSwordsman()
  {
      if (currentTile.district is City){
          Troops troop = new Swordsman(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddKnight()
  {
      if (currentTile.district is City){
          Troops troop = new Knight(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddCatapult()
  {
      if (currentTile.district is City){
          Troops troop = new Catapult(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddFrigate()
  {
      if (currentTile.district is Harbour){
          Troops troop = new Frigate(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddCaraval()
  {
      if (currentTile.district is Harbour){
          Troops troop = new Caraval(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddMusketman()
  {
      if (currentTile.district is City){
          Troops troop = new Musketman(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddCannon()
  {
      if (currentTile.district is City){
          Troops troop = new Cannon(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddCavalry()
  {
      Troops troop = new Cavalry(currentPlayer);
      if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
          currentTile.unit = troop;
          currentPlayer.money -= troop.Cost;
                      if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
      }
      Draw(drawUnit : true);
  }

  public void AddMachineGun()
  {
      if (currentTile.district is City){
          Troops troop = new MachineGun(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddInfantry()
  {
      if (currentTile.district is City){
          Troops troop = new Infantry(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddArtillery()
  {
      if (currentTile.district is City){
          Troops troop = new Artillery(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                          if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddTank()
  {
      Troops troop = new Tank(currentPlayer);
      if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
          currentTile.unit = troop;
          currentPlayer.money -= troop.Cost;
                      if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
      }
      Draw(drawUnit : true);
  }

  public void AddZeppelin()
  {
      Troops troop = new Zeppelin(currentPlayer);
      if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
          currentTile.unit = troop;
          currentPlayer.money -= troop.Cost;
                      if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
      }
      Draw(drawUnit : true);
  }

  public void AddBiplane()
  {
      Troops troop = new Biplane(currentPlayer);
      if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
          currentTile.unit = troop;
          currentPlayer.money -= troop.Cost;
                                if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
      }
      Draw(drawUnit : true);
  }

  public void AddDreadnort()
  {
      Troops troop = new Dreadnort(currentPlayer);
      if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
          currentTile.unit = troop;
          currentPlayer.money -= troop.Cost;
                                if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
      }
      Draw(drawUnit : true);
  }

  public void AddCruiser()
  {
      Troops troop = new Cruiser(currentPlayer);
      if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
          currentTile.unit = troop;
          currentPlayer.money -= troop.Cost;
                                if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
      }
      Draw(drawUnit : true);
  }

  public void AddShip()
  {
      if (currentTile.district is Harbour){
          Troops troop = new Ship(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                                    if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }

  public void AddRammingShip()
  {
      if (currentTile.district is Harbour){
          Troops troop = new RammingShip(currentPlayer);
          if(currentTile.unit == null && currentPlayer.money >= troop.Cost){
              currentTile.unit = troop;
              currentPlayer.money -= troop.Cost;
                                    if(HasActivePolicy("Nationalism")){
                currentPlayer.culture += troop.Cost/2;
              }
          }
      }
      Draw(drawUnit : true);
  }*/

    public void AddWall()
    {
        if(currentPlayer.money >= 5 && currentTile.hasWall == false){
            currentTile.hasWall = true;
            currentPlayer.money -= 5;
        }
        
        Draw(drawWalls : true);
    }
    public void AddPasture(){
        if(currentTile.tileType == "Plains"){
            if(currentTile.building == null && currentTile.forestResource == "" && currentPlayer.money >= Pasture.cost&&currentTile.district == null ){
                currentTile.building = new Pasture();
                currentPlayer.money -= Pasture.cost;
            }
        }
        Draw(drawBuildings : true, drawTile : true);
    }
    public void AddFarm(){
        if(currentTile.tileType == "Plains"||currentTile.tileType == "Desert"){
            if(currentTile.building == null && currentTile.forestResource == "" && currentPlayer.money >= Farm.cost&&currentTile.resource=="Crop"&&currentTile.district == null ){
                currentTile.building = new Farm();
                currentPlayer.money -= Farm.cost;
                currentTile.resource = "";
            }
        }
        Draw(drawBuildings : true, drawTile : true);
    }
    public void AddFishingBoats(){
            if(currentTile.building == null  && currentPlayer.money >= FishingBoats.cost &&currentTile.district == null ){
                currentTile.building = new FishingBoats();
                currentPlayer.money -= FishingBoats.cost;
                currentTile.resource = "";
            }
        
        Draw(drawBuildings : true, drawTile : true);
    }
    public void AddMine(){
            if(currentTile.building == null  && currentPlayer.money >= Mine.cost &&currentTile.district == null ){
                currentTile.building = new Mine();
                currentPlayer.money -= Mine.cost;
                currentTile.resource = "";
            }
        
        Draw(drawBuildings : true, drawTile : true);
    }
    public void AddMonument(){
            if(currentTile.district != null && currentTile.district.building == null  && currentPlayer.money >= Monument.cost ){
                currentTile.district.building = new Monument();
                currentPlayer.money -= Monument.cost;
                currentTile.resource = "";
            }
        
        Draw(drawBuildings : true, drawTile : true);
    }
    public void AddQuarry(){
            if(currentTile.building == null  && currentPlayer.money >= Quarry.cost &&currentTile.district == null ){
                currentTile.building = new Quarry();
                currentPlayer.money -= Quarry.cost;
                currentTile.resource = "";
            }
        
        Draw(drawBuildings : true, drawTile : true);
    }
    public void AddMarket(){
    if(currentTile.district != null && currentTile.district.building == null && currentPlayer.money >= Market.cost){
        currentTile.district.building = new Market();
        currentPlayer.money -= Market.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true);
}

public void AddCustomsHouse(){
    if(currentTile.district != null && currentTile.district.building == null && currentPlayer.money >= CustomHouse.cost){
        currentTile.district.building = new CustomHouse();
        currentPlayer.money -= CustomHouse.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true);
}

public void AddTower(){
    if(currentTile.district != null && currentTile.district.building == null && currentPlayer.money >= Tower.cost){
        currentTile.district.building = new Tower();
        currentPlayer.money -= Tower.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true);
}

public void AddLightHouse(){
    if(currentTile.district != null && currentTile.district.building == null && currentPlayer.money >= LightHouse.cost){
        currentTile.district.building = new LightHouse();
        currentPlayer.money -= LightHouse.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true);
}

public void AddShipyard(){
    if(currentTile.district != null && currentTile.district.building == null && currentPlayer.money >= Shipyard.cost){
        currentTile.district.building = new Shipyard();
        currentPlayer.money -= Shipyard.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true);
}

public void AddCarpentryWorkshop(){
    if(currentTile.district != null && currentTile.district.building == null && currentPlayer.money >= CarpentryWorkshop.cost){
        currentTile.district.building = new CarpentryWorkshop();
        currentPlayer.money -= CarpentryWorkshop.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true);
}

public void AddBank(){
    if(currentTile.district != null && currentTile.district.building == null && currentPlayer.money >= Bank.cost){
        currentTile.district.building = new Bank();
        currentPlayer.money -= Bank.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true);
}

public void AddNavalBase(){
    if(currentTile.district != null && currentTile.district.building == null && currentPlayer.money >= NavelBase.cost){
        currentTile.district.building = new NavelBase();
        currentPlayer.money -= NavelBase.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true);
}

public void AddSkyScrapers(){
    if(currentTile.district != null && currentTile.district.building == null && currentPlayer.money >= SkyScrapers.cost){
        currentTile.district.building = new SkyScrapers();
        currentPlayer.money -= SkyScrapers.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true);
}

public void AddWarehouses(){
    if(currentTile.district != null && currentTile.district.building == null && currentPlayer.money >= Warehouses.cost){
        currentTile.district.building = new Warehouses();
        currentPlayer.money -= Warehouses.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true);
}

public void AddTankFactory(){
    if(currentTile.district != null && currentTile.district.building == null && currentPlayer.money >= Factorys.cost){
        currentTile.district.building = new Factorys();
        currentPlayer.money -= Factorys.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true);
}
public void AddWaterwheel(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= Waterwheel.cost){
        currentTile.building = new Waterwheel();
        currentPlayer.money -= Waterwheel.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}

public void AddFurTradingPost(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= FurTradingPost.cost){
        currentTile.building = new FurTradingPost();
        currentPlayer.money -= FurTradingPost.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}

public void AddLumberHut(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= LumberHut.cost){
        currentTile.building = new LumberHut();
        currentPlayer.money -= LumberHut.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}

public void AddForge(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= Forge.cost){
        currentTile.building = new Forge();
        currentPlayer.money -= Forge.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}

public void AddFort(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= Fort.cost){
        currentTile.building = new Fort();
        currentPlayer.money -= Fort.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}

public void AddWindmill(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= Windmill.cost){
        currentTile.building = new Windmill();
        currentPlayer.money -= Windmill.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}

public void AddSawmill(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= Sawmill.cost){
        currentTile.building = new Sawmill();
        currentPlayer.money -= Sawmill.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}

public void AddWhalingShip(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= WhalingShip.cost){
        currentTile.building = new WhalingShip();
        currentPlayer.money -= WhalingShip.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}

public void AddPaperMill(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= Papermill.cost){
        currentTile.building = new Papermill();
        currentPlayer.money -= Papermill.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}

public void AddMeatProcessingPlant(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= MeatProcessingPlant.cost){
        currentTile.building = new MeatProcessingPlant();
        currentPlayer.money -= MeatProcessingPlant.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}

public void AddAirport(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= Airport.cost){
        currentTile.building = new Airport();
        currentPlayer.money -= Airport.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}

public void AddUniversity(){
    if(currentTile.building == null && currentTile.district == null && currentPlayer.money >= University.cost){
        currentTile.building = new University();
        currentPlayer.money -= University.cost;
        currentTile.resource = "";
    }
    Draw(drawBuildings : true, drawTile : true);
}
    public void AddRoad(){
        if(currentPlayer.money >= 2){
            currentTile.hasRoad = true;
            currentPlayer.money -= 2;
            if(HasActivePolicy("Public Works")){
                currentPlayer.culture += 2;
            }
        }
        Draw(drawRoads : true, drawTradeRoutes : true);
    }
    public void AddTrainTracks(){
        if(currentPlayer.money >= 5){
            currentTile.hasTrainTrack = true;
            currentPlayer.money -= 5;
            if(HasActivePolicy("Public Works")){
                currentPlayer.culture += 2;
            }
        }
        Draw(drawTrainTracks : true);
    }
    public void AddTradeRoute(){
        if(currentPlayer.money >= 2){
            currentTile.hasTradeRoute = true;
            currentPlayer.money -= 2;
            if(HasActivePolicy("Public Works")){
                currentPlayer.culture += 2;
            }
        }
        Draw(drawTradeRoutes : true, drawRoads : true);
    } 
    public void huntAnimal(){
        currentTile.resource ="";
        currentPlayer.money += 3;
        Draw(drawTile : true);
        ResetMovementAttackFlags();

    }
    public void huntFish(){
        currentTile.resource ="";
        currentPlayer.money += 3;
        Draw(drawTile : true);
        ResetMovementAttackFlags();

    }
    public void SpawnBorders(Tile tile, int x, int y)
    {
        if (tile.owner == null) return;

        Vector3 basePos = CalculateHexPosition(x, y)+ new Vector3(0f, 0.35f, 0f);

        if (y % 2 == 0)//even row
        {
            if (!SameOwner(x+1, y, tile.owner))
                SpawnBorder(basePos, 0f, tile);

            if (!SameOwner(x, y + 1, tile.owner))
                SpawnBorder(basePos, 300f, tile);

            if (!SameOwner(x-1, y+1 , tile.owner))
                SpawnBorder(basePos, 240f,tile);

            if (!SameOwner(x - 1, y, tile.owner))
                SpawnBorder(basePos, 180f,tile);

            if (!SameOwner(x-1, y - 1, tile.owner))
                SpawnBorder(basePos, 120f,tile);

            if (!SameOwner(x, y - 1, tile.owner))
                SpawnBorder(basePos, 60f,tile);
        }else//odd row
        {
            if (!SameOwner(x + 1, y, tile.owner))
                SpawnBorder(basePos, 0f,tile);

            if (!SameOwner(x+1, y + 1, tile.owner))
                SpawnBorder(basePos, 300f,tile);

            if (!SameOwner(x , y +1, tile.owner))
                SpawnBorder(basePos, 240f,tile);

            if (!SameOwner(x-1 , y, tile.owner))
                SpawnBorder(basePos, 180f,tile);

            if (!SameOwner(x , y -1, tile.owner))
                SpawnBorder(basePos, 120f,tile);

            if (!SameOwner(x+1, y-1 , tile.owner))
                SpawnBorder(basePos, 60f,tile);
        }
    }

    bool SameOwner(int nx, int ny, Player owner)
    {
        if (nx < 0 || ny < 0 || nx >= currentMap.width || ny >= currentMap.height)
            return false;

        Tile n = currentMap.tiles[nx, ny];

        if (n == null || n.owner == null) 
            return false;

        return n.owner == owner;
    }

    void SpawnBorder(Vector3 basePos, float rot, Tile tile)
    {
        GameObject b = Instantiate(borderPrefab, basePos, Quaternion.Euler(0, rot, 0));
        b.tag = "Destroyable";
                    b.tag = "Borders";

        SetLayerRecursively(b,LayerMask.NameToLayer("UILayer"));
        ColorUnit(b, tile.owner.playerColor,tile.owner.SecondaryColor);
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
       obj.layer = layer;
       foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
    public void SpawnWalls(Tile tile, int x, int y)
    {
        if (!tile.hasWall) return;

        Vector3 basePos = CalculateHexPosition(x, y)/* + new Vector3(0f, 0.35f, 0f)*/;

        if (y % 2 == 0) // even row
        {
            if (!HasWall(x + 1, y))
                SpawnWall(basePos, 0f);

            if (!HasWall(x, y + 1))
                SpawnWall(basePos, 300f);

            if (!HasWall(x - 1, y + 1))
                SpawnWall(basePos, 240f);

            if (!HasWall(x - 1, y))
                SpawnWall(basePos, 180f);

            if (!HasWall(x - 1, y - 1))
                SpawnWall(basePos, 120f);

            if (!HasWall(x, y - 1))
                SpawnWall(basePos, 60f);
        }
        else // odd row
        {
            if (!HasWall(x + 1, y))
                SpawnWall(basePos, 0f);

            if (!HasWall(x + 1, y + 1))
                SpawnWall(basePos, 300f);

            if (!HasWall(x, y + 1))
                SpawnWall(basePos, 240f);

            if (!HasWall(x - 1, y))
                SpawnWall(basePos, 180f);

            if (!HasWall(x, y - 1))
                SpawnWall(basePos, 120f);

            if (!HasWall(x + 1, y - 1))
                SpawnWall(basePos, 60f);
        }
    }
    bool HasWall(int nx, int ny)
    {
        if (nx < 0 || ny < 0 || nx >= currentMap.width || ny >= currentMap.height)
            return false;

        Tile t = currentMap.tiles[nx, ny];
        if (t == null) return false;

        return t.hasWall;
    }
    void SpawnWall(Vector3 basePos, float rot)
    {
        GameObject w = Instantiate(wallPrefab, basePos, Quaternion.Euler(0, rot, 0));
        w.tag = "Destroyable";
                w.tag = "Walls";

    }
    public void SpawnRoads(Tile tile, int x, int y)
{
    if (!tile.hasRoad) return;

    Vector3 basePos = CalculateHexPosition(x, y);

    if (y % 2 == 0) // even row
    {
        TrySpawnRoad(x + 1, y,     basePos, 180f,   tile);
        TrySpawnRoad(x,     y + 1, basePos, 120f, tile);
        TrySpawnRoad(x - 1, y + 1, basePos, 60f, tile);
        TrySpawnRoad(x - 1, y,     basePos, 0f, tile);
        TrySpawnRoad(x - 1, y - 1, basePos, 300f, tile);
        TrySpawnRoad(x,     y - 1, basePos, 240f,  tile);
    }
    else // odd row
    {
        TrySpawnRoad(x + 1, y,     basePos, 180f,   tile);
        TrySpawnRoad(x + 1, y + 1, basePos, 120f, tile);
        TrySpawnRoad(x,     y + 1, basePos, 60f, tile);
        TrySpawnRoad(x - 1, y,     basePos, 0f, tile);
        TrySpawnRoad(x,     y - 1, basePos, 300f, tile);
        TrySpawnRoad(x + 1, y - 1, basePos, 240f,  tile);
    }
    // ---- ROAD CONNECTOR LOGIC ----
List<int> connectedDirs = new List<int>();

if (y % 2 == 0) // even row
{
    if (HasRoad(x + 1, y))     connectedDirs.Add(0);
    if (HasRoad(x, y + 1))     connectedDirs.Add(1);
    if (HasRoad(x - 1, y + 1)) connectedDirs.Add(2);
    if (HasRoad(x - 1, y))     connectedDirs.Add(3);
    if (HasRoad(x - 1, y - 1)) connectedDirs.Add(4);
    if (HasRoad(x, y - 1))     connectedDirs.Add(5);
}
else // odd row
{
    if (HasRoad(x + 1, y))     connectedDirs.Add(0);
    if (HasRoad(x + 1, y + 1)) connectedDirs.Add(1);
    if (HasRoad(x, y + 1))     connectedDirs.Add(2);
    if (HasRoad(x - 1, y))     connectedDirs.Add(3);
    if (HasRoad(x, y - 1))     connectedDirs.Add(4);
    if (HasRoad(x + 1, y - 1)) connectedDirs.Add(5);
}

// only corners (exactly two connections, not opposite)
if (connectedDirs.Count == 2)
{
    int d1 = connectedDirs[0];
    int d2 = connectedDirs[1];

    if (!AreOpposite(d1, d2)|| connectedDirs.Count > 2)
    {
        GameObject connectorPrefab =
            (tile.tileType == "River")
            ? BridgeConnectorPrefab
            : null;

        float rot = GetCornerRotation(d1, d2);
        if(connectorPrefab != null){
        GameObject c = Instantiate(
            connectorPrefab,
            basePos + new Vector3(0f, 0, 0f),
            Quaternion.Euler(0, rot, 0)
        );
        c.tag = "Destroyable";
                c.tag = "Roads";

        }
    }
}
    if (connectedDirs.Count > 2)
    {
        GameObject connectorPrefab =
            (tile.tileType == "River")
            ? BridgeConnectorPrefab
            : null;

        if(connectorPrefab != null){
        GameObject c = Instantiate(
            connectorPrefab,
            basePos + new Vector3(0f, 0, 0f),
            Quaternion.Euler(0, 0, 0)
        );
        c.tag = "Destroyable";
                        c.tag = "Roads";

        }
    }

}
void TrySpawnRoad(int nx, int ny, Vector3 basePos, float rot, Tile tile)
{
    if (!HasRoad(nx, ny)) return;

    Tile neighbor = currentMap.tiles[nx, ny];

    // choose bridge vs road
    GameObject prefab =
        (tile.tileType == "River")
        ? BridgePrefab
        : roadPrefab; // ← edge road piece, NOT full tile road

    SpawnRoadEdge(prefab, basePos, rot);
}
float GetCornerRotation(int d1, int d2)
{
    if ((d1 == 0 && d2 == 1) || (d1 == 1 && d2 == 0)) return 0f;
    if ((d1 == 1 && d2 == 2) || (d1 == 2 && d2 == 1)) return 300f;
    if ((d1 == 2 && d2 == 3) || (d1 == 3 && d2 == 2)) return 240f;
    if ((d1 == 3 && d2 == 4) || (d1 == 4 && d2 == 3)) return 180f;
    if ((d1 == 4 && d2 == 5) || (d1 == 5 && d2 == 4)) return 120f;
    if ((d1 == 5 && d2 == 0) || (d1 == 0 && d2 == 5)) return 60f;

    return 0f;
}

bool HasRoad(int nx, int ny)
{
    if (nx < 0 || ny < 0 || nx >= currentMap.width || ny >= currentMap.height)
        return false;

    Tile t = currentMap.tiles[nx, ny];
    if (t == null) return false;

    return t.hasRoad||t.hasTradeRoute;
}
void SpawnRoadEdge(GameObject prefab, Vector3 basePos, float rot)
{

   // Vector3 spawnPos = basePos + new Vector3(0f, 0.26f, 0f);

    GameObject r = Instantiate(prefab, basePos, Quaternion.Euler(0, rot, 0));
    r.tag = "Destroyable";
        r.tag = "Roads";

}
private void SpawnTrainTracks(Tile tile, int x, int y)
{
    if (!tile.hasTrainTrack) return;

    Vector3 basePos = CalculateHexPosition(x, y) + new Vector3(0f, 0f, 0f); // raised

    bool isEven = (y % 2 == 0);

    // Each direction gets its own prefab if adjacent tile has a train track
    // Direction order: Right, TopRight, TopLeft, Left, BottomLeft, BottomRight
    Tile[] neighbors = new Tile[6];
    List<Vector2Int> neighborPositions = GetHexNeighborsPositions(x, y);
    for (int i = 0; i < neighborPositions.Count; i++)
    {
        Vector2Int pos = neighborPositions[i];
        if (pos.x >= 0 && pos.x < currentMap.width && pos.y >= 0 && pos.y < currentMap.height)
            neighbors[i] = currentMap.tiles[pos.x, pos.y];
        else
            neighbors[i] = null;
    }

    // Right
    if (neighbors[0] != null && neighbors[0].hasTrainTrack&&tile.tileType != "River")
        Instantiate(trainTrackRightPrefab, basePos, Quaternion.identity).tag = "TrainTracks";

    // TopRight
    if (neighbors[1] != null && neighbors[1].hasTrainTrack&&tile.tileType != "River")
        Instantiate(trainTrackTopRightPrefab, basePos, Quaternion.identity).tag = "TrainTracks";

    // TopLeft
    if (neighbors[2] != null && neighbors[2].hasTrainTrack&&tile.tileType != "River")
        Instantiate(trainTrackTopLeftPrefab, basePos, Quaternion.identity).tag = "TrainTracks";

    // Left
    if (neighbors[3] != null && neighbors[3].hasTrainTrack&&tile.tileType != "River")
        Instantiate(trainTrackLeftPrefab, basePos, Quaternion.identity).tag = "TrainTracks";

    // BottomLeft
    if (neighbors[4] != null && neighbors[4].hasTrainTrack&&tile.tileType != "River")
        Instantiate(trainTrackBottomLeftPrefab, basePos, Quaternion.identity).tag = "TrainTracks";

    // BottomRight
    if (neighbors[5] != null && neighbors[5].hasTrainTrack&&tile.tileType != "River")
        Instantiate(trainTrackBottomRightPrefab, basePos, Quaternion.identity).tag = "TrainTracks";

    // Bridges: only spawn if tile is a river
    if (tile.tileType == "River")
    {
        if (neighbors[0] != null && neighbors[0].hasTrainTrack)
            Instantiate(trainTrackBridgeRightPrefab, basePos, Quaternion.identity).tag = "TrainTracks";
        if (neighbors[1] != null && neighbors[1].hasTrainTrack)
            Instantiate(trainTrackBridgeTopRightPrefab, basePos, Quaternion.identity).tag = "TrainTracks";
        if (neighbors[2] != null && neighbors[2].hasTrainTrack)
            Instantiate(trainTrackBridgeTopLeftPrefab, basePos, Quaternion.identity).tag = "TrainTracks";
        if (neighbors[3] != null && neighbors[3].hasTrainTrack)
            Instantiate(trainTrackBridgeLeftPrefab, basePos, Quaternion.identity).tag = "TrainTracks";
        if (neighbors[4] != null && neighbors[4].hasTrainTrack)
            Instantiate(trainTrackBridgeBottomLeftPrefab, basePos, Quaternion.identity).tag = "TrainTracks";
        if (neighbors[5] != null && neighbors[5].hasTrainTrack)
            Instantiate(trainTrackBridgeBottomRightPrefab, basePos, Quaternion.identity).tag = "TrainTracks";
    }
    List<int> connectedDirs = new List<int>();

for (int i = 0; i < 6; i++)
{
    if (neighbors[i] != null && neighbors[i].hasTrainTrack)
        connectedDirs.Add(i);
        if (connectedDirs.Count == 2)
{
    int d1 = connectedDirs[0];
    int d2 = connectedDirs[1];

    if (!AreOpposite(d1, d2))
    {
        // CORNER CASE
        GameObject cornerPrefab = null;
        float rot = 0f;

        if (tile.tileType == "River")
        {
            cornerPrefab = trainTrackBridgeConnectorPrefab;
        }
        else
        {
            cornerPrefab = turnStilePrefab;
        }



        GameObject corner = Instantiate(cornerPrefab, basePos, Quaternion.Euler(0, rot, 0));
        corner.tag = "TrainTracks";
    }
}

}

}
bool AreOpposite(int a, int b)
{
    return (a == 0 && b == 3) ||
           (a == 3 && b == 0) ||
           (a == 1 && b == 4) ||
           (a == 4 && b == 1) ||
           (a == 2 && b == 5) ||
           (a == 5 && b == 2);
}

// Utility: returns neighbor positions in hex order
private List<Vector2Int> GetHexNeighborsPositions(int x, int y)
{
    bool odd = (y % 2 == 1);
    int[,] dirsEven =
    {
        { +1,  0 },
        {  0, +1 },
        { -1, +1 },
        { -1,  0 },
        { -1, -1 },
        {  0, -1 }
    };

    int[,] dirsOdd =
    {
        { +1,  0 },
        { +1, +1 },
        {  0, +1 },
        { -1,  0 },
        {  0, -1 },
        { +1, -1 }
    };

    int[,] dirs = odd ? dirsOdd : dirsEven;
    List<Vector2Int> neighbors = new List<Vector2Int>();
    for (int i = 0; i < 6; i++)
        neighbors.Add(new Vector2Int(x + dirs[i, 0], y + dirs[i, 1]));

    return neighbors;
}
public void SpawnTradeRoutes(Tile tile, int x, int y)
{
    if (!tile.hasTradeRoute) return;

    // trade routes ONLY on water
    if (tile.tileType != "Ocean" && tile.tileType != "Coast" && tile.tileType != "River")
        return;

    Vector3 basePos = CalculateHexPosition(x, y);

    if (y % 2 == 0) // even row
    {
        TrySpawnTradeRoute(x + 1, y,     basePos, 180f);
        TrySpawnTradeRoute(x,     y + 1, basePos, 120f);
        TrySpawnTradeRoute(x - 1, y + 1, basePos, 60f);
        TrySpawnTradeRoute(x - 1, y,     basePos, 0f);
        TrySpawnTradeRoute(x - 1, y - 1, basePos, 300f);
        TrySpawnTradeRoute(x,     y - 1, basePos, 240f);
    }
    else // odd row
    {
        TrySpawnTradeRoute(x + 1, y,     basePos, 180f);
        TrySpawnTradeRoute(x + 1, y + 1, basePos, 120f);
        TrySpawnTradeRoute(x,     y + 1, basePos, 60f);
        TrySpawnTradeRoute(x - 1, y,     basePos, 0f);
        TrySpawnTradeRoute(x,     y - 1, basePos, 300f);
        TrySpawnTradeRoute(x + 1, y - 1, basePos, 240f);
    }
}void TrySpawnTradeRoute(int nx, int ny, Vector3 basePos, float rot)
{
    if (!InBounds(nx, ny)) return;

    Tile neighbor = currentMap.tiles[nx, ny];
    if (neighbor == null) return;

    // must connect to road OR trade route
    if (!neighbor.hasRoad && !neighbor.hasTradeRoute) return;

    // neighbor must be water OR land-road (harbour logic)
    if (!IsWater(neighbor) && !neighbor.hasRoad) return;

    SpawnTradeRouteEdge(basePos, rot);
}
void SpawnTradeRouteEdge(Vector3 basePos, float rot)
{
    GameObject t = Instantiate(
        tradeRoutePrefab,
        basePos,
        Quaternion.Euler(0, rot, 0)
    );
    t.tag = "TradeRoutes";
}
bool IsWater(Tile t)
{
    return t.tileType == "Ocean" || t.tileType == "Coast" || t.tileType == "River";
}

bool InBounds(int x, int y)
{
    return x >= 0 && y >= 0 && x < currentMap.width && y < currentMap.height;
}
    public void SpawnHexColliders()
    {
        clickHexes = new ClickHex[currentMap.width, currentMap.height];

        for (int y = 0; y < currentMap.height; y++)
        {
            for (int x = 0; x < currentMap.width; x++)
            {
                // Calculate world position of the hex
                float xOffset = (y % 2 == 0) ? 0 : hexWidth / 2f;
                Vector3 worldPos = new Vector3(x * hexWidth + xOffset, 0f, y * hexHeight * 0.75f);

                // Spawn invisible collider
                GameObject go = Instantiate(colliderPrefab, worldPos, Quaternion.identity);
                go.name = $"HexCollider_{x}_{y}";

                // Ensure collider exists
                if (go.GetComponent<Collider>() == null)
                {
                    BoxCollider box = go.AddComponent<BoxCollider>();
                    box.size = new Vector3(hexWidth, 0.1f, hexHeight); // flat thin collider
                }

                // Attach ClickHex script to store grid coordinates
                ClickHex ch = go.AddComponent<ClickHex>();
                ch.x = x;
                ch.y = y;

                clickHexes[x, y] = ch;

                // Optional: make invisible
                MeshRenderer mr = go.GetComponent<MeshRenderer>();
                if (mr != null) mr.enabled = false;
            }
        }
    }
        public (int x, int y)? GetClickedHexCoordinates()
    {
        if (!Input.GetMouseButtonDown(0)) return null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ClickHex ch = hit.collider.GetComponent<ClickHex>();
            if (ch != null)
            {
                return (ch.x, ch.y); // return grid coordinates
            }
        }

        return null; // nothing clicked
    }
    public void menu(){
        menuPanel.SetActive(true);
        ReturnBtn.SetActive(true);
    }
public void stats()
{
    statsPanel.SetActive(true);
    ReturnBtn.SetActive(true);

    string difficultyName = aiDifficulty switch
    {
        1 => "Easy",
        2 => "Normal",
        3 => "Hard",
        4 => "Expert",
        5 => "Extreme",
        _ => "Unknown"
    };

    textParent.transform.GetChild(16).GetComponent<TextMeshProUGUI>().text = "Difficulty: " + difficultyName;

    GameObject bigTextTemplate = textParent.transform.GetChild(9).gameObject;  // Big text for tribe
    GameObject smallTextTemplate = textParent.transform.GetChild(10).gameObject; // Small text for player/AI


    // Loop through all players
    for (int i = 0; i < currentMap.players.Length; i++)
    {
        Player p = currentMap.players[i];

        // Clone big text and small text
        GameObject bigTextClone = Instantiate(bigTextTemplate, textParent.transform);
        GameObject smallTextClone = Instantiate(smallTextTemplate, textParent.transform);

        // Set their text
        bigTextClone.GetComponent<TextMeshProUGUI>().text = p.tribeType; // Tribe name
        smallTextClone.GetComponent<TextMeshProUGUI>().text = "Player "  + (i + 1) + (p.isPlayer ? "" : " (AI)");

        // Optional: move clones below each other
        bigTextClone.transform.localPosition -= new Vector3(0, i * 100, 0);  // adjust spacing as needed
        smallTextClone.transform.localPosition -= new Vector3(0, i * 100, 0); 
    }

    // -------- POPULATION --------
    int pop = 0;

    for (int x = 0; x < currentMap.width; x++)
    {
        for (int y = 0; y < currentMap.height; y++)
        {
            Tile tile = currentMap.tiles[x, y];

            if (tile.owner == currentPlayer && tile.district != null && tile.district is City)
            {
                pop += tile.district.returnLevel(x, y, currentMap);
            }
        }
    }

    textParent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
        "Get 15 city levels: " + pop + "/10";


    // -------- CONNECTED --------
    int connected = 0;

    for (int x = 0; x < currentMap.width; x++)
    {
        for (int y = 0; y < currentMap.height; y++)
        {
            Tile tile = currentMap.tiles[x, y];

            if (tile.owner == currentPlayer && tile.district != null && tile.district is City)
            {
                if (currentMap.IsConnectedToCapitalByRoad(tile, currentPlayer))
                    connected++;

                if (currentMap.IsConnectedToCapitalByTrain(tile, currentPlayer))
                    connected++;
            }
        }
    }

    textParent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
        "Connect 10 citys to your capital: " + connected + "/10";


    // -------- EXPLORED TILES --------
    int explored = 0;

    for (int x = 0; x < currentMap.width; x++)
    {
        for (int y = 0; y < currentMap.height; y++)
        {
            Tile tile = currentMap.tiles[x, y];

            if (currentPlayer.exploredTiles[x, y])   // FIXED
            {
                explored++;
            }
        }
    }

    textParent.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
        "Explore "+ currentMap.tiles.Length+ " tiles: "+ explored + "/" + currentMap.tiles.Length;
}
public void checkIfPopMonument(){
        if(currentMap.loadingScene || isAITurn || currentPlayer.popWonderUnlocked)
    return;
     currentPlayer.popWonderUnlocked = true;
    int pop = 0;

    for (int x = 0; x < currentMap.width; x++)
    {
        for (int y = 0; y < currentMap.height; y++)
        {
            Tile tile = currentMap.tiles[x, y];

            if (tile.owner == currentPlayer && tile.district != null && tile.district is City)
            {
                pop += tile.district.returnLevel(x, y, currentMap);
            }
        }
    }
    if(pop >= 15){
    TribeDestroyedPanel.SetActive(true);
    TribeDestroyedPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "You can now build the population monument!";

    TribeDestroyedPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Glory!";
        Button closeButton = TribeDestroyedPanel.transform.GetChild(4).GetComponent<Button>();

    // Add a listener to close the panel
    closeButton.onClick.AddListener(() =>
    {
        TribeDestroyedPanel.SetActive(false);
    });
    }
}
public void checkIfTradeMonument(){
        if(currentMap.loadingScene || isAITurn|| currentPlayer.tradeWonderUnlocked)
    return;
         currentPlayer.tradeWonderUnlocked = true;
    int connected = 0;

    for (int x = 0; x < currentMap.width; x++)
    {
        for (int y = 0; y < currentMap.height; y++)
        {
            Tile tile = currentMap.tiles[x, y];

            if (tile.owner == currentPlayer && tile.district != null && tile.district is City)
            {
                if (currentMap.IsConnectedToCapitalByRoad(tile, currentPlayer))
                    connected++;

                if (currentMap.IsConnectedToCapitalByTrain(tile, currentPlayer))
                    connected++;
            }
        }
    }
    if(connected >= 10){
    TribeDestroyedPanel.SetActive(true);
    TribeDestroyedPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "You can now build the trade monument!";

    TribeDestroyedPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Glory!";
        Button closeButton = TribeDestroyedPanel.transform.GetChild(4).GetComponent<Button>();

    // Add a listener to close the panel
    closeButton.onClick.AddListener(() =>
    {
        TribeDestroyedPanel.SetActive(false);
    });
    }

}
    public void checkIfExpMonument()
    {
        if(currentMap.loadingScene || isAITurn || currentPlayer.expWonderUnlocked)
            return;
        
        currentPlayer.expWonderUnlocked = true;

        int explored = 0;

        for (int x = 0; x < currentMap.width; x++)
        {
            for (int y = 0; y < currentMap.height; y++)
            {
                Tile tile = currentMap.tiles[x, y];

                if (currentPlayer.exploredTiles[x, y])
                {
                    explored++;
                }
            }
        }
        if(explored == currentMap.tiles.Length)
        {
            TribeDestroyedPanel.SetActive(true);
            TribeDestroyedPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "You can now build the explorer monument!";

            TribeDestroyedPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Glory!";
            Button closeButton = TribeDestroyedPanel.transform.GetChild(4).GetComponent<Button>();

            closeButton.onClick.AddListener(() =>
            {
                TribeDestroyedPanel.SetActive(false);
            });

        }
    }
}