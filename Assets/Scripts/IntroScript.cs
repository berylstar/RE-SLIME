using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    public GameObject[] panel;

    public GameObject pick;

    private int menuIndex = 0;
    private List<int> mmi = new List<int>() { 2, 3 };
    private int pickIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) pickIndex = Mathf.Min(pickIndex + 1, mmi[menuIndex]);
        if (Input.GetKeyDown(KeyCode.UpArrow)) pickIndex = Mathf.Max(pickIndex - 1, 0);

        pick.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, pickIndex * -100, 0f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ButtonClick();
        }
    }

    public void ButtonClick()
    {
        if (menuIndex == 0)
        {
            if (pickIndex == 0)
            {
                // DataManager.inst.LoadData();
                //UnityEngine.SceneManagement.SceneManager.LoadScene(1);

                ActivePanel(1);
            }
            else if (pickIndex == 1)
            {
                print("OPTION");
            }
            else
            {
                Application.Quit();
            }
        }
        else if (menuIndex == 1)
        {
            //if (pickIndex == 0)
            //{

            //}
            //else if (pickIndex == 1)
            //{

            //}
            //else if (pickIndex == 2)
            //{

            //}
            if (pickIndex <= 3)
            {
                DataManager.inst.NewData(pickIndex);
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }
            else
            {
                ActivePanel(0);
            }
        }
    }

    private void ActivePanel(int idx)
    {
        for (int i = 0; i < panel.Length; i++)
        {
            panel[i].SetActive(false);
        }

        menuIndex = idx;
        pickIndex = 0;
        panel[idx].SetActive(true);
    }

    // PointerEnter 감지용 함수
    public void SetIndex(int i)
    {
        pickIndex = i;
    }
}
