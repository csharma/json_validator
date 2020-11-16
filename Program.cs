using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace json_validator
{
    class Program
    {
         

        public float parseToFloat(string str)
        {

            string[] parts = str.Split(" ");
            float message = float.Parse(parts[0]);
            return message;

        }

        public bool loadJson(string filePath, string fileName)
        {
            List<float> messageList = new List<float>();
            List<string> testList = new List<string>();
            List<int> thresholdList = new List<int>();
            bool flag = false;
            //string fileName = "/Users/cartik_sharma/Projects/json_validator/json_validator/study_Complete_1_0__validation.json";
            //string fileName = "/Users/cartik_sharma/Projects/json_validator/json_validator/study_Sleep+Wrist_1_0__validation.json";
            // string fileName = "/Users/cartik_sharma/Projects/json_validator/json_validator/testStudy.json";
            // List<Dictionary<string,string>> items = new List<Dictionary<string,string>>();
            fileName = filePath + fileName;
            using (StreamReader r = new StreamReader(fileName))
            {
                 string json = r.ReadToEnd();
               // m = (ModelRoot[])JsonConvert.DeserializeObject(json);
                // dynamic result  = JsonConvert.DeserializeObject<ModelRoot>(json);
                //Console.WriteLine(m[0].models[0]);
                var resultObjects = AllChildren(JObject.Parse(json)).First(c => c.Type == JTokenType.Array && c.Path.Contains("results"))
            .Children<JObject>();
              
                foreach (JObject result in resultObjects)
                {
                    foreach (JProperty property in result.Properties())
                    {
                        // do something with the property belonging to result
                        string name = property.Name;
                        string value = property.Value.ToString();
                        if (name == "message")
                        {
                            Console.WriteLine("message " + parseToFloat(value));
                            messageList.Add(parseToFloat(value));
                        }
                        else if (name == "test")
                        {
                            Console.WriteLine("test " + value);
                            testList.Add(value);
                        }
                        else if (name == "threshold")
                        {
                            Console.WriteLine("threshold " + value);
                            thresholdList.Add(Int32.Parse(value));
                        }
                    }
                    
                }
                
            }
            for (int i = 0; i < messageList.Count; i++)
            {
                if (messageList[i] >= (thresholdList[i] / 3600000))
                {
                     flag = true;
                }
                else
                {
                     flag = false;
                    break;
                }
            }
            return flag;

        }

        public class testData
        {
            public string test { get; set;}
            public string message { get; set; }
            public string threshold { get; set; }

        }
        public void writeJsonToFile(string filePath, string fileName)
        { 
            List<testData> _data = new List<testData>();
            _data.Add(new testData()
            {
                message = "1.15 hours",
                test = "sleep study",
                threshold = "3600000"
            }) ;

            _data.Add(new testData()
            {
                message = "0.25 hours",
                test = "sleep study",
                threshold = "3600000"

            });
            _data.Add(new testData()
            {
                message = "0.5 hours",
                test = "sleep study",
                threshold = "3600000"

            });
            _data.Add(new testData()
            {
                message = "0.5 hours",
                test = "sleep study",
                threshold = "1800000"

            });
            _data.Add(new testData()
            {
                message = "7.5 hours",
                test = "sleep study",
                threshold = "36000000"

            });

            string json = JsonConvert.SerializeObject(_data.ToArray());
            fileName = filePath + fileName;
            //write string to file
            //System.IO.File.WriteAllText(@"/Users/cartik_sharma/Projects/json_validator/json_validator/testStudy_2.json", json);
            System.IO.File.WriteAllText(@fileName, json);
        }
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }

        public bool testLoadJson()
        {

            string filePath = "/Users/cartik_sharma/Projects/json_validator/json_validator/";
            string fileName = "testStudy.json";
            bool testPass = false;

            bool testVal = loadJson(filePath, fileName);

             if (testVal != true)
            {
                testPass = true;

            }
            else
                testPass= false;
            return testPass;
        }

        static void Main(string[] args)
            {
                Program obj = new Program();
            string filePath = "/Users/cartik_sharma/Projects/json_validator/json_validator/";
                string fileName = "study_Complete_1_0__validation.json";
                bool retVal = obj.loadJson(filePath, fileName);
            fileName = "testStudy.json";
            //obj.writeJsonToFile(filePath, fileName);
            bool testPass = obj.testLoadJson();
            Console.WriteLine("  Study complete " + retVal);
            Console.WriteLine(" Unit testing pass" + testPass);
            } 
    }
}



