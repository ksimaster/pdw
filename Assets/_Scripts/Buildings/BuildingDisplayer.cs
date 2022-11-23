using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDisplayer : MonoBehaviour
{
    public BuildingSO building;

    BuildManager buildManagerScript;

    Button button;
    TextMeshProUGUI title;
    Image icon;
    TextMeshProUGUI cost;

    bool isButtonDisabled = false;

    void Awake()
    {
        buildManagerScript = GameObject.FindGameObjectWithTag("BuildManager").GetComponent<BuildManager>();
        button = GetComponent<Button>();
        title = transform.Find("Title").GetComponent<TextMeshProUGUI>();    
        icon = transform.Find("Icon").GetComponent<Image>();    
        cost = transform.Find("Cost").GetComponent<TextMeshProUGUI>();    
    }

    void Start()
    {
        title.text = building.name;
        icon.sprite = building.icon;
        cost.text = building.cost.ToString();
    }

    void Update()
    {
        ButtonState();
    }

    void ButtonState()
    {
        if (building.type == BuildingType.shieldDome)
        {
            button.interactable = (buildManagerScript.Energy >= building.cost) && !buildManagerScript.CurrentBuildSlotGroup.IsShieldDomeBuilt;
        }
        else
        {
            button.interactable = !isButtonDisabled && (buildManagerScript.Energy >= building.cost);
        }
    }

    public void DisableButtons()
    {
        isButtonDisabled = true;
    }

    public void EnableButtons()
    {
        isButtonDisabled = false;
    }
}
