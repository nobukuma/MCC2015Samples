using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Collections;
using Json.NETMF;

namespace NetduinoApplication2
{
    public class Program
    {
        public static void Main()
        {
            Program prog = new Program();
            prog.ReadTemperatreLoop();
            return;
        }

        private const string ApplicationUrl = @"http://thermomobileservice.azure-mobile.net/tables/TemperatureData";
        private const string ApplicationKey = @"key";

        private const int MeasureIntervalInMillSec = 5000;
        private const int MeasureCount = 5;

        private const float LatitudeOfNSC = 35.144935f;
        private const float LongitudeOfNSC = 136.910849f;

        private const ushort Address = 0x48;
        private const int ClockFrequency = 100;
        private const int Timeout = 100;

        private I2CDevice.Configuration config;
        private I2CDevice i2c;

        public Program()
        {
            this.config = new I2CDevice.Configuration(Address, ClockFrequency);
            this.i2c = new I2CDevice(config);
        }

        private void ReadTemperatreLoop()
        {
            for (int i = 0; ; i++)
            {
                var tmp = this.ReadTemperatre();
                Debug.Print("temp[" + i + "][" +
                    DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "]=" + tmp);
                this.SendTemperatureData(tmp);
                Thread.Sleep(MeasureIntervalInMillSec);
            }
            return;
        }

        private void ReadI2C(byte address, byte[] data)
        {
            i2c.Execute(
                new I2CDevice.I2CTransaction[1] {
                    I2CDevice.CreateWriteTransaction(new Byte[] { address})},
                    Timeout);
            i2c.Execute(
                new I2CDevice.I2CTransaction[1] {
                    I2CDevice.CreateReadTransaction(data) },
                    Timeout);
            return;
        }

        private float ReadTemperatre()
        {
            byte[] data = new byte[8];
            ReadI2C(0, data);

            ushort val;
            val = (ushort)(data[0] << 8);
            val |= data[1];
            val >>= 3;

            int ival = (int)val;
            if ((val & (0x8000 >> 3)) != 0)
            {
                ival = ival - 8192;
            }

            float temp = (float)ival / 16.0f;

            return temp;
        }

        private void SendTemperatureData(float temp)
        {
            Hashtable ht = CreateTemperatureData(DateTime.Now, temp, LatitudeOfNSC, LongitudeOfNSC);

            JsonSerializer ser = new JsonSerializer();
            string jsonData = ser.Serialize(ht);

            byte[] postDataBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
            int contentLength = postDataBytes.Length;

            Uri serviceUri = new Uri(ApplicationUrl);
            HttpWebRequest req = HttpWebRequest.Create(serviceUri) as HttpWebRequest;
            try
            {
                req.Method = "POST";
                req.ContentType = "application/json";
                req.ContentLength = contentLength;
                req.Headers.Add("X-ZUMO-APPLICATION", ApplicationKey);

                var requestStream = req.GetRequestStream();
                requestStream.Write(postDataBytes, 0, contentLength);
                requestStream.Close();                

                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
                using (var responseStream = resp.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(responseStream);
                    var str = sr.ReadToEnd();

                    Debug.Print("StatusCode=" + resp.StatusCode);
                    Debug.Print("Response=" + str);
                    
                    //var result = ser.Deserialize(str);
                    //Debug.Print(result.GetType().ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        private Hashtable CreateTemperatureData(
            DateTime dateTime,
            float temp,
            float latitude,
            float longitude)
        {
            Hashtable ht = new Hashtable();
            //ht["id"] = Guid.NewGuid().ToString();
            ht["dateTime"] = dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            ht["temperature"] = temp;
            ht["latitude"] = latitude;
            ht["longitude"] = longitude;
            return ht;
        }

    }
}
