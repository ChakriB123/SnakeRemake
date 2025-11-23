using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
public class SnakeController : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;   
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private float inputCooldownTime;
    private bool isCooldown = false;
    private List<Transform> segments;
    private Bounds bounds;
    public int initialSnakeSize;
    public BoxCollider2D gridArea;
    public Transform snakeBodyPrefab;

    [Header("CO-OP Settings")]
    public bool isPlayer1;

    private void Awake()
    {
        bounds = gridArea.bounds;
        inputCooldownTime = 0.1f;
        gridPosition = new Vector2Int(0,0);
        gridMoveTimerMax = 0.2f;
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
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            HandleSegmentMovement();
            gridPosition += gridMoveDirection;
            gridPosition = HandleWrapping(gridPosition);
            transform.position = new Vector3Int(gridPosition.x, gridPosition.y);
            gridMoveTimer -= gridMoveTimerMax;


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
            segments.Add(Instantiate(snakeBodyPrefab));
        }
    }

    // For handling Player inputs for both single and Co-op
    private void HandleInput()
    {
        if (isPlayer1)
        {
            if (!isCooldown)
            {
                if (Input.GetKeyDown(KeyCode.W) && gridMoveDirection != Vector2Int.down)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.up;
                    
                }
                 if (Input.GetKeyDown(KeyCode.S) && gridMoveDirection != Vector2Int.up)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.down;
                    
                }

                 if(Input.GetKeyDown(KeyCode.D) && gridMoveDirection != Vector2Int.left)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.right;
                    
                }
                 if (Input.GetKeyDown(KeyCode.A) && gridMoveDirection != Vector2Int.right)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.left;
   
                }

            }
        }
        else {


            if (!isCooldown)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && gridMoveDirection != Vector2Int.down)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.up;

                }
                 if (Input.GetKeyDown(KeyCode.DownArrow) && gridMoveDirection != Vector2Int.up)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.down;

                }

                 if(Input.GetKeyDown(KeyCode.RightArrow) && gridMoveDirection != Vector2Int.left)
                {
                    StartCoroutine(InputCooldown());
                    gridMoveDirection = Vector2Int.right;

                }
                 if (Input.GetKeyDown(KeyCode.LeftArrow) && gridMoveDirection != Vector2Int.right)
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
        if (segments.Count > Size)
        {
            for (int i = 0; i < Size; i++)
            {
                Transform segment = segments[segments.Count - 1];
                segments.Remove(segment);
                Destroy(segment.gameObject);
            }
        }
    }

    //Each segment follows the pervious segment vice vasa
    private void HandleSegmentMovement()
    {
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }
    }
    

}
