using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Food : MonoBehaviour 
{
    public BoxCollider2D gridArea;
    public int growthSize;
    public int shrinkSize;
    private float time;
    public float maxTimer;
    private SpriteRenderer spriteRenderer;


    // According to GDD we have two types of foods (Mass Gainer,Mass Burner)
    // I implemented both in one GameObject in some form of object pooling lol
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        RandomizeFood();  
    }
    private void Update()
    {
        ResetFood();
    }


    //doesn't really spawns the food but sets a timer to reset the food position 
    private void ResetFood()
    {
        time += Time.deltaTime;
        if (time >= maxTimer)
        {
            RandomizeFood();
            time -= maxTimer;
        }
    }
    private void RandomizeFood()
    {
        // colour of food determine's Food type
        // red = mass burner
        // green = mass gainer 
        // A 50/50 probabiliy to get either of the food types
       int chance = Random.Range(1, 100);
        if(chance > 50)
            spriteRenderer.color = Color.green;
        else
            spriteRenderer.color = Color.red;

        //for getting x and y boundaries of the collider
        Bounds bounds = gridArea.bounds;
        int x = (int)Random.Range(bounds.min.x, bounds.max.x);
        int y = (int)Random.Range(bounds.min.y, bounds.max.y);
        transform.position = new Vector3(x, y);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if food spawns on the snake it resets its position
        if (collision.CompareTag("Snake"))
        {
            time = 0;
            RandomizeFood();
        }
        if (collision.gameObject.GetComponent<SnakeController>() != null)
        {
            SnakeController snakeController = collision.gameObject.GetComponent<SnakeController>();
            if (spriteRenderer.color == Color.green)
            {
                snakeController.Grow(growthSize);
            }
            if(spriteRenderer.color == Color.red)
            {
                snakeController.Shrink(growthSize); 
            }
            
            time = 0;
            RandomizeFood();
        }

    }
   
}
