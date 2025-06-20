using RestAPI_ProcessValidated_PartnerInfo.DTO;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RestAPI_ProcessValidated_PartnerInfo.Service
{
    public interface IAesEncryptionService
    {
        public Result<string> Encrypt(string rawText);
        public Result<string> Decrypt(string cypherText);

        public Task<Result<string>> EncryptAsync(string rawText);
        public Task<Result<string>> DecryptAsync(string cypherText);

    }

    public class AesEncryptionService : IAesEncryptionService, IDisposable
    {
        private readonly ILoggerService loggerService;
        private readonly Settings settings; 
        private readonly byte[] _key;
        private readonly Aes _aes;

        public AesEncryptionService(ILoggerService loggerService, Settings settings)
        {
            this.loggerService = loggerService;
            this.settings = settings;

            byte[] key = Encoding.UTF8.GetBytes(this.settings.Key);

            if (key == null || key.Length != 32)
            {
                this.loggerService.Error($"Key - {key.ToString()} does not meet requirements", default);
                throw new ArgumentException("Key does not meet the 256 bit length", nameof(key));
            }

            this._key = key;
            this._aes = Aes.Create();
            this._aes.Mode = CipherMode.CBC;
            this._aes.Padding = PaddingMode.PKCS7;
        }

        public Result<string> Encrypt(string rawText)
        {
            
            byte[] iv;
            byte[] encryptedBytes;

            bool isInvalidText = string.IsNullOrWhiteSpace(rawText);

            if (isInvalidText)
            {
                return Result<string>.Failed($"[AesEncryptionService][Encrypt] - Raw text is blank {rawText}");
            }

            _aes.GenerateIV();
            iv = _aes.IV;

            using (var encryptor = _aes.CreateEncryptor(_key, iv))
            {
                var plainBytes = Encoding.UTF8.GetBytes(rawText);
                encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }

            byte[] combined = new byte[iv.Length + encryptedBytes.Length];
            Buffer.BlockCopy(iv, 0, combined, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, 0, combined, iv.Length, encryptedBytes.Length);

            var cypherText = Convert.ToBase64String(combined);

            return Result<string>.Success(cypherText);
        }

        public Result<string> Decrypt(string cypherText)
        {
            byte[] iv = new byte[16];
            byte[] combined;
            byte[] encryptedBytes;
            byte[] decryptedBytes;

            bool isInvalidText = string.IsNullOrWhiteSpace(cypherText);

            int byteLength = (cypherText.Length * 3) / 4;
            Span<byte> buffer = new byte[byteLength];

            if (isInvalidText)
            {
                return Result<string>.Failed($"[AesEncryptionService][Decrypt] - Cypher text is blank {cypherText}");
            }

            bool IsDecodable = Convert.TryFromBase64String(cypherText, buffer, out int writtenBytes);
            if (!IsDecodable)
            {
                return Result<string>.Failed($"[AesEncryptionService][Decrypt] - Cypher text provided is not a Base64 string");
            }

            combined = Convert.FromBase64String(cypherText);
            Buffer.BlockCopy(combined, 0, iv, 0, iv.Length);

            int cypherTextLength = combined.Length - iv.Length;
            encryptedBytes = new byte[cypherTextLength];

            Buffer.BlockCopy(combined, iv.Length, encryptedBytes, 0, cypherTextLength);

            using (var decryptor = _aes.CreateDecryptor(_key, iv))
            {
                decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            }

            var decryptedText = Encoding.UTF8.GetString(decryptedBytes);

            return Result<string>.Success(decryptedText);
        }

        public Task<Result<string>> EncryptAsync(string rawText)
        {
            return Task.FromResult(this.Encrypt(rawText));
        }

        public Task<Result<string>> DecryptAsync(string cypherText)
        {
            return Task.FromResult(this.Decrypt(cypherText));
        }

        public void Dispose()
        {
            _aes?.Dispose();
        }
    }
}
