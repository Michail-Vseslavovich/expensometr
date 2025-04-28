using expense_service.domain.dto;
using expense_service.domain;
using expense_service.domain.Intefaces;
using expense_service.domain.Models;
using expense_service.infrastructure.db;
using Microsoft.EntityFrameworkCore;
using expense_service.domain.enums;

namespace expense_service.application.Services
{
    public class IncomeService() : IMoneyService<Income, IncomeType>
    {
        public async Task<List<Income>> GetAllByUserId(int id)
        {
            using (var db = new IncomeContext())
            {
                return await db.Incomes.Where(x => x.RelatedUserId == id).ToListAsync();
            }
        }

        public async Task<bool> Post(Income entity)
        {
            using (var db = new IncomeContext())
            {
                if (entity == null)
                {
                    return false;
                }
                
                await db.Incomes.AddAsync(entity);
                db.SaveChanges();
                return true;
            }
        }

        public async Task<bool> Update(Income entity)
        {
            using (var db = new IncomeContext())
            {
                if (entity == null)
                {
                    return false;
                }

                var e = await db.Incomes.FirstOrDefaultAsync(x => x.Id == entity.Id);

                if (e != null)
                {
                    e.Value = entity.Value;
                    e.Description = entity.Description;
                    db.Incomes.Update(e);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public async Task<bool> Delete(int id, int UserId)
        {
            using (var db = new IncomeContext())
            {
                Income? e = await db.Incomes.FirstOrDefaultAsync(x => x.Id == id && x.RelatedUserId == UserId);

                if (e != null)
                {
                    db.Incomes.Remove(e);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public async Task<(int, Dictionary<IncomeType, int>)> CalculateMonthStatistic(int UserId)
        {
            using (var db = new IncomeContext())
            {
                var Now = DateTime.Now;
                IncomeDTO[]? IcDtoArray = db.Incomes.Where(x => x.RelatedUserId == UserId && x.Created.Subtract(Now).TotalDays < 30).Select(u => new IncomeDTO(u.Value, u.Type)).ToArray();
                int SummmaryEX = IcDtoArray.Sum(x => x.value);
                Dictionary<IncomeType, int> IncomesValues = new Dictionary<IncomeType, int>();

                foreach (var income in IcDtoArray)
                {
                    if (IncomesValues.ContainsKey(income.exType))
                    {
                        IncomesValues[income.exType] += income.value;
                    }
                    else
                    {
                        IncomesValues.Add(income.exType, income.value);
                    }

                }
                return (SummmaryEX, IncomesValues);
            }
        }


        public async Task<(int, Dictionary<IncomeType, int>)> CalculateTotalStatistic(int UserId)
        {
            using (var db = new IncomeContext())
            {
                var Now = DateTime.Now;
                IncomeDTO[]? IcDtoArray = db.Incomes.Where(x => x.RelatedUserId == UserId).Select(u => new IncomeDTO(u.Value, u.Type)).ToArray();
                int SummmaryEX = IcDtoArray.Sum(x => x.value);
                Dictionary<IncomeType, int> IncomesValues = new Dictionary<IncomeType, int>();

                foreach (var income in IcDtoArray)
                {
                    if (IncomesValues.ContainsKey(income.exType))
                    {
                        IncomesValues[income.exType] += income.value;
                    }
                    else
                    {
                        IncomesValues.Add(income.exType, income.value);
                    }

                }
                return (SummmaryEX, IncomesValues);
            }
        }
    }
}
