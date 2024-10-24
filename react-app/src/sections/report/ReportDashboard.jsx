import React, { useState } from 'react';
import {
  Box,
  Button,
  Card,
  CardContent,
  Container,
  Grid,
  Typography,
} from '@mui/material';
import EmployeeReport from './EmployeeReport';
import ExpenseReport from './ExpenseReport';
import LeaveReport from './LeaveReport';
import ShiftReport from './ShiftReport';

const ReportDashboard = () => {
  const [selectedReport, setSelectedReport] = useState(null);

  const renderReport = () => {
    switch (selectedReport) {
      case 'employee':
        return <EmployeeReport />;
      case 'leave':
        return <LeaveReport />;
      case 'expense':
        return <ExpenseReport />;
      case 'shift':
        return <ShiftReport />;
      default:
        return null;
    }
  };

  return (
    <Container maxWidth="lg">
      <Typography variant="h4" gutterBottom>
        Report Dashboard
      </Typography>
      <Grid container spacing={3}>
        <Grid item xs={12} md={3}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Select Report
              </Typography>
              <Box display="flex" flexDirection="column">
                <Button
                  variant={selectedReport === 'employee' ? 'contained' : 'outlined'}
                  onClick={() => setSelectedReport('employee')}
                  sx={{ mb: 1 }}
                >
                  Employee Report
                </Button>
                <Button
                  variant={selectedReport === 'leave' ? 'contained' : 'outlined'}
                  onClick={() => setSelectedReport('leave')}
                  sx={{ mb: 1 }}
                >
                  Leave Report
                </Button>
                <Button
                  variant={selectedReport === 'expense' ? 'contained' : 'outlined'}
                  onClick={() => setSelectedReport('expense')}
                  sx={{ mb: 1 }}
                >
                  Expense Report
                </Button>
                <Button
                  variant={selectedReport === 'shift' ? 'contained' : 'outlined'}
                  onClick={() => setSelectedReport('shift')}
                >
                  Shift Report
                </Button>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={12} md={9}>
          {renderReport()}
        </Grid>
      </Grid>
    </Container>
  );
};

export default ReportDashboard;