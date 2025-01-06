using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    public GameObject[] itemPrefabs;

    private void OnTriggerEnter2D(Collider2D collision) {
        string tag = collision.gameObject.tag;

        if(tag == "Player") {
            Player player = collision.GetComponent<Player>();
            if(player != null) player.Trap();
        }
        if(tag == "Item") Destroy(collision.gameObject);
        if(tag == "Box") {
            Destroy(collision.gameObject);
            SpawnRandomItem(collision.transform.position); // 아이템 생성
        }
    }

    private void SpawnRandomItem(Vector2 position) {
        if(itemPrefabs.Length > 0) {
            int randomIndex = Random.Range(0, itemPrefabs.Length); // 랜덤 인덱스 선택
            Instantiate(itemPrefabs[randomIndex], position, Quaternion.identity);
        }
    }
}
