using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomOp.Objects
{
    internal static class Accumulators
    {

        public static Accumulator<String> getStringAcc(string acc)
        {
            switch (acc)
            {
                case "JSonList": return new JSonListAccumulator();
            }
            return null;
        }

        public static string strAccumulate(List<string> input, string accType, string startingString)
        {
            Accumulator<String> acc = getStringAcc(accType);
            string usedValue = startingString;
            usedValue = acc.preAction(usedValue);
            foreach(string val in input)
            {
                usedValue = acc.combine(usedValue, val);
            }
            usedValue = acc.postAction(usedValue);
            return usedValue;
        }
        

    }

    abstract class  Accumulator<T>
    {

        public abstract T combine(T e1, T e2);

        public virtual T preAction(T element) { return element; }
        public virtual T postAction(T element) { return element; }
    }

    class JSonListAccumulator : Accumulator<String>
    {
        public override String combine(String e1, String e2)
        {
            string s1 = e1.ToString();
            string s2 = e2.ToString();
            String toRet = s1+ "," + s2;
            return toRet ;
        }


        public override String postAction(String element)
        {
            base.postAction(element);

            return "[" + element.Substring(1, element.Length - 1) + "]";
        }
    }
}
