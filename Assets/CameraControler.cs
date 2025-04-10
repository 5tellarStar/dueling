using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private PlayerManager player1;
    [SerializeField] private PlayerManager player2;
    private Camera c;

    private Vector3 pos;
    private float size;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        c = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {

        pos = new Vector3((player1.transform.position.x + player2.transform.position.x)/2,0,-10);

        transform.position = Vector3.MoveTowards(transform.position, pos, 0.5f * Time.deltaTime);

        size = (transform.position.x - player1.transform.position.x + 4) / c.aspect;

        c.orthographicSize = Mathf.MoveTowards(c.orthographicSize, size, 0.5f * Time.deltaTime);
    }
}
