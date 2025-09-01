using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float delayTime = 2f;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private ParticleSystem crashParticles;
    [SerializeField] private ParticleSystem successParticles;

    AudioSource _audioSource;

    private bool _isControllable = true;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_isControllable) { return; }
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Everything is friendly");
                break;
            case "Finish":
                Debug.Log("You landed safely!");
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }
    private void StartSuccessSequence()
    {
        //todo add particle & sound effects
        _isControllable = false;
        _audioSource.Stop();
        _audioSource.PlayOneShot(successSound);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", delayTime);
    }
    private void StartCrashSequence()
    {
        //todo add particle & sound effects
        _isControllable = false;
        _audioSource.Stop();
        _audioSource.PlayOneShot(crashSound);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delayTime);
    }
    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
