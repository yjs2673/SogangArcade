using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject[] itemPrefabs;

    public Vector2 direction;
    public GameObject StreamPrefab_Mid;
    public GameObject StreamPrefab_Up;
    public GameObject StreamPrefab_Down;
    public GameObject StreamPrefab_Left;
    public GameObject StreamPrefab_Right;
    public bool isSquare;
    public Rigidbody2D rb;
    public Player player;

    [SerializeField] public float moveSpeed = 6f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, GameObject midPrefab, GameObject upPrefab, GameObject downPrefab, GameObject leftPrefab, GameObject rightPrefab, bool isSquare) {
        this.direction = direction;
        this.StreamPrefab_Mid = midPrefab;
        this.StreamPrefab_Up = upPrefab;
        this.StreamPrefab_Down = downPrefab;
        this.StreamPrefab_Left = leftPrefab;
        this.StreamPrefab_Right = rightPrefab;
        this.isSquare = isSquare;
        player = FindObjectOfType<Player>();

        Vector2 spawnPosition = (Vector2)player.transform.position;
        
        float currX = spawnPosition.x, currY = spawnPosition.y;
        float newX = Mathf.Round(currX), newY = Mathf.Round(currY);
        if(currX > newX) spawnPosition.x = newX + 0.5f;
        else spawnPosition.x = newX - 0.5f;
        if(currY > newY) spawnPosition.y = newY + 0.5f;
        else spawnPosition.y = newY - 0.5f;

        transform.position = spawnPosition; // 물풍선의 위치

        // 물풍선이 전진하도록 설정
        rb.velocity = direction * moveSpeed; // 전진 방향으로 속도 설정   
    }

    void Update() {
        if(rb != null) transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        string tag = collision.gameObject.tag;

        if(tag == "Monster" || tag == "Boss" || tag == "Ghost") {
            Explode(0); 
            return;
        }

        if(tag == "Box") {
            Destroy(collision.gameObject);
            SpawnRandomItem(collision.transform.position); // 아이템 생성
            Explode(1);
        }

        if(tag == "Block") Explode(1);
    }

    private void Explode(int n) {
        if (isSquare) GenerateSquare();
        else GenerateLine(n);

        /* int n = isSquare ? 3 : 1;
        if(player != null) player.Count += n; // 터지면 다시 물풍선 개수 회복
        GameManager.instance.IncreaseCount(n); */
        Destroy(gameObject);
    }

    private void GenerateLine(int n) {
        Vector2 startPosition = transform.position;

        float currX = startPosition.x, currY = startPosition.y;
        float newX = Mathf.Round(currX), newY = Mathf.Round(currY);
        if(currX > newX) startPosition.x = newX + 0.5f;
        else startPosition.x = newX - 0.5f;
        if(currY > newY) startPosition.y = newY + 0.5f;
        else startPosition.y = newY - 0.5f;

        for(int i = 0; i < player.Power; i++) {
            int flag = 0;

            Vector2 streamPosition = startPosition + (Vector2)direction * i;
            // 시작점에서 현재 streamPosition까지의 충돌 체크
            RaycastHit2D hit = Physics2D.Raycast(streamPosition, direction, i);

            if(hit.collider != null) {
                if(hit.collider.CompareTag("Block") || hit.collider.CompareTag("Box")) flag = 1;
            }
        
            GameObject prefab = GetPrefabByDirection(direction);
            GameObject stream = Instantiate(prefab, streamPosition, Quaternion.identity);
            Destroy(stream, 0.5f);
            
            if(n == 1 || flag == 1) break;
        }
    }

    private void GenerateSquare() {
        Vector2 startPosition = transform.position;

        float currX = startPosition.x, currY = startPosition.y;
        float newX = Mathf.Round(currX), newY = Mathf.Round(currY);
        if(currX > newX) startPosition.x = newX + 0.5f;
        else startPosition.x = newX - 0.5f;
        if(currY > newY) startPosition.y = newY + 0.5f;
        else startPosition.y = newY - 0.5f;

        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                Vector2 streamPosition = (Vector2)transform.position + new Vector2(x, y);
                RaycastHit2D hit = Physics2D.Raycast(startPosition, streamPosition - startPosition, Vector2.Distance(startPosition, streamPosition));

                GameObject Stream = Instantiate(StreamPrefab_Mid, streamPosition, Quaternion.identity); // 기본적으로 위쪽 Prefab 사용
                Destroy(Stream, 0.5f);
            }
        }
    }

    private GameObject GetPrefabByDirection(Vector2 direction) {
        if(direction == Vector2.up) return StreamPrefab_Up;
        if(direction == Vector2.down) return StreamPrefab_Down;
        if(direction == Vector2.left) return StreamPrefab_Left;
        if(direction == Vector2.right) return StreamPrefab_Right;
        return null;
    }

    private void SpawnRandomItem(Vector2 position) {
        if(itemPrefabs.Length > 0) {
            int randomIndex = Random.Range(0, itemPrefabs.Length); // 랜덤 인덱스 선택
            GameObject item = Instantiate(itemPrefabs[randomIndex], position + new Vector2(0, 0.3f), Quaternion.identity);
            Vector3 itemPosition = item.transform.position;
            item.transform.position = new Vector3(itemPosition.x, itemPosition.y, itemPosition.y);  // z값을 y값과 동일하게 설정
        }
    }
}
