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
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { getShiftReport } from '../../services/api';
import useNotification from '../../hooks/useNotification';
import { fDate } from '../../utils/formatTime';

const ShiftReport = () => {
  const [startDate, setStartDate] = useState(new Date(new Date().getFullYear(), 0, 1));
  const [endDate, setEndDate] = useState(new Date());
  const [reportData, setReportData] = useState([]);
  const { addNotification } = useNotification();

  const fetchReport = useCallback(async () => {
    try {
      const response = await getShiftReport(fDate(startDate), fDate(endDate));
      setReportData(response.data);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch shift report' });
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
          <Typography variant="h4" gutterBottom>Shift Report</Typography>
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
            <BarChart data={reportData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="department" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Bar dataKey="morningShifts" name="Morning Shifts" fill="#8884d8" />
              <Bar dataKey="afternoonShifts" name="Afternoon Shifts" fill="#82ca9d" />
              <Bar dataKey="nightShifts" name="Night Shifts" fill="#ffc658" />
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>
    </LocalizationProvider>
  );
};

export default ShiftReport;