using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager I;

    private GameData newDATA = new GameData();
    private string path;

    private void Awake()
    {
        // ΩÃ±€≈Ê
        if (I == null) I = this;
        else if (I != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        path = Application.persistentDataPath + "/SLOT_";
    }

    public string ReturnPath(string slot)
    {
        return path + slot;
    }

    public void SaveData(string slot)
    {
        newDATA.savedFloor = GameController.savedFloor;

        newDATA.playerLife = GameController.playerLife;

        newDATA.coin = GameController.coin;

        GameController.myEquips.Clear();
        InventoryScript.I.SAVE();
        newDATA.myEquips = GameController.myEquips;

        newDATA.endTutorial = GameController.endTutorial;

        File.WriteAllText(path + slot, JsonUtility.ToJson(newDATA));

        print("SAVED!");
    }

    public void LoadData(string slot)
    {
        newDATA = JsonUtility.FromJson<GameData>(File.ReadAllText(path + slot));

        GameController.savedFloor = newDATA.savedFloor;

        GameController.playerLife = newDATA.playerLife;

        GameController.coin = newDATA.coin;

        GameController.myEquips = newDATA.myEquips;

        GameController.endTutorial = newDATA.endTutorial;

        print("LOAD!");
    }

    public void NewData()
    {
        newDATA = new GameData();

        GameController.savedFloor = newDATA.savedFloor;

        GameController.playerLife = newDATA.playerLife;

        GameController.coin = newDATA.coin;

        GameController.myEquips = newDATA.myEquips;

        GameController.endTutorial = newDATA.endTutorial;

        print("NEW!");
    }
}
