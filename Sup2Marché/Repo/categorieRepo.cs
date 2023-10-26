using Dapper;
using Microsoft.Data.SqlClient;
using Sup2Marché.Model;
using System.Data;

namespace Sup2Marché.Repo
{
    public class categorieRepo
    {
        private readonly string configurationString;
        public categorieRepo(IConfiguration configuration) => configurationString = configuration.GetConnectionString("SQL");
        public void Created(categorieEntity model)
        {
            using (IDbConnection db = new SqlConnection(configurationString))
            {
                string insertQuery = model.CreatedCategorie();

                db.Execute(insertQuery, new
                {
                    model.id,
                    model.nom
                });
            }
        }
        public categorieEntity Read(int id)
        {
            categorieEntity model = new categorieEntity();
            using (IDbConnection db = new SqlConnection(configurationString))
            {
                return db.QueryFirstOrDefault<categorieEntity>(model.ReadCategorie(), new { id = id });
            };
        }
        public void Update(categorieEntity model)
        {
            using (IDbConnection db = new SqlConnection(configurationString))
            {
                db.Execute(model.UpdateCategorie(), model);
            };
        }

        public void Delete(int id)
        {
            categorieEntity model = new categorieEntity();
            using (IDbConnection db = new SqlConnection(configurationString))
            {
                db.QueryFirstOrDefault<categorieEntity>(model.DeleteCategorie(), new { id = id });
            };
        }

        public List<categorieEntity> ReadListCat()
        {
            List<categorieEntity> listItem = new List<categorieEntity>();
            categorieEntity model = new categorieEntity();
            var oSqlConnection = new SqlConnection(configurationString);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(model.ReadCategorie(), oSqlConnection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                listItem.Add(new categorieEntity
                {
                    id = (int)row["id"],
                    nom = (string)row["nom"]

                });
            }

            return listItem;
        }
    }
}
