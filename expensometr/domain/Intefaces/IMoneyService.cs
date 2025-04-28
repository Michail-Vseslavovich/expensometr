namespace expense_service.domain.Intefaces
{
    // T - доход\расход  L - тип доходов и расходов ( нужен для работы последних двух)
    public interface IMoneyService<T,L> where L : struct
    {
        public  Task<List<T>> GetAllByUserId(int id);

        public Task<bool> Post(T entity);

        public Task<bool> Update(T entity);

        public Task<bool> Delete(int id, int UserId);
        public Task<(int, Dictionary<L, int>)> CalculateMonthStatistic(int UserId);

        public Task<(int, Dictionary<L, int>)> CalculateTotalStatistic(int UserId);

    }
}
