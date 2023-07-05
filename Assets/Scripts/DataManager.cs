using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager I;

    private GameData newDATA = new GameData();
    private string path;
    private int slotNumber = 0;

    private void Awake()
    {
        // ΩÃ±€≈Ê
        if (I == null) I = this;
        else if (I != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        path = Application.persistentDataPath + "/SLOT_";
    }

    public string ReturnPath(int slot)
    {
        return path + slot.ToString();
    }

    public void SaveData()
    {
        newDATA.savedFloor = GameController.savedFloor;

        newDATA.playerLife = GameController.playerLife;

        newDATA.coin = GameController.coin;

        GameController.myEquips.Clear();
        InventoryScript.I.SAVE();
        newDATA.myEquips = GameController.myEquips;

        newDATA.tutorial = GameController.tutorial;

        File.WriteAllText(path + slotNumber.ToString(), JsonUtility.ToJson(newDATA));

        print((slotNumber, "SAVED!"));
    }

    public void LoadData(int slot)
    {
        newDATA = JsonUtility.FromJson<GameData>(File.ReadAllText(path + slot.ToString()));

        slotNumber = newDATA.slotNumber;
        GameController.savedFloor = newDATA.savedFloor;

        GameController.playerLife = newDATA.playerLife;

        GameController.coin = newDATA.coin;

        GameController.myEquips = newDATA.myEquips;

        GameController.tutorial = newDATA.tutorial;

        print("LOAD!");
    }

    public void NewData(int slotnum)
    {
        newDATA = new GameData();

        slotNumber = slotnum;
        GameController.savedFloor = newDATA.savedFloor;

        GameController.playerLife = newDATA.playerLife;

        GameController.coin = newDATA.coin;

        GameController.myEquips = newDATA.myEquips;

        GameController.tutorial = newDATA.tutorial;

        print("NEW!");
    }

    public void RemoveData()
    {
        File.Delete(path + slotNumber.ToString());
    }
}
