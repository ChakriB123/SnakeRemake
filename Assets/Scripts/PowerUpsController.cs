using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

//powerup types enum
public enum PowerUpType { Shield, ScoreBoost, SpeedUp };
[Serializable]
public class PowerUp
{
    public PowerUpType type;
    public float duration;
    public int multiplier;
    public Color powerupColor ;

    public PowerUp(PowerUpType type, float duration, Color color, int multiplier)
    {
        this.type = type;
        this.duration = duration;
        this.powerupColor = color;
        this.multiplier = multiplier;
    }
}
public class PowerUpsController : MonoBehaviour
{
    private float time;
    [SerializeField]private float maxTimer;
    [SerializeField]private float minTimer;
    private float RandomTimer;
    [SerializeField]private BoxCollider2D gridArea;
    public SnakeController snakeScript;
    private SpriteRenderer spriteRenderer;
    public PowerUp[] powerUps;
    private PowerUp currentPowerUp;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    private void Start()
    { 
        ResetPowerUp();
    }
    private void Update()
    {
        PowerupTimeout();
    }


    //sets a timer to reset the food position and cooldown at random controlled time intervals
    private void PowerupTimeout()
    {
        time += Time.deltaTime;
        if (time >= RandomTimer)
        {
            RandomTimer = Random.Range(minTimer, maxTimer);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            ResetPowerUp();
            time = 0;
        }
    }

    //used for setting Random Powerup at a random position
    private void ResetPowerUp()
    {
        int randomNumber = Random.Range(0, powerUps.Length);
        currentPowerUp = powerUps[randomNumber];
        spriteRenderer.color = currentPowerUp.powerupColor;

        Bounds bounds = gridArea.bounds;
        int x = (int)Random.Range(bounds.min.x, bounds.max.x);
        int y = (int)Random.Range(bounds.min.y, bounds.max.y);
        transform.position = new Vector3(x, y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if Powerups spawn on the snake it resets its position
        if (collision.CompareTag("Snake"))
        {
            time = 0;
            ResetPowerUp();
        }
        if (collision.CompareTag("Player"))
        {
            if (Audiomanager.Instance != null)
                Audiomanager.Instance.play(SoundsEnum.PowerUp);

            ApplyPowerUp(currentPowerUp);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }


        
    }

    //applying stats to snake depending on the PowerUp type
    // and also resets the before powerup with current picked powerup
    // so powerups dont stack
    public void ApplyPowerUp(PowerUp powerUp)
    {
        StopAllCoroutines();
        snakeScript.SnakeDefault();
        switch (powerUp.type)
        {
            case PowerUpType.SpeedUp: 
                snakeScript.speedMultiplier = powerUp.multiplier;
                StartCoroutine(PowerUpDuration(() => snakeScript.speedMultiplier = 1, powerUp.duration));
                break;

            case PowerUpType.Shield:
                snakeScript.isShield = true;
                StartCoroutine(PowerUpDuration(() => snakeScript.isShield = false, powerUp.duration));
                break;
             
            case PowerUpType.ScoreBoost:
                snakeScript.scoreMultiplier = powerUp.multiplier;
                StartCoroutine(PowerUpDuration(() => snakeScript.scoreMultiplier = 1, powerUp.duration));
                break;


        }
    }

    //After the duration we revert back to normal state
    private IEnumerator PowerUpDuration(Action revertEffect, float delay)
    {
        yield return new WaitForSeconds(delay);
        revertEffect.Invoke(); // executes the "undo" logic
    }
}



