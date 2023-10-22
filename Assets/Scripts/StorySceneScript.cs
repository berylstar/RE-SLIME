using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySceneScript : MonoBehaviour
{
    [SerializeField] private GameObject textSkip;
    [SerializeField] private bool isStart;

    private InputManager _input;

    //private void Awake()
    //{
    //    Screen.SetResolution(1366, 768, false);
    //}

    private void Start()
    {
        _input = InputManager.Instance;

        _input.StoryState.OnSpace = SkipStory;
        _input.StoryState.OnESC = SkipStory;
        _input.StateEnqueue(_input.StoryState);

        if (!isStart)
            InventoryScript.I.transform.position = new Vector3(100, 100, 1);
    }

    // InputSystem
    private void SkipStory()
    {
        if (!textSkip.activeInHierarchy)
            textSkip.SetActive(true);
        else
            EndStory();
    }

    // 애니매이션 끝날 때 자동 실행
    public void EndStory()
    {
        _input.StateDequeue();

        if (!isStart)
            SceneManager.LoadScene("ResultScene");
        else
            SceneManager.LoadScene("TitleScene");
    }
}
