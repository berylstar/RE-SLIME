using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSceneScript : MonoBehaviour
{
    public Image imageEnd;
    public Text textEnd;
    public Text textResult;
    public Text textRestart;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("IntroScene");
    }
}
