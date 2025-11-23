using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
  public static LeverManager Instance { get; private set; }

   private List<Interaction_Lever> activeLevers = new List<Interaction_Lever>();

    public int maxActiveLevers = 2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterActivation(Interaction_Lever lever)
    {
        if (activeLevers.Contains(lever)) return;

        activeLevers.Add(lever);

        if (activeLevers.Count > maxActiveLevers)
        {
            // 가장 오래된 레버 (리스트의 맨 앞)를 가져옴
            Interaction_Lever oldestLever = activeLevers[0];

            // 리스트에서 제거
            activeLevers.RemoveAt(0);

            // 해당 레버를 강제로 비활성화시킴
            oldestLever.ForceDeactivate();
        }
    }

    public void RegisterDeactivation(Interaction_Lever lever)
    {
        // 리스트에 있다면 제거
        if (activeLevers.Contains(lever))
        {
            activeLevers.Remove(lever);
        }
    }
}
