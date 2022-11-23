using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSlotGroup : MonoBehaviour
{
    // Buildings prefabs
    [SerializeField] GameObject machineGunTurret;
    [SerializeField] GameObject blasterTurret;
    [SerializeField] GameObject powerGenerator;
    [SerializeField] GameObject shieldDome;
    [SerializeField] GameObject missileTurret;
    [SerializeField] GameObject laserTurret;
    [SerializeField] GameObject scrapTurret;

    [SerializeField] GameObject fake_shieldDome;
    [SerializeField] GameObject iconPrefab;
    [SerializeField] TextMeshProUGUI capacity;
    [SerializeField] Image deleteButton;
    [SerializeField] Sprite deleteButtonSprite;
    [SerializeField] Sprite arrowSprite;

    //SFXs
    [SerializeField] AudioClip delete_button;
    [SerializeField] AudioClip remove_building;

    BuildManager buildManagerScript;
    SFXManager sm;

    GameObject subMenu;
    GameObject addButton;

    Transform buildingsParent;
    Transform buildingIconsParent;
    Transform shieldDomeIconPlace;

    List<GameObject> buildings = new List<GameObject>();
    List<GameObject> icons = new List<GameObject>();

    public int BuildingsCount { get => buildings.Count; }
    int buildingsLimit = 5;

    bool isShieldDomeBuilt = false;
    public bool IsShieldDomeBuilt { get => isShieldDomeBuilt; set => isShieldDomeBuilt = value; }

    bool isDeleteModeActive = false;
    public bool IsDeleteModeActive { set => isDeleteModeActive = value; }

    void Awake()
    {
        buildManagerScript = GameObject.FindGameObjectWithTag("BuildManager").GetComponent<BuildManager>();
        subMenu = transform.Find("SubMenu").gameObject;
        addButton = subMenu.transform.Find("Add Button").gameObject;
        buildingsParent = transform.Find("Building Objects").transform;
        buildingIconsParent = transform.Find("Building Icons").transform;
        shieldDomeIconPlace = transform.Find("Shield Dome Place").transform;
        sm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
    }

    void Start()
    {
        addButton.SetActive(false);
    }

    void Update()
    {
        ChangeAddButtonState();
        DisplayBuildingCapacity();
        ChangeDeleteButtonState();
    }

    void ChangeAddButtonState()
    {
        if (BuildingsCount >= buildingsLimit && isShieldDomeBuilt)
        {
            addButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            addButton.GetComponent<Button>().interactable = true;
        }
        
        subMenu.SetActive(buildManagerScript.IsUIActive);
        addButton.SetActive(buildManagerScript.IsUIActive);
    }

    void DisplayBuildingCapacity()
    {
        capacity.text = $"{BuildingsCount}/{buildingsLimit}";
    }

    public void CreateBuilding(BuildingSO building)
    {
        if (building.type == BuildingType.shieldDome)
        {
            InstantiateShieldDome(building);
        }
        else
        {
            InstantiateBuilding(building);
            InstantiateIcon(building);
        }
    }

    void InstantiateBuilding(BuildingSO building)
    {
        GameObject newBuilding = Instantiate(GetBuildingPrefab(building.type), buildingsParent.position, transform.rotation, buildingsParent);

        buildings.Add(newBuilding);
    }

    void InstantiateIcon(BuildingSO building)
    {
        GameObject newIcon = Instantiate(iconPrefab, buildingIconsParent.position, transform.rotation, buildingIconsParent);

        // Catch buildingIcon script
        BuildingIcon buildIconScript = newIcon.GetComponent<BuildingIcon>();

        // Give a icon slot number and buildings Parent
        buildIconScript.buildingsParent = buildingsParent;
        buildIconScript.ISlot = buildingIconsParent.childCount - 1;

        // Setting icon image
        Image iconImage = newIcon.GetComponent<Image>();
        iconImage.sprite = building.icon;

        icons.Add(newIcon);
    }

    void InstantiateShieldDome(BuildingSO building)
    {
        // Instantiate icon
        GameObject shieldDomeIcon = Instantiate(iconPrefab, shieldDomeIconPlace.position, shieldDomeIconPlace.rotation, shieldDomeIconPlace);

        BuildingIcon buildIconScript = shieldDomeIcon.GetComponent<BuildingIcon>();
        buildIconScript.buildingsParent = transform;
        buildIconScript.ISlot = 7;

        Image iconImage = shieldDomeIcon.GetComponent<Image>();
        iconImage.sprite = building.icon;

        // Instantiate building
        Instantiate(shieldDome, transform.position, transform.rotation, transform);
    }

    void ChangeDeleteButtonState()
    {
        if (icons.Count < 1)
        {
            deleteButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            deleteButton.GetComponent<Button>().interactable = true;
        }
    }

    public void SwitchDeleteModeState()
    {
        isDeleteModeActive = !isDeleteModeActive;
        DisplayDeleteButtonIcon();
        SwitchDeleteIconsState(isDeleteModeActive);

        sm.PlayOneShot(delete_button);
    }

    public void DisplayDeleteButtonIcon()
    {
        deleteButton.sprite = isDeleteModeActive ? arrowSprite : deleteButtonSprite;
    }

    public void SwitchDeleteIconsState(bool value)
    {
        foreach (GameObject icon in icons)
        {
            icon.transform.Find("Delete Icon").gameObject.SetActive(value);
        }
    }

    public void RemoveBuildingIconAndObject(int iSlotIndex)
    {
        DestroyBuilding(iSlotIndex);
        DestroyIcon(iSlotIndex);

        sm.PlayOneShot(remove_building);
    }

    void DestroyBuilding(int index)
    {
       if (buildings[index].name == "Power Generator(Clone)")
        {
            PowerGenerator powerGenerator = buildings[index].GetComponent<PowerGenerator>();

            if (powerGenerator.BonusGiven)
            {
                buildManagerScript.ReduceEnergyProduction(powerGenerator.ProductionIncrease);
            }
        }
        else if (buildings[index].name == "Machine Gun Turret(Clone)")
        {
            MachineGunTurret mgTurret = buildings[index].GetComponent<MachineGunTurret>();
            mgTurret.RevomeTurretFromList();
        }
        else if (buildings[index].name == "Laser Turret(Clone)")
        {
            LaserTurret lTurret = buildings[index].GetComponent<LaserTurret>();
        }

        Destroy(buildings[index].gameObject);
        buildings.RemoveAt(index);
    }

    void DestroyIcon(int index)
    {
        Destroy(icons[index].gameObject);
        icons.RemoveAt(index);

        for (int i = index; i < icons.Count; i++)
        {
            icons[i].GetComponent<BuildingIcon>().ISlot--;
        }
    }

    GameObject GetBuildingPrefab(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.powerGenerator:
                return powerGenerator;
            case BuildingType.machineGunTurret:
                return machineGunTurret;
            case BuildingType.blasterTurret:
                return blasterTurret;
            case BuildingType.shieldDome:
                return shieldDome;
            case BuildingType.missileTurret:
                return missileTurret;
            case BuildingType.laserTurret:
                return laserTurret;
            case BuildingType.scrapTurret:
                return scrapTurret;
            default:
                return null;
        }
    }
}
