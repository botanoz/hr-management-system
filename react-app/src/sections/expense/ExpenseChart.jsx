import React, { useState, useEffect, useCallback } from 'react';
import { Paper, Typography } from '@mui/material';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { getExpenses } from '../../services/api';
import useNotification from '../../hooks/useNotification';
import { EXPENSE_CATEGORIES } from '../../utils/constants';
import { fCurrency } from '../../utils/formatNumber';

const ExpenseChart = () => {
  const [chartData, setChartData] = useState([]);
  const { addNotification } = useNotification();

  const fetchExpenseData = useCallback(async () => {
    try {
      const response = await getExpenses();
      const expenses = response.data;

      const categoryTotals = Object.keys(EXPENSE_CATEGORIES).reduce((acc, category) => {
        acc[category] = 0;
        return acc;
      }, {});

      expenses.forEach(expense => {
        if (Object.prototype.hasOwnProperty.call(categoryTotals, expense.category)) {
          categoryTotals[expense.category] += expense.amount;
        }
      });

      const formattedData = Object.entries(categoryTotals).map(([category, total]) => ({
        category: EXPENSE_CATEGORIES[category],
        total,
      }));

      setChartData(formattedData);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch expense data' });
    }
  }, [addNotification]);

  useEffect(() => {
    fetchExpenseData();
  }, [fetchExpenseData]);

  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="h4" gutterBottom>
        Expense Analysis
      </Typography>
      <ResponsiveContainer width="100%" height={400}>
        <BarChart data={chartData}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="category" />
          <YAxis tickFormatter={(value) => fCurrency(value)} />
          <Tooltip formatter={(value) => fCurrency(value)} />
          <Legend />
          <Bar dataKey="total" fill="#8884d8" />
        </BarChart>
      </ResponsiveContainer>
    </Paper>
  );
};

export default ExpenseChart;