using expense_service.domain;
using expense_service.domain.dto;
using expense_service.domain.Intefaces;
using expense_service.domain.Models;
using expense_service.infrastructure.db;
using Microsoft.EntityFrameworkCore;

namespace expense_service.application.Services
{
    public class ExpenseService() : IMoneyService<Expense, ExpenseType>
    {
        public async Task<List<Expense>> GetAllByUserId(int id)
        {
            using (var db = new ExpenseContext())
            {
                return await db.Expenses.Where(x => x.RelatedUserId == id).ToListAsync();
            }
        }

        public async Task<bool> Post(Expense entity)
        {
            using (var db = new ExpenseContext())
            {
                if (entity == null)
                {
                    return false;
                }
                
                await db.Expenses.AddAsync(entity);
                await db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> Update(Expense entity)
        {
            using (var db = new ExpenseContext())
            {
                if (entity == null)
                {
                    return false;
                }

                var e = await db.Expenses.FirstOrDefaultAsync(x => x.Id == entity.Id);

                if (e != null)
                {
                    e.Value = entity.Value;
                    e.Description = entity.Description;
                    db.Expenses.Update(e);
                    await db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<bool> Delete(int id, int UserId)
        {
            using (var db = new ExpenseContext())
            {
                Expense? e = await db.Expenses.FirstOrDefaultAsync(x => x.Id == id && x.RelatedUserId == UserId);

                if (e != null)
                {
                    db.Expenses.Remove(e);
                    await db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }


        public async Task<(int, Dictionary<ExpenseType, int>)> CalculateMonthStatistic(int UserId)
        {
            using (var db = new ExpenseContext())
            {
                var Now = DateTime.Now;
                ExpenseDTO[]? ExDtoArray = db.Expenses.Where(x => x.RelatedUserId == UserId && x.Created.Subtract(Now).TotalDays < 30).Select(u => new ExpenseDTO(u.Value, u.Type)).ToArray();
                int SummmaryEX = ExDtoArray.Sum(x => x.value);
                Dictionary<ExpenseType, int> ExpensesValues = new Dictionary<ExpenseType, int>();

                foreach (var expense in ExDtoArray)
                {
                    if (ExpensesValues.ContainsKey(expense.exType))
                    {
                        ExpensesValues[expense.exType] += expense.value;
                    }
                    else
                    {
                        ExpensesValues.Add(expense.exType, expense.value);
                    }

                }
                return (SummmaryEX, ExpensesValues);
            }
        }

        public async Task<(int, Dictionary<ExpenseType, int>)> CalculateTotalStatistic(int UserId)
        {
            using (var db = new ExpenseContext())
            {
                var Now = DateTime.Now;
                ExpenseDTO[]? ExDtoArray = db.Expenses.Where(x => x.RelatedUserId == UserId).Select(u => new ExpenseDTO(u.Value, u.Type)).ToArray();
                int SummmaryEX = ExDtoArray.Sum(x => x.value);
                Dictionary<ExpenseType, int> ExpensesValues = new Dictionary<ExpenseType, int>();

                foreach (var expense in ExDtoArray)
                {
                    if (ExpensesValues.ContainsKey(expense.exType))
                    {
                        ExpensesValues[expense.exType] += expense.value;
                    }
                    else
                    {
                        ExpensesValues.Add(expense.exType, expense.value);
                    }

                }
                return (SummmaryEX, ExpensesValues);
            }
        }
    }
}
