using System.Collections;
using UnityEngine;

public class Sanity : MonoBehaviour
{
    public Player player;
    private Entity_Stat entityStat;

    public float currentSan;
    public bool isCrazy = false;

    public float Regeninterval = 1f;
    public float Amountinterval = 1f;

    public void Awake()
    {
        entityStat = GetComponent<Entity_Stat>();

        isCrazy = false;
    }

    private void Start()
    {
        currentSan = entityStat.GetSanity();
    }

    public void IncreaseSanPoint(float increase)
    {
        if (isCrazy)
            return;

        float newSanPoint = currentSan + increase;
        float maxSanPoint = entityStat.GetSanity();

        currentSan = Mathf.Min(newSanPoint, maxSanPoint);
    }

    public void ReduceSanpoint(float Reduce)
    {
        currentSan -= Reduce;

        if (currentSan <= 0f)
        {
            currentSan = 0f;
            crazy();
        }
    }
    public void RegenerateSanpoint()
    {
        if (isCrazy)
            return;
        float regenAmount = entityStat.san.SanityRegen.GetValue();
        IncreaseSanPoint(regenAmount * Time.deltaTime);
    }

    public void DrainSanpoint()
    {
        if (isCrazy)
            return;
        float drainAmount = entityStat.san.SanityAmount.GetValue();
        ReduceSanpoint(drainAmount * Time.deltaTime);
    }

    private void crazy()
    {
        isCrazy = true;
        player?.PlayerCrazy();
    }
}
