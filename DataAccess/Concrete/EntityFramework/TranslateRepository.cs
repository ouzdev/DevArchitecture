﻿using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
using Core.Entities.Concrete;
using System.Threading.Tasks;
using System.Collections.Generic;
using Core.Entities.Dtos;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DataAccess.Concrete.EntityFramework
{
    public class TranslateRepository : EfEntityRepositoryBase<Translate, ProjectDbContext>, ITranslateRepository
    {
        public TranslateRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<List<TranslateDto>> GetTranslateDto()
        {
            var list = await (from lng in context.Languages
                        join trs in context.Translates on lng.Id equals trs.LangId
                        select new TranslateDto()
                        {
                            Id = trs.Id,
                            Code = trs.Code,
                            Language = lng.Code,
                            Value = trs.Value

                        }).ToListAsync();

            return list;
        }

        public async Task<string> GetTranslatesByLang(string langCode)
        {
            var data= await (from trs in context.Translates
                              join lng in context.Languages on trs.LangId equals lng.Id
                              where lng.Code == langCode
                              select trs).ToDictionaryAsync(x => (string)x.Code, x => (string)x.Value);

            var str=JsonConvert.SerializeObject(data);
            return str;
       
        }

        public async Task<Dictionary<string, string>> GetTranslateWordList(string lang)
        {
            var list = await context.Translates.Where(x => x.Code == lang).ToListAsync();

            return list.ToDictionary(x => x.Code, x => x.Value);
        }
    }
}
