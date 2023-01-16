using Newtonsoft.Json;
using Trisatech.AspNet.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.Helpers
{
    public class ApiAuthHelper
    {
        /// <summary>
        /// Database hash256
        /// Token Mobile Encrypted
        /// Web App Decrypted base64string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isRemember"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="verificationKey"></param>
        /// <returns></returns>
        public static string Set(object obj, bool isRemember, string encryptionKey, string verificationKey)
        {
            string strJsonUser = JsonConvert.SerializeObject(obj);
            
            CryptoService cryptoService = new CryptoService(encryptionKey, verificationKey);

            var byteToken = Encoding.UTF8.GetBytes(strJsonUser);
            var result = cryptoService.Protect(byteToken);
            var token = Convert.ToBase64String(result);

            return token;
        }

        public static T Get<T>(string token, string encryptionKey, string verificationKey) where T : class
        {
            T userAuth = null;

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            
            CryptoService cryptoService = new CryptoService(encryptionKey, verificationKey);

            var resultByte = cryptoService.Unprotect(Convert.FromBase64String(token));

            userAuth = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(resultByte));

            return userAuth;
        }

        public static void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
