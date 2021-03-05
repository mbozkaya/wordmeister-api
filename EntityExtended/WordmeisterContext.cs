using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Helpers;

namespace wordmeister_api.Entity
{
    public partial class WordmeisterContext:DbContext
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
