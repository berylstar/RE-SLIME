using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager inst;

    GameData dd = new GameData();

    string path;
    string filename;

    private void Awake()
    {
        if (inst == null) inst = this;
        else if (inst != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        path = Application.persistentDataPath + "/";
        filename = "SLOT";
    }

    public void SaveData()
    {
        print("SAVED!");
        dd.savedFloor = GameController.savedFloor;

        dd.playerLife = GameController.playerLife;
        dd.playerMaxHP = GameController.playerMaxHP;
        dd.playerAP = GameController.playerAP;
        dd.playerDP = GameController.playerDP;
        dd.playerSpeed = GameController.playerSpeed;
        dd.playerTimeDamage = GameController.playerTimeDamage;
        dd.skillC = GameController.skillC;
        dd.skillV = GameController.skillV;
        dd.ShopGrade = GameController.ShopGrade;

        dd.coin = GameController.coin;
        dd.probCoin = GameController.probCoin;
        dd.RedCoin = GameController.RedCoin;
        dd.potionEff = GameController.potionEff;
        dd.probPotion = GameController.probPotion;

        dd.effBattery = GameController.effBattery;
        dd.effcrescent = GameController.effcrescent;
        dd.effSkate = GameController.effSkate;
        dd.effLastleaf = GameController.effLastleaf;

        File.WriteAllText(path + filename, JsonUtility.ToJson(dd));
    }

    public void LoadData()
    {
        print("LOAD!");
        dd = JsonUtility.FromJson<GameData>(File.ReadAllText(path + filename));

        GameController.savedFloor = dd.savedFloor;

        GameController.playerLife = dd.playerLife;
        GameController.playerMaxHP = dd.playerMaxHP;
        GameController.playerAP = dd.playerAP;
        GameController.playerDP = dd.playerDP;
        GameController.playerSpeed = dd.playerSpeed;
        GameController.playerTimeDamage = dd.playerTimeDamage;
        GameController.skillC = dd.skillC;
        GameController.skillV = dd.skillV;
        GameController.ShopGrade = dd.ShopGrade;

        GameController.coin = dd.coin;
        GameController.probCoin = dd.probCoin;
        GameController.RedCoin = dd.RedCoin;
        GameController.potionEff = dd.potionEff;
        GameController.probPotion = dd.probPotion;

        GameController.effBattery = dd.effBattery;
        GameController.effcrescent = dd.effcrescent;
        GameController.effSkate = dd.effSkate;
        GameController.effLastleaf = dd.effLastleaf;
    }
}