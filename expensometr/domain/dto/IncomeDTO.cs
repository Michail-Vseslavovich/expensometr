using expense_service.domain.enums;

namespace expense_service.domain.dto
{
    public class IncomeDTO
    {
        public readonly int value;
        public readonly IncomeType exType;

        public IncomeDTO(int value, IncomeType exType)
        {
            this.value = value;
            this.exType = exType;
        }
    }
}
