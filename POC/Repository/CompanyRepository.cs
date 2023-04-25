using Dapper;
using POC.Context;
using POC.Contracts;
using POC.Dto;
using POC.Entities;
using System.Data;

namespace POC.Repository
{
    public class CompanyRepository :ICompanyRepository
    {
        private readonly DapperContext _context;
        public CompanyRepository(DapperContext context)=> _context = context;

        public async  Task AddRandom()
        {
            var procedureName = "procedure_name";

            using (var connection = _context.CreateConnection())
            {
                var company = await connection.ExecuteAsync(procedureName,commandType: CommandType.StoredProcedure);
            }

        }

        public async  Task<Company> CreateCompany(CompanyForCreationDto company)
        {
            var query = "insert into Companies(Name, Address, Country) Values (@Name, @Address, @Country)"+
                "Select cast (scope_identity() as int)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var createdCompany = new Company
                {
                    Name = company.Name,
                    Address = company.Address,
                    Country = company.Country,
                    Id = id
                };
                return createdCompany;
            }
        }

        public async Task DeleteCompany(int id)
        {
            var query = "DELETE FROM Companies WHERE Id = @Id";
            
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new {id});

            }
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var query = "Select * from Companies";
            using (var connection = _context.CreateConnection())
            {
                var companies = await connection.QueryAsync<Company>(query);
                return companies.ToList();
            }
        }

        public async Task<Company> GetCompany(int id)
        {
            var query = "Select * from Companies where Id = @Id";
            using (var connection = _context.CreateConnection()) 
            {
                var company = await connection.QuerySingleOrDefaultAsync<Company>(query, new { id });
                return company;
            }
        }

        public async Task UpdateCompany(int id, CompanyForUpdateDto company)
        {
            var query = "Update Companies Set name=@name, address = @address, country=@country where Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);   

            }

        }
    }
}
