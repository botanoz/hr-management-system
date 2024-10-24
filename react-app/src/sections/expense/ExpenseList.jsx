import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
  IconButton,
  Typography,
  Chip,
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { getExpenses, deleteExpense } from '../../services/api';
import useAuth from '../../hooks/useAuth';
import useNotification from '../../hooks/useNotification';
import { canManageExpenses } from '../../utils/permissions';
import { fDate } from '../../utils/formatTime';
import { fCurrency } from '../../utils/formatNumber';
import ExpenseForm from './ExpenseForm';

const ExpenseList = () => {
  const [expenses, setExpenses] = useState([]);
  const [selectedExpense, setSelectedExpense] = useState(null);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const { user } = useAuth();
  const { addNotification } = useNotification();

  const fetchExpenses = useCallback(async () => {
    try {
      const response = await getExpenses();
      setExpenses(response.data);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch expenses' });
    }
  }, [addNotification]);

  useEffect(() => {
    fetchExpenses();
  }, [fetchExpenses]);

  const handleEdit = (expense) => {
    setSelectedExpense(expense);
    setIsFormOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this expense?')) {
      try {
        await deleteExpense(id);
        addNotification({ type: 'success', message: 'Expense deleted successfully' });
        fetchExpenses();
      } catch (error) {
        addNotification({ type: 'error', message: 'Failed to delete expense' });
      }
    }
  };

  const handleFormClose = () => {
    setSelectedExpense(null);
    setIsFormOpen(false);
    fetchExpenses();
  };

  const getStatusColor = (status) => {
    switch (status) {
      case 'approved':
        return 'success';
      case 'rejected':
        return 'error';
      case 'pending':
        return 'warning';
      default:
        return 'default';
    }
  };

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Expenses
      </Typography>
      <Button variant="contained" color="primary" onClick={() => setIsFormOpen(true)} sx={{ mb: 2 }}>
        New Expense
      </Button>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Date</TableCell>
              <TableCell>Employee</TableCell>
              <TableCell>Category</TableCell>
              <TableCell>Amount</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {expenses.map((expense) => (
              <TableRow key={expense.id}>
                <TableCell>{fDate(expense.date)}</TableCell>
                <TableCell>{expense.employeeName}</TableCell>
                <TableCell>{expense.category}</TableCell>
                <TableCell>{fCurrency(expense.amount)}</TableCell>
                <TableCell>
                  <Chip label={expense.status} color={getStatusColor(expense.status)} />
                </TableCell>
                <TableCell>
                  {(canManageExpenses(user.role) || user.id === expense.employeeId) && (
                    <>
                      <IconButton onClick={() => handleEdit(expense)}>
                        <EditIcon />
                      </IconButton>
                      <IconButton onClick={() => handleDelete(expense.id)}>
                        <DeleteIcon />
                      </IconButton>
                    </>
                  )}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <ExpenseForm
        open={isFormOpen}
        onClose={handleFormClose}
        expense={selectedExpense}
      />
    </div>
  );
};

ExpenseList.propTypes = {
  // If you have any props for ExpenseList, define them here
};

export default ExpenseList;

// ExpenseForm component (you might want to move this to a separate file)


ExpenseForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  expense: PropTypes.shape({
    id: PropTypes.string,
    date: PropTypes.oneOfType([PropTypes.string, PropTypes.instanceOf(Date)]),
    employeeName: PropTypes.string,
    category: PropTypes.string,
    amount: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    status: PropTypes.string,
    employeeId: PropTypes.string,
  }),
};

ExpenseForm.defaultProps = {
  expense: null,
};

// LeaveForm component (you might want to move this to a separate file)
const LeaveForm = ({ open, onClose, leave }) => {
  // Implementation of LeaveForm
};

LeaveForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  leave: PropTypes.shape({
    id: PropTypes.string,
    startDate: PropTypes.oneOfType([PropTypes.string, PropTypes.instanceOf(Date)]),
    endDate: PropTypes.oneOfType([PropTypes.string, PropTypes.instanceOf(Date)]),
    // Add other leave properties here
  }),
};

