using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public Sprite upSprite; // 위쪽 스프라이트
    [SerializeField] public Sprite downSprite; // 아래쪽 스프라이트
    [SerializeField] public Sprite leftSprite; // 왼쪽 스프라이트
    [SerializeField] public Sprite rightSprite; // 오른쪽 스프라이트
    [SerializeField] public Sprite trapSprite; // 물풍선에 갇혔을 때 스프라이트
    private SpriteRenderer spriteRenderer;

    [SerializeField] public float Speed = 3f; // 이동 속도
    [SerializeField] public int Count = 10; // 물풍선 개수
    [SerializeField] public int Power = 1; // 물줄기 길이

    [SerializeField] public GameObject BombPrefab;
    [SerializeField] public GameObject StreamPrefab_Mid;
    [SerializeField] public GameObject StreamPrefab_Up;
    [SerializeField] public GameObject StreamPrefab_Down;
    [SerializeField] public GameObject StreamPrefab_Left;
    [SerializeField] public GameObject StreamPrefab_Right;

    private Vector2 lastDirection = Vector2.up; // 방향

    private int escapeCount = 0; // 탈출 연타 수
    public bool movable = true;
    public bool clear = false;
    private float trapTimer = 5f; // 탈출 제한 시간 5초
    private bool isGameOver = false;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if(isGameOver) { // 게임 오버 상태에서는 아무것도 못 함
            if(transform.position.y <= -10f) {
                Destroy(gameObject);
                GameManager.instance.ShowGmaeOverPanel();
            }
        }

        if(movable){
            HandleMovement();
            HandleShooting();
        }
        else if(!movable && !clear) HandleEscape();
    }

    void HandleMovement() { // 정상적인 이동
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if(Input.GetKey(KeyCode.UpArrow)) {
            spriteRenderer.sprite = upSprite;
            verticalInput = 1f;
        }
        else if(Input.GetKey(KeyCode.DownArrow)) {
            spriteRenderer.sprite = downSprite;
            verticalInput = -1f;
        }
        else if(Input.GetKey(KeyCode.LeftArrow)) {
            spriteRenderer.sprite = leftSprite;
            horizontalInput = -1f;
        }
        else if(Input.GetKey(KeyCode.RightArrow)) {
            spriteRenderer.sprite = rightSprite;
            horizontalInput = 1f;
        }

        Vector3 moveTo = new Vector3(horizontalInput, verticalInput, 0f);
        transform.position += moveTo * Speed * Time.deltaTime;
        // z값 y값과 똑같게
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        if(moveTo != Vector3.zero) lastDirection = moveTo.normalized; // 바라보는 방향
    }

    void HandleShooting() {
        if(Input.GetKeyDown(KeyCode.Z)) Shoot_Line();
        if(Input.GetKeyDown(KeyCode.X)) Shoot_Square();
    }

    void HandleEscape() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            escapeCount++;
            GameManager.instance.CountDown();
            if (escapeCount >= 10) {
                ReleaseFromTrap(); // 탈출 성공
                return;
            }
        }

        trapTimer -= Time.deltaTime;
        if(trapTimer <= 0f) {
            movable = true;
            GameOver(); // 탈출 실패
        }
    }

    void Shoot_Line() { // 직선 폭발
        if(Count > 0) {
            Count--;
            GameManager.instance.DecreaseCount(1);
            Vector2 bombDirection = GetDirection();
            Vector3 bombSpawnPosition = transform.position;
            GameObject bomb = Instantiate(BombPrefab, bombSpawnPosition, Quaternion.identity);

            bomb.GetComponent<Bomb>().Initialize(
                bombDirection,
                StreamPrefab_Mid,
                StreamPrefab_Up,
                StreamPrefab_Down,
                StreamPrefab_Left,
                StreamPrefab_Right,
                false
            );
        }
    }

    void Shoot_Square() { // 범위 폭발
        if(Count >= 3) {
            Count -= 3;
            GameManager.instance.DecreaseCount(3);
            Vector2 bombDirection = GetDirection();
            Vector3 bombSpawnPosition = transform.position;
            GameObject bomb = Instantiate(BombPrefab, bombSpawnPosition, Quaternion.identity);

            bomb.GetComponent<Bomb>().Initialize(
                bombDirection,
                StreamPrefab_Mid,
                StreamPrefab_Up,
                StreamPrefab_Down,
                StreamPrefab_Left,
                StreamPrefab_Right,
                true
            );
        }
    }

    Vector2 GetDirection() { // 플레이어의 방향을 저장
        if(spriteRenderer.sprite == upSprite) return Vector2.up;
        if(spriteRenderer.sprite == downSprite) return Vector2.down;
        if(spriteRenderer.sprite == leftSprite) return Vector2.left;
        if(spriteRenderer.sprite == rightSprite) return Vector2.right;
        return lastDirection;
    }

    public void Trap()  {
        GameManager.instance.ShowEscapePanel();
        movable = false;
        spriteRenderer.sprite = trapSprite; // 갇힌 스프라이트로 변경
        escapeCount = 0;
        trapTimer = 5f; // 타이머 초기화
    }

    private void ReleaseFromTrap(){
        GameManager.instance.CloseEscapePanel();
        movable = true;
        spriteRenderer.sprite = downSprite; // 기본 스프라이트로 복구
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        string tag = collision.gameObject.tag;
        if(tag == "Monster" || tag == "Boss") GameOver();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        string tag = collision.gameObject.tag;
        if(tag == "Ghost" || tag == "Poison") GameOver();
    }

    private void GameOver(){
        isGameOver = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.gray;

        Collider2D collider = GetComponent<Collider2D>();
        if(collider != null) collider.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1f;
        // Update()에서 게임오버패널 생성
    }
}
