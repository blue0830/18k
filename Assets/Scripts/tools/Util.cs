using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;

public class Util
{
    public static byte[] StrToFixLenByte(string str , byte[] byteArray)
    {
        byte[] strByte = Encoding.UTF8.GetBytes(str);
        Array.Copy(strByte, 0, byteArray, 0, strByte.Length); 

        return byteArray;
    }

    public static char[] StrToFixLenChar(string str, char[] charArray)
    {

        char[] strChar = str.ToCharArray();

        Array.Copy(strChar, 0, charArray, 0, strChar.Length); ;

        return charArray;
    }

    public static string FixLenChartoString(char[] charArray)
    {
        if (charArray == null)
            return "";

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
       
        for (int i = 0; i < charArray.Length; ++i)
        {
            if (charArray[i] != '\0')
            {
                sb.Append(charArray[i]);
            }
        }

        return sb.ToString().Trim();
    }

    public static string BytetoString(byte[] charArray)
    {
        Encoding gb2312 = Encoding.GetEncoding("GB2312");
		return gb2312.GetString(charArray).TrimEnd('\0');
    }

    public static byte[] StringtoFixedByte_GB2312(string str, byte[] byteArray)
    {
        Encoding gb2312 = Encoding.GetEncoding("GB2312");
        byte[] strByte = gb2312.GetBytes(str);
        Array.Copy(strByte, 0, byteArray, 0, strByte.Length);

        return byteArray;
    }

    public static string GetMd5Hash(string input)
    {
        MD5 md5Hash = MD5.Create();
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();
        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    public static bool isNumberic(string message)
    {
        System.Text.RegularExpressions.Regex rex =
        new System.Text.RegularExpressions.Regex(@"^\d+$");
        if (rex.IsMatch(message))
        {
            return true;
        }
        else
            return false;
    }
}