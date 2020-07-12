using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Identifier : MonoBehaviour
{
    const string MagicKey = "KIWIKIWIKIWIKIWIKIWIKIWIKIWIKIWI";
    const string SeedFile = "seed";
    const string PrivFile = "priv";

    public void SetSessionId(string sessionId)
    {
        SessionId = sessionId;
    }

    public string SessionId { private set; get; }

    string Load(string path, string file)
    {
        var str = string.Empty;
        try
        {
            using (var reader = new StreamReader(Path.Combine(path, file)))
            {
                str = reader.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            Debug.Log("error: " + e.Message);
        }
        return str;
    }

    void Save(string path, string file, string data)
    {
        using (var writer = new StreamWriter(Path.Combine(path, file)))
        {
            writer.WriteLine(data);
            writer.Flush();
        }
    }

    public void SetPrivateCode(string priv)
    {
        Save(Application.persistentDataPath, PrivFile, priv);
    }

    public string GetPrivateKey()
    {
        return Load(Application.persistentDataPath, PrivFile);
    }

    public string GetSeed(string platform, long ticks)
    {
        Debug.Log(Application.persistentDataPath);
        var seed = Load(Application.persistentDataPath, SeedFile);
        if (seed.Equals(string.Empty))
        {
            Debug.Log(GetPlatformIdentifier() + " " + platform + " " + ticks.ToString() + "," + MagicKey);
            seed = GenerateHash(GetPlatformIdentifier() + platform + ticks.ToString(), MagicKey) + "," + ticks.ToString();
            Save(Application.persistentDataPath, SeedFile, seed);
        }

        return seed;
    }

    public string AddNonce(string seed, long nonce)
    {
        Debug.Log(seed + " " + nonce.ToString() + "," + MagicKey);
        return GenerateHash(seed + nonce.ToString(), MagicKey);
    }

    public string GenerateHash(string plane, string key)
    {
        var encode = new UTF8Encoding();
        var planeBytes = encode.GetBytes(plane);
        var keyBytes = encode.GetBytes(key);
        var sha = new HMACSHA256(keyBytes);
        var hashBytes = sha.ComputeHash(planeBytes);
        return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
    }

    public string GetPlatformIdentifier()
    {
        return GenerateHash(SystemInfo.deviceUniqueIdentifier, MagicKey);
    }
}
