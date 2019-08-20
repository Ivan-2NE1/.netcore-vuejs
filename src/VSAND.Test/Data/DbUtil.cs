using Microsoft.EntityFrameworkCore;
using VSAND.Data;
using VSAND.Data.Repositories;

namespace VSAND.Test.Data
{
    public class DbUtil
    {
        public static DbContextOptions<VsandContext> GetOptions(string testName)
        {
            return new DbContextOptionsBuilder<VsandContext>()
                .UseInMemoryDatabase(databaseName: testName)
                .Options;            
        }

        public static VsandContext GetContext(DbContextOptions<VsandContext> options)
        {
            return new VsandContext(options);
        }

        public static UnitOfWork GetUow(VsandContext context)
        {
            return new UnitOfWork(context);
        }
    }
}
