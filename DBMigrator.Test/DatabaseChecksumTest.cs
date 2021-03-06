﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBMigrator.Test
{
    [TestClass]
    public class DatabaseChecksumTest
    {
        [TestMethod]
        public void GetTablesViewsAndColumnsChecksum_doesnt_change()
        {
            var database = new Database("");
            var checksum = database.GetTablesViewsAndColumnsChecksum();
            var checksum2 = database.GetTablesViewsAndColumnsChecksum();
            Assert.AreEqual(checksum, checksum2);
        }

        [TestMethod]
        public void TablesChecksum_changes()
        {
            var database = new Database("");
            var checksum = database.GetTablesViewsAndColumnsChecksum();
            database.ExecuteSingleCommand("CREATE TABLE [dbo].[DBMigratorTable]([Test][nvarchar](max) NULL)");
            var checksum2 = database.GetTablesViewsAndColumnsChecksum();
            database.ExecuteSingleCommand("DROP TABLE [dbo].[DBMigratorTable]");
            Assert.AreNotEqual(checksum, checksum2);
        }

        [TestMethod]
        public void ViewsChecksum_changes()
        {
            var database = new Database("");
            database.ExecuteSingleCommand("CREATE TABLE [dbo].[DBMigratorTable]([Test][nvarchar](max) NULL)");
            var checksum = database.GetTablesViewsAndColumnsChecksum();
            database.ExecuteSingleCommand("CREATE VIEW [dbo].DBMigratorView AS SELECT Test FROM [dbo].[DBMigratorTable]");
            var checksum2 = database.GetTablesViewsAndColumnsChecksum();
            database.ExecuteSingleCommand("DROP VIEW [dbo].[DBMigratorView]");
            database.ExecuteSingleCommand("DROP TABLE [dbo].[DBMigratorTable]");
            Assert.AreNotEqual(checksum, checksum2);
        }

        [TestMethod]
        public void TableColumnsChecksum_changes()
        {
            var database = new Database("");
            database.ExecuteSingleCommand("CREATE TABLE [dbo].[DBMigratorTable]([Test][nvarchar](max) NULL)");
            var checksum = database.GetTablesViewsAndColumnsChecksum();
            database.ExecuteSingleCommand("ALTER TABLE [dbo].[DBMigratorTable] ADD Test2 int");
            var checksum2 = database.GetTablesViewsAndColumnsChecksum();
            database.ExecuteSingleCommand("DROP TABLE [dbo].[DBMigratorTable]");
            Assert.AreNotEqual(checksum, checksum2);
        }

        [TestMethod]
        public void TableColumnsDataTypeChecksum_changes()
        {
            var database = new Database("");
            database.ExecuteSingleCommand("CREATE TABLE [dbo].[DBMigratorTable]([Test][nvarchar](max) NULL)");
            var checksum = database.GetTablesViewsAndColumnsChecksum();
            database.ExecuteSingleCommand("ALTER TABLE [dbo].[DBMigratorTable] ALTER COLUMN [Test] int");
            var checksum2 = database.GetTablesViewsAndColumnsChecksum();
            database.ExecuteSingleCommand("DROP TABLE [dbo].[DBMigratorTable]");
            Assert.AreNotEqual(checksum, checksum2);
        }

        [TestMethod]
        public void ViewColumnsChecksum_changes()
        {
            var database = new Database("");
            database.ExecuteSingleCommand("CREATE TABLE [dbo].[DBMigratorTable]([Test][nvarchar](max) NULL, Test2 int)");
            database.ExecuteSingleCommand("CREATE VIEW [dbo].DBMigratorView AS SELECT Test FROM [dbo].[DBMigratorTable]");
            var checksum = database.GetTablesViewsAndColumnsChecksum();
            database.ExecuteSingleCommand("ALTER VIEW [dbo].DBMigratorView AS SELECT Test, Test2 FROM [dbo].[DBMigratorTable]");
            var checksum2 = database.GetTablesViewsAndColumnsChecksum();
            database.ExecuteSingleCommand("DROP VIEW [dbo].[DBMigratorView]");
            database.ExecuteSingleCommand("DROP TABLE [dbo].[DBMigratorTable]");
            Assert.AreNotEqual(checksum, checksum2);
        }

    }
}
