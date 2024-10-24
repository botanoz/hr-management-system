using HrManagementSystem.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface IExpenseService
    {
        Task<ExpenseDto> GetExpenseByIdAsync(int expenseId);
        Task<IEnumerable<ExpenseDto>> GetExpensesByEmployeeAsync(Guid employeeId);
        Task<ExpenseDto> CreateExpenseAsync(ExpenseDto expenseDto);
        Task<bool> UpdateExpenseAsync(ExpenseDto expenseDto);
        Task<bool> DeleteExpenseAsync(int expenseId);
        Task<bool> ApproveExpenseAsync(ApprovalDto approvalDto);
        Task<bool> RejectExpenseAsync(ApprovalDto approvalDto);
        Task<decimal> GetTotalExpensesByEmployeeAsync(Guid employeeId, DateTime startDate, DateTime endDate);
        Task<PaginatedResultDto<ExpenseDto>> GetPaginatedExpensesAsync(int companyId, int pageNumber, int pageSize);
    }
}