using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CustomOp.Objects
{
    public class OpData
    {

        protected Dictionary<String, MutableObject> data;

        public OpData()
        {
            data = new Dictionary<String, MutableObject>();

        }

        public bool contains(String id)
        {
            return data.ContainsKey(id);
        }

        public bool containsType(string id, Type t)
        {
            return data.ContainsKey(id) && data[id].getType().Equals(t) ;
        }

        public Object getObject(String id)
        {
            return data[id].getData();
        }


        public int getInt(String id)
        {
            if (!data.ContainsKey(id) || !data[id].getType().Equals(typeof(Int32)))
            {
                throw new Exception($"Error in GetInt Method in OpData. Invalid variable name ID: {id}");
            }
           return (int)(data[id].getData());
        }

        public string getString(string id)
        {
            if (!data.ContainsKey(id) || !data[id].getType().Equals(typeof(string)))
            {
                throw new Exception($"Error in GetString Method in OpData. Invalid variable name ID: {id}");
            }
            return (string)(data[id].getData());
        }

        public List<int> getIntList(string id)
        {
            if (!data.ContainsKey(id) || !data[id].getType().Equals(typeof(List<int>)))
            {
                throw new Exception($"Error in GetIntList Method in OpData. Invalid variable name ID: {id}");
            }
            return (List<int>)(data[id].getData());
        }

        public Dictionary<string,string> getMap(string id)
        {
            if (!data.ContainsKey(id) || !data[id].getType().Equals(typeof(Dictionary<string, string>)))
            {
                throw new Exception($"Error in GetMap Method in OpData. Invalid variable name ID: {id}");
            }
            return (Dictionary<string, string>)(data[id].getData());
        }

        public DataTable getDataTable(string id)
        {
            if (!data.ContainsKey(id) || !data[id].getType().Equals(typeof(DataTable)))
            {
                throw new Exception($"Error in GetDataTable Method in OpData. Invalid variable name ID: {id}");
            }
            return (DataTable)(data[id].getData());
        }

        public List<string> getStringList(string id)
        {
            if (!data.ContainsKey(id) || !data[id].getType().Equals(typeof(List<string>)))
            {
                throw new Exception($"Error in GetDataTable Method in OpData. Invalid variable name ID: {id}");
            }
            return (List<string>)(data[id].getData());
        }

        public Dictionary<string, MutableObject> getMutableMap(string id)
        {
            if (!data.ContainsKey(id) || !data[id].getType().Equals(typeof(Dictionary<string, MutableObject>)))
            {
                throw new Exception($"Error in getMutableMap Method in OpData. Invalid variable name ID: {id}");
            }
            return (Dictionary<string, MutableObject>)(data[id].getData());
        }

        public JSONObject getJSONObject(string id)
        {
            if (!data.ContainsKey(id) || !data[id].getType().Equals(typeof(JSONObject)))
            {
                throw new Exception($"Error in getJSONObject Method in OpData. Invalid variable name ID: {id}");
            }
            return (JSONObject)(data[id].getData());
        }


        public void put(String name, Object i)
        {
            if (data.ContainsKey(name))
            {
                data.Remove(name);
            }
            data.Add(name, new MutableObject(i));
        }


        public OpData merge(OpData otherData)
        {
            foreach(var key in otherData.data.Keys)
            {
                //Base OpData has priority in merge conflicts
                if (!data.Keys.Contains(key))
                {
                    data.Add(key, otherData.data[key]);
                }
            }
            return this;
        }

        public void remove(string id)
        {
            if (id!=null && data.ContainsKey(id) && data[id]!=null)
            {
                data.Remove(id);
            }
        }

        public List<string> getIDs()
        {
            return data.Keys.ToList<string>();
        }

    }
}
