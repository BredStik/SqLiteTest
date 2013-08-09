using NHibernate.Criterion;
using SqLiteTest.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SqLiteTest
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //IList<User> users = new List<User>();

                NHibernateSessionFactoryHelper.ActionWithSession(session => {
                    Response.Write(string.Format("Found {0} users in database.", session.CreateCriteria<User>().SetProjection(Projections.RowCount()).UniqueResult<int>()));
                });

                NHibernateSessionFactoryHelper.ActionWithSession(session =>
                {
                    Response.Write(string.Format("Found {0} documents in database.", session.CreateCriteria<Document>().SetProjection(Projections.RowCount()).UniqueResult<int>()));
                });

                //var sw = new Stopwatch();
                //sw.Start();

                //for (int index = 0; index < 300; index++)
                //{
                //    NHibernateSessionFactoryHelper.ActionWithSession(session =>
                //    {
                //        session.Save(new User { Name = index.ToString(), Password = index.ToString() });
                //        session.Flush();
                //    }, true);
                //};

                //sw.Stop();

                //Response.Write(string.Format("Inserting 300 users in parallel took {0}.", sw.Elapsed));
            }
        }

        protected void createDocButton_Click(object sender, EventArgs e)
        {
            NHibernateSessionFactoryHelper.ActionWithSession(session =>
            {
                session.Save(new Document { Name = "Doc1", ContentLength = 0, ContentType = "text/plain" });
            }, true);
        }
    }
}