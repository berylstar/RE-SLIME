using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    public GameObject pick;

    private int index = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) index = Mathf.Min(index + 1, 2);
        if (Input.GetKeyDown(KeyCode.UpArrow)) index = Mathf.Max(index - 1, 0);

        pick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, index * -100, 0f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ButtonClick();
        }
    }

    public void ButtonClick()
    {
        if (index == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        else if (index == 1)
        {
            print("OPTION");
        }
        else
        {
            Application.Quit();
        }
    }

    public void SetIndex(int i)
    {
        index = i;
    }
}
