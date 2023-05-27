using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomOp.Objects
{
    internal static class Accumulators
    {

        public static Accumulator<StringBuilder, String> getStringAcc(string acc)
        {
            switch (acc)
            {
                case "JSonList": return new JSonListAccumulator();
            }
            return null;
        }

        public static string strAccumulate(List<string> input, string accType, string startingString)
        {
            Accumulator<StringBuilder, String> acc = getStringAcc(accType);
            StringBuilder usedValue = new StringBuilder(startingString);
            usedValue = acc.preAction(usedValue);
            foreach(string val in input)
            {
                usedValue = acc.combine(usedValue, val);
            }
            usedValue = acc.postAction(usedValue);
            return usedValue.ToString();
        }
        

    }

    //K is the accumulator object, sometimes this will be the same as T but for strings it will be StringBuilder, String for Efficiency
    abstract class  Accumulator<K,T>
    {

        public abstract K combine(K e1, T e2);

        public virtual K preAction(K element) { return element; }
        public virtual K postAction(K element) { return element; }
    }

    class JSonListAccumulator : Accumulator<StringBuilder, String>
    {
        public override StringBuilder combine(StringBuilder e1, String e2)
        {
            e1.Append(",");
            e1.Append(e2);
            return e1 ;
        }


        public override StringBuilder postAction(StringBuilder element)
        {
            base.postAction(element);

            return new StringBuilder("[" + element.ToString().Substring(1, element.Length - 1) + "]");
        }
    }
}
