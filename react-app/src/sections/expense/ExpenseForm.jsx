import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { createExpense, updateExpense } from '../../services/api';
import useNotification from '../../hooks/useNotification';
import useAuth from '../../hooks/useAuth';
import { EXPENSE_CATEGORIES } from '../../utils/constants';

const initialFormState = {
  date: null,
  category: '',
  amount: '',
  description: '',
};

const ExpenseForm = ({ open, onClose, expense }) => {
  const [formData, setFormData] = useState(initialFormState);
  const { addNotification } = useNotification();
  const { user } = useAuth();

  useEffect(() => {
    if (expense) {
      setFormData({
        ...expense,
        date: new Date(expense.date),
      });
    } else {
      setFormData(initialFormState);
    }
  }, [expense]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handleDateChange = (date) => {
    setFormData((prevData) => ({ ...prevData, date }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const expenseData = {
        ...formData,
        employeeId: user.id,
        status: 'pending',
      };

      if (expense) {
        await updateExpense(expense.id, expenseData);
        addNotification({ type: 'success', message: 'Expense updated successfully' });
      } else {
        await createExpense(expenseData);
        addNotification({ type: 'success', message: 'Expense created successfully' });
      }
      onClose();
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to save expense' });
    }
  };

  return (
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <Dialog open={open} onClose={onClose}>
        <DialogTitle>{expense ? 'Edit Expense' : 'New Expense'}</DialogTitle>
        <form onSubmit={handleSubmit}>
          <DialogContent>
            <DatePicker
              label="Date"
              value={formData.date}
              onChange={handleDateChange}
              renderInput={(params) => <TextField {...params} fullWidth margin="dense" required />}
            />
            <FormControl fullWidth margin="dense">
              <InputLabel>Category</InputLabel>
              <Select
                name="category"
                value={formData.category}
                onChange={handleChange}
                required
              >
                {Object.entries(EXPENSE_CATEGORIES).map(([key, value]) => (
                  <MenuItem key={key} value={key}>{value}</MenuItem>
                ))}
              </Select>
            </FormControl>
            <TextField
              margin="dense"
              name="amount"
              label="Amount"
              type="number"
              fullWidth
              value={formData.amount}
              onChange={handleChange}
              required
            />
            <TextField
              margin="dense"
              name="description"
              label="Description"
              type="text"
              fullWidth
              multiline
              rows={4}
              value={formData.description}
              onChange={handleChange}
              required
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={onClose}>Cancel</Button>
            <Button type="submit" color="primary">
              {expense ? 'Update' : 'Submit'}
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </LocalizationProvider>
  );
};

ExpenseForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  expense: PropTypes.shape({
    id: PropTypes.string,
    date: PropTypes.oneOfType([PropTypes.string, PropTypes.instanceOf(Date)]),
    category: PropTypes.string,
    amount: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    description: PropTypes.string,
  }),
};


export default ExpenseForm;
