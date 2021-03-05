using System.Collections.Generic;

namespace wordmeister_api.Model
{
    public class Word : BaseModel
    {
        public string Text { get; set; }
        public virtual List<Sentence> Sentences { get; set; }
        public virtual ICollection<UserWord> UserWords { get; set; }
        public virtual ICollection<WordAntonym> WordAntonyms { get; set; }
        public virtual ICollection<WordDefinition> WordDefinitions { get; set; }
        public virtual ICollection<WordDerivation> WordDerivations { get; set; }
        public virtual ICollection<WordFrequency> WordFrequencies { get; set; }
        public virtual ICollection<WordHasType> WordHasTypes { get; set; }
        public virtual ICollection<WordPronunciation> WordPronunciations { get; set; }
        public virtual ICollection<WordRyhme> WordRyhmes { get; set; }
        public virtual ICollection<WordSyllable> WordSyllables { get; set; }
        public virtual ICollection<WordSynonym> WordSynonyms { get; set; }
        public virtual ICollection<WordTypeOf> WordTypeOfs { get; set; }
    }
}
