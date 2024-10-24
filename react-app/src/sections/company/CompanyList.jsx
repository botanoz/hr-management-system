import React, { useState, useEffect, useCallback } from 'react';
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
  IconButton,
  Typography,
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { getCompanies, deleteCompany } from '../../services/api';
import useAuth from '../../hooks/useAuth';
import useNotification from '../../hooks/useNotification';
import { canManageCompanies } from '../../utils/permissions';
import { COMPANY_TYPES } from '../../utils/constants';
import CompanyForm from './CompanyForm';

const CompanyList = () => {
  const [companies, setCompanies] = useState([]);
  const [selectedCompany, setSelectedCompany] = useState(null);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const { user } = useAuth();
  const { addNotification } = useNotification();

  const fetchCompanies = useCallback(async () => {
    try {
      const response = await getCompanies();
      setCompanies(response.data);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch companies' });
    }
  }, [addNotification]);

  useEffect(() => {
    fetchCompanies();
  }, [fetchCompanies]);

  const handleEdit = (company) => {
    setSelectedCompany(company);
    setIsFormOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this company?')) {
      try {
        await deleteCompany(id);
        addNotification({ type: 'success', message: 'Company deleted successfully' });
        fetchCompanies();
      } catch (error) {
        addNotification({ type: 'error', message: 'Failed to delete company' });
      }
    }
  };

  const handleFormClose = () => {
    setSelectedCompany(null);
    setIsFormOpen(false);
    fetchCompanies();
  };

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Companies
      </Typography>
      {canManageCompanies(user.role) && (
        <Button variant="contained" color="primary" onClick={() => setIsFormOpen(true)} sx={{ mb: 2 }}>
          Add New Company
        </Button>
      )}
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Type</TableCell>
              <TableCell>Industry</TableCell>
              <TableCell>Employees</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {companies.map((company) => (
              <TableRow key={company.id}>
                <TableCell>{company.name}</TableCell>
                <TableCell>{COMPANY_TYPES[company.type]}</TableCell>
                <TableCell>{company.industry}</TableCell>
                <TableCell>{company.employeeCount}</TableCell>
                <TableCell>
                  {canManageCompanies(user.role) && (
                    <>
                      <IconButton onClick={() => handleEdit(company)}>
                        <EditIcon />
                      </IconButton>
                      <IconButton onClick={() => handleDelete(company.id)}>
                        <DeleteIcon />
                      </IconButton>
                    </>
                  )}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <CompanyForm
        open={isFormOpen}
        onClose={handleFormClose}
        company={selectedCompany}
      />
    </div>
  );
};

export default CompanyList;
