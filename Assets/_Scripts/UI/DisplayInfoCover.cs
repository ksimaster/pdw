using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayInfoCover : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] GameObject infoDialog;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
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
        cover.SetActive(true);

        SetInformation();

        if (!buildManagerScript.IsInfoDialogActive)
        {
            buildManagerScript.IsInfoDialogActive = true;
        }
    }

    void SetInformation()
    {
        title.text = building.name;
        description.text = building.description;
    }
}
