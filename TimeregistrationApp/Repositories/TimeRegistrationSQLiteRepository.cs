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
        const int LatestVersion = 2;

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
            await context.CreateTableAsync<DbVersion>();

            await PerformMigrations();
        }

        private async Task PerformMigrations()
        {
            var version = await context.Table<DbVersion>().FirstOrDefaultAsync();
            int currentVersion = 0;

            if (version == null)
            {
                var firstVersion = new DbVersion { Id = 1, Version = 1 };
                currentVersion = 1;
                await context.InsertAsync(firstVersion);
            }
            else
            {
                currentVersion = version.Version;
            }


            if (currentVersion < LatestVersion)
            {
                if (currentVersion == 1)
                {
                    var columns = await context.GetTableInfoAsync("TijdsRegistratie");
                    bool columnExists = columns.Any(col => col.Name == "IsHoliday");

                    if (!columnExists)
                    {
                        await context.ExecuteAsync("ALTER TABLE TijdsRegistratie ADD COLUMN IsHoliday INTEGER NOT NULL DEFAULT 0");
                    }

                    await context.ExecuteAsync("UPDATE DbVersion SET Version = 2 WHERE Id = 1");
                }
                // Future migrations can be added here
            }

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