using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BulletHellGame.DataAccess.DataLoaders
{
    public static class JsonUtility
    {
        public static JObject LoadJson(string filePath)
        {

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            string json = File.ReadAllText(filePath);

            var jsonObj = JObject.Parse(json);

            return jsonObj;
        }

    }
}
