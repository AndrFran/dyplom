using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.Script.Serialization;
namespace WpfApplication2
{
    class JsonParser
    {
        JavaScriptSerializer serializer;
        public JsonParser()
        {
            serializer = new JavaScriptSerializer();
        }
        public Dictionary<string, object> Deserialize(string json)
        {
            serializer.Deserialize<Dictionary<string, object>>(json);
            Dictionary<string, object> ParsedFunction =
            serializer.Deserialize<Dictionary<string, object>>(json);
            return ParsedFunction;
        }
    }

}