using UnityEngine;
using System.Collections;
using UnityEngine.UI;  
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public float restartLevelDelay = 1f;        
    public int pointsPerFood = 10;              
    public int pointsPerSoda = 20;              
    public int wallDamage = 1;                  
    public Text foodText;                       
    public AudioClip moveSound1;                
    public AudioClip moveSound2;                
    public AudioClip eatSound1;                 
    public AudioClip eatSound2;                 
    public AudioClip drinkSound1;               
    public AudioClip drinkSound2;               
    public AudioClip gameOverSound;             

    private Animator animator;                  
    private int food;                           
    private Vector2 touchOrigin = -Vector2.one;
    public static Vector2 position;


    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Food: " + food;
        position.x = position.y = 2;
        base.Start();
    }


    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }


    private void Update()
    {
        if (!GameManager.instance.playersTurn) return;
        bool canMove = false;
        int horizontal = 0;     
        int vertical = 0;       

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0)
        {
            vertical = 0;
        }
        if (horizontal != 0 || vertical != 0)
        {
            canMove = AttemptMove<Wall>(horizontal, vertical);
            if (canMove)
            {
                position.x += horizontal;
                position.y += vertical;
                GameManager.instance.UpdateBoard(horizontal, vertical);
            }
        }
    }

    

    protected override bool AttemptMove<T>(int xDir, int yDir)
    {
        bool hit = base.AttemptMove<T>(xDir, yDir);
        GameManager.instance.playersTurn = false;
        return hit;
    }


    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;

        hitWall.DamageWall(wallDamage);

        animator.SetTrigger("playerChop");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);

            enabled = false;
        }

        else if (other.tag == "Food")
        {
            food += pointsPerFood;

            foodText.text = "+" + pointsPerFood + " Food: " + food;

            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);

            other.gameObject.SetActive(false);
        }

        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;

            foodText.text = "+" + pointsPerSoda + " Food: " + food;

            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);

            other.gameObject.SetActive(false);
        }
    }


    private void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }


    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");

        food -= loss;

        foodText.text = "-" + loss + " Food: " + food;

        CheckIfGameOver();
    }


    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }
}