using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SnakeController : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;   
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private float inputCooldownTime;
    public BoxCollider2D gridArea;
    private Bounds bounds;
    private bool isCooldown = false;

    public GameObject PanelGameOver;
    public List<Transform> segments {  get; private set; }
    public int initialSnakeSize;
    public Transform snakeBodyPrefab;
    [SerializeField]private float shrinklimit = 3;

    public ScoreController scoreController;
    private CoopUIManager coopUIManager;


    public float speedMultiplier { get; set; }
    public int scoreMultiplier { get; set; }
    public bool isShield { get; set; }

    [Header("CO-OP Settings")]
    public bool isCoop;
    [SerializeField]private PlayerType player;

   
    private void Awake()
    {
        SnakeDefault();
        bounds = gridArea.bounds;
        inputCooldownTime = 0.06f;
        gridPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        gridMoveTimerMax = 0.1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1,0);
    }
    private void Start()
    {
        segments = new List<Transform>();
        segments.Add(transform);
        HandleInitialSize();
    }
    private void Update()
    {
        HandleInput();
        HandleGridMovement();
    }

  
    private void HandleGridMovement()
    {
        // Move snake based on timer
        gridMoveTimer += Time.deltaTime * speedMultiplier;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            HandleSegmentMovement();
            gridPosition += gridMoveDirection;
            gridPosition = HandleWrapping(gridPosition);
            transform.position = new Vector3Int(gridPosition.x, gridPosition.y);
            gridMoveTimer = 0;
        }
    }
    private Vector2Int HandleWrapping(Vector2Int gridPosition)
    {

        if (gridPosition.x < (int)bounds.min.x)
        {
            gridPosition.x = (int)bounds.max.x;

        }
        if (gridPosition.x > (int)bounds.max.x)
        {
            gridPosition.x = (int)bounds.min.x;

        }
        if (gridPosition.y < (int)bounds.min.y)
        {
            gridPosition.y = (int)bounds.max.x;

        }
        if (gridPosition.y > (int)bounds.max.y)
        {
            gridPosition.y = (int)bounds.min.y;

        }


        return gridPosition;
    }

    // To set the inital snakebody size
    private void HandleInitialSize()
    {
        for (int i = 1; i < initialSnakeSize; i++)
        {
            Vector3 initalPosition = new Vector3(transform.position.x - i, transform.position.y);
            segments.Add(Instantiate(snakeBodyPrefab, initalPosition, Quaternion.identity));
           
        }
    }

    // For handling Player inputs for both single and Co-op
    private void HandleInput()
    {

        // Input Handling for player 1
        if (player == PlayerType.Player1)
        {
            if (!isCooldown)
            {
                if (Input.GetKeyDown(KeyCode.W) && gridMoveDirection != Vector2Int.down)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.up;
                    
                }
                else if (Input.GetKeyDown(KeyCode.S) && gridMoveDirection != Vector2Int.up)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.down;
                    
                }

                else if (Input.GetKeyDown(KeyCode.D) && gridMoveDirection != Vector2Int.left)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.right;

                }
                else if (Input.GetKeyDown(KeyCode.A) && gridMoveDirection != Vector2Int.right)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.left;

                }

            }
        }
        // Input Handling for player 2
        if (player == PlayerType.Player2) {

            if (!isCooldown)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && gridMoveDirection != Vector2Int.down)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.up;

                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && gridMoveDirection != Vector2Int.up)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.down;

                }

                else if (Input.GetKeyDown(KeyCode.RightArrow) && gridMoveDirection != Vector2Int.left)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.right;

                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && gridMoveDirection != Vector2Int.right)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.left;

                }

            }
        }
    }
 

    // Input cooldown to avoid Irregular Movement
    private IEnumerator InputCooldown()
    {
        isCooldown = true;
       yield return new WaitForSeconds(inputCooldownTime);
       //yield return new WaitForEndOfFrame();
        isCooldown = false;
    }

    //Growth by Eating Mass Gainer
    public void Grow(int Size)
    {
        for (int i = 0; i < Size; i++)
        {
            Transform segment = Instantiate(snakeBodyPrefab);
            segment.position = segments[segments.Count - 1].position;
            segments.Add(segment);
        }
    }

    //Shrink by Eating Mass Burner
    public void Shrink(int Size)
    {
        if (segments.Count > shrinklimit)
        {
            for (int i = 0; i < Size; i++)
            {
                Transform segment = segments[segments.Count - 1];
                segments.Remove(segment);
                Destroy(segment.gameObject);
            }
        }
    }

    //Each segment follows the previous segment vice vasa
    private void HandleSegmentMovement()
    {
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }
    }

    public void HandleScore()
    {
        scoreController.incrementScore(scoreMultiplier);
    }
    //reset the powerUps to default
    public void SnakeDefault()
    {
        scoreMultiplier = 1;
        speedMultiplier = 1;
        isShield = false;
    }
    private void SnakeDead()
    {
        if (!isShield)
        {
            if(player == PlayerType.Player1)
            {
                scoreController.updatePlayerWon(PlayerType.Player2.ToString());
            }
            else
            {
                scoreController.updatePlayerWon(PlayerType.Player1.ToString());
            }
            PanelGameOver.SetActive(true);
            Time.timeScale = 0f;
            if (Audiomanager.Instance != null)
                Audiomanager.Instance.play(SoundsEnum.PlayerDead);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Snake"))
        {  
            SnakeDead();
        }
    }

    // To set players for CO-OP
    public enum PlayerType
    {
        Player1,
        Player2,
    }
}

