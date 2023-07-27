using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class DataManager : MonoBehaviour
{
    private static readonly string privateKey = "re51imemadeByMINSANG0N202306";

    public static DataManager I;

    private GameData newDATA = new GameData();
    private string path;
    private int slotNumber = 0;

    public static string Decrypt(string textToDecrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        return Encoding.UTF8.GetString(plainText);
    }

    public static string Encrypt(string textToEncrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }
                Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
    }

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
        return path + slot.ToString() + Application.version;
    }

    public void SaveData()
    {
        newDATA.savedFloor = GameController.savedFloor;
        newDATA.inTime = GameController.inTime;
        newDATA.getCoin = GameController.getCoin;
        newDATA.kills = GameController.kills;

        newDATA.playerLife = GameController.playerLife;

        newDATA.coin = GameController.coin;

        GameController.myEquips.Clear();
        InventoryScript.I.SAVE();
        newDATA.myEquips = GameController.myEquips;

        newDATA.tutorial = GameController.tutorial;

        File.WriteAllText(path + slotNumber.ToString() + Application.version, Encrypt(JsonUtility.ToJson(newDATA), privateKey));

        print((slotNumber, "SAVED!"));
        // UIScript.I.TextBlink("ΩΩ∑‘ " + slotNumber + " ¿˙¿Â !");
    }

    public void LoadData(int slot)
    {
        newDATA = JsonUtility.FromJson<GameData>(Decrypt(File.ReadAllText(path + slot.ToString() + Application.version), privateKey));

        slotNumber = newDATA.slotNumber;
        GameController.savedFloor = newDATA.savedFloor;
        GameController.inTime = newDATA.inTime;
        GameController.getCoin = newDATA.getCoin;
        GameController.kills = newDATA.kills;

        GameController.playerLife = newDATA.playerLife;

        GameController.coin = newDATA.coin;

        GameController.myEquips = newDATA.myEquips;

        GameController.tutorial = newDATA.tutorial;

        print("LOAD!");
    }

    public void NewData(int slotnum)
    {
        newDATA = new GameData(slotnum);

        slotNumber = newDATA.slotNumber;
        GameController.savedFloor = newDATA.savedFloor;
        GameController.inTime = newDATA.inTime;
        GameController.getCoin = newDATA.getCoin;
        GameController.kills = newDATA.kills;

        GameController.playerLife = newDATA.playerLife;

        GameController.coin = newDATA.coin;

        GameController.myEquips = newDATA.myEquips;

        GameController.tutorial = newDATA.tutorial;

        print("NEW!");
    }

    public void RemoveData()
    {
        File.Delete(path + slotNumber.ToString() + Application.version);

        Destroy(GameObject.Find("INVENTORY"));

        print("REMOVED");
    }
}
