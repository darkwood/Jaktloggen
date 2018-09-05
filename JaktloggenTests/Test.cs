using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

using Jaktloggen;

namespace JaktloggenTests
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public async Task ExportHunt_FileCreatedOnServer()
        {
            var aboutViewModel = new AboutViewModel();
            await aboutViewModel.ExportData();

            Assert.AreEqual(4, aboutViewModel.ExportCount);
        }

        [Test()]
        public async Task ImportHunts_HuntsDownloaded()
        {
            var aboutViewModel = new AboutViewModel();
            await aboutViewModel.ImportData();

            Assert.AreEqual(HttpStatusCode.OK, aboutViewModel.ImportStatusCode);
        }
    }
}
