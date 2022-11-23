//using System.Collections.Generic;
//using UnityEngine;

//public class LaserTurretSFX : MonoBehaviour
//{
//    public static LaserTurretSFX instance;

//    [SerializeField] AudioClip laserTurret_SFX;

//    public AudioSource audioSource;

//    bool isActive = false;
//    public bool IsActive { get => isActive; }

//    int l_index = 0;
//    public int L_index { get => l_index; set => l_index = value; }

//    Dictionary<int, bool> turrets = new Dictionary<int, bool>() { };
//    public Dictionary<int, bool> Turrets { get => turrets; set => turrets = value; }

//    void Awake()
//    {
//        SingletonMe();

//        audioSource = GetComponent<AudioSource>();
//    }

//    void Update()
//    {
//        PlayLaserSFX();
//    }

//    void PlayLaserSFX()
//    {
//        if (audioSource.enabled == false) return;

//        if (!isActive && IsSomeLaserShooting())
//        {
//            isActive = true;
//            audioSource.clip = laserTurret_SFX;
//            audioSource.Play();
//        }
//        else if (!IsSomeLaserShooting() && isActive)
//        {
//            isActive = false;
//            audioSource.Stop();
//        }
//    }

//    bool IsSomeLaserShooting()
//    {
//        bool result = false;

//        foreach (var item in turrets)
//        {
//            if (item.Value)
//            {
//                result = true;
//            }
//        }

//        return result;
//    }

//    void SingletonMe()
//    {
//        if (instance != null && instance != this)
//        {
//            Destroy(this.gameObject);
//        }
//        else
//        {
//            instance = this;
//            DontDestroyOnLoad(this.gameObject);
//        }
//    }
//}
