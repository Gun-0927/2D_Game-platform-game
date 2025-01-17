using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D CapsulCollider;
    public int nextMove;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        CapsulCollider = GetComponent<CapsuleCollider2D>();

        Invoke("Think",1);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        //PlatformCheck
        Vector2 forntVec = new Vector2(rigid.position.x+ nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(forntVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(forntVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    //����Լ�
    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);

        //Sprite Animation
        animator.SetInteger("WalkSpeed", nextMove);
        //Filp Sprite
        if(nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }
        //���
        float nextThinkTime = Random.Range(2f,6f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 5);
    }

    public void OnDamaged()
    {
        //Sprit Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprit Flip Y
        spriteRenderer.flipY = true;
        //Collider Disable
        CapsulCollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        //Destroy
        Invoke("DeActive", 5);
    }
    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
