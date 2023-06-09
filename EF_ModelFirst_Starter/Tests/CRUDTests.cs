using NUnit.Framework;
using System.Linq;
using EF_ModelFirst;

namespace Tests
{
    public class CRUDTests
    {
        private Customer testCustomer1;

        [SetUp]
        public void SetUp()
        {
            using (var db = new SouthwindContext())
            {
                testCustomer1 = new Customer() { CustomerId = "TestId1", ContactName = "Test1", City = "TestCity1", Country = "TestCountry1", PostalCode = "TestPostalCode1" };
                db.Customers.Add(testCustomer1);
                db.SaveChanges();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new SouthwindContext())
            {
                var delete = new Delete();
                delete.Execute(testCustomer1.CustomerId);
                db.SaveChanges();
            }
        }

        [Test]
        public void TestAdd()
        {
            using (var db = new SouthwindContext())
            {
                var add = new Add();
                var testCustomer2 = new Customer() { CustomerId = "TestId2", ContactName = "Test2", City = "TestCity2", Country = "TestCountry2", PostalCode = "TestPostalCode2" };
                add.Execute(testCustomer2);

                var customer = db.Customers.FirstOrDefault(c => c.ContactName == "Test2");

                Assert.IsNotNull(customer);
                Assert.That(customer.CustomerId, Is.EqualTo("TestId2"));
                Assert.That(customer.ContactName, Is.EqualTo("Test2"));
                Assert.That(customer.City, Is.EqualTo("TestCity2"));
                Assert.That(customer.Country, Is.EqualTo("TestCountry2"));
                Assert.That(customer.PostalCode, Is.EqualTo("TestPostalCode2"));

                var delete = new Delete();
                delete.Execute(testCustomer2.CustomerId);
                db.SaveChanges();
            }
        }

        [Test]
        public void TestReadAll()
        {
            using (var db = new SouthwindContext())
            {
                var read = new Read();
                read.Execute("all");

                Assert.IsTrue(db.Customers.Count() >= 1);
            }
        }

        [Test]
        public void TestRead()
        {
            using (var db = new SouthwindContext())
            {
                var read = new Read();
                read.Execute(testCustomer1.CustomerId);

                var customer = db.Customers.Find(testCustomer1.CustomerId);

                Assert.That(customer.CustomerId, Is.EqualTo("TestId1"));
                Assert.That(customer.ContactName, Is.EqualTo("Test1"));
                Assert.That(customer.City, Is.EqualTo("TestCity1"));
                Assert.That(customer.Country, Is.EqualTo("TestCountry1"));
                Assert.That(customer.PostalCode, Is.EqualTo("TestPostalCode1"));
            }
        }

        [Test]
        public void TestUpdate()
        {
            using (var db = new SouthwindContext())
            {
                var update = new Update();
                update.Execute(testCustomer1.CustomerId, "ContactName", "NewName");

                var customer = db.Customers.Find(testCustomer1.CustomerId);

                Assert.IsNotNull(customer);
                Assert.That(customer.ContactName, Is.EqualTo("NewName"));
            }
        }

        [Test]
        public void TestDelete()
        {
            using (var db = new SouthwindContext())
            {
                var delete = new Delete();
                delete.Execute(testCustomer1.CustomerId);
                var deletedCustomer = db.Customers.Find(testCustomer1.CustomerId);
                Assert.IsNull(deletedCustomer);
            }
        }
    }
}
