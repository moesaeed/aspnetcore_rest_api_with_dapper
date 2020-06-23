using aspnetcore_rest_api_with_dapper.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace aspnetcore_rest_api_with_dapper.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        private  IDbConnection _connection { get { return new SqlConnection(_connectionString); }}

        public ProductRepository()
        {
            // TODO: It will be refactored...
            _connectionString = "Data Source=MOSAEED8-09\\MSSQLSERVER2016;Initial Catalog=Delete;Integrated Security=False;User ID=sa;Password=sa_123";
        }

        public async Task<Product> GetAsync(long id)
        {
            using (IDbConnection dbConnection = _connection)
            {
                string query = @"SELECT [Id] ,[CategoryId]
                                ,[Name]
                                ,[Description]
                                ,[Price]
                                ,[CreatedDate]
                                FROM [dbo].[Products]
                                WHERE [Id] = @Id";

                var product = await dbConnection.QueryFirstOrDefaultAsync<Product>(query, new{ @Id = id });

                return product;
            }
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            //TODO: Paging...
            using (IDbConnection dbConnection = _connection)
            {
                string query = @"SELECT [Id] ,[CategoryId]
                                ,[Name]
                                ,[Description]
                                ,[Price]
                                ,[CreatedDate]
                                FROM [dbo].[Products]";

                var product = await dbConnection.QueryAsync<Product>(query);

                return product;
            }
        }

        public async Task AddAsync(Product product)
        {
            using (IDbConnection dbConnection = _connection)
            {
                string query = @"INSERT INTO [dbo].[Products] (
                                [Id],
                                [CategoryId],
                                [Name],
                                [Description],
                                [Price],
                                [CreatedDate]) VALUES (
                                @Id,
                                @CategoryId,
                                @Name,
                                @Description,
                                @Price,
                                @CreatedDate)";

                await dbConnection.ExecuteAsync(query, product);
            }
        }
    }
}