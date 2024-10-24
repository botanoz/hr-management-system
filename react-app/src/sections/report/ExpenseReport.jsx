import React, { useState, useEffect, useCallback } from 'react';
import {
  Card,
  CardContent,
  Typography,
  Grid,
  TextField,
  Button,
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { getExpenseReport } from '../../services/api';
import useNotification from '../../hooks/useNotification';
import { fDate } from '../../utils/formatTime';
import { fCurrency } from '../../utils/formatNumber';

const ExpenseReport = () => {
  const [startDate, setStartDate] = useState(new Date(new Date().getFullYear(), 0, 1));
  const [endDate, setEndDate] = useState(new Date());
  const [reportData, setReportData] = useState([]);
  const { addNotification } = useNotification();

  const fetchReport = useCallback(async () => {
    try {
      const response = await getExpenseReport(fDate(startDate), fDate(endDate));
      setReportData(response.data);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch expense report' });
    }
  }, [startDate, endDate, addNotification]);

  useEffect(() => {
    fetchReport();
  }, [fetchReport]);

  const handleGenerateReport = () => {
    fetchReport();
  };

  return (
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <Card>
        <CardContent>
          <Typography variant="h4" gutterBottom>Expense Report</Typography>
          <Grid container spacing={2} alignItems="center" sx={{ mb: 3 }}>
            <Grid item xs={12} sm={4}>
              <DatePicker
                label="Start Date"
                value={startDate}
                onChange={(newValue) => setStartDate(newValue)}
                renderInput={(params) => <TextField {...params} fullWidth />}
              />
            </Grid>
            <Grid item xs={12} sm={4}>
              <DatePicker
                label="End Date"
                value={endDate}
                onChange={(newValue) => setEndDate(newValue)}
                renderInput={(params) => <TextField {...params} fullWidth />}
              />
            </Grid>
            <Grid item xs={12} sm={4}>
              <Button variant="contained" onClick={handleGenerateReport} fullWidth>
                Generate Report
              </Button>
            </Grid>
          </Grid>
          <ResponsiveContainer width="100%" height={400}>
            <LineChart data={reportData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="date" />
              <YAxis tickFormatter={(value) => fCurrency(value)} />
              <Tooltip formatter={(value) => fCurrency(value)} />
              <Legend />
              <Line type="monotone" dataKey="totalExpense" name="Total Expense" stroke="#8884d8" />
              <Line type="monotone" dataKey="averageExpense" name="Average Expense" stroke="#82ca9d" />
            </LineChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>
    </LocalizationProvider>
  );
};

export default ExpenseReport;
