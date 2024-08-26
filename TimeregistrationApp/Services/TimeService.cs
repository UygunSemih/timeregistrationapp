using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeregistrationApp.Models;
using TimeregistrationApp.Repositories;

namespace TimeregistrationApp.Services
{
    public class TimeService
    {
        private readonly TimeRegistrationSQLiteRepository timeRegistrationRepo;

        public TimeService(TimeRegistrationSQLiteRepository repo)
        {
            timeRegistrationRepo = repo;
        }

        public async Task<List<TijdsRegistratie>> GetAllTimeRegistrations()
        {
            return await timeRegistrationRepo.GetTimeRegistrationsAsync();
        }

        public async void AddTimeRegistration(TijdsRegistratie timeRegistrationToAdd)
        {
            await timeRegistrationRepo.SaveTimeRegistrationAsync(timeRegistrationToAdd);
        }

        public async void SaveTimeRegistration(TijdsRegistratie timeRegistrationToAdd)
        {
            await timeRegistrationRepo.SaveTimeRegistrationAsync(timeRegistrationToAdd);
        }

        public async void DeleteTimeRegistrationAsync(TijdsRegistratie itemToDelete)
        {
            await timeRegistrationRepo.DeleteTimeRegistrationAsync(itemToDelete);
        }
    }
}
