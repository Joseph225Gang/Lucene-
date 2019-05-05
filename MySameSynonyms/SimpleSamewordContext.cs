using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySameSynonyms
{
    public class SimpleSamewordContext
    {
        Dictionary<String, String[]> maps = new Dictionary<String, String[]>();
        public SimpleSamewordContext()
        {
            maps.Add("我", new String[] { "自", "咱"});
            maps.Add("I", new string[] { "You", "They" });
        }
    
        public String[] getSamewords(String name)
        {
            return maps.ContainsKey(name) ? maps[name] : null;
        }
    }
}
