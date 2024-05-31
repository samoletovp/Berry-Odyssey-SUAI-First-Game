using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Text berriesCollectedText;

    private void Start()
    {
        int totalBerries = BerryManager.Instance.GetTotalBerries();
        berriesCollectedText.text = "Total Berries Collected: " + totalBerries;
    }
}
