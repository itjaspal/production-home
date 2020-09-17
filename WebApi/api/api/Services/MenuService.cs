using api.DataAccess;
using api.Interfaces;
using api.ModelViews;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web;

namespace api.Services
{
    public class MenuService : IMenuService
    {
        public CommonSearchView<MasterMenuView> Search(MasterMenuSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                //define model view
                CommonSearchView<MasterMenuView> view = new ModelViews.CommonSearchView<ModelViews.MasterMenuView>()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,

                    datas = new List<ModelViews.MasterMenuView>()
                };

                //query data
                //List<su_menu> menu = ctx.menu
                //    .Where(x => (x.MENU_ID.Contains(model.menuFunctionId) || model.menuFunctionId == "")
                //    && (x.MENU_NAME.Contains(model.menuFunctionName) || model.menuFunctionName == "")
                //    && (x.MENU_ID.Contains("MOB"))
                //    )
                //    .OrderBy(o => o.MENU_ID)
                //    .ToList();

                string sql = "select  menu_id, menu_name , menu_type, link_name , main_menu , icon_name from su_menu where menu_id like '%'||:p_menu_id||'%' and menu_name like '%'||:p_menu_name||'%' and menu_id  like 'MOBB%' order by menu_id";


                List<menuView> menu = ctx.Database.SqlQuery<menuView>(sql, new OracleParameter("p_menu_id", model.menuFunctionId), new OracleParameter("p_menu_name", model.menuFunctionName)).ToList();



                //count , select data from pageIndex, itemPerPage
                view.totalItem = menu.Count;
                menu = menu.Skip(view.pageIndex * view.itemPerPage)
                    .Take(view.itemPerPage)
                    .ToList();

                //prepare model to modelView
                foreach (var i in menu)
                {
                    view.datas.Add(new ModelViews.MasterMenuView()
                    {
                        menuFunctionId = i.menu_id,
                        menuFunctionGroupId = i.main_menu,
                        menuFunctionName = i.menu_name,
                        iconName = i.icon_name,
                        menuURL = i.link_name,
                    });
                }

                //return data to contoller
                return view;
            }
        }


        public void Update(MasterMenuView model)
        {
            using (var ctx = new ConXContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                   
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();

                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                        new OracleParameter("p_menu_id", model.menuFunctionId),
                        new OracleParameter("p_menu_name", model.menuFunctionName),
                        new OracleParameter("p_main_menu", model.menuFunctionGroupId),
                        new OracleParameter("p_link_name", model.menuURL),
                        new OracleParameter("p_icon_name", model.iconName),

                    };
                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update su_menu set menu_name = :p_menu_name , main_menu =:p_main_menu , link_name = :p_link_name , icon_name =:p_icon_name  where menu_id = :p_menu_id";

                   
                    oraCommand.ExecuteNonQuery();

                    conn.Close();


                    scope.Complete();
                }
            }
        }


        public MasterMenuView GetInfo(string code)
        {
            using (var ctx = new ConXContext())
            {
                
                string sql = "select menu_id, menu_name , menu_type, link_name , main_menu , icon_name from su_menu where menu_id = :p_menu_id";

                menuView menu = ctx.Database.SqlQuery<menuView>(sql, new OracleParameter("p_menu_id", code)).SingleOrDefault();

                return new MasterMenuView
                {
                    menuFunctionId = menu.menu_id,
                    menuFunctionGroupId = menu.main_menu,
                    menuFunctionName = menu.menu_name,
                    menuURL = menu.link_name,
                    iconName = menu.icon_name
                    
                };
            }
        }

        
    }
}