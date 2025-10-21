using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialExit : MonoBehaviour
{

private void OnTriggerEnter(Collider other)
    {
        //Load Main Menu Scene
        SceneManager.LoadScene(0);
    }
    
}
