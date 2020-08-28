using api.DataAccess;
using api.Interfaces;
using api.ModelViews;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DropdownlistService));

        public AuthenticationService()
        {

        }

        public AuthenticationData login(string username, string password)
        {
            using (var ctx = new ConXContext())
            {
                //su_user user = ctx.user
                //    .Include("departments")
                //    .Include("user_mac")
                //    .Where(z => z.USER_ID == username.ToUpper())
                //    .SingleOrDefault();

                var vuser = username.ToUpper();

                string sql = "select a.user_id username , a.user_name name , a.user_password, a.dept_code , a.active statusId , b.dept_namet dept_name , c.mc_code  from su_user a , department b , pd_mapp_user_mac c where a.dept_code=b.dept_code and a.user_id = c.user_id and a.user_id = :p_user_id and c.status='A'";

                AuthenticationData user = ctx.Database.SqlQuery<AuthenticationData>(sql, new OracleParameter("p_user_id", vuser)).FirstOrDefault();




                if (user == null)
                {
                    throw new Exception("รหัสผู้ใช้หรือรหัสผ่านไม่ถูกต้อง / ไม่ได้กำหนด Machine");
                }
                //else if (auth == null)
                //{
                //    throw new Exception("ยังไมได้กำนหด หน่วยงาน");
                //}
                else
                {
                    if (!user.user_password.Equals(password))
                    {
                        throw new Exception("รหัสผู้ใช้หรือรหัสผ่านไม่ถูกต้อง");
                    }

                    if (!user.statusId.Equals("Y"))
                    {
                        throw new Exception("สถานะผู้ใช้งานนี้ถูกยกเลิก");
                    }



                    //if (!user.user_mac.STATUS.Equals("A"))
                    //{
                    //    throw new Exception("ไม่มีการกำหนด Machine");
                    //}
                }

                //whmobileprnt_default whmobileprnt = ctx.mobileprnt_def
                //  .Where(z => z.MC_CODE == user.user_mac.MC_CODE).SingleOrDefault();

                //auth_function auth = ctx.auth
                //   .Where(z => z.USER_ID == user.USER_ID && z.FUNCTION_ID == "PDOPTM_WEB").SingleOrDefault();

                string sqlp = "select series_no from whmobileprnt_default where mc_code = :p_mc_code ";
                string printer = ctx.Database.SqlQuery<string>(sqlp, new OracleParameter("p_mc_code", user.mc_code)).FirstOrDefault();

                string sqla = "select dept_code from auth_function where user_id = :p_user_id and function_id = 'PDOPTM_WEB'";
                string auth = ctx.Database.SqlQuery<string>(sqla, new OracleParameter("p_user_id", vuser)).FirstOrDefault();

                string def_printer = null;
                string wc_code = null;

                if (auth == null)
                {
                    throw new Exception("ยังไมได้กำนหด หน่วยงาน");
                }

                if (printer == null)
                {
                    def_printer = "";
                }
                else
                {
                    def_printer = printer;
                }

                if (auth == null)
                {
                    wc_code = "";
                }
                else
                {
                    wc_code = auth;
                }



                AuthenticationData data = new AuthenticationData()
                {
                    username = user.username,
                    name = user.name,
                    dept_code = user.dept_code,
                    dept_name = user.dept_name,
                    mc_code = user.mc_code,
                    def_printer = def_printer,
                    def_wc_code = wc_code,
                    statusId = user.statusId,
                    menuGroups = new List<ModelViews.menuFunctionGroupView>(),
                };



               
                    data.menuGroups = getUserRole((string)user.username);
                


                return data;
            }

        }

        public List<menuFunctionGroupView> getUserRole(string userId)
        {
            using (var ctx = new ConXContext())
            {

                //List<su_menu> menu = ctx.menu.SqlQuery("select  LEVEL , MENU_ID, MENU_NAME , MENU_TYPE, LINK_NAME , MAIN_MENU , ICON_NAME from su_menu where EXISTS   (select MENU_ID  from su_role_menu  WHERE MENU_ID= SU_MENU.MENU_ID AND EXISTS (select role_id from su_user_role  WHERE ROLE_ID= SU_ROLE_MENU.ROLE_ID  and user_id = :param1)) CONNECT BY PRIOR MENU_ID = MAIN_MENU START WITH  menu_id ='MOB0000000' ORDER BY MENU_ID", new OracleParameter("param1", userId)).ToList();
                string sql = "select  level , menu_id, menu_name , menu_type, main_menu , icon_name , link_name from su_menu where EXISTS   (select MENU_ID  from su_role_menu  WHERE MENU_ID= SU_MENU.MENU_ID AND EXISTS (select role_id from su_user_role  WHERE ROLE_ID= SU_ROLE_MENU.ROLE_ID  and user_id = :param1)) CONNECT BY PRIOR MENU_ID = MAIN_MENU START WITH  menu_id ='MOB0000000' ORDER BY MENU_ID";

                List<menuView> menu = ctx.Database.SqlQuery<menuView>(sql, new OracleParameter("param1", userId)).ToList();

                List<menuFunctionView> functionViews = new List<menuFunctionView>();


                foreach (var x in menu)
                {
                        
                            menuFunctionView view = new menuFunctionView()
                            {
                                menuFunctionGroupId = x.main_menu,
                                menuFunctionId = x.menu_id,
                                menuFunctionName = x.menu_name,
                                iconName = x.icon_name,
                                menuURL = x.link_name,
                            };


                            functionViews.Add(view);    
                  
                }



                List<menuFunctionGroupView> groupView = new List<menuFunctionGroupView>();

                foreach (var x in menu)
                {
                    menuFunctionGroupView view = new menuFunctionGroupView()
                    {
                        menuFunctionGroupId = x.menu_id,
                        menuFunctionGroupName = x.menu_name,
                        iconName = x.icon_name,
                        menuFunctionList = functionViews
                                .Where(o => o.menuFunctionGroupId == x.menu_id)
                                .ToList()

                    };

                    if (x.menu_type == "M" && x.menu_id != "MOB0000000")
                    {
                        groupView.Add(view);
                    }
                }

                return groupView;


            }
        }


    }
}