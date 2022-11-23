using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayInfo : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] GameObject infoDialog;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image vsShieldImage;
    [SerializeField] Image vsHullImage;
    [SerializeField] TextMeshProUGUI rangeText;
    [SerializeField] TextMeshProUGUI precisionText;
    [SerializeField] GameObject cover;

    BuildingSO building;
    BuildManager buildManagerScript;

    void Awake()
    {
        building = GetComponent<BuildingDisplayer>().building;
        buildManagerScript = GameObject.FindGameObjectWithTag("BuildManager").GetComponent<BuildManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cover.SetActive(false);

        SetInformation();
        DisplayStatBarSprites();

        if (!buildManagerScript.IsInfoDialogActive)
        {
            buildManagerScript.IsInfoDialogActive = true;
        }
    }

    void SetInformation()
    {
        title.text = building.name;
        description.text = building.description;
        rangeText.text = building.range;
        precisionText.text = building.precision;
    }

    void DisplayStatBarSprites()
    {
        Sprite vsShield = Resources.Load<Sprite>($"Sprites/Icons/stat_{building.vsShield}");
        vsShieldImage.sprite = vsShield;

        Sprite vsHull = Resources.Load<Sprite>($"Sprites/Icons/stat_{building.vsHull}");
        vsHullImage.sprite = vsHull;
    }
}
