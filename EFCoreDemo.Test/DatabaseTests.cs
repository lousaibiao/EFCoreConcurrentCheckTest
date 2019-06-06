using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EFCoreDemo.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.Data.MySqlClient;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace EFCoreDemo.Test
{
    public class DatabaseTests : IClassFixture<DatabaseFixture>
    {
        private DatabaseFixture _databaseFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public DatabaseTests(DatabaseFixture databaseFixture, ITestOutputHelper testOutputHelper)
        {
            _databaseFixture = databaseFixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task TransactionTest()
        {
            var jhwContext = _databaseFixture.JhwContext;
            {
                var trans = await jhwContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
                _testOutputHelper.WriteLine(trans.GetDbTransaction().IsolationLevel.ToString());
                trans.Commit();
            }
        }

        [Fact]
        public async Task TextCommand()
        {
            //都不能获取到新值
//            var jhwContext = _databaseFixture.JhwContext;
//            {
//                using (var trans = jhwContext.Database.BeginTransaction(IsolationLevel.ReadCommitted))
//                {
//                    var test1 = await jhwContext.Test1.FirstOrDefaultAsync(w => w.Id == 1);
//                    _testOutputHelper.WriteLine(test1.Val1.ToString());
//                    var p1 = new MySqlParameter("@p1", 2);
//                    var p2 = new MySqlParameter("@p2", 1);
//                    int row = jhwContext.Database.ExecuteSqlCommand("update test1 set val1=val1+@p1 where id =@p2;", p1,
//                        p2);
//                    _testOutputHelper.WriteLine(test1.Val1.ToString());
//                    test1 = await jhwContext.Test1.FromSql("select * from test1 where id =@p2", p2)
//                        .FirstOrDefaultAsync();
//                    _testOutputHelper.WriteLine(test1.Val1.ToString());
//                    trans.Commit();
//                }
//            }
        }

//        [Fact]
//        public async Task GetTransaction()
//        {
//            var jhwContext = _databaseFixture.JhwContext;
//            {
//                using (var trans = jhwContext.Database.BeginTransaction(IsolationLevel.ReadCommitted))
//                {
//                    var test1 = jhwContext.Test1.FirstOrDefault(w => w.Id == 1);
//                    test1.Name3 = "3";
//                    jhwContext.Test1.Update(test1);
//                    jhwContext.SaveChanges();
//                    _testOutputHelper.WriteLine($"commit 第一个trans:{trans.TransactionId.ToString()}");
//                    trans.Commit();
//                }
//
//                using (var trans = jhwContext.Database.BeginTransaction(IsolationLevel.ReadCommitted))
//                {
//                    var test1 = jhwContext.Test1.FirstOrDefault(w => w.Id == 1);
//                    test1.Name3 = "4";
//                    jhwContext.Test1.Update(test1);
//                    jhwContext.SaveChanges();
//                    _testOutputHelper.WriteLine($"commit 第二个trans:{trans.TransactionId.ToString()}");
//                    trans.Commit();
//                }
//            }
//        }

        [Fact]
        public async Task Remove1()
        {
            var jhwContext = _databaseFixture.JhwContext;
            var test1 = jhwContext.Test1.FirstOrDefault(w => w.Id == 1);
            jhwContext.Test1.Remove(test1);
            jhwContext.SaveChanges();
        }

        [Fact]
        public async Task AddTest1()
        {
            var jhwContext = _databaseFixture.JhwContext;
            using (var trans = jhwContext.Database.BeginTransaction())
            {
                var test1 = jhwContext.Test1.Single(w => w.Id == 1);
                if (test1!=null)
                {
                    jhwContext.Test1.Remove(test1);
                    jhwContext.SaveChanges();    
                }
                test1 = new Test1()
                {
                    Id = 1,
                    Name3 = "hello",
                    Name4 = DateTime.Now,
                    Name5 = 11,
                    Val1 = 223,
                };
                await jhwContext.Test1.AddAsync(test1);
                await jhwContext.SaveChangesAsync();
                trans.Commit();
            }
            
        }

        [Fact]
        public async Task UpdateTest1()
        {
            try
            {
                var jhwContext = _databaseFixture.JhwContext;
                var test1 = jhwContext.Test1.Single(w => w.Id == 1);
                test1.Name3 = DateTime.Now.ToShortTimeString();
                _testOutputHelper.WriteLine(test1.Name3);
//            jhwContext.Database.ExecuteSqlCommand("update test1 set name3='46' where id =1; ");//没有开事务，所以，模拟的是另一个客户端的操作
                jhwContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}