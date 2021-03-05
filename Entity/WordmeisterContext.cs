using Microsoft.EntityFrameworkCore;
using wordmeister_api.Model;

namespace wordmeister_api.Entity
{
    public partial class WordmeisterContext : DbContext
    {
        public WordmeisterContext(DbContextOptions<WordmeisterContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WordFrequency>().Property(obj => obj.Zipf).HasPrecision(4, 2);
            modelBuilder.Entity<WordFrequency>().Property(obj => obj.Diversity).HasPrecision(4, 2);
            modelBuilder.Entity<WordFrequency>().Property(obj => obj.PerMillion).HasPrecision(4, 2);
            modelBuilder.Entity<UserWord>().Property(obj => obj.Point).HasPrecision(4, 2);
            OnModelCreatingPartial(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<User> Users { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<UserWord> UserWords { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<WordAntonym> WordAntonyms { get; set; }
        public DbSet<WordDefinition> WordDefinations { get; set; }
        public DbSet<WordDerivation> WordDerivations { get; set; }
        public DbSet<WordFrequency> WordFrequencies { get; set; }
        public DbSet<WordHasType> WordHasTypes { get; set; }
        public DbSet<WordPronunciation> WordPronunciations { get; set; }
        public DbSet<WordRyhme> WordRyhmes { get; set; }
        public DbSet<WordSyllable> WordSyllables { get; set; }
        public DbSet<WordSynonym> WordSynonyms { get; set; }
        public DbSet<WordTypeOf> WordTypeOfs { get; set; }
        public DbSet<UserWordSetting> UserWordSettings{ get; set; }
    }
}
