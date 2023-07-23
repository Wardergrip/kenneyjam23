using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToDeathScreen : MonoBehaviour
{
    public void GoToDeathScreenFunction()
    {
        SceneManager.LoadScene(2);
    }
}
