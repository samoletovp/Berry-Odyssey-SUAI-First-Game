using UnityEngine;

public class BerryManager : MonoBehaviour
{
    public static BerryManager Instance { get; private set; }

    private int totalBerriesCollected = 0;

    private void Awake()
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

    public void AddBerries(int count)
    {
        totalBerriesCollected += count;
    }

    public int GetTotalBerries()
    {
        return totalBerriesCollected;
    }
}
