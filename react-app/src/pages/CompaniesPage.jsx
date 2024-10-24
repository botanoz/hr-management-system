import React, { useState, useEffect, useCallback } from 'react';
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
} from '@mui/material';
import { useTheme } from '@mui/material/styles';
import Iconify from '../components/iconify';
import Scrollbar from '../components/scrollbar';
import useAuth from '../hooks/useAuth';
import { getCompanies, deleteCompany } from '../services/api';
import CompanyForm from '../sections/company/CompanyForm';
import useNotification from '../hooks/useNotification';

const CompaniesPage = () => {
  const theme = useTheme();
  const { user } = useAuth();
  const { addNotification } = useNotification();

  const [companies, setCompanies] = useState([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [selectedCompany, setSelectedCompany] = useState(null);

  const fetchCompanies = useCallback(async () => {
    try {
      setLoading(true);
      const response = await getCompanies();
      setCompanies(response.data);
    } catch (err) {
      setError('Failed to fetch companies');
      console.error('Error fetching companies:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchCompanies();
  }, [fetchCompanies]);

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
    if (window.confirm('Are you sure you want to delete this company?')) {
      try {
        await deleteCompany(id);
        addNotification({ type: 'success', message: 'Company deleted successfully' });
        fetchCompanies(); // Refresh the list after deletion
      } catch (err) {
        addNotification({ type: 'error', message: 'Failed to delete company' });
        console.error('Error deleting company:', err);
      }
    }
  };

  const handleEdit = (company) => {
    setSelectedCompany(company);
    setIsFormOpen(true);
  };

  const handleFormClose = () => {
    setSelectedCompany(null);
    setIsFormOpen(false);
    fetchCompanies();
  };

  const filteredCompanies = companies.filter((company) =>
    company.name.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const paginatedCompanies = filteredCompanies.slice(
    page * rowsPerPage,
    page * rowsPerPage + rowsPerPage
  );

  const getStatusColor = (isApproved) => (isApproved ? 'success' : 'warning');

  if (loading) {
    return <Typography>Loading...</Typography>;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  return (
    <Container maxWidth="xl">
      <Typography variant="h4" sx={{ mb: 5 }}>
        Companies
      </Typography>

      <Card>
        <Box sx={{ p: 2, display: 'flex', justifyContent: 'space-between' }}>
          <TextField
            label="Search Companies"
            variant="outlined"
            size="small"
            value={searchTerm}
            onChange={handleSearch}
          />
          {user.role === 'Admin' && (
            <Button
              variant="contained"
              startIcon={<Iconify icon="eva:plus-fill" />}
              onClick={() => setIsFormOpen(true)}
            >
              New Company
            </Button>
          )}
        </Box>

        <Scrollbar>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Name</TableCell>
                  <TableCell>Registration Date</TableCell>
                  <TableCell>Address</TableCell>
                  <TableCell>Employee Count</TableCell>
                  <TableCell>Status</TableCell>
                  {user.role === 'Admin' && <TableCell>Actions</TableCell>}
                </TableRow>
              </TableHead>
              <TableBody>
                {paginatedCompanies.map((company) => (
                  <TableRow key={company.id}>
                    <TableCell>{company.name}</TableCell>
                    <TableCell>{new Date(company.registrationDate).toLocaleDateString()}</TableCell>
                    <TableCell>{company.address}</TableCell>
                    <TableCell>{company.employeeCount}</TableCell>
                    <TableCell>
                      <Chip
                        label={company.isApproved ? 'Approved' : 'Pending'}
                        color={getStatusColor(company.isApproved)}
                      />
                    </TableCell>
                    {user.role === 'Admin' && (
                      <TableCell>
                        <Button size="small" onClick={() => handleEdit(company)}>
                          Edit
                        </Button>
                        <Button
                          size="small"
                          color="error"
                          onClick={() => handleDelete(company.id)}
                        >
                          Delete
                        </Button>
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
          count={filteredCompanies.length}
          rowsPerPage={rowsPerPage}
          page={page}
          onPageChange={handleChangePage}
          onRowsPerPageChange={handleChangeRowsPerPage}
        />
      </Card>

      <CompanyForm
        open={isFormOpen}
        onClose={handleFormClose}
        company={selectedCompany}
      />
    </Container>
  );
};

export default CompaniesPage;
