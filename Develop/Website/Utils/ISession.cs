using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Utils
{
    public interface ISession
    {
        void Clear();

        bool IsLogin { get; set; }

        string FullName { get; set; }

        string NickName { get; set; }
    }
}