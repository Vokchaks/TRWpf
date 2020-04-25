using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;

namespace TRWpf
{
    public class ComPort : IDisposable
    {
        const string pattern1 = @"[0-9]{3},[0-9]{5}";
        const string pattern2 = @"[0-9ABCDEF]{8}] [0-9]{5}";
        readonly string comPort;
        SerialPort serialPort = null;
        private bool disposed = false;
        static IFormatProvider provider = CultureInfo.CreateSpecificCulture("ru-RU");


        public ComPort() : this(null)
        {
        }
        public ComPort(string com)
        {
            if (com == null)
                comPort = ConfigurationManager.AppSettings["Port"] ?? "NOT FOUND";
            else
                comPort = com;

            Debug.WriteLine("PORT: " + comPort);

            serialPort = new SerialPort(comPort)
            {
                BaudRate = 9600,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None,
                RtsEnable = true,
                ReadTimeout = 500,
                WriteTimeout = 500
            };

            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            if (serialPort.IsOpen)
                return;
            try
            {
                serialPort.Open();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            SerialPort sp = (SerialPort)sender;
            string message = sp.ReadLine();
            int i = 0;
            Match m = Regex.Match(message, pattern1, RegexOptions.IgnoreCase);

            if (m.Success)
                i = NumCardStringToInt(m.Value);
            else
            {
                m = Regex.Match(message, pattern2, RegexOptions.IgnoreCase);
                if (m.Success)
                {

                    string[] str = m.Value.Split("] ");
                    if (str.Length == 2)
                    {
                        int ln = int.Parse(str[1], provider);
                        int fn = (Convert.ToInt32(str[0], 16) & 65535) & 255;
                        i = (fn << 16) + ln;

                    }
                }
            }

            if (i > 0)
            {
                var msg = new ComMessage { Msg = i.ToString(provider) };
                Messenger.Default.Send<ComMessage>(msg);
            }
        }

        public bool IsOpen => serialPort.IsOpen;

        static int NumCardStringToInt(string strCard)
        {
            int result = 0;
            string[] str = strCard.Split(',');
            if (str.Length == 2)
            {
                int fn = int.Parse(str[0], provider);
                int ln = int.Parse(str[1], provider);
                result = (fn << 16) ^ ln;
            }
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Close()
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                if (serialPort.IsOpen)
                {

                    serialPort.Close();
                    serialPort = null;
                }
                disposed = true;
            }
        }

        ~ComPort()
        {
            Dispose(false);
        }
    }
}
