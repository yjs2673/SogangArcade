using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_follow : MonoBehaviour 
{
    [SerializeField] public Sprite upSprite; // 위쪽 스프라이트
    [SerializeField] public Sprite downSprite; // 아래쪽 스프라이트
    [SerializeField] public Sprite leftSprite; // 왼쪽 스프라이트
    [SerializeField] public Sprite rightSprite; // 오른쪽 스프라이트
    private SpriteRenderer spriteRenderer;

    private bool isKilled = false;
    private bool movable = true;

    [SerializeField] public float moveSpeed = 3f;
    private Transform target;

    public void Start() {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if(isKilled) { // 플레이어랑 똑같이 사라짐
            if(transform.position.y <= -10f) {
                Destroy(gameObject);
            }
        }

        if(movable) MovePattern();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(isKilled) return;
        string tag = collision.gameObject.tag;
        if(tag == "Bomb") Kill();
        if(tag == "Stream") Kill();
    }

    private void Kill() {
        movable = false;
        GameManager.instance.DecreaseMonster();

        isKilled = true; 
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.gray;

        Collider2D collider = GetComponent<Collider2D>();
        if(collider != null) collider.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1f;
    }

    private void MovePattern() {
        float currX = transform.position.x, currY = transform.position.y;
        float tarX = target.position.x, tarY = target.position.y;
        float disX = Mathf.Abs(currX - tarX);
        float disY = Mathf.Abs(currY - tarY);

        Vector3 moveTo;
        if(disY < 0.1f) moveTo = new Vector3((tarX - currX) / disX, 0f, 0f);
        else if(disX < 0.1f) moveTo = new Vector3(0f, (tarY - currY) / disY, 0f);
        else if(disX > disY) moveTo = new Vector3((tarX - currX) / disX, 0f, 0f);
        else moveTo = new Vector3(0f, (tarY - currY) / disY, 0f);
        transform.position += moveTo * moveSpeed * Time.deltaTime;

        if(moveTo.x != 0) {
            if(currX < transform.position.x) spriteRenderer.sprite = rightSprite;
            else spriteRenderer.sprite = leftSprite;
        }
        else {
            if(currY < transform.position.y) spriteRenderer.sprite = upSprite;
            else spriteRenderer.sprite = downSprite;
        }

        // z값 y값과 동일하게 
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}
