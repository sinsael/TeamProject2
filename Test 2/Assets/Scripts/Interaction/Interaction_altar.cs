using UnityEngine;
using UnityEngine.UI;

public class Interaction_altar : Interaction_Obj
{
    [Header("UI")]
    [SerializeField] private GameObject altarUI;
    [SerializeField] private Button closeButton;
    [SerializeField] private bool pauseGame = true;

    private float prevTimeScale = 1f;
    private bool isOpen = false;

    public override void Start()
    {
        base.Start();

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close);
        }

        if (altarUI != null && altarUI.activeSelf)
        {
            isOpen = true;
        }
    }

    private void Update()
    {
        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public override void OnInteract(PlayerInputHandler playerInput)
    {
        if (altarUI == null) return;

        if (isOpen)
        {
            Close();
            return;
        }

        Open();
    }

    private void Open()
    {
        isOpen = true;
        altarUI.SetActive(true);

        if (pauseGame)
        {
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        // UI yeol ttae seontaek sang tae jeong ri
        if (Inventory.Instance != null)
        {
            Inventory.Instance.DeselectCurrent();
        }

        AltarUIManager altarMgr = altarUI.GetComponentInChildren<AltarUIManager>(true);
        if (altarMgr != null)
        {
            altarMgr.DeselectCurrent();
        }
    }

    public void Close()
    {
        if (altarUI == null) return;

        isOpen = false;
        altarUI.SetActive(false);

        if (pauseGame)
        {
            Time.timeScale = prevTimeScale;
        }

        if (Inventory.Instance != null)
        {
            Inventory.Instance.DeselectCurrent();
        }
    }
}
