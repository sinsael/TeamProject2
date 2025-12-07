using UnityEngine;
using UnityEngine.UI;

public class Interaction_altar : Interaction_Obj
{
    [Header("UI")]
    [SerializeField] private GameObject altarUI;
    [SerializeField] private Button closeButton;
    [SerializeField] private bool pauseGame = true;

    private bool isOpen;

    private PlayerInputHandler[] lockedInputs;
    private bool[] lockedPrevEnabled;

    public override void Start()
    {
        base.Start();

        if (closeButton != null)
            closeButton.onClick.AddListener(Close);

        if (altarUI != null)
            altarUI.SetActive(false);

        isOpen = false;
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

        LockAllPlayers(true);

        Inventory.Instance.DeselectCurrent();

        AltarUIManager altarMgr = altarUI.GetComponentInChildren<AltarUIManager>(true);

        altarMgr.DeselectCurrent();
    }

    public void Close()
    {
        isOpen = false;

        altarUI.SetActive(false);
        LockAllPlayers(false);
        Inventory.Instance.DeselectCurrent();
    }

    private void LockAllPlayers(bool locked)
    {
        if (locked)
        {
            lockedInputs = FindObjectsOfType<PlayerInputHandler>(true);
            lockedPrevEnabled = new bool[lockedInputs.Length];

            for (int i = 0; i < lockedInputs.Length; i++)
            {
                lockedPrevEnabled[i] = lockedInputs[i].enabled;
                lockedInputs[i].enabled = false;

                Rigidbody2D rb = lockedInputs[i].GetComponentInParent<Rigidbody2D>();
                if (rb != null)
                    rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            if (lockedInputs == null || lockedPrevEnabled == null)
                return;

            for (int i = 0; i < lockedInputs.Length; i++)
                lockedInputs[i].enabled = lockedPrevEnabled[i];
        }
    }
}
