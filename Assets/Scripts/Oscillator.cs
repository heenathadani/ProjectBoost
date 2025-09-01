using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float speed = 1f;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _movementFactor; 
    
    void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition + movementVector;
    }
    
    void Update()
    {
        _movementFactor = Mathf.PingPong(Time.time * speed, 1);
        transform.position = Vector3.Lerp(_startPosition, _endPosition, _movementFactor);
    }
    
}
