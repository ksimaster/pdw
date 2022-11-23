using UnityEngine;

public class MenuInstance : MonoBehaviour
{
    public static MenuInstance instance;

    void Awake()
    {
        SingletonMe();
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
