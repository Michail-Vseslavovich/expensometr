namespace expense_service.domain.dto
{
    public class ExpenseDTO
    {
        public readonly int value;
        public readonly ExpenseType exType;

        public ExpenseDTO(int value, ExpenseType exType)
        {
            this.value = value;
            this.exType = exType;
        }
    }
}
