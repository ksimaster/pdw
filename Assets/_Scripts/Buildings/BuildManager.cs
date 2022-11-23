using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class BuildManager : MonoBehaviour
{
    [SerializeField] RectTransform overlay;
    [SerializeField] GameObject buildMenu;
    [SerializeField] GameObject infoDialog;
    [SerializeField] Transform buildMenuButtons;
    [SerializeField] GameObject shieldDomeButton;
    [SerializeField] TextMeshProUGUI totalEnergyText;
    [SerializeField] TextMeshProUGUI energyProductionText;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI powerGeneratorText;
    [SerializeField] GameObject[] buildingsSlotGroups;

    // SFXs
    [SerializeField] AudioClip slow_time_on;
    [SerializeField] AudioClip slow_time_off;
    [SerializeField] AudioClip add_button;
    [SerializeField] AudioClip buy_building;

    [Header("Settings")]
    [SerializeField] int initialEnergy = 1200;
    [SerializeField] int energyIncrease = 10;
    [SerializeField] float productionTime = 10f;

    BuildingSlotGroup currentBuildSlotGroup;
    public BuildingSlotGroup CurrentBuildSlotGroup { get => currentBuildSlotGroup; }

    GameManager gameManagerScript;
    SFXManager sm;

    float slowTime = 0.05f;
    float slowTimeRecovery = 2f;

    bool isUIActive = false;
    public bool IsUIActive { get => isUIActive; }

    bool isBuildMenuActive = false;
    public bool IsBuildMenuActive { get => isBuildMenuActive; }

    bool isInfoDialogActive = false;
    public bool IsInfoDialogActive { get => isInfoDialogActive; set => isInfoDialogActive = value; }

    int currentEnergy;
    public int Energy { get => currentEnergy; }
    
    float sliderTime = 0f;

    int numOfPowerGenerators = 0;

    int buildGroupIndex;
    int buildingsLimit = 5;

    bool slowOnPlaying = false;
    bool slowOffPlaying = false;

    void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
    }

    void Start()
    {
        currentEnergy = initialEnergy;

        DisplayEnergyValues();
        RestartSliderTime();
        buildMenu.SetActive(false);
        infoDialog.SetActive(false);
    }

    void Update()
    {
        if (!gameManagerScript.MenuOpened
            &&!gameManagerScript.IsGameOver
            && gameManagerScript.IsGameStarted)
        {
            SlowBuildTime();
        }
        ChangeOverlayState();
        DisplayEnergyValues();
        EnergySliderProgress();
        ShieldDomeButtonState();
        ChangeInfoDialogState();
    }

    void SlowBuildTime()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = slowTime;

            if (!slowOnPlaying && !isUIActive) StartCoroutine(PlaySlowOnSFX());

            isUIActive = true;
        }
        else
        {
            if (!slowOffPlaying && isUIActive) StartCoroutine(PlaySlowOffSFX());

            isUIActive = false;

            DisableDeleteBuildingsState();
            
            if (Time.timeScale != 1)
            {
                Time.timeScale += (1f / slowTimeRecovery) * Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            }

            if (isBuildMenuActive)
            {
                ChangeBuildMenuState(false);
            }

            isInfoDialogActive = false;
        }
    }

    void ChangeOverlayState()
    {
        overlay.sizeDelta = isUIActive ? new Vector2(21.5f, 21.5f) : new Vector2(7.5f, 7.5f); 
    }

    public void ChangeBuildMenuState(bool value)
    {
        isBuildMenuActive = value;
        buildMenu.SetActive(value);

        sm.PlayOneShot(add_button);
    }

    void ChangeInfoDialogState()
    {
        infoDialog.SetActive(isInfoDialogActive);
    }

    void DisplayEnergyValues()
    {
        totalEnergyText.text = currentEnergy.ToString();
        energyProductionText.text = $"+{energyIncrease}";
        powerGeneratorText.text = numOfPowerGenerators.ToString();
    }

    void EnergySliderProgress()
    {
        if (sliderTime < productionTime)
        {
            sliderTime += Time.deltaTime;

            slider.value = sliderTime / productionTime;
        }
        else
        {
            IncreaseEnergy();
            RestartSliderTime();
        }
    }

    void RestartSliderTime()
    {
        sliderTime = 0;
    }

    void IncreaseEnergy()
    {
        currentEnergy += energyIncrease;
    }

    public void IncreaseEnergyProduction(int amount)
    {
        energyIncrease += amount;
        numOfPowerGenerators++;
    }

    public void ReduceEnergyProduction(int amount)
    {
        energyIncrease -= amount;
        numOfPowerGenerators--;
    }

    public void BuyBuilding(BuildingSO building)
    {
        currentEnergy -= building.cost;
        DisplayEnergyValues();
        currentBuildSlotGroup.CreateBuilding(building);

        if (building.type == BuildingType.shieldDome)
        {
            currentBuildSlotGroup.IsShieldDomeBuilt = true;
        }

        CheckBuildButtonsState();

        sm.PlayOneShot(buy_building);
    }

    public void SetBuildMenuIndex(int index)
    {
        buildGroupIndex = index;

        CheckBuildButtonsState();
    }

    void CheckBuildButtonsState()
    {
        currentBuildSlotGroup = buildingsSlotGroups[buildGroupIndex].GetComponent<BuildingSlotGroup>();

        if (currentBuildSlotGroup.BuildingsCount >= buildingsLimit)
        {
            foreach (Transform child in buildMenuButtons)
            {
                child.GetComponentInChildren<BuildingDisplayer>().DisableButtons();
            }
        }
        else
        {
            foreach (Transform child in buildMenuButtons)
            {
                child.GetComponentInChildren<BuildingDisplayer>().EnableButtons();
            }
        }
    }

    void ShieldDomeButtonState()
    {
        if (currentBuildSlotGroup == null) return;

        if (currentBuildSlotGroup.IsShieldDomeBuilt)
        {
            shieldDomeButton.GetComponent<Button>().interactable = currentBuildSlotGroup.IsShieldDomeBuilt;
        }
    }

    void DisableDeleteBuildingsState()
    {
        foreach (GameObject buildingSlot in buildingsSlotGroups)
        {
            if (buildingSlot == null) return;

            BuildingSlotGroup buildSlot = buildingSlot.GetComponent<BuildingSlotGroup>();
            buildSlot.IsDeleteModeActive = false;
            buildSlot.DisplayDeleteButtonIcon();
            buildSlot.SwitchDeleteIconsState(false);
        }
    }

    IEnumerator PlaySlowOnSFX()
    {
        slowOnPlaying = true;
        sm.PlayOneShot(slow_time_on);

        yield return new WaitForSecondsRealtime(0.5f);
        slowOnPlaying = false;
    }

    IEnumerator PlaySlowOffSFX()
    {
        slowOffPlaying = true;
        sm.PlayOneShot(slow_time_off);

        yield return new WaitForSecondsRealtime(0.5f);
        slowOffPlaying = false;
    }
}