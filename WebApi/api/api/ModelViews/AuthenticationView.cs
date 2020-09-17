using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class AuthenticationParam
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class AuthenticationData
    {
        public string username { get; set; }
        public string name { get; set; }
        public string user_password { get; set; }
        public string dept_code { get; set; }
        public string dept_name { get; set; }
        public string statusId { get; set; }
        public string def_printer { get; set; }
        public string def_wc_code { get; set; }
        public string mc_code { get; set; }
        public virtual List<UserEntityPrvlg> userEntityPrvlgList { get; set; }
        public virtual List<UserWcPrvlg> userWcPrvlgList { get; set; }
        public List<menuFunctionGroupView> menuGroups { get; set; }

       
    }

    public class AuthenticationUserRoleParam
    {
        public string userRoleId { get; set; }
    }

    public class UserEntityPrvlg
    {
        public string entity_code { get; set; }
        public string entity_name { get; set; }
        public string user_id { get; set; }
    }

    public class UserWcPrvlg
    {
        public string wc_code { get; set; }
        public string wc_name { get; set; }
    }

    public class menuFunctionGroupView
    {
        public string menuFunctionGroupId { get; set; }

        public string menuFunctionGroupName { get; set; }

        public string iconName { get; set; }
        //public int orderDisplay { get; set; }
        //public string menuGroup { get; set; }


        //public bool isPC { get; set; }

        public virtual List<menuFunctionView> menuFunctionList { get; set; }
    }

    public class menuFunctionView
    {
        public string menuFunctionId { get; set; }
        public string menuFunctionGroupId { get; set; }
        public string menuFunctionName { get; set; }
        public string menuURL { get; set; }
        public string iconName { get; set; }
       // public int orderDisplay { get; set; }

        //public Dictionary<string, Boolean> actions { get; set; }
    }

    public class menuView
    {
        public int level { get; set; }
        public string menu_id { get; set; }
        public string menu_name { get; set; }
        public string menu_type { get; set; }
        public string link_name { get; set; }
        public string main_menu { get; set; }
        public string icon_name { get; set; }
    }
}