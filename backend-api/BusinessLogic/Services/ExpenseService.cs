

using AutoMapper;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;
using HrManagementSystem.DataLayer.Entities;
using HrManagementSystem.DataLayer.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace HrManagementSystem.BusinessLogic.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ExpenseService> _logger;

        public ExpenseService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ExpenseService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ExpenseDto> GetExpenseByIdAsync(int expenseId)
        {
            try
            {
                var expense = await _unitOfWork.Expenses.GetByIdAsync(expenseId);
                if (expense == null)
                {
                    throw new Exception($"Expense with ID {expenseId} not found.");
                }
                return _mapper.Map<ExpenseDto>(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting expense with ID {expenseId}");
                throw;
            }
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesByEmployeeAsync(Guid employeeId)
        {
            try
            {
                var expenses = await _unitOfWork.Expenses.GetExpensesByEmployeeAsync(employeeId);
                return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting expenses for employee with ID {employeeId}");
                throw;
            }
        }

        public async Task<ExpenseDto> CreateExpenseAsync(ExpenseDto expenseDto)
        {
            try
            {
                var expense = _mapper.Map<Expense>(expenseDto);
                await _unitOfWork.Expenses.AddAsync(expense);
                await _unitOfWork.CompleteAsync();
                return _mapper.Map<ExpenseDto>(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new expense");
                throw;
            }
        }

        public async Task<bool> UpdateExpenseAsync(ExpenseDto expenseDto)
        {
            try
            {
                var existingExpense = await _unitOfWork.Expenses.GetByIdAsync(expenseDto.Id);
                if (existingExpense == null)
                {
                    throw new Exception($"Expense with ID {expenseDto.Id} not found.");
                }

                _mapper.Map(expenseDto, existingExpense);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating expense with ID {expenseDto.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId)
        {
            try
            {
                var expense = await _unitOfWork.Expenses.GetByIdAsync(expenseId);
                if (expense == null)
                {
                    throw new Exception($"Expense with ID {expenseId} not found.");
                }

                await _unitOfWork.Expenses.DeleteAsync(expense);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting expense with ID {expenseId}");
                throw;
            }
        }

        public async Task<bool> ApproveExpenseAsync(ApprovalDto approvalDto)
        {
            try
            {
                var expense = await _unitOfWork.Expenses.GetByIdAsync(approvalDto.Id);
                if (expense == null)
                {
                    throw new Exception($"Expense with ID {approvalDto.Id} not found.");
                }

                expense.Status = "Approved";
                expense.ApproverComments = approvalDto.Comments;
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while approving expense with ID {approvalDto.Id}");
                throw;
            }
        }

        public async Task<bool> RejectExpenseAsync(ApprovalDto approvalDto)
        {
            try
            {
                var expense = await _unitOfWork.Expenses.GetByIdAsync(approvalDto.Id);
                if (expense == null)
                {
                    throw new Exception($"Expense with ID {approvalDto.Id} not found.");
                }

                expense.Status = "Rejected";
                expense.ApproverComments = approvalDto.Comments;
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while rejecting expense with ID {approvalDto.Id}");
                throw;
            }
        }

        public async Task<decimal> GetTotalExpensesByEmployeeAsync(Guid employeeId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var expenses = await _unitOfWork.Expenses.GetExpensesByEmployeeAsync(employeeId);
                return expenses
                    .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate && e.Status == "Approved")
                    .Sum(e => e.Amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while calculating total expenses for employee {employeeId}");
                throw;
            }
        }

        public async Task<PaginatedResultDto<ExpenseDto>> GetPaginatedExpensesAsync(int companyId, int pageNumber, int pageSize)
        {
            try
            {
                var allExpenses = await _unitOfWork.Expenses.GetAllAsync();

                // Filter company expenses, handling possible null Employee references
                var companyExpenses = allExpenses.Where(e => e.Employee?.CompanyId == companyId);

                var totalCount = companyExpenses.Count();
                var paginatedExpenses = companyExpenses
                    .OrderByDescending(e => e.ExpenseDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var expenseDtos = _mapper.Map<IEnumerable<ExpenseDto>>(paginatedExpenses);

                return new PaginatedResultDto<ExpenseDto>
                {
                    Items = expenseDtos.ToList(),
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting paginated expenses for company {companyId}");
                throw;
            }
        }

    }
}