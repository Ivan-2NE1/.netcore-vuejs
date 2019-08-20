using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSAND.Data;
using VSAND.Data.Repositories;
using VSAND.Test.Data;
using VSAND.Data.Entities;
using VSAND.Services.Data.Schools;
using System.Threading.Tasks;
using VSAND.Data.ViewModels;
using System.Collections.Generic;

namespace VSAND.Test.Services.Data.Schools
{
    [TestClass]
    public class SchoolServiceTests
    {
        [TestMethod]
        public async Task AutocompleteSchoolAsyncReturnsEmptyListWithEmptyKeyword()
        {
            var options = DbUtil.GetOptions("AutocompleteSchoolAsyncReturnsEmptyListWithEmptyKeyword");
            var context = DbUtil.GetContext(options);
            var uow = DbUtil.GetUow(context);
            var cache = CacheUtil.GetInMemoryCache();

            var searchKeyword = "";

            var oSchoolSvc = new SchoolService(uow, cache);

            var oResult = (List < ListItem<int> > )await oSchoolSvc.AutocompleteAsync(searchKeyword);

            Assert.IsNotNull(oResult, "AutocompleteAsync service should return an empty list, not null, when search keyword is empty");
            Assert.IsTrue(oResult.Count == 0, "AutocompleteAsync server should return 0-item list when search keyword is empty");
        }

    }
}
