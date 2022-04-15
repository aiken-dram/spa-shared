using System.Security.Cryptography;
using System.Text;

namespace Shared.Application.Helpers;

/// <summary>
/// Static class for working with encryption
/// </summary>
public static class EncryptorHelper
{
    /// <summary>
    /// Generates MD5 hash for text
    /// </summary>
    /// <param name="text">text</param>
    /// <returns>MD5 hash</returns>
    public static string? MD5Hash(string text)
    {
        var md5 = MD5.Create();
        md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
        byte[]? res = md5.Hash;

        if (res == null)
            return null;

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < res.Length; i++)
            sb.Append(res[i].ToString("x2"));

        return sb.ToString();
    }
}
