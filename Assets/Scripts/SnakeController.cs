using System.Collections;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;   
    private float gridMoveTimer;
    public float gridMoveTimerMax;
    private float inputCooldownTime = 0.1f;
    private bool isCooldown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        gridPosition = new Vector2Int(0,0);
        gridMoveTimerMax = 0.2f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1,0);
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
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetEulerAngleFromVector(gridMoveDirection));
        }
          

    }
    private float GetEulerAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;   
    }

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
    private IEnumerator InputCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(inputCooldownTime);
        isCooldown = false;
    }

}
