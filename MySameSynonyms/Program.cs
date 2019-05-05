using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySameSynonyms
{
    class Program
    {
        private static string IndexPath = "C:\\LuceneIndex";

        static void Main(string[] args)
        {
            var fsDir = FSDirectory.Open(IndexPath);
            try
            {
                Analyzer a2 = new MySameAnalyzer(new SimpleSamewordContext());
                String txt = "You";
                using (var idxWriter = new IndexWriter(
                    fsDir, 
                    a2, 
                    true,
                    IndexWriter.MaxFieldLength.UNLIMITED //不限定欄位內容長度
                    ))
                {
                    Document doc = new Document();
                    doc.Add(new Field("content", txt, Field.Store.YES, Field.Index.ANALYZED));
                    idxWriter.AddDocument(doc);

                    idxWriter.Commit();
                    idxWriter.Optimize();
                }

                var searcher = new IndexSearcher(fsDir, true);
                //指定欄位名傳入參數
                QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "content", a2);
                Query q = qp.Parse("I");
                var hits = searcher.Search(q, 10); //查詢前10筆
                Console.WriteLine($"找到{hits.TotalHits}筆");
                foreach (var doc1 in hits.ScoreDocs)
                {
                    Console.WriteLine($"{searcher.Doc(doc1.Doc).Get("content")}");
                }
                Console.ReadKey();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}
