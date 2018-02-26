using UnityEngine;
using System.Collections;

namespace Speech.XunFei
{
    public class Config
    {
        public string appid;
        public Config(string appid)
        {
            this.appid = appid;
        }
        public override string ToString()
        {
            return string.Format("appid={0}", appid);
        }
    }
}