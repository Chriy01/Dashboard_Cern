using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;
using CryptSharp;
using NuGet.Versioning;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.Implementation
{
    public class UtenteRepository : BaseRepository,IBaseRepositable<Utente>
    {
        private readonly AppDbContext _dbContext;

        public UtenteRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Utente entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Utente GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetByUsernamePassword(string username, string password)
        {
            try
            {
                password = ComputeMD5Hash(password);
                var us = await _dbContext.Utente.Where(u => u.Username.ToUpper() == username && u.Password == password).SingleOrDefaultAsync();
                if (us != null)
                {
       
                    return us.Utente_Id;

                }
                else
                {
                    return -1;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'esecuzione della query: {ex.Message}");
                return -1;
            }

        }

        public bool Update(Utente entity)
        {
            throw new NotImplementedException();
        }
    }
}
