﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class JSONListToTableOperation : Operation
    {
        public JSONListToTableOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string JsonListString = data.getString("JSonListString");
            DataTable dt = new DataTable();
            JSONObject obj = new JSONObject(JsonListString);
            if (obj.list == null)
            {
                throw new Exception("Error in JSonListToTable Operation: JSonString is not a List!");
            }
            foreach(JSONObject map in obj.list)
            {
                if (map.map == null)
                {
                    //If the list doesn't contain maps make the table a single column with a name of ListName
                    Dictionary<string, string> newRow = new Dictionary<string, string>();
                    newRow.Add("ListName", map.ToString());
                    dt.addRow(newRow);
                }
                else
                {
                    dt.addRow(processJSonMap(map));
                }
                
            }

            data.put("JSONDataTable", dt);
        }

        private Dictionary<string, string> processJSonMap(JSONObject map)
        {
            Dictionary<string, string> toRet = new Dictionary<string, string>();
            foreach(string key in map.map.Keys)
            {
                toRet.Add(key, map.map[key].ToString());
            }
            return toRet;
        }
    }


}
