using UnityEngine;

public class FoodRandomizer : MonoBehaviour 
{
    public BoxCollider2D gridArea;    
    private void Start()
    {
        RandomizeFood();
    }

    private void RandomizeFood()
    {
        Bounds bounds = gridArea.bounds;

        int x = (int)Random.Range(bounds.min.x, bounds.max.x);
        int y = (int)Random.Range(bounds.min.y, bounds.max.y);
        transform.position = new Vector3(x, y);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SnakeController>() != null)
        {
            RandomizeFood();
        }

    }
}
