using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    public int playerDamage;
    private Animator animator;
    private Transform target;
    private bool skipMove;
    public AudioClip enemmyAttack1;
    public AudioClip enemmyAttack2;


    protected override void Start () {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}

    protected override bool AttemptMove<T>(int xDir, int yDir)
    {
        return true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;
        if (Mathf.Abs(target.position.x - transform.position.x) < float .Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        } 
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }
        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("enemyAttack");
        SoundManager.instance.RandomizeSfx(enemmyAttack1, enemmyAttack2);
        hitPlayer.LoseFood(playerDamage);
    }

}
