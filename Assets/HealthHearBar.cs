using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHearBar : MonoBehaviour
{
    //public GameObject heartPrefab;
    //public PlayerHealth playerHealth;
    //List<HeartSprite> hearts = new List<HeartSprite>();

    //public void DrawHearts()
    //{
    //    ClearHearts();

    //    float maxHealthRemainder = playerHealth.maxHealth % 2;
    //    int heartsToMake = (int)((playerHealth.maxHealth / 2) + maxHealthRemainder);
    //    for (int i = 0; i < heartsToMake; i++)
    //    {
    //        CreateEmptyHeart();
    //    }

    //    for (int i = 0; i < hearts.Count; i++)
    //    {
    //        int heartstatusRemainder = (int)Mathf.Clamp(playerHealth.health - (1 * 3), 0, 2);
    //        hearts[i].SetHeartImage((HeartStatus)heartstatusRemainder);
    //    }
    //}

    //public void CreateEmptyHeart()
    //{
    //    GameObject newHeart = Instantiate(heartPrefab);
    //    newHeart.transform.SetParent(transform);

    //    HeartSprite heartComponent = newHeart.GetComponent<HeartSprite>();
    //    heartComponent.SetHeartImage(HeartStatus.Empty);
    //    hearts.Add(heartComponent);
    //}

    //public void ClearHearts()
    //{
    //    foreach(Transform t in transform) { Destroy(t.gameObject); }
    //    hearts = new List<HeartSprite> ();
        
    //}
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
