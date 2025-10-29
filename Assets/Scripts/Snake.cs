using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;   
    private float gridMoveTimer;
    private float gridMoveTimerMax;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        gridPosition = new Vector2Int(0,0);
        gridMoveTimerMax = 0.5f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1,0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
  
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
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
        }
          

    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Vertical"))
        {

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
        if (Input.GetButtonDown("Horizontal"))
        {
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
        
}
