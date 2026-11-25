using UnityEngine;

public class StaticVars : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static StaticVars instance;
    public bool AxeFlag;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
