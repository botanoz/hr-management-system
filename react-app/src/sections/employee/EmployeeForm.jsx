import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from '@mui/material';
import { createEmployee, updateEmployee } from '../../services/api';
import useNotification from '../../hooks/useNotification';

const initialFormState = {
  name: '',
  email: '',
  position: '',
  department: '',
};

const EmployeeForm = ({ open, onClose, employee }) => {
  const [formData, setFormData] = useState(initialFormState);
  const { addNotification } = useNotification();

  useEffect(() => {
    if (employee) {
      setFormData(employee);
    } else {
      setFormData(initialFormState);
    }
  }, [employee]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (employee) {
        await updateEmployee(employee.id, formData);
        addNotification({ type: 'success', message: 'Employee updated successfully' });
      } else {
        await createEmployee(formData);
        addNotification({ type: 'success', message: 'Employee created successfully' });
      }
      onClose();
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to save employee' });
    }
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>{employee ? 'Edit Employee' : 'Add New Employee'}</DialogTitle>
      <form onSubmit={handleSubmit}>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            name="name"
            label="Name"
            type="text"
            fullWidth
            value={formData.name}
            onChange={handleChange}
            required
          />
          <TextField
            margin="dense"
            name="email"
            label="Email"
            type="email"
            fullWidth
            value={formData.email}
            onChange={handleChange}
            required
          />
          <TextField
            margin="dense"
            name="position"
            label="Position"
            type="text"
            fullWidth
            value={formData.position}
            onChange={handleChange}
            required
          />
          <FormControl fullWidth margin="dense">
            <InputLabel>Department</InputLabel>
            <Select
              name="department"
              value={formData.department}
              onChange={handleChange}
              required
            >
              <MenuItem value="HR">HR</MenuItem>
              <MenuItem value="IT">IT</MenuItem>
              <MenuItem value="Finance">Finance</MenuItem>
              <MenuItem value="Marketing">Marketing</MenuItem>
            </Select>
          </FormControl>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancel</Button>
          <Button type="submit" color="primary">
            {employee ? 'Update' : 'Create'}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};

// PropTypes tanımları
EmployeeForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  employee: PropTypes.shape({
    id: PropTypes.number,
    name: PropTypes.string,
    email: PropTypes.string,
    position: PropTypes.string,
    department: PropTypes.string,
  }),
};



export default EmployeeForm;
