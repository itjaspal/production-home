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
               
                var vuser = username.ToUpper();

                string sql = "select a.user_id username , a.user_name name , a.user_password, a.dept_code , a.active statusId , b.dept_namet dept_name , c.mc_code  from su_user a , department b , pd_mapp_user_mac c where a.dept_code=b.dept_code and a.user_id = c.user_id and a.user_id = :p_user_id and c.status='A'";

                AuthenticationData user = ctx.Database.SqlQuery<AuthenticationData>(sql, new OracleParameter("p_user_id", vuser)).FirstOrDefault();




                if (user == null)
                {
                    throw new Exception("รหัสผู้ใช้หรือรหัสผ่านไม่ถูกต้อง / ไม่ได้กำหนด Machine");
                }
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
                }

               

                //string sqlp = "select series_no from whmobileprnt_default where mc_code = :p_mc_code ";
                //string printer = ctx.Database.SqlQuery<string>(sqlp, new OracleParameter("p_mc_code", user.mc_code)).FirstOrDefault();

                //string sqla = "select dept_code from auth_function where user_id = :p_user_id and function_id = 'PDOPTM_WEB'";
                //string auth = ctx.Database.SqlQuery<string>(sqla, new OracleParameter("p_user_id", vuser)).FirstOrDefault();

                //string def_printer = null;
                //string wc_code = null;

                //if (auth == null)
                //{
                //    throw new Exception("ยังไมได้กำนหด หน่วยงาน");
                //}

                //if (printer == null)
                //{
                //    def_printer = "";
                //}
                //else
                //{
                //    def_printer = printer;
                //}

                //if (auth == null)
                //{
                //    wc_code = "";
                //}
                //else
                //{
                //    wc_code = auth;
                //}


                List<UserEntityPrvlg> entityViews = new List<UserEntityPrvlg>();

                string sqle = "select a.entity_code , a.entity_namet entity_name from entity a , su_user_entity b where a.entity_code=b.entity_allowed and  a.entity_ctrl='H10' and user_id=:p_user_id";
                List<UserEntityPrvlg> entity = ctx.Database.SqlQuery<UserEntityPrvlg>(sqle, new OracleParameter("p_user_id", vuser)).ToList();

                foreach (var y in entity)
                {
                    UserEntityPrvlg eView = new UserEntityPrvlg()
                    {
                        entity_code = y.entity_code,
                        entity_name = y.entity_name,
                        user_id = vuser
                        
                    };

                    entityViews.Add(eView);

                }


                List<UserWcPrvlg> wcViews = new List<UserWcPrvlg>();

                string sqlw = "select a.dept_code wc_code , b.wc_tdesc wc_name from auth_function a , wc_mast b where a.dept_code = b.wc_code and a.function_id='PDOPTHM' and a.user_id=:p_user_id";
                List<UserWcPrvlg> wc = ctx.Database.SqlQuery<UserWcPrvlg>(sqlw, new OracleParameter("p_user_id", vuser)).ToList();

                foreach (var z in wc)
                {
                    UserWcPrvlg wView = new UserWcPrvlg()
                    {
                        wc_code = z.wc_code,
                        wc_name = z.wc_name

                    };

                    wcViews.Add(wView);

                }


                AuthenticationData data = new AuthenticationData()
                {
                    username = user.username,
                    name = user.name,
                    dept_code = user.dept_code,
                    dept_name = user.dept_name,
                    mc_code = user.mc_code,
                    //def_printer = def_printer,
                    //def_wc_code = wc_code,
                    statusId = user.statusId,
                    userEntityPrvlgList = entityViews,
                    userWcPrvlgList = wcViews,
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
                string sql = "select  level , menu_id, menu_name , menu_type, main_menu , icon_name , link_name from su_menu where EXISTS   (select MENU_ID  from su_role_menu  WHERE MENU_ID= SU_MENU.MENU_ID AND EXISTS (select role_id from su_user_role  WHERE ROLE_ID= SU_ROLE_MENU.ROLE_ID  and user_id = :param1)) CONNECT BY PRIOR MENU_ID = MAIN_MENU START WITH  menu_id ='MOBB000000' ORDER BY MENU_ID";

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

                    if (x.menu_type == "M" && x.menu_id != "MOBB000000")
                    {
                        groupView.Add(view);
                    }
                }

                return groupView;


            }
        }


    }
}