import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
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
import { getEmployees, deleteEmployee } from '../../services/api';
import useAuth from '../../hooks/useAuth';
import useNotification from '../../hooks/useNotification';
import { canManageEmployees } from '../../utils/permissions';
import EmployeeForm from './EmployeeForm';

const EmployeeList = () => {
  const [employees, setEmployees] = useState([]);
  const [selectedEmployee, setSelectedEmployee] = useState(null);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const { user } = useAuth();
  const { addNotification } = useNotification();

  const fetchEmployees = useCallback(async () => {
    try {
      const response = await getEmployees();
      setEmployees(response.data);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch employees' });
    }
  }, [addNotification]);

  useEffect(() => {
    fetchEmployees();
  }, [fetchEmployees]);

  const handleEdit = (employee) => {
    setSelectedEmployee(employee);
    setIsFormOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this employee?')) {
      try {
        await deleteEmployee(id);
        addNotification({ type: 'success', message: 'Employee deleted successfully' });
        fetchEmployees();
      } catch (error) {
        addNotification({ type: 'error', message: 'Failed to delete employee' });
      }
    }
  };

  const handleFormClose = () => {
    setSelectedEmployee(null);
    setIsFormOpen(false);
    fetchEmployees();
  };

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Employees
      </Typography>
      {canManageEmployees(user.role) && (
        <Button variant="contained" color="primary" onClick={() => setIsFormOpen(true)} sx={{ mb: 2 }}>
          Add New Employee
        </Button>
      )}
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Position</TableCell>
              <TableCell>Department</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {employees.map((employee) => (
              <TableRow key={employee.id}>
                <TableCell>{employee.name}</TableCell>
                <TableCell>{employee.email}</TableCell>
                <TableCell>{employee.position}</TableCell>
                <TableCell>{employee.department}</TableCell>
                <TableCell>
                  {canManageEmployees(user.role) && (
                    <>
                      <IconButton onClick={() => handleEdit(employee)}>
                        <EditIcon />
                      </IconButton>
                      <IconButton onClick={() => handleDelete(employee.id)}>
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
      <EmployeeForm
        open={isFormOpen}
        onClose={handleFormClose}
        employee={selectedEmployee}
      />
    </div>
  );
};

EmployeeList.propTypes = {
  // If you have any props for EmployeeList, define them here
};

export default EmployeeList;

// EmployeeForm component (you might want to move this to a separate file)


EmployeeForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  employee: PropTypes.shape({
    id: PropTypes.string,
    name: PropTypes.string,
    email: PropTypes.string,
    position: PropTypes.string,
    department: PropTypes.string,
    // Add other employee properties here
  }),
};

EmployeeForm.defaultProps = {
  employee: null,
};

// ExpenseForm component (you might want to move this to a separate file)
const ExpenseForm = ({ open, onClose, expense }) => {
  // Implementation of ExpenseForm
};

ExpenseForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  expense: PropTypes.shape({
    id: PropTypes.string,
    date: PropTypes.oneOfType([PropTypes.string, PropTypes.instanceOf(Date)]),
    // Add other expense properties here
  }),
};

