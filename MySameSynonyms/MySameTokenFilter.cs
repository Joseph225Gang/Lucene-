using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySameSynonyms
{
    public class MySameTokenFilter : TokenFilter
    {
        
        private ITermAttribute _termAttr;
        private ITypeAttribute _typeAtt;
        private IPositionIncrementAttribute _posIncrAtt;
        private AttributeSource.State _current = null;
        private Stack<String> sames = null;
        private SimpleSamewordContext samewordContext;
        private Queue<string> _synonymTokenQueue = new Queue<string>();

        public MySameTokenFilter(TokenStream input, SimpleSamewordContext samewordContext) : base(input)
        {
            _termAttr = AddAttribute<ITermAttribute>();
            
            _typeAtt = AddAttribute<ITypeAttribute>();
            _posIncrAtt = AddAttribute<IPositionIncrementAttribute>();
            sames = new Stack<String>();
            this.samewordContext = samewordContext;
        }


        public override bool IncrementToken()
        {
            //if (sames.Count() > 0)
            if (_synonymTokenQueue.Count() > 0)
            {
                //将元素出栈，并且获取这个同义词
                //String str = sames.Pop();
                //还原状态
                RestoreState(_current);

                //_termAttr.SetTermBuffer(sames.Pop());
                _termAttr.SetTermBuffer(_synonymTokenQueue.Dequeue());
                _typeAtt.Type = "<SYNONYM>";
                _posIncrAtt.PositionIncrement = 0;
                return true;
            }

            if (!this.input.IncrementToken()) return false;

            if (addSames(_termAttr.Term))
            {
                //如果有同义词将当前状态先保存
                _current = CaptureState();
            }
            return true;
        }

        private Boolean addSames(String name)
        {
            String[] sws = samewordContext.getSamewords(name);
            if (sws != null)
            {
                foreach (String str in sws)
                {
                    //sames.Push(str);
                    _synonymTokenQueue.Enqueue(str);
                }
                return true;
            }
            return false;
        }
    }
}
