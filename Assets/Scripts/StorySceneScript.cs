using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySceneScript : MonoBehaviour
{
    public GameObject textSkip;

    private void Start()
    {
        if (GameController.floor == 100)
            GameObject.Find("INVENTORY").transform.position = new Vector3(100, 100, 1);
    }

    private void Update()
    {
        if (GameController.floor == 100)
            return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!textSkip.activeInHierarchy)
                textSkip.SetActive(true);
            else
                EndStory();
        }
    }

    public void EndStory()
    {
        if (GameController.floor == 100)
        {
            SceneManager.LoadScene("ResultScene");
        }
        else
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
