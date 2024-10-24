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
  TableHead,
  TableRow,
  Paper,
  Button,
  TextField,
  Box,
  TablePagination,
  Modal,
  MenuItem,
  Select,
  InputLabel,
  FormControl,
  Grid,
  IconButton,
  CircularProgress,
  InputAdornment,
} from '@mui/material';
import { useTheme } from '@mui/material/styles';
import { Add, Edit, Delete, Search } from '@mui/icons-material';

// Components
import Scrollbar from '../components/scrollbar';
import useAuth from '../hooks/useAuth';

// API
import { getShifts, deleteShift, createShift, updateShift, getEmployees } from '../services/api';

const ShiftsPage = () => {
  const theme = useTheme();
  const { user } = useAuth();
  const [shifts, setShifts] = useState([]);
  const [employees, setEmployees] = useState([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [editingShift, setEditingShift] = useState(null);
  const [openModal, setOpenModal] = useState(false);
  const [newShift, setNewShift] = useState({
    employeeId: '',
    employeeName: '',
    startTime: '',
    endTime: '',
    shiftType: '',
    notes: '',
  });

  const fetchShifts = useCallback(async () => {
    try {
      setLoading(true);
      const response = await getShifts();
      setShifts(response.data);
    } catch (err) {
      setError('Vardiyalar alınamadı');
      console.error('Vardiyalar alınamadı:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  const fetchEmployees = useCallback(async () => {
    try {
      if (user.role === 'Manager') { 
        const response = await getEmployees(); 
        setEmployees(response.data);
      }
    } catch (err) {
      setError('Çalışanlar alınamadı');
      console.error('Çalışanlar alınamadı:', err);
    }
  }, [user.role]);

  useEffect(() => {
    fetchShifts();
    fetchEmployees();
  }, [fetchShifts, fetchEmployees]);

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

  const handleDelete = async (id) => {
    try {
      await deleteShift(id);
      fetchShifts();
    } catch (err) {
      console.error('Vardiya silinemedi:', err);
    }
  };

  const handleEdit = (shift) => {
    setEditingShift(shift);
    setNewShift({
      employeeId: shift.employeeId || '',
      employeeName: shift.employeeName || '',
      startTime: new Date(shift.startTime).toISOString().slice(0, 16),
      endTime: new Date(shift.endTime).toISOString().slice(0, 16),
      shiftType: shift.shiftType || '',
      notes: shift.notes || '',
    });
    setOpenModal(true);
  };

  const handleCloseModal = () => {
    setEditingShift(null);
    setOpenModal(false);
  };

  const handleSaveShift = async () => {
    const { employeeId, employeeName, startTime, endTime, shiftType, notes } = newShift;
    if (!employeeId || !employeeName || !startTime || !endTime || !shiftType) {
      setError('Tüm alanlar doldurulmalıdır');
      return;
    }

    try {
      if (editingShift) {
        await updateShift(editingShift.id, newShift);
      } else {
        await createShift(newShift);
      }
      handleCloseModal();
      fetchShifts();
    } catch (err) {
      console.error('Vardiya kaydedilemedi:', err);
      setError('Vardiya kaydedilemedi');
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewShift((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleEmployeeChange = (e) => {
    const selectedEmployee = employees.find(emp => emp.id === e.target.value);
    setNewShift((prev) => ({
      ...prev,
      employeeId: selectedEmployee.id,
      employeeName: selectedEmployee.fullName,
    }));
  };

  const filteredShifts = shifts.filter((shift) =>
    (shift.employeeName || '').toLowerCase().includes(searchTerm.toLowerCase())
  );

  const paginatedShifts = filteredShifts.slice(
    page * rowsPerPage,
    page * rowsPerPage + rowsPerPage
  );

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
        <title> Vardiyalar | İKYS </title>
      </Helmet>
      <Container maxWidth="xl" sx={{ mt: 4 }}>
        <Typography variant="h4" sx={{ mb: 5, fontWeight: 'bold', color: theme.palette.primary.main }}>
          Vardiyalar
        </Typography>

        <Card sx={{ p: 3, mb: 4 }}>
          <Grid container spacing={2} alignItems="center">
            <Grid item xs={12} md={6}>
              <TextField
                label="Vardiya Arama"
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
              {user.role === 'Manager' && (
                <Button
                  variant="contained"
                  startIcon={<Add />}
                  onClick={() => {
                    setEditingShift(null);
                    setNewShift({
                      employeeId: '',
                      employeeName: '',
                      startTime: '',
                      endTime: '',
                      shiftType: '',
                      notes: '',
                    });
                    setOpenModal(true);
                  }}
                  sx={{ backgroundColor: theme.palette.secondary.main }}
                >
                  Yeni Vardiya
                </Button>
              )}
            </Grid>
          </Grid>

          <Scrollbar>
            <TableContainer component={Paper} sx={{ mt: 2 }}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Çalışan</TableCell>
                    <TableCell>Başlangıç Saati</TableCell>
                    <TableCell>Bitiş Saati</TableCell>
                    <TableCell>Vardiya Türü</TableCell>
                    {user.role === 'Manager' && <TableCell>İşlemler</TableCell>}
                  </TableRow>
                </TableHead>
                <TableBody>
                  {paginatedShifts.map((shift) => (
                    <TableRow key={shift.id}>
                      <TableCell>{shift.employeeName || 'Bilinmeyen'}</TableCell>
                      <TableCell>{new Date(shift.startTime).toLocaleString()}</TableCell>
                      <TableCell>{new Date(shift.endTime).toLocaleString()}</TableCell>
                      <TableCell>{shift.shiftType}</TableCell>
                      {user.role === 'Manager' && (
                        <TableCell>
                          <IconButton color="primary" onClick={() => handleEdit(shift)}>
                            <Edit />
                          </IconButton>
                          <IconButton color="error" onClick={() => handleDelete(shift.id)}>
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
            count={filteredShifts.length}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
          />
        </Card>

        <Modal
          open={openModal}
          onClose={handleCloseModal}
          aria-labelledby="modal-modal-title"
          aria-describedby="modal-modal-description"
        >
          <Box
            sx={{
              position: 'absolute',
              top: '50%',
              left: '50%',
              transform: 'translate(-50%, -50%)',
              width: 400,
              bgcolor: 'background.paper',
              borderRadius: 2,
              boxShadow: 24,
              p: 4,
            }}
          >
            <Typography id="modal-modal-title" variant="h6" sx={{ mb: 2, color: theme.palette.primary.main }}>
              {editingShift ? 'Vardiya Düzenle' : 'Yeni Vardiya'}
            </Typography>

            <FormControl fullWidth margin="normal">
              <InputLabel id="employee-label">Çalışan</InputLabel>
              <Select
                labelId="employee-label"
                name="employeeId"
                value={newShift.employeeId}
                onChange={handleEmployeeChange}
                label="Çalışan"
              >
                {employees.map((employee) => (
                  <MenuItem key={employee.id} value={employee.id}>
                    {employee.fullName}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>

            <TextField
              fullWidth
              margin="normal"
              label="Başlangıç Saati"
              name="startTime"
              type="datetime-local"
              value={newShift.startTime}
              onChange={handleInputChange}
              InputLabelProps={{ shrink: true }}
            />
            <TextField
              fullWidth
              margin="normal"
              label="Bitiş Saati"
              name="endTime"
              type="datetime-local"
              value={newShift.endTime}
              onChange={handleInputChange}
              InputLabelProps={{ shrink: true }}
            />
            <TextField
              fullWidth
              margin="normal"
              label="Vardiya Türü"
              name="shiftType"
              value={newShift.shiftType}
              onChange={handleInputChange}
            />
            <TextField
              fullWidth
              margin="normal"
              label="Notlar"
              name="notes"
              value={newShift.notes}
              onChange={handleInputChange}
            />
            <Box sx={{ mt: 2, display: 'flex', justifyContent: 'flex-end' }}>
              <Button onClick={handleCloseModal} sx={{ mr: 2 }}>
                İptal
              </Button>
              <Button variant="contained" onClick={handleSaveShift}>
                {editingShift ? 'Güncelle' : 'Oluştur'}
              </Button>
            </Box>
          </Box>
        </Modal>
      </Container>
    </>
  );
};

export default ShiftsPage;
