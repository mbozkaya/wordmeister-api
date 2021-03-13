using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wordmeister_api.Entity;
using wordmeister_api.Helpers;

namespace wordmeister_api_test.Mock.Entity
{
    public partial class MockDbContext : WordmeisterContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }

    public partial class MockDbContext : WordmeisterContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        bool isEncrypted = property.GetAnnotations().Where(w => w.Value.ToString() == "EncryptedField").Any();
                        if (isEncrypted)
                        {
                            property.SetValueConverter(new Converter());
                        }
                    }
                }
            }
        }
    }
}
