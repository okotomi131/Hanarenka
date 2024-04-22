using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography; // AESを使うために追加
using System.IO;
using System.Text;

public static class AesExample
{
    // 初期化ベクトル"<半角16文字[1byte=8bit, 8bit*16=128bit]>"
    //private static string[] AES_IV_256 = new string[50];  // 毎回ランダムにする場合
    private static Dictionary<int, string> AES_IV_256 = new Dictionary<int, string>();
    // 暗号化鍵<半角32文字[8bit*32文字=256bit]>
    private const string AES_KEY_256 = @"kxvuA&k|WDRkzgG47yAsuhwFzkQZMNf3";
    private static string ENCRYPTFILE_PATH; //セーブデータパス
    private const int STAGE_NUM = 40;       //総ステージ数

    // 暗号化のための関数
    // 引数は暗号化したいデータ(string), ステージ番号(int)
    public static string EncryptStringToBytes_Aes(string plainText, int stageNum)
    {
        byte[] encrypted;

        using(Aes aesAlg = Aes.Create())
        {
            // AESの設定
            aesAlg.BlockSize = 128;
            aesAlg.KeySize = 256;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            // ステージ番号が設定されていなかったら処理
            //if (!AES_IV_256.ContainsKey(stageNum))
            //{
            //    // ステージ番号と16桁のランダムな英数字をセットで設定
            //    AES_IV_256.Add(stageNum, System.Guid.NewGuid().ToString("N").Substring(0, 16));
            //}
            //AES_IV_256[stageNum] = System.Guid.NewGuid().ToString("N").Substring(0, 16);  // 毎回ランダムにする場合
            //Debug.Log("暗号番号：" + stageNum + "　IV：" + AES_IV_256[stageNum]);
            aesAlg.IV = Encoding.ASCII.GetBytes(AES_IV_256[stageNum]);
            aesAlg.Key = Encoding.ASCII.GetBytes(AES_KEY_256);

            // ストリーム変換を実行するための暗号化機能を作成
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // 暗号化に使用するストリームを作成
            using(MemoryStream msEncrypt = new MemoryStream())
            {
                using(CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using(StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // すべてのデータをストリームに書き込み
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // メモリ ストリームから暗号化されたバイトを返却(byte[] → stringに変換)
        return System.Convert.ToBase64String(encrypted);
    }


    // 復号のための関数
    // 引数は暗号化されたデータ(string), ステージ番号(int)
    public static string DecryptStringFromBytes_Aes(string cipherText, int stageNum)
    {
        string plaintext = null;

        using (Aes aesAlg = Aes.Create())
        {
            // AESの設定(暗号と同じ)
            aesAlg.BlockSize = 128;
            aesAlg.KeySize = 256;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            
            // ステージ番号が設定されていたら処理
            if (AES_IV_256.ContainsKey(stageNum))
            {
                //Debug.Log("復号番号：" + stageNum + "　IV：" + AES_IV_256[stageNum]);
                aesAlg.IV = Encoding.ASCII.GetBytes(AES_IV_256[stageNum]);
                aesAlg.Key = Encoding.ASCII.GetBytes(AES_KEY_256);

                // ストリーム変換を実行するデクリプターを作成
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // 復号化に使用するストリームを作成
                using (MemoryStream msDecrypt = new MemoryStream(System.Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // 復号化ストリームから復号化されたバイトの読み取り、文字列に配置
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        return plaintext;
    }

    public static void CreateEncryptData()
    {
        ENCRYPTFILE_PATH = Path.Combine(Application.dataPath, "Save", "Encrypt.csv");    //UnityEditor上での暗号化ファイルパス

        //- ディレクトリパスを取得する
        string directoryPath = new FileInfo(ENCRYPTFILE_PATH).Directory.FullName;
        //- ディレクトリが存在しているか
        if (!Directory.Exists(directoryPath))
        {
            //- 存在していなかったら作成する
            Directory.CreateDirectory(directoryPath);
        }
        //- テキストを書き込む
        using (StreamWriter sw = new StreamWriter(ENCRYPTFILE_PATH, false, Encoding.ASCII))
        {
            string line;
            //- ステージ分初期化する
            for (int i = 0; i < STAGE_NUM; i++)
            {
                line = System.Guid.NewGuid().ToString("N").Substring(0, 16);
                AES_IV_256.Add(i, line);
                sw.WriteLine(line);
            }
        }
    }

    public static void EncryptDataLoad()
    {
        ENCRYPTFILE_PATH = Path.Combine(Application.dataPath, "Save", "Encrypt.csv");    //UnityEditor上での暗号化ファイルパス
        //- パスの位置にファイルが存在するか
        if (File.Exists(ENCRYPTFILE_PATH))
        {
            //- テキストを読み込む
            using (StreamReader sr = new StreamReader(ENCRYPTFILE_PATH, Encoding.ASCII)) //using:自動的にクローズする
            {
                string line;    //1行分の文字列を読み込む
                int i = 0;
                //- 読み込んでいる行がnullじゃないかつ行数分ループする
                while ((line = sr.ReadLine()) != null && i < STAGE_NUM)
                {
                    if (!AES_IV_256.ContainsKey(i))
                        AES_IV_256.Add(i, line);
                    i++;

                    //--- 復号の確認 ---
                    //Debug.Log("復号化：" + DecryptFlag);
                }
            }
        }
        else
        {
            //- ファイルが存在しなければ生成する
            CreateEncryptData();
        }
    }
}
