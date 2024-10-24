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
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Grid,
  CircularProgress,
  IconButton,
} from '@mui/material';
import { useTheme } from '@mui/material/styles';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { Add, Edit, Delete } from '@mui/icons-material';

// Components
import Scrollbar from '../components/scrollbar';

// Hooks
import useAuth from '../hooks/useAuth';
import useNotification from '../hooks/useNotification';

// API
import { getHolidays, createHoliday, updateHoliday, deleteHoliday } from '../services/api';

const HolidaysPage = () => {
  const theme = useTheme();
  const { user } = useAuth();
  const { addNotification } = useNotification();

  const [holidays, setHolidays] = useState([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [selectedHoliday, setSelectedHoliday] = useState({ name: '', date: null, description: '' });

  const fetchHolidays = useCallback(async () => {
    try {
      setLoading(true);
      const response = await getHolidays();
      setHolidays(response.data);
    } catch (err) {
      setError('Tatil günleri alınamadı');
      console.error('Tatil günleri alınamadı:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchHolidays();
  }, [fetchHolidays]);

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const handleOpenForm = (holiday = { name: '', date: null, description: '' }) => {
    setSelectedHoliday(holiday);
    setIsFormOpen(true);
  };

  const handleCloseForm = () => {
    setIsFormOpen(false);
    setSelectedHoliday({ name: '', date: null, description: '' });
  };

  const handleSaveHoliday = async () => {
    try {
      if (selectedHoliday.id) {
        await updateHoliday(selectedHoliday.id, selectedHoliday);
        addNotification({ type: 'success', message: 'Tatil güncellemesi başarılı' });
      } else {
        await createHoliday(selectedHoliday);
        addNotification({ type: 'success', message: 'Tatil başarıyla oluşturuldu' });
      }
      fetchHolidays();
      handleCloseForm();
    } catch (err) {
      console.error('Tatil kaydedilemedi:', err);
      setError('Tatil kaydedilemedi');
    }
  };

  const handleDeleteHoliday = async (id) => {
    if (window.confirm('Bu tatili silmek istediğinize emin misiniz?')) {
      try {
        await deleteHoliday(id);
        addNotification({ type: 'success', message: 'Tatil başarıyla silindi' });
        fetchHolidays();
      } catch (err) {
        console.error('Tatil silinemedi:', err);
        setError('Tatil silinemedi');
      }
    }
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setSelectedHoliday((prev) => ({
      ...prev,
      [name]: value,
    }));
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
        <title> Tatil Günleri | İKYS </title>
      </Helmet>
      <Container maxWidth="xl" sx={{ mt: 4 }}>
        <Typography variant="h4" sx={{ mb: 5, fontWeight: 'bold', color: theme.palette.primary.main }}>
          Şirket Tatil Günleri
        </Typography>

        <Card sx={{ p: 3, mb: 4 }}>
          <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
            {user.role === 'Manager' && (
              <Button
                variant="contained"
                startIcon={<Add />}
                onClick={() => handleOpenForm()}
                sx={{ backgroundColor: theme.palette.secondary.main }}
              >
                Yeni Tatil
              </Button>
            )}
          </Box>

          <Scrollbar>
            <TableContainer component={Paper} sx={{ mt: 2 }}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>İsim</TableCell>
                    <TableCell>Tarih</TableCell>
                    <TableCell>Açıklama</TableCell>
                    {user.role === 'Manager' && <TableCell>İşlemler</TableCell>}
                  </TableRow>
                </TableHead>
                <TableBody>
                  {holidays
                    .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                    .map((holiday) => (
                      <TableRow key={holiday.id}>
                        <TableCell>{holiday.name}</TableCell>
                        <TableCell>{new Date(holiday.date).toLocaleDateString()}</TableCell>
                        <TableCell>{holiday.description}</TableCell>
                        {user.role === 'Manager' && (
                          <TableCell>
                            <IconButton color="primary" onClick={() => handleOpenForm(holiday)}>
                              <Edit />
                            </IconButton>
                            <IconButton color="error" onClick={() => handleDeleteHoliday(holiday.id)}>
                              <Delete />
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
            count={holidays.length}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
          />
        </Card>

        <Dialog open={isFormOpen} onClose={handleCloseForm} fullWidth maxWidth="sm">
          <DialogTitle>{selectedHoliday.id ? 'Tatil Gününü Düzenle' : 'Yeni Tatil Günü Ekle'}</DialogTitle>
          <DialogContent>
            <TextField
              fullWidth
              margin="normal"
              label="İsim"
              name="name"
              value={selectedHoliday.name}
              onChange={handleInputChange}
            />
            <LocalizationProvider dateAdapter={AdapterDateFns}>
              <DatePicker
                label="Tarih"
                value={selectedHoliday.date}
                onChange={(newValue) => {
                  setSelectedHoliday((prev) => ({
                    ...prev,
                    date: newValue,
                  }));
                }}
                renderInput={(params) => <TextField fullWidth margin="normal" {...params} />}
              />
            </LocalizationProvider>
            <TextField
              fullWidth
              margin="normal"
              label="Açıklama"
              name="description"
              value={selectedHoliday.description}
              onChange={handleInputChange}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={handleCloseForm}>İptal</Button>
            <Button variant="contained" onClick={handleSaveHoliday}>
              {selectedHoliday.id ? 'Güncelle' : 'Oluştur'}
            </Button>
          </DialogActions>
        </Dialog>
      </Container>
    </>
  );
};

export default HolidaysPage;
