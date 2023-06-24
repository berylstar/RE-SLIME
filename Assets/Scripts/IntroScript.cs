using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class IntroScript : MonoBehaviour
{
    public GameObject[] panel;
    public GameObject pick;
    public Button[] slots;

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
                ActivePanel(1);

                if (File.Exists(DataManager.I.ReturnPath("1")))
                    slots[1].GetComponent<Button>().interactable = true;

                if (File.Exists(DataManager.I.ReturnPath("2")))
                    slots[2].GetComponent<Button>().interactable = true;
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
            if (pickIndex <= 2)
            {
                if (!slots[pickIndex].interactable)
                    return;

                if (pickIndex == 1 || pickIndex == 2)
                    DataManager.I.LoadData(pickIndex.ToString());
                else
                    DataManager.I.NewData();

                UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
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
