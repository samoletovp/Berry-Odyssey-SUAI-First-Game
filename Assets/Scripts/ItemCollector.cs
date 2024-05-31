using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int cherries = 0;

    [SerializeField] private Text cherriesText;
    [SerializeField] private AudioSource collectionSoundEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {
            collectionSoundEffect.Play();
            cherries++;
            BerryManager.Instance.AddBerries(1); // Добавляем одну ягоду к общему количеству
            Destroy(collision.gameObject);
            cherriesText.text = "Cherries: " + cherries;
        }
    }
}
