using Dapper;
using Microsoft.Data.SqlClient;
using Sup2Marché.Model;
using System.Data;

namespace Sup2Marché.Repo
{
    public class produitRepo
    {

        private readonly string configurationString;
        public produitRepo(IConfiguration configuration) => configurationString = configuration.GetConnectionString("SQL");
        public void Created(produitEntity model)
        {
            using (IDbConnection db = new SqlConnection(configurationString))
            {
                string insertQuery = model.CreatedProduit();

                db.Execute(insertQuery, new
                {
                    model.id,
                    model.nom,
                    model.quantite,
                    model.categorie
                });
            }
        }
        public produitEntity Read(int id)
        {
            produitEntity model = new produitEntity();
            using (IDbConnection db = new SqlConnection(configurationString))
            {
                return db.QueryFirstOrDefault<produitEntity>(model.ReadProduit(), new { id = id });
            };
        }
        public void Update(produitEntity model)
        {
            using (IDbConnection db = new SqlConnection(configurationString))
            {
                db.Execute(model.UpdateProduit(), model);
            };
        }

        public void Delete(int id)
        {
            produitEntity model = new produitEntity();
            using (IDbConnection db = new SqlConnection(configurationString))
            {
                db.QueryFirstOrDefault<produitEntity>(model.DeleteProduit(), new { id = id });
            };
        }

        public List<produitEntity> ReadListProd()
        {
            List<produitEntity> listItem = new List<produitEntity>();
            produitEntity model = new produitEntity();
            var oSqlConnection = new SqlConnection(configurationString);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(model.Readall(), oSqlConnection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                listItem.Add(new produitEntity
                {
                    id = (int)row["id"],
                    nom = (string)row["nom"],
                    quantite = (int)row["quantite"],
                    categorie = (int)row["categorie"]

                });
            }

            return listItem;
        }

    }
}
