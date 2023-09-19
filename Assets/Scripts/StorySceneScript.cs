using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySceneScript : MonoBehaviour
{
    public GameObject textSkip;

    //private void Awake()
    //{
    //    Screen.SetResolution(1366, 768, false);
    //}

    private void Start()
    {
        if (GameController.floor == 100)
            InventoryScript.I.transform.position = new Vector3(100, 100, 1);
    }

    // InputSystem
    private void OnSkip()
    {
        if (!textSkip.activeInHierarchy)
            textSkip.SetActive(true);
        else
            EndStory();
    }

    // �ִϸ��̼� ���� �� �ڵ� ����
    public void EndStory()
    {
        if (GameController.floor == 100)
            SceneManager.LoadScene("ResultScene");
        else
            SceneManager.LoadScene("TitleScene");
    }
}
