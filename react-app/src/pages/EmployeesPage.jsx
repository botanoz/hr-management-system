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
  Modal,
  CircularProgress,
  Grid,
  IconButton,
} from '@mui/material';
import { useTheme } from '@mui/material/styles';
import { Add, Edit, Delete, Visibility } from '@mui/icons-material';
import Scrollbar from '../components/scrollbar';
import useAuth from '../hooks/useAuth';
import useNotification from '../hooks/useNotification';
import { getEmployees, deleteEmployee, getResume } from '../services/api';
import EmployeeForm from '../sections/employee/EmployeeForm';

const EmployeesPage = () => {
  const theme = useTheme();
  const { user } = useAuth();
  const { addNotification } = useNotification();

  const [employees, setEmployees] = useState([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [selectedEmployee, setSelectedEmployee] = useState(null);
  const [cvModalOpen, setCvModalOpen] = useState(false);
  const [resumeData, setResumeData] = useState(null);
  const [cvLoading, setCvLoading] = useState(false);
  const [cvError, setCvError] = useState(null);

  const fetchEmployees = useCallback(async () => {
    try {
      setLoading(true);
      const response = await getEmployees();
      setEmployees(response.data);
    } catch (err) {
      setError('Çalışanlar getirilemedi');
      console.error('Çalışanlar getirilemedi:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchEmployees();
  }, [fetchEmployees]);

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

  const handleEdit = (employee) => {
    setSelectedEmployee(employee);
    setIsFormOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu çalışanı silmek istediğinizden emin misiniz?')) {
      try {
        await deleteEmployee(id);
        addNotification({ type: 'success', message: 'Çalışan başarıyla silindi' });
        fetchEmployees();
      } catch (err) {
        addNotification({ type: 'error', message: 'Çalışan silinemedi' });
        console.error('Çalışan silinemedi:', err);
      }
    }
  };

  const handleFormClose = () => {
    setSelectedEmployee(null);
    setIsFormOpen(false);
    fetchEmployees();
  };

  const handleViewCV = async (employeeId) => {
    setCvLoading(true);
    setCvError(null);
    try {
      const response = await getResume(employeeId);
      setResumeData(response.data);
    } catch (err) {
      setCvError('Özgeçmiş verileri getirilemedi');
      console.error('Özgeçmiş verileri getirilemedi:', err);
    } finally {
      setCvLoading(false);
      setCvModalOpen(true);
    }
  };

  const handleCloseCvModal = () => {
    setCvModalOpen(false);
    setResumeData(null);
  };

  const renderCvContent = () => {
    if (cvLoading) {
      return <CircularProgress />;
    }

    if (cvError) {
      return <Typography color="error">{cvError}</Typography>;
    }

    if (resumeData) {
      return (
        <Box>
          <Typography variant="subtitle1" sx={{ mt: 2 }}>
            <strong>Ad:</strong> {resumeData.employeeName || 'N/A'}
          </Typography>
          <Typography variant="h6" sx={{ mt: 2, color: theme.palette.primary.main }}>
            Eğitim
          </Typography>
          {resumeData.educations.map((education) => (
            <Box key={education.id} sx={{ mt: 1 }}>
              <Typography variant="body2">
                <strong>Okul:</strong> {education.schoolName}
              </Typography>
              <Typography variant="body2">
                <strong>Derece:</strong> {education.degree} - {education.fieldOfStudy}
              </Typography>
              <Typography variant="body2">
                <strong>Devam Süresi:</strong> {new Date(education.startDate).toLocaleDateString()} - {new Date(education.endDate).toLocaleDateString()}
              </Typography>
            </Box>
          ))}
          <Typography variant="h6" sx={{ mt: 2, color: theme.palette.primary.main }}>
            İş Deneyimi
          </Typography>
          {resumeData.workExperiences.map((experience) => (
            <Box key={experience.id} sx={{ mt: 1 }}>
              <Typography variant="body2">
                <strong>Şirket:</strong> {experience.companyName}
              </Typography>
              <Typography variant="body2">
                <strong>Pozisyon:</strong> {experience.position}
              </Typography>
              <Typography variant="body2">
                <strong>Açıklama:</strong> {experience.description}
              </Typography>
              <Typography variant="body2">
                <strong>Devam Süresi:</strong> {new Date(experience.startDate).toLocaleDateString()} - {new Date(experience.endDate).toLocaleDateString()}
              </Typography>
            </Box>
          ))}
          <Typography variant="h6" sx={{ mt: 2, color: theme.palette.primary.main }}>
            Yetenekler
          </Typography>
          {resumeData.skills.map((skill) => (
            <Box key={skill.id} sx={{ mt: 1 }}>
              <Typography variant="body2">
                <strong>Yetenek:</strong> {skill.name}
              </Typography>
              <Typography variant="body2">
                <strong>Uzmanlık:</strong> {skill.proficiency}
              </Typography>
            </Box>
          ))}
          <Typography variant="h6" sx={{ mt: 2, color: theme.palette.primary.main }}>
            Sertifikalar
          </Typography>
          {resumeData.certifications.map((certification) => (
            <Box key={certification.id} sx={{ mt: 1 }}>
              <Typography variant="body2">
                <strong>Sertifika:</strong> {certification.name}
              </Typography>
              <Typography variant="body2">
                <strong>Veren Kuruluş:</strong> {certification.issuer}
              </Typography>
              <Typography variant="body2">
                <strong>Veriliş Tarihi:</strong> {new Date(certification.dateIssued).toLocaleDateString()}
              </Typography>
            </Box>
          ))}
          <Typography variant="h6" sx={{ mt: 2, color: theme.palette.primary.main }}>
            Diller
          </Typography>
          {resumeData.languages.map((language) => (
            <Box key={language.id} sx={{ mt: 1 }}>
              <Typography variant="body2">
                <strong>Dil:</strong> {language.name}
              </Typography>
              <Typography variant="body2">
                <strong>Uzmanlık:</strong> {language.proficiency}
              </Typography>
            </Box>
          ))}
          <Typography variant="h6" sx={{ mt: 2, color: theme.palette.primary.main }}>
            Ek Bilgiler
          </Typography>
          <Typography variant="body2">
            {resumeData.additionalInformation || 'N/A'}
          </Typography>
        </Box>
      );
    }

    return <Typography>Özgeçmiş verisi bulunamadı</Typography>;
  };

  const filteredEmployees = employees.filter((employee) =>
    employee.fullName.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const paginatedEmployees = filteredEmployees.slice(
    page * rowsPerPage,
    page * rowsPerPage + rowsPerPage
  );

  if (loading) {
    return <Typography>Yükleniyor...</Typography>;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  return (
    <>
      <Helmet>
        <title> Çalışanlar | İKYS </title>
      </Helmet>
      <Container maxWidth="xl" sx={{ mt: 4 }}>
        <Typography variant="h3" sx={{ mb: 5, fontWeight: 'bold', color: theme.palette.primary.main }}>
          Çalışanlar
        </Typography>

        <Card sx={{ p: 3, mb: 4 }}>
          <Grid container spacing={2} sx={{ mb: 2 }}>
            <Grid item xs={12} md={6}>
              <TextField
                label="Çalışanlarda Ara"
                variant="outlined"
                size="small"
                fullWidth
                value={searchTerm}
                onChange={handleSearch}
              />
            </Grid>
            <Grid item xs={12} md={6} display="flex" justifyContent="flex-end" alignItems="center">
              {user.role === 'Manager' && (
                <Button
                  variant="contained"
                  color="primary"
                  startIcon={<Add />}
                  onClick={() => setIsFormOpen(true)}
                >
                  Yeni Çalışan
                </Button>
              )}
            </Grid>
          </Grid>

          <Scrollbar>
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Adı Soyadı</TableCell>
                    <TableCell>Email</TableCell>
                    <TableCell>Pozisyon</TableCell>
                    <TableCell>Departman</TableCell>
                    {user.role === 'Manager' && <TableCell>İşlemler</TableCell>}
                  </TableRow>
                </TableHead>
                <TableBody>
                  {paginatedEmployees.map((employee) => (
                    <TableRow key={employee.id}>
                      <TableCell>{employee.fullName}</TableCell>
                      <TableCell>{employee.email}</TableCell>
                      <TableCell>{employee.position}</TableCell>
                      <TableCell>{employee.departmentName || 'N/A'}</TableCell>
                      {user.role === 'Manager' && (
                        <TableCell>
                          <IconButton color="primary" onClick={() => handleEdit(employee)}>
                            <Edit />
                          </IconButton>
                          <IconButton color="secondary" onClick={() => handleDelete(employee.id)}>
                            <Delete />
                          </IconButton>
                          <IconButton color="info" onClick={() => handleViewCV(employee.employeeId)}>
                            <Visibility />
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
            count={filteredEmployees.length}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
          />
        </Card>

        <EmployeeForm
          open={isFormOpen}
          onClose={handleFormClose}
          employee={selectedEmployee}
        />

        <Modal
          open={cvModalOpen}
          onClose={handleCloseCvModal}
          aria-labelledby="modal-title"
          aria-describedby="modal-description"
        >
          <Box
            sx={{
              position: 'absolute',
              top: '50%',
              left: '50%',
              transform: 'translate(-50%, -50%)',
              width: '80%',
              maxWidth: 800,
              bgcolor: 'background.paper',
              borderRadius: 2,
              boxShadow: 24,
              p: 4,
            }}
          >
            <Typography id="modal-title" variant="h5" sx={{ mb: 2, color: theme.palette.primary.main }}>
              Özgeçmiş Bilgileri
            </Typography>
            {renderCvContent()}
          </Box>
        </Modal>
      </Container>
    </>
  );
};

export default EmployeesPage;
