import React, { useState, useEffect, useCallback } from 'react';
import { Helmet } from 'react-helmet-async';
import {
  Container,
  Typography,
  Card,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TablePagination,
  TableHead,
  TableRow,
  Paper,
  Button,
  TextField,
  Box,
  Chip,
  Grid,
  CircularProgress,
  IconButton,
  InputAdornment,
} from '@mui/material';
import { useTheme } from '@mui/material/styles';
import { Search, Add, Edit, CheckCircle, Cancel } from '@mui/icons-material';

// Components
import Iconify from '../components/iconify';
import Scrollbar from '../components/scrollbar';
import useAuth from '../hooks/useAuth';
import useNotification from '../hooks/useNotification';

// API
import { getExpenses, approveExpense, rejectExpense } from '../services/api';

// Custom Components
import ExpenseForm from '../sections/expense/ExpenseForm';

const ExpensesPage = () => {
  const theme = useTheme();
  const { user } = useAuth();
  const { addNotification } = useNotification();

  const [expenses, setExpenses] = useState([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [selectedExpense, setSelectedExpense] = useState(null);

  const fetchExpenses = useCallback(async () => {
    try {
      setLoading(true);
      const response = await getExpenses();

      // Ensure all employee names are non-null, replacing nulls with "Unknown"
      const expensesWithNames = response.data.map((expense) => ({
        ...expense,
        employeeName: expense.employeeName || 'Unknown',
      }));

      setExpenses(expensesWithNames);
    } catch (err) {
      setError('Harcamalar alınamadı');
      console.error('Harcamalar alınamadı:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchExpenses();
  }, [fetchExpenses]);

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const handleSearch = (event) => {
    setSearchTerm(event.target.value);
    setPage(0);
  };

  const handleApprove = async (id) => {
    try {
      await approveExpense(id);
      addNotification({ type: 'success', message: 'Harcama başarıyla onaylandı' });
      fetchExpenses(); // Refresh the list after approval
    } catch (err) {
      addNotification({ type: 'error', message: 'Harcama onaylanamadı' });
      console.error('Harcama onaylanamadı:', err);
    }
  };

  const handleReject = async (id) => {
    try {
      await rejectExpense(id);
      addNotification({ type: 'success', message: 'Harcama reddedildi' });
      fetchExpenses(); // Refresh the list after rejection
    } catch (err) {
      addNotification({ type: 'error', message: 'Harcama reddedilemedi' });
      console.error('Harcama reddedilemedi:', err);
    }
  };

  const handleEdit = (expense) => {
    setSelectedExpense(expense);
    setIsFormOpen(true);
  };

  const handleFormClose = () => {
    setSelectedExpense(null);
    setIsFormOpen(false);
    fetchExpenses();
  };

  const filteredExpenses = expenses.filter(
    (expense) =>
      expense.employeeName &&
      expense.employeeName.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const paginatedExpenses = filteredExpenses.slice(
    page * rowsPerPage,
    page * rowsPerPage + rowsPerPage
  );

  const getStatusColor = (status) => {
    switch (status) {
      case 'Approved':
        return 'success';
      case 'Rejected':
        return 'error';
      case 'Pending':
        return 'warning';
      default:
        return 'default';
    }
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '100vh' }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  return (
    <>
      <Helmet>
        <title> Harcamalar | İKYS </title>
      </Helmet>
      <Container maxWidth="xl" sx={{ mt: 4 }}>
        <Typography variant="h4" sx={{ mb: 5, fontWeight: 'bold', color: theme.palette.primary.main }}>
          Harcama Talepleri
        </Typography>

        <Card sx={{ p: 3, mb: 4 }}>
          <Grid container spacing={2} alignItems="center">
            <Grid item xs={12} md={6}>
              <TextField
                label="Harcama Taleplerinde Ara"
                variant="outlined"
                size="small"
                fullWidth
                value={searchTerm}
                onChange={handleSearch}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <Search />
                    </InputAdornment>
                  ),
                }}
              />
            </Grid>
            <Grid item xs={12} md={6} display="flex" justifyContent="flex-end">
              <Button
                variant="contained"
                startIcon={<Add />}
                onClick={() => setIsFormOpen(true)}
                sx={{ backgroundColor: theme.palette.secondary.main }}
              >
                Yeni Harcama Talebi
              </Button>
            </Grid>
          </Grid>

          <Scrollbar>
            <TableContainer component={Paper} sx={{ mt: 2 }}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Çalışan</TableCell>
                    <TableCell>Tarih</TableCell>
                    <TableCell>Tür</TableCell>
                    <TableCell>Tutar</TableCell>
                    <TableCell>Durum</TableCell>
                    {user.role === 'Manager' && <TableCell>İşlemler</TableCell>}
                  </TableRow>
                </TableHead>
                <TableBody>
                  {paginatedExpenses.map((expense) => (
                    <TableRow key={expense.id}>
                      <TableCell>{expense.employeeName}</TableCell>
                      <TableCell>{new Date(expense.expenseDate).toLocaleDateString()}</TableCell>
                      <TableCell>{expense.expenseType}</TableCell>
                      <TableCell>${expense.amount.toFixed(2)}</TableCell>
                      <TableCell>
                        <Chip
                          label={expense.status}
                          color={getStatusColor(expense.status)}
                          sx={{ width: '100%' }}
                        />
                      </TableCell>
                      {user.role === 'Manager' && (
                        <TableCell>
                          <IconButton color="success" onClick={() => handleApprove(expense.id)}>
                            <CheckCircle />
                          </IconButton>
                          <IconButton color="error" onClick={() => handleReject(expense.id)}>
                            <Cancel />
                          </IconButton>
                          <IconButton color="primary" onClick={() => handleEdit(expense)}>
                            <Edit />
                          </IconButton>
                        </TableCell>
                      )}
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </Scrollbar>

          <TablePagination
            component="div"
            count={filteredExpenses.length}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
          />
        </Card>

        <ExpenseForm
          open={isFormOpen}
          onClose={handleFormClose}
          expense={selectedExpense}
        />
      </Container>
    </>
  );
};

export default ExpensesPage;
