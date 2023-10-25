using Dapper;
using Microsoft.Data.SqlClient;
using Sup2Marché.Model;
using System.Data;

namespace Sup2Marché.Repo
{
    public class UserRepo
    {

        private readonly IConfiguration? _configuration;

        public UserRepo(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public UserEntity? GetById(int id)
        {
            var oEntity = new UserEntity();
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));

            var aEntity = oSqlConnection.Query<UserEntity>(oEntity.ReadUser("id"),
                new { id = id }).ToList();
            aEntity.ForEach(o => oEntity = o);

            oSqlConnection.Close();
            return (oEntity.email == null && oEntity.password == null ) ? null : oEntity;
        }

        public UserEntity GetByEmail(string email)
        {
            var oEntity = new UserEntity();
            var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));

            var aEntity = oSqlConnection.Query<UserEntity>(oEntity.ReadUser("email"),
                new { email = email }).ToList();
            aEntity.ForEach(o => oEntity = o);

            oSqlConnection.Close();
            return (oEntity.email == null && oEntity.password == null ) ? null : oEntity;
        }

        public bool Update(UserEntity oEntity)
        {
            try
            {
                var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
                var oSqlParam1 = new SqlParameter("@id", oEntity.id);
                var oSqlParam2 = new SqlParameter("@email", oEntity.email);
                var oSqlParam3 = new SqlParameter("@password", BCrypt.Net.BCrypt.HashPassword(oEntity.password));


                var oSqlCommand = new SqlCommand(oEntity.UpdateUser());

                oSqlCommand.Parameters.AddRange(new SqlParameter[] { oSqlParam1, oSqlParam2, oSqlParam3});

                oSqlCommand.Connection = oSqlConnection;
                oSqlConnection.Open();

                oSqlCommand.ExecuteNonQuery();
                oSqlConnection.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                UserEntity oEntity = new UserEntity();
                var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
                var oSqlParam = new SqlParameter("@id", id);
                var oSqlCommand = new SqlCommand(oEntity.DeleteUser(), oSqlConnection);
                oSqlCommand.Parameters.Add(oSqlParam);

                oSqlConnection.Open();

                oSqlCommand.ExecuteNonQuery();
                oSqlConnection.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Insert(UserEntity oEntity)
        {
            if (GetByEmail(oEntity.email) == null)
            {
                var oSqlConnection = new SqlConnection(_configuration?.GetConnectionString("SQL"));
                var oSqlParam1 = new SqlParameter("@email", oEntity.email);
                var oSqlParam2 = new SqlParameter("@password", BCrypt.Net.BCrypt.HashPassword(oEntity.password));

                oSqlConnection.Open();
                var oSqlTransaction = oSqlConnection.BeginTransaction();
                try
                {
                    var oSqlCommand = new SqlCommand(oEntity.CreateUser(), oSqlConnection, oSqlTransaction);

                    oSqlCommand.Parameters.AddRange(new SqlParameter[] { oSqlParam1, oSqlParam2});

                    oSqlTransaction.Commit();
                    return Convert.ToInt32(oSqlCommand.ExecuteScalar());
                }
                catch (Exception)
                {
                    oSqlTransaction.Rollback();
                    return -1;
                }
                finally
                {
                    oSqlConnection.Close();
                }
            }
            else
            {
                throw new Exception("Utilisateur déjà existant.");
            }
        }

        public string Login(UserEntity oEntity)
        {
            Model.Tools.JwtHandler jwtHandler = new Model.Tools.JwtHandler(_configuration.GetSection("JwtSettings")["SecretKey"]);
            string? token = null;

            var user = GetByEmail(oEntity.email);

            if (user == null)
            {
                throw new Exception("Utilisateur introuvable.");
            }
            else
            {
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(oEntity.password, user.password);

                if (!isPasswordValid)
                {
                    throw new Exception("Mot de passe incorrect.");
                }

                token = jwtHandler.GenerateToken(user.id.ToString(), user.email);

            }

            return token;
        }

    }
}
