using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeregistrationApp.Models;

namespace TimeregistrationApp.Repositories
{
    public class TimeRegistrationSQLiteRepository
    {
        SQLiteAsyncConnection context;

        public TimeRegistrationSQLiteRepository()
        {

        }

        async Task Init()
        {
            if (context is not null)
            {
                return;
            }

            context = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await context.CreateTableAsync<TijdsRegistratie>();
        }

        public async Task<List<TijdsRegistratie>> GetTimeRegistrationsAsync()
        {
            await Init();
            return await context.Table<TijdsRegistratie>().ToListAsync();
        }

        public async Task<int> SaveTimeRegistrationAsync(TijdsRegistratie itemToSave)
        {
            await Init();
            if (itemToSave.ID != 0)
                return await context.UpdateAsync(itemToSave);
            else
                return await context.InsertAsync(itemToSave);
        }

        public async Task<int> DeleteTimeRegistrationAsync(TijdsRegistratie itemToDelete)
        {
            return await context.DeleteAsync(itemToDelete);
        }
    }
}
