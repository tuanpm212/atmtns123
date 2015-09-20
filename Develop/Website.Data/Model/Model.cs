using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Data
{
    public class ModelHome
    {
        public List<Slide> Sliders { get; set; }
        public About about { get; set; }
        public List<Product> Products { get; set; }
        public List<Service> Services { get; set; }
    }

    public class LoginModel
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
    }
}
