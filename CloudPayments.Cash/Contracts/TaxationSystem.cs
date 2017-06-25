namespace CloudPayments.Cash.Contracts
{
    public enum TaxationSystem
    {
        /// <summary>
        /// Common taxation system
        /// </summary>
        Common = 0,

        /// <summary>
        /// Simplified taxation system (Income)
        /// </summary>
        SimpleIncome = 1,

        /// <summary>
        /// Simplified taxation system (Income minus Expenditure)
        /// </summary>
        SimpleIncomeMinusExpenditure = 2,

        /// <summary>
        /// A single tax on imputed income
        /// </summary>
        UnifiedOnImputedIncome = 3,

        /// <summary>
        /// Unified agricultural tax
        /// </summary>
        UnifiedAgricultural = 4,

        /// <summary>
        /// Patent system of taxation
        /// </summary>
        Patent = 5
    }
}