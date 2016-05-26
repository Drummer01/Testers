using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

//https://github.com/Self747/Testers/blob/master/Testers/Log.cs
namespace Testers
{
    [Serializable]
    public class Log
    {
        public List<string> logs { get; set; }

        delegate void SetTextCallback(object text);

        public static Form1 mainControlForm;

        public static ListBox logSource;

        public Log()
        {
            logs = new List<string>();
        }

        public void push(string text, bool showOnForm = false)
        {
            logs.Add(text);
            if (showOnForm)
                show(text);
        }

        public static void show(string text)
        {
            __setText(text);
        }

        public void Save(Stream fs)
        {
            //FileStream fs = new FileStream(path, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            try
            {
                serializer.Serialize(fs, this);
            }
            catch (SerializationException e)
            {
                Log.show("Failed to serialize. Reason: " + e.Message);
            }
            finally
            {
                fs.Close();
            }
        }

        public static Log Open(Stream fs)
        {
            Log savedLogs = null;
            //FileStream fs = new FileStream("logs.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(Log));
            try
            {
                savedLogs = (Log)serializer.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Log.show("Failed to deserialize. Reason: " + e.Message);
            }
            finally
            {
                fs.Close();
            }
            return savedLogs;
        }

        public EfficiencyChartItems CalculateEfficiency()
        {                   
            EfficiencyChartItems chartItems = new EfficiencyChartItems();
            Regex status = new Regex(@"\[(.+)\]", RegexOptions.ECMAScript);
            Regex correct = new Regex(@".+написана\s([\w+]+)\sє\s([а-я]+)", RegexOptions.ECMAScript);

            foreach (string item in this.logs)
            {               
                Match statusMatch = status.Match(item);
                switch (statusMatch.Groups[0].Value)
                {           
                    case "[ТЕСТУЄ]":
                        Match correctMatch = correct.Match(item);
                        string name = correctMatch.Groups[1].Value;
                        if (!name.Equals(""))
                        {   
                            bool progCorrect = correctMatch.Groups[2].Value.Equals("правильна");
                            chartItems[name].update(progCorrect);
                        }   
                        break;
                }           
            }
            return chartItems;
        }
       

        /*
        ################################
                     PIVATE
        ################################
        */

        private static void __setText(object text)
        {

            if (logSource.InvokeRequired)
            {
                SetTextCallback cb = new SetTextCallback(__setText);
                mainControlForm.Invoke(cb, new object[] { text });
            }
            else
            {
                logSource.Items.Add(text ?? "null");
                int visibleItems = logSource.ClientSize.Height / logSource.ItemHeight;
                logSource.TopIndex = Math.Max(logSource.Items.Count - visibleItems + 1, 0);
            }
        }
    }
}
