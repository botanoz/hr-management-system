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
  Chip,
  TablePagination,
  Modal,
  Grid,
  InputAdornment,
  IconButton,
  CircularProgress,
} from '@mui/material';
import { useTheme } from '@mui/material/styles';
import { Search, AddCircle, CheckCircle, Cancel } from '@mui/icons-material';
import Scrollbar from '../components/scrollbar';
import useAuth from '../hooks/useAuth';
import { getLeaves, createLeave, updateLeave } from '../services/api';

const LeavesPage = () => {
  const theme = useTheme();
  const { user } = useAuth();
  const [leaves, setLeaves] = useState([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [openModal, setOpenModal] = useState(false);
  const [newLeave, setNewLeave] = useState({
    employeeId: user.id,
    employeeName: user.fullName || 'Anonymous',
    startDate: '',
    endDate: '',
    leaveType: '',
    reason: '',
  });

  const fetchLeaves = useCallback(async () => {
    try {
      setLoading(true);
      const response = await getLeaves();
      setLeaves(response.data);
    } catch (err) {
      setError('İzin istekleri alınamadı');
      console.error('İzin istekleri alınamadı:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchLeaves();
  }, [fetchLeaves]);

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

  const handleOpenModal = () => setOpenModal(true);
  const handleCloseModal = () => setOpenModal(false);

  const handleCreateLeave = async () => {
    try {
      const leaveData = {
        ...newLeave,
        status: 'Pending',
        approverId: null,
        approverName: null,
      };
      await createLeave(leaveData);
      fetchLeaves();
      handleCloseModal();
    } catch (err) {
      console.error('Yeni izin isteği oluşturulamadı:', err);
    }
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setNewLeave({ ...newLeave, [name]: value });
  };

  const handleApprove = async (id) => {
    try {
      const leaveToUpdate = leaves.find((leave) => leave.id === id);
      const updatedLeave = {
        ...leaveToUpdate,
        status: 'Approved',
        approverId: user.id,
        approverName: user.fullName,
      };
      await updateLeave(id, updatedLeave);
      fetchLeaves();
    } catch (err) {
      console.error('İzin isteği onaylanamadı:', err);
    }
  };

  const handleReject = async (id) => {
    try {
      const leaveToUpdate = leaves.find((leave) => leave.id === id);
      const updatedLeave = {
        ...leaveToUpdate,
        status: 'Rejected',
        approverId: user.id,
        approverName: user.fullName,
      };
      await updateLeave(id, updatedLeave);
      fetchLeaves();
    } catch (err) {
      console.error('İzin isteği reddedilemedi:', err);
    }
  };

  const paginatedLeaves = leaves
    .filter((leave) => leave.employeeName && leave.employeeName.toLowerCase().includes(searchTerm.toLowerCase()))
    .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);

  const getStatusColor = (status) => {
    if (status === 'Approved') return 'success';
    if (status === 'Rejected') return 'error';
    return 'warning';
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
        <title> İzin Taleplerim | İKYS </title>
      </Helmet>
      <Container maxWidth="xl" sx={{ mt: 4 }}>
        <Typography variant="h3" sx={{ mb: 5, color: theme.palette.primary.main, fontWeight: 'bold' }}>
          İzin Taleplerim
        </Typography>

        <Card sx={{ p: 3, mb: 4 }}>
          <Grid container spacing={2} alignItems="center">
            <Grid item xs={12} md={6}>
              <TextField
                label="İzinlerde Ara"
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
                startIcon={<AddCircle />}
                onClick={handleOpenModal}
                sx={{ backgroundColor: theme.palette.secondary.main }}
              >
                Yeni İzin İsteği
              </Button>
            </Grid>
          </Grid>

          <Scrollbar>
            <TableContainer component={Paper} sx={{ mt: 2 }}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Çalışan</TableCell>
                    <TableCell>Başlangıç Tarihi</TableCell>
                    <TableCell>Bitiş Tarihi</TableCell>
                    <TableCell>İzin Türü</TableCell>
                    <TableCell>Durum</TableCell>
                    {user.role === 'Manager' && <TableCell>İşlemler</TableCell>}
                  </TableRow>
                </TableHead>
                <TableBody>
                  {paginatedLeaves.map((leave) => (
                    <TableRow key={leave.id}>
                      <TableCell>{leave.employeeName || 'İsimsiz Çalışan'}</TableCell>
                      <TableCell>{new Date(leave.startDate).toLocaleDateString()}</TableCell>
                      <TableCell>{new Date(leave.endDate).toLocaleDateString()}</TableCell>
                      <TableCell>{leave.leaveType}</TableCell>
                      <TableCell>
                        <Chip
                          label={leave.status}
                          color={getStatusColor(leave.status)}
                          sx={{ width: '100%' }}
                        />
                      </TableCell>
                      {user.role === 'Manager' && leave.status === 'Pending' && (
                        <TableCell>
                          <IconButton color="success" onClick={() => handleApprove(leave.id)}>
                            <CheckCircle />
                          </IconButton>
                          <IconButton color="error" onClick={() => handleReject(leave.id)}>
                            <Cancel />
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
            count={leaves.length}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
          />
        </Card>

        <Modal
          open={openModal}
          onClose={handleCloseModal}
          aria-labelledby="modal-title"
          aria-describedby="modal-description"
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
            <Typography id="modal-title" variant="h6" sx={{ mb: 2, color: theme.palette.primary.main }}>
              Yeni İzin İsteği Oluştur
            </Typography>
            <TextField
              label="İzin Türü"
              name="leaveType"
              value={newLeave.leaveType}
              onChange={handleInputChange}
              fullWidth
              sx={{ mt: 2 }}
            />
            <TextField
              label="Sebep"
              name="reason"
              value={newLeave.reason}
              onChange={handleInputChange}
              fullWidth
              sx={{ mt: 2 }}
            />
            <TextField
              label="Başlangıç Tarihi"
              type="date"
              name="startDate"
              value={newLeave.startDate}
              onChange={handleInputChange}
              fullWidth
              sx={{ mt: 2 }}
              InputLabelProps={{
                shrink: true,
              }}
            />
            <TextField
              label="Bitiş Tarihi"
              type="date"
              name="endDate"
              value={newLeave.endDate}
              onChange={handleInputChange}
              fullWidth
              sx={{ mt: 2 }}
              InputLabelProps={{
                shrink: true,
              }}
            />
            <Box sx={{ mt: 3, display: 'flex', justifyContent: 'flex-end' }}>
              <Button onClick={handleCloseModal} sx={{ mr: 1 }}>
                İptal
              </Button>
              <Button variant="contained" onClick={handleCreateLeave}>
                Oluştur
              </Button>
            </Box>
          </Box>
        </Modal>
      </Container>
    </>
  );
};

export default LeavesPage;
