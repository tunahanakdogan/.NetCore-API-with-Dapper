using POC.Dto;
using POC.Entities;

namespace POC.Contracts
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<Company>> GetCompanies();
        public Task<Company> GetCompany(int id);
        //<> ile return edilecek tipi veriyoruz aslında
        public  Task<Company> CreateCompany(CompanyForCreationDto company);
        public Task UpdateCompany(int id, CompanyForUpdateDto company);
        public Task DeleteCompany(int id);

        public Task AddRandom();
      
    }
}
