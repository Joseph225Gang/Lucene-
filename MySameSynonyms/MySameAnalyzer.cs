using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySameSynonyms
{
    public class MySameAnalyzer : StandardAnalyzer
    {
        private SimpleSamewordContext samewordContext;

        public MySameAnalyzer(SimpleSamewordContext swc) : base(Lucene.Net.Util.Version.LUCENE_30)
        {
            samewordContext = swc;
        }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            //TokenStream result = new StandardTokenizer(Lucene.Net.Util.Version.LUCENE_30, reader);

            TokenStream result = new WhitespaceTokenizer(reader);
            return new MySameTokenFilter(result, samewordContext);
        }
    }
}
