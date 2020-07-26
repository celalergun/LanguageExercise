using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Xml.Serialization;

namespace LangExercise
{
    public class LanguageData
    {
        public string Name { get; set; }
        
        public string Code { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        private Dictionary<string, string> Entries { get; set; }

        [XmlIgnore]
        public bool IsModified { get; set; }

        public LanguageData()
        {
            IsModified = false;
            Entries = new Dictionary<string, string>();
            DateCreated = DateTime.Now;
            DateModified = DateCreated;
        }

        public LanguageData(string name, string code): this()
        {
            Name = name;
            Code = code;
        }

        Random random = new Random();

        public KeyValuePair<string, string> GetRandomEntry()
        {
            if (Entries.Count == 0)
                return new KeyValuePair<string, string>();

            List<KeyValuePair<string, string>> list = Entries.ToList();
            return list[random.Next() % list.Count];
        }

        public static LanguageData LoadFromFile(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LanguageData));

            string content = File.ReadAllText(fileName);
            using (StringReader sr = new StringReader(content))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LanguageData));
                var tmp = (LanguageData)xmlSerializer.Deserialize(sr);
                tmp.IsModified = false;
                return tmp;
            }
        }

        public void SaveToFile(string fileName)
        {
            using (StreamWriter sr = new StreamWriter(fileName))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LanguageData));
                xmlSerializer.Serialize(sr, this);
                IsModified = false;
            }
        }

        public void Add(string sourceSentence, string targetSentence)
        {
            Entries.Add(sourceSentence, targetSentence);
            IsModified = true;
        }

        public void Clear()
        {
            Entries.Clear();
            IsModified = true;
        }

        public bool Remove(string sourceSentence)
        {
            bool deleted = Entries.Remove(sourceSentence);
            if (deleted)
                IsModified = true;
            return deleted;
        }

        public int Count() => Entries.Count;

    }
}