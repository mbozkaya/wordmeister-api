using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Helpers;
using wordmeister_api.Model;

namespace wordmeister_api.Entity
{
    public partial class WordmeisterContext : DbContext
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

        void OnModelCreatingSeed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSettingType>().HasData(GetStaticSettingType());
        }

        private List<UserSettingType> GetStaticSettingType()
        {
            var settingTypes = Enum.GetValues(typeof(Enums.SettingType)).Cast<Enums.SettingType>();
            List<UserSettingType> userSettingTypes = new List<UserSettingType>();

            foreach (var item in settingTypes.Select((value, i) => new { value, i }))
            {
                userSettingTypes.Add(new UserSettingType
                {
                    Id = item.i + 1,
                    Order = item.i + 1,
                    Title = item.value.ToString(),
                });
            }
            return userSettingTypes;
        }
    }
}
