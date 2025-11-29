using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject BookUI;
    public GameObject FadeUI;
    public bool IsUI;

    public void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetActiveBookUI()
    {
        BookUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void FalseFade()
    {
        FadeUI.SetActive(false);
    }
}
