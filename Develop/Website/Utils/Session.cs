using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Website.Utils
{
    public class Session:ISession
    {
        public void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        public bool IsLogin
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.IS_LOGIN] == null)
                    return Convert.ToBoolean(GetValueByKeyWord(Constant.IS_LOGIN));
                else
                    return Convert.ToBoolean(HttpContext.Current.Session[Constant.IS_LOGIN].ToString());
            }
            set { HttpContext.Current.Session[Constant.IS_LOGIN] = value; }
        }

        public string FullName
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.FULL_NAME] == null)
                    return null;
                else
                    return HttpContext.Current.Session[Constant.FULL_NAME].ToString();
            }
            set { HttpContext.Current.Session[Constant.FULL_NAME] = value; }
        }

        public string NickName
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.FULL_NICKNAME] == null)
                    return null;
                else
                    return HttpContext.Current.Session[Constant.FULL_NICKNAME].ToString();
            }
            set { HttpContext.Current.Session[Constant.FULL_NICKNAME] = value; }
        }


        private string GetValueByKeyWord(string Keyword)
        {
            return WebConfigurationManager.AppSettings[Keyword];
        }
    }
}