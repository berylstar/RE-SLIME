using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySceneScript : MonoBehaviour
{
    public GameObject textSkip;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "EndingScene")
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!textSkip.activeInHierarchy)
                textSkip.SetActive(true);
            else
                EndStory();
        }
    }

    public void EndStory()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
