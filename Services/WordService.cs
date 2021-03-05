using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using wordmeister_api.Dtos.Word;
using wordmeister_api.Entity;
using wordmeister_api.Helpers;
using wordmeister_api.Interfaces;
using wordmeister_api.Model;
using static wordmeister_api.Dtos.General.General;
using static wordmeister_api.Helpers.Enums;

namespace wordmeister_api.Services
{
    public class WordService : IWordService
    {
        WordmeisterContext _dbContext;
        ITranslateService _translateService;
        IServiceScopeFactory _serviceScopeFactory;
        IWordAPIService _wordAPIService;
        IMapper _mapper;

        public WordService(WordmeisterContext dbContext, ITranslateService translateService, IWordAPIService wordAPIService, IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _dbContext = dbContext;
            _translateService = translateService;
            _serviceScopeFactory = serviceScopeFactory;
            _wordAPIService = wordAPIService;
            _mapper = mapper;
        }

        public WordResponse.Word GetWord(long wordId, int userId)
        {
            var exist = _dbContext.UserWords.FirstOrDefault(x => x.WordId == wordId && x.UserId == userId);

            if (exist.IsNullOrDefault())
                return null;


            return new WordResponse.Word
            {
                Id = exist.WordId,
                Text = exist.Word.Text,
                Description = exist.Description,
                Sentences = exist.Word.Sentences.Select(s => new SentenceDto
                {
                    Id = s.Id,
                    Text = s.Text
                }).ToList()
            };
        }

        public PageResponse GetWords(int pageNumber, int pageSize, int userId)
        {
            var query = _dbContext.UserWords.Where(x => x.UserId == userId);
            var page = query.OrderByDescending(x => x.CreatedDate)
                .Select(x => new WordResponse.Word
                {
                    Id = x.WordId,
                    Description = x.Description,
                    Text = x.Word.Text,
                    Sentences = x.Word.Sentences.Select(s => new SentenceDto
                    {
                        Id = s.Id,
                        Text = s.Text
                    }).ToList(),
                    CreatedDate = x.CreatedDate,
                })
            .Skip((pageNumber) * pageSize).Take(pageSize).ToList();

            int total = query.Count();
            var words = page.Select(x => x);

            return new PageResponse { Data = words, Total = total };
        }

        public ResponseResult AddWord(WordRequest.Add model, int userId)
        {
            var existWord = _dbContext.Words.FirstOrDefault(x => x.Text == model.Text);

            var userWord = new UserWord();

            if (existWord.IsNullOrDefault())
            {
                var newWord = _dbContext.Words.Add(new Word
                {
                    Text = model.Text,
                    Sentences = null,
                    CreatedDate = DateTime.Now,
                });

                _dbContext.SaveChanges();

                userWord = new UserWord
                {
                    UserId = userId,
                    WordId = newWord.Entity.Id,
                    Description = model.Description,
                    CreatedDate = DateTime.Now
                };
                _dbContext.UserWords.Add(userWord);
                _dbContext.SaveChanges();

                Task.Run(() => { AddWordModel(newWord.Entity); });
            }
            else
            {
                var exist = _dbContext.UserWords.FirstOrDefault(x => x.UserId == userId && x.WordId == existWord.Id);

                if (!exist.IsNullOrDefault())
                    return new ResponseResult() { Error = true, ErrorMessage = "Such a word has been added before." };

                userWord = new UserWord
                {
                    UserId = userId,
                    WordId = existWord.Id,
                    Description = model.Description,
                    CreatedDate = DateTime.Now
                };
                _dbContext.UserWords.Add(userWord);
                _dbContext.SaveChanges();
            }

            return new ResponseResult();
        }

        public void DeleteWord(long wordId, int userId)
        {
            var exist = _dbContext.UserWords.FirstOrDefault(x => x.WordId == wordId && x.UserId == userId);

            _dbContext.UserWords.Remove(exist);
            _dbContext.SaveChanges();
        }

        public void UpdateWord(WordRequest.Add model, int userId)
        {
            var existWord = _dbContext.UserWords.FirstOrDefault(x => x.WordId == model.Id && x.UserId == userId);

            existWord.Description = model.Description;
            existWord.UpdateDate = DateTime.Now;

            _dbContext.SaveChanges();
        }

        public WordResponse.WordCard GetWordCard(int userId, int currentIndex = 1, bool isRandom = false)
        {

            var authorizedWords = GetUserWordsBySettings(userId);

            if (currentIndex > authorizedWords.words.Count)
            {
                return new WordResponse.WordCard
                {
                    IsOver = true,
                };
            }

            var userWord = new UserWord();

            if (isRandom)
            {
                var random = new Random();

                userWord = authorizedWords.words[random.Next(authorizedWords.words.Count)];
            }
            else
            {
                userWord = authorizedWords.words[currentIndex - 1];
            }

            return new WordResponse.WordCard
            {
                Sentences = userWord.Word.Sentences.Where(w => !w.IsPrivate || (w.IsPrivate && w.UserId == userWord.UserId)).Select(s => s.Text).ToList(),
                Description = userWord.Description,
                Word = userWord.Word.Text,
                CurrentIndex = currentIndex,
                IsFavorite = userWord.IsFavorite,
                IsOver = currentIndex == authorizedWords.words.Count,
                Point = userWord.Point,
                UserWordId = userWord.Id,
                WordCount = authorizedWords.words.Where(w => !w.IsLearned).Count(),
                Definations = userWord.Word.WordDefinitions.Select(s => new WordResponse.Definations
                {
                    Defination = s.Definition,
                    Id = s.Id,
                    Type = s.PartOfSpeech,
                }).ToList(),
                Frequency = userWord.Word.WordFrequencies.Select(s => s.PerMillion).FirstOrDefault(),
                Prononciations = userWord.Word.WordPronunciations.Select(s => new WordResponse.Prononciation
                {
                    All = s.All ?? string.Empty,
                    Verb = s.Verb ?? string.Empty,
                    Noun = s.Noun ?? string.Empty,
                }).FirstOrDefault(),
                IsLearned = userWord.IsLearned,
            };

        }

        public ResponseResult SetWordPoint(WordRequest.WordPoint model)
        {
            var userWord = GetUserWord(model.UserWordId);

            if (userWord.IsNullOrDefault())
            {
                return new ResponseResult() { Error = true, ErrorMessage = "The word not found" };
            }

            userWord.Point = (byte)model.Point;
            _dbContext.SaveChanges();

            return new ResponseResult();
        }

        public ResponseResult SetWordFavorite(WordRequest.WordFavorite model)
        {
            var userWord = GetUserWord(model.UserWordId);

            if (userWord.IsNullOrDefault())
            {
                return new ResponseResult() { Error = true, ErrorMessage = "The word not found" };
            }

            userWord.IsFavorite = model.IsFavorite;
            userWord.UpdateDate = DateTime.Now;
            _dbContext.SaveChanges();

            return new ResponseResult();
        }

        public ResponseResult AddCustomSentence(WordRequest.CustomSentence model)
        {
            var userWord = GetUserWord(model.UserWordId);

            if (userWord.IsNullOrDefault())
            {
                return new ResponseResult() { Error = true, ErrorMessage = "The word not found" };
            }

            _dbContext.Sentences.Add(new Sentence
            {
                CreatedDate = DateTime.Now,
                IsPrivate = model.IsPrivate,
                UserId = userWord.UserId,
                Text = model.Sentence,
                WordId = userWord.WordId,
            });

            _dbContext.SaveChanges();

            return new ResponseResult();
        }

        public ResponseResult SetWordLearned(WordRequest.Learned model)
        {
            var userWord = GetUserWord(model.UserWordId);

            if (userWord.IsNullOrDefault())
            {
                return new ResponseResult() { Error = true, ErrorMessage = "The word not found" };
            }

            userWord.IsLearned = model.IsLearned;

            if (model.IsLearned)
            {
                userWord.LearnedDate = DateTime.Now;
            }
            else
            {
                userWord.UpdateDate = DateTime.Now;
            }
            _dbContext.SaveChanges();

            return new ResponseResult();
        }

        public ResponseResult SetUserWordSetting(WordRequest.UserWordSetting model, int userId)
        {
            var userWordSetting = GetUserWordSettingByUserId(userId);
            //userWordSetting = _mapper.Map<WordRequest.UserWordSetting, UserWordSetting>(model);
            userWordSetting.IsIncludeFavorite = model.IsIncludeFavorite;
            userWordSetting.IsIncludeLearned = model.IsIncludeLearned;
            userWordSetting.IsIncludePoint = model.IsIncludePoint;
            userWordSetting.Point = model.Point;
            userWordSetting.Order = model.Order;
            userWordSetting.OrderBy = model.OrderBy;
            userWordSetting.ConditionType = (byte)model.ConditionType;
            _dbContext.SaveChanges();

            return new ResponseResult();
        }

        public ResponseResult GetUserWordSetting(int userId)
        {
            var data = _mapper.Map<UserWordSetting, WordResponse.UserWordSetting>(GetUserWordSettingByUserId(userId));

            return new ResponseResult() { Data = data };
        }
        public async void GetRandomWord()
        {
            var result = await _wordAPIService.GetRandom();
        }

        private async void AddSentences(Word word)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<WordmeisterContext>();

                var isExistSentences = db.Sentences.Where(w => w.WordId == word.Id).Any();

                if (!isExistSentences)
                {
                    var sentences = await _wordAPIService.GetExample(word.Text);

                    foreach (var example in sentences.Examples)
                    {
                        db.Sentences.Add(new Sentence
                        {
                            CreatedDate = DateTime.Now,
                            Text = example,
                            WordId = word.Id
                        });
                    }

                    db.SaveChanges();
                }
            }

        }

        private async void AddWordModel(Word word)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<WordmeisterContext>();

                var response = await _wordAPIService.GetWord(word.Text);

                var now = DateTime.Now;
                var executionStrategy = db.Database.CreateExecutionStrategy();

                executionStrategy.Execute(() =>
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.WordFrequencies.Add(new WordFrequency
                            {
                                WordId = word.Id,
                                PerMillion = response.Frequency,
                                Zipf = decimal.Zero,
                                Diversity = decimal.Zero,
                                CreatedDate = now,
                            });

                            db.WordPronunciations.Add(new WordPronunciation
                            {
                                WordId = word.Id,
                                All = response.Pronunciation.All,
                                Noun = response.Pronunciation.Noun,
                                Verb = response.Pronunciation.Verb,
                                CreatedDate = now,
                            });

                            response.Syllables.List.ForEach(f =>
                            {
                                db.WordSyllables.Add(new WordSyllable
                                {
                                    Syllable = f,
                                    WordId = word.Id,
                                    CreatedDate = now,
                                });
                            });

                            response.Results.ForEach(result =>
                            {
                                result.Antonyms.ForEach(antonym =>
                                {
                                    db.WordAntonyms.Add(new WordAntonym
                                    {
                                        WordId = word.Id,
                                        Antonym = antonym,
                                        CreatedDate = now,
                                    });
                                });

                                db.WordDefinations.Add(new WordDefinition
                                {
                                    WordId = word.Id,
                                    Definition = result.Definition,
                                    PartOfSpeech = result.PartOfSpeech,
                                    CreatedDate = now
                                });

                                result.Examples.ForEach(example =>
                                {
                                    db.Sentences.Add(new Sentence
                                    {
                                        WordId = word.Id,
                                        Text = example,
                                        CreatedDate = now
                                    });
                                });

                                result.HasTypes.ForEach(hasType =>
                                {
                                    db.WordHasTypes.Add(new WordHasType
                                    {
                                        WordId = word.Id,
                                        Type = hasType,
                                        CreatedDate = now,
                                    });
                                });

                                result.Synonyms.ForEach(synonym =>
                                {
                                    db.WordSynonyms.Add(new WordSynonym
                                    {
                                        WordId = word.Id,
                                        Synonym = synonym,
                                        CreatedDate = now,
                                    });
                                });


                                result.TypeOf.ForEach(typeOf =>
                                {
                                    db.WordTypeOfs.Add(new WordTypeOf
                                    {
                                        WordId = word.Id,
                                        TypeOf = typeOf,
                                        CreatedDate = now
                                    });
                                });

                            });


                            db.SaveChanges();

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            //TODO Log
                            transaction.Rollback();
                        }
                    }
                });
            }
        }

        private UserWord GetUserWord(int userWordId)
        {
            return _dbContext.UserWords
                .Where(w => w.Id == userWordId)
                .FirstOrDefault();
        }

        private UserWordSetting GetUserWordSettingByUserId(int userId)
        {
            var userwordSetting = _dbContext.UserWordSettings
              .Where(w => w.UserId == userId)
              .AsTracking()
              .FirstOrDefault();

            if (userwordSetting == null)
            {
                userwordSetting = new UserWordSetting
                {
                    CreatedDate = DateTime.Now,
                    IsIncludeFavorite = true,
                    IsIncludeLearned = false,
                    IsIncludePoint = true,
                    Order = "CreatedDate",
                    OrderBy = "desc",
                    UserId = userId,
                    ConditionType = (byte)DynamicConditions.Equal,
                };
                _dbContext.UserWordSettings.Add(userwordSetting);

                _dbContext.SaveChanges();
            }

            return userwordSetting;
        }

        private (List<UserWord> words, UserWordSetting userWordSetting) GetUserWordsBySettings(int userId)
        {
            var userwordSetting = GetUserWordSettingByUserId(userId);

            var conditionDic = new Dictionary<DynamicConditions, string>()
            {
                {DynamicConditions.Equal , "=" },
                {DynamicConditions.GreaterThan , ">" },
                {DynamicConditions.GreaterThanOrEqual , ">=" },
                {DynamicConditions.LessThan, "<" },
                {DynamicConditions.LessThanOrEqual , "<=" },
                {DynamicConditions.NotEqual, "!=" },
            };

            var userWords = _dbContext.UserWords
                .Where(w => w.UserId == userId)
                .WhereIf(!userwordSetting.IsIncludeLearned, w => !w.IsLearned)
                .WhereIf(!userwordSetting.IsIncludeFavorite, w => !w.IsFavorite)
                .WhereIf(userwordSetting.IsIncludePoint, string.Concat("Point != 0 && Point ", conditionDic[(DynamicConditions)userwordSetting.ConditionType.GetValueOrDefault(1)], " ", userwordSetting.Point.GetValueOrDefault(10)))
                .Include(i => i.Word).ThenInclude(i => i.Sentences)
                .Include(i => i.Word).ThenInclude(i => i.WordDefinitions)
                .Include(i => i.Word).ThenInclude(i => i.WordFrequencies)
                .Include(i => i.Word).ThenInclude(i => i.WordPronunciations)
                .OrderBy($"{userwordSetting.Order} {userwordSetting.OrderBy}")
                .ToList();


            return (userWords, userwordSetting);
        }
    }
}