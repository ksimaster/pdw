using UnityEngine;
using UnityEngine.UI;

public class BuildingIcon : MonoBehaviour
{
    public Transform buildingsParent;

    Building buildingScript;
    BuildingSlotGroup buildingSlotGroupScript;
    
    Image image;
    Slider slider;

    int iSlot;
    public int ISlot { get => iSlot; set => iSlot = value; }

    float buildTime = 5f;
    float sliderTime = 0f;

    void Awake()
    {
        image = GetComponent<Image>();
        slider = GetComponentInChildren<Slider>();
        buildingSlotGroupScript = transform.parent.GetComponentInParent<BuildingSlotGroup>();
    }

    void Start()
    {
        RestartSliderTime();
        AlphaColorState(0.25f);
    }

    void Update()
    {
        SliderProgress();
    }

    void SliderProgress()
    {
        if (sliderTime < buildTime)
        {
            sliderTime += Time.deltaTime;

            slider.value = sliderTime / buildTime;
        }
        else
        {
            RestartSliderTime();

            slider.gameObject.SetActive(false);
            AlphaColorState(1);

            if (iSlot == 7)
            {
                buildingScript = buildingsParent.Find("Shield Dome(Clone)").GetComponent<Building>();
                buildingScript.IsBuildingActive = true;
            }
            else
            {
                buildingScript = buildingsParent.GetChild(iSlot).GetComponent<Building>();
                buildingScript.IsBuildingActive = true;
            }
        }
    }

    void RestartSliderTime()
    {
        sliderTime = 0;
    }

    void AlphaColorState(float value)
    {
        Color tempColor = image.color;
        tempColor.a = value;
        image.color = tempColor;
    }

    public void RemoveBuildingAtIndex()
    {
        buildingSlotGroupScript.SwitchDeleteModeState();
        buildingSlotGroupScript.RemoveBuildingIconAndObject(ISlot);
    }
}
