using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType {SpeedUp, CountUp, PowerUp, CoinUp};
    public ItemType itemType;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player != null) {
            switch (itemType) {
                case ItemType.SpeedUp:
                    if(player.Speed <= 8f) player.Speed += 1f;
                    break;
                case ItemType.CountUp:
                    player.Count++;
                    GameManager.instance.IncreaseCount(1);
                    break;
                case ItemType.PowerUp:
                    if(player.Power <= 5) player.Power++;
                    break;
                case ItemType.CoinUp:
                    GameManager.instance.IncreaseScore();
                    break;
            }
            Destroy(gameObject);
        }
    }
}
