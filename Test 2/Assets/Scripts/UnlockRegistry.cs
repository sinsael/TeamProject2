using System;
using UnityEngine;

public static class UnlockRegistry
{
    public static bool BookPileUnlocked { get; private set; }

    public static event Action OnBookPileUnlocked;

    public static void ResetAll()
    {
        BookPileUnlocked = false;
    }

    public static void UnlockBookPile()
    {
        if (BookPileUnlocked) return;

        BookPileUnlocked = true;
        if (OnBookPileUnlocked != null) OnBookPileUnlocked();
    }
}
