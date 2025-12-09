using UnityEngine;

public class Hero : MonoBehaviour
{
    public GameObject map;
    private Grid grid;

    void Awake()
    {
        grid = map.GetComponent<Grid>();
        if (!grid)
        {
            Debug.LogError("Grid component not found in map");
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
