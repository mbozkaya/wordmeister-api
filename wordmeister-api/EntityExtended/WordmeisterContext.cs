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

        private List<Country> GetStaticCountry()
        {
            List<Country> countries = new List<Country>()
            {
                new Country{Iso="AF",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AL",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="DZ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AS",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AD",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AI",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AQ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AW",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AU",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AT",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AZ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BS",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BH",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BD",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BB",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BY",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BZ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BJ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BT",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BW",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BV",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="IO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BF",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="BI",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="KH",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CV",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="KY",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CF",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TD",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CL",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CX",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CC",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="KM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CD",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CK",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CI",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="HR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CU",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CY",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CZ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="DK",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="DJ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="DM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="DO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="EC",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="EG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SV",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GQ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="ER",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="EE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="ET",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="FK",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="FO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="FJ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="FI",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="FR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GF",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PF",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TF",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="DE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GH",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GI",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GL",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GD",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GP",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GU",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GT",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GW",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GY",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="HT",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="HM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="VA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="HN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="HK",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="HU",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="IS",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="IN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="ID",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="IR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="IQ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="IE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="IL",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="IT",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="JM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="JP",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="JO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="KZ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="KE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="KI",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="KP",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="KR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="KW",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="KG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="LA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="LV",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="LB",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="LS",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="LR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="LY",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="LI",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="LT",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="LU",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MK",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MW",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MY",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MV",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="ML",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MT",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MH",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MQ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MU",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="YT",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MX",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="FM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MD",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MC",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MS",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MZ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NP",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NL",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NC",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NZ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NI",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NU",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NF",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="MP",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="NO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="OM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PK",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PW",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PS",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PY",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PH",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PL",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PT",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="QA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="RE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="RO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="RU",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="RW",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SH",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="KN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="LC",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="PM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="VC",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="WS",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="ST",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CS",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SC",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SL",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SK",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SI",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SB",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="ZA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GS",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="ES",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="LK",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SD",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SJ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SZ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="CH",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="SY",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TW",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TJ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TZ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TH",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TL",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TK",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TO",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TT",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TR",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TC",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="TV",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="UG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="UA",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="AE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="GB",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="US",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="UM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="UY",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="UZ",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="VU",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="VE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="VN",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="VG",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="VI",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="WF",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="EH",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="YE",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="ZM",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
                new Country{Iso="ZW",Name="",NiceName="",Iso3="",NumCode=0,PhoneCode=null},
            };

            return countries;
        }

    }
}
