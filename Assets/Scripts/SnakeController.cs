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
    public int initialSnakeSize;
    public Transform snakeBodyPrefab; 

    private void Awake()
    {
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
            gridMoveTimer -= gridMoveTimerMax;
            transform.position = new Vector3Int(gridPosition.x, gridPosition.y);
        }

    }

    // To set the inital snakebody size
    private void HandleInitialSize()
    {
        for (int i = 1; i < initialSnakeSize; i++)
        {
            segments.Add(Instantiate(snakeBodyPrefab));
        }
    }

    // For handling Player input
    private void HandleInput()
    {
        if (Input.GetButtonDown("Vertical") && !isCooldown)
        {
            StartCoroutine(InputCooldown());

            if (Input.GetAxis("Vertical") > 0)
            {
                if (gridMoveDirection.y != -1)
                {
                    gridMoveDirection.x = 0;
                    gridMoveDirection.y = 1;
                }
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                if (gridMoveDirection.y != 1)
                {
                    gridMoveDirection.x = 0;
                    gridMoveDirection.y = -1;
                }
            }

        }
        if (Input.GetButtonDown("Horizontal") && !isCooldown)
        {
            StartCoroutine(InputCooldown());
            if (Input.GetAxis("Horizontal") > 0)
            {
                if (gridMoveDirection.x != -1)
                {
                    gridMoveDirection.x = 1;
                    gridMoveDirection.y = 0;
                }
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                if (gridMoveDirection.x != 1)
                {
                    gridMoveDirection.x = -1;
                    gridMoveDirection.y = 0;
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
