using System.Collections;
using UnityEngine;

public class Sanity : MonoBehaviour
{
    public float MaxSanPoint = 100f;
    public float SanPoint;
    public bool isCrazy = false;

    private Coroutine reduceSanityCoroutine = null;
    private Coroutine IncreaseSanityOverTime = null;

    private void Start()
    {
        SanPoint = MaxSanPoint;
    }

    public virtual void DecreaseSanPoint(float decrease)
    {
        if (isCrazy)
            return;
    
        SanPoint -= decrease;
        if (SanPoint <= 0)
        {
            SanPoint = 0;
            crazy();
        }
    }

    public void IncreaseSanPoint(float increase)
    {
        SanPoint += increase;
      
        if (SanPoint > MaxSanPoint)
        {
            SanPoint = MaxSanPoint;
        }
    }

    private void crazy()
    {
        Debug.Log("You are going crazy!");
        isCrazy = true;
    }

    public void StartReduceSanity(float amount, float interval)
    {
        if (isCrazy) return;

        if (reduceSanityCoroutine == null)
        {
            reduceSanityCoroutine = StartCoroutine(ReduceSanityOverTime(amount, interval));
        }
    }

    public void StopReduceSanity()
    {
        if (reduceSanityCoroutine != null)
        {
            StopCoroutine(reduceSanityCoroutine);
            reduceSanityCoroutine = null;
        }
    }

    public void StartIncreaseSanity(float amount, float interval)
    {
        if (IncreaseSanityOverTime == null)
        {
            IncreaseSanityOverTime = StartCoroutine(IncreaseSanity(amount, interval));
        }
    }

    public void StopIncreaseSanity()
    {
        if (IncreaseSanityOverTime != null)
        {
            StopCoroutine(IncreaseSanityOverTime);
            IncreaseSanityOverTime = null;
        }
    }

    private IEnumerator ReduceSanityOverTime(float amount, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            DecreaseSanPoint(amount);
        }
    }

    private IEnumerator IncreaseSanity(float amount, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            IncreaseSanPoint(amount);
        }
    }

}
