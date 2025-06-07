using System;
using System.Security.Cryptography;
using System.Text;

namespace RestAPI_ProcessValidated_PartnerInfo.Helper
{
    public static class SignatureHelper
    {
        public static string GenerateSignature(
        string timestamp,
        string partnerKey,
        string partnerRefNo,
        string totalAmount,
        string encryptedPartnerCode)
        {
            string raw = $"{timestamp}{partnerKey}{partnerRefNo}{totalAmount}{encryptedPartnerCode}";

            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw));

            string hexHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            Console.WriteLine("Hex Hash: " + hexHash); // For debugging

            string base64Signature = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(hexHash)    
            );

            return base64Signature;
        }
    }
}
