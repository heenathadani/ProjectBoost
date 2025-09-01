using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{ 
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;
    [SerializeField] private float thrustStrength = 1000f;
    [SerializeField] private float rotationStrength = 100f;
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem leftThrusterParticles;
    [SerializeField] private ParticleSystem rightThrusterParticles;
    
    Rigidbody _rb;
    AudioSource _audioSource;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }
    
    private void OnDisable()
    {
        thrust.Disable();
        rotation.Disable();
    }
    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }
    void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            _rb.AddRelativeForce(Vector3.up * (thrustStrength * Time.fixedDeltaTime));
            if (!mainEngineParticles.isPlaying)
            {
                mainEngineParticles.Play();
            }
            if (!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(mainEngine);
            }
        }
        else
        {
            _audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }
    void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            ApplyRotation(rotationStrength);
            if (!rightThrusterParticles.isPlaying)
            {
                leftThrusterParticles.Stop();
                rightThrusterParticles.Play();
            }
        }
        else if (rotationInput > 0)
        {
            ApplyRotation(-rotationStrength);
            if (!leftThrusterParticles.isPlaying)
            {
                rightThrusterParticles.Stop();
                leftThrusterParticles.Play();
            }
        }
        else
        {
            leftThrusterParticles.Stop();
            rightThrusterParticles.Stop();
        }
    }
    private void ApplyRotation(float rotationThisFrame)
    {
        _rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * (rotationThisFrame * Time.fixedDeltaTime));
        _rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}
