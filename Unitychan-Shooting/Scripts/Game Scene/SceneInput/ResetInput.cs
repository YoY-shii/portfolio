using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            RestartScene();
        }
    }

    void RestartScene() => SceneManager.LoadScene("Game Scene");
}
