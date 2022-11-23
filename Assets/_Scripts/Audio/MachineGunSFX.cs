using System.Collections.Generic;
using UnityEngine;

public class MachineGunSFX : MonoBehaviour
{
    public static MachineGunSFX instance;

    [SerializeField] AudioClip machineGun_SFX;

    public AudioSource audioSource;

    bool isActive = false;

    int mg_index = 0;
    public int MG_index { get => mg_index; set => mg_index = value; }

    Dictionary<int, bool> turrets = new Dictionary<int, bool>() { }; 
    public Dictionary<int, bool> Turrets { get => turrets; set => turrets = value; }

    void Awake()
    {
        SingletonMe();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        PlayMachineGunSFX();
    }

    void PlayMachineGunSFX()
    {
        if (audioSource.enabled == false) return;

        if (!isActive && IsSomeMachineGunShooting())
        {
            audioSource.clip = machineGun_SFX;
            audioSource.Play();
            isActive = true;
        }
        else if (!IsSomeMachineGunShooting())
        {
            isActive = false;
            audioSource.Stop();
        }
    }

    bool IsSomeMachineGunShooting()
    {
        bool result = false;

        foreach (var item in turrets)
        {
            if (item.Value)
            {
                result = true;
            }
        }

        return result;
    }

    void SingletonMe()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
