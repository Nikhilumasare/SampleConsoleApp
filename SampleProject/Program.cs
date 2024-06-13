using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject
{
    public class Program 
    {
        static void Main(string[] args)
        {
            #region test
            //string dateString = "31-03-2024 00:00:00 13:08:59.92";
            //string date = dateString.Split(' ')[0];
            ////var date1 = Convert.ToDateTime(date);
            //string time = dateString.Split(' ')[2];
            //string datetime1 = date + " " + time;
            //string format = "dd-MM-yyyy HH:mm:ss.ff";
            //DateTime dateTime = DateTime.ParseExact(datetime1, format, CultureInfo.InvariantCulture);
            //string date2 = dateTime.ToString("dd MMM yyyy");
            //string time2 = dateTime.ToString("HH:mm:ss");
            //-----------------------------------------------------------------------------------------------
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            //string version = fvi.FileVersion;
            //Console.WriteLine(version);
            //-----------------------------------------------------------------------------------------------
            //int var = 20;
            //int* p = &var;
            //Console.WriteLine("Data is: {0} ", var);
            //Console.WriteLine("Address is: {0}", (int)p);
            //Console.ReadKey();
            #endregion

            // Input: a = "11", b = "1"
            //Output: "100"

            //Convert.ToInt32("1001101", 2).ToString();
            //string a = "10100000100100110110010000010101111011011001101110111111111101000000101111001110001111100001101", b = "110101001011101110001111100110001010100001101011101010000011011011001011101111001100000011011110011";
            //var res = Convert.ToInt32(ConvertBinaryToDecimal(a)) + Convert.ToInt32(ConvertBinaryToDecimal(b));
            //string binaryRes = ConvertDecimalToBinary(res.ToString());

            //string dateString = "16-05-2024 00:00:00 11:13:15.51";
            //string format = "dd-MM-yyyy HH:mm:ss ff";
            //DateTime result = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

            //AuthReq authReq = new AuthReq() { username = "admin", password = "password123" };
            //string result = HttpClientWebRequest(authReq).Result;
            //Console.WriteLine("Response Using HttpClient: " + result);

            //string result2 = HttpWebRequest(authReq).Result;
            //Console.WriteLine("Response Using HttpWebRequest: " + result2);

            string key = "abcd123456789012";
            string iv = "abcd123456789034";
            AESEncryptionDecryption aESEncryptionDecryption = new AESEncryptionDecryption(key, iv);

            string plainText = "Aes Encryption and Decryption";
            string encryptedText = aESEncryptionDecryption.Encryption(plainText);
            Console.WriteLine(encryptedText);

            string decryptedText = aESEncryptionDecryption.Decryption(encryptedText);
            Console.WriteLine(decryptedText);

            string date = "20-12-2010";
            DateTime dateObj = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string formatedDate = dateObj.ToString("yyyy-MM-dd hh:mm:ss");



            
        }

        public int Add(int num1 , int num2)
        {
            var res = num1 + num2;
            return res;
        }
        public async static Task<string> HttpClientWebRequest(object reqObj)
        {
            string response = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://restful-booker.herokuapp.com");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonString = JsonConvert.SerializeObject(reqObj);
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                //var bytedata = Encoding.UTF8.GetBytes(jsonString);
                //var byteContent = new ByteArrayContent(bytedata);
                HttpResponseMessage responseMessage = await httpClient.PostAsync("auth", stringContent);

                response = responseMessage.Content.ReadAsStringAsync().Result;
            }
            return response;
        }
        public async static Task<string> HttpWebRequest(object reqObj)
        {
            string URL = "https://restful-booker.herokuapp.com/auth";
            string response = string.Empty;
            string reqObjStr = JsonConvert.SerializeObject(reqObj);
            var reqData = Encoding.UTF8.GetBytes(reqObjStr);
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(URL);
            //httpRequest.Accept = "application/json";
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(reqData, 0, reqData.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpRequest.GetResponse();
            StreamReader responseReader = new StreamReader(httpWebResponse.GetResponseStream());
            response = responseReader.ReadToEnd();
            return response;
        }
        public static string ConvertBinaryToDecimal(string binaryNum)
        {
            return Convert.ToInt32(binaryNum, 2).ToString();
        }
        public static string ConvertDecimalToBinary(string decimalNum)
        {
            int num = Convert.ToInt32(decimalNum);
            return Convert.ToString(num, 2);
        }
        
    }
    public class AESEncryptionDecryption
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        public AESEncryptionDecryption(string keyString, string ivString)
        {
            _key = Encoding.UTF8.GetBytes(keyString);
            _iv = Encoding.UTF8.GetBytes(ivString);
        }

        public string Encryption(string plainText)
        {
            string encryptedText = string.Empty;
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using(MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        StreamWriter streamWriter = new StreamWriter(cryptoStream);
                        streamWriter.Write(plainText);
                    }
                    encryptedText = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            return encryptedText;
        }
        public string Decryption(string encryptedText)
        {
            string decryptedText = string.Empty;
            using(var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        StreamReader streamReader = new StreamReader(cryptoStream);
                        decryptedText = streamReader.ReadToEnd();
                    }
                    
                }
            }
            return decryptedText;
        }
    }
    
    public class AuthReq
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
