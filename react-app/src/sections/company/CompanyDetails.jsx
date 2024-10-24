import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from '@mui/material';
import { createCompany, updateCompany } from '../../services/api';
import useNotification from '../../hooks/useNotification';
import { COMPANY_TYPES } from '../../utils/constants';

const initialFormState = {
  name: '',
  type: '',
  industry: '',
  employeeCount: '',
  address: '',
  website: '',
};

const CompanyForm = ({ open, onClose, company }) => {
  const [formData, setFormData] = useState(initialFormState);
  const { addNotification } = useNotification();

  useEffect(() => {
    if (company) {
      setFormData(company);
    } else {
      setFormData(initialFormState);
    }
  }, [company]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (company) {
        await updateCompany(company.id, formData);
        addNotification({ type: 'success', message: 'Company updated successfully' });
      } else {
        await createCompany(formData);
        addNotification({ type: 'success', message: 'Company created successfully' });
      }
      onClose();
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to save company' });
    }
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>{company ? 'Edit Company' : 'Add New Company'}</DialogTitle>
      <DialogContent>
        <form onSubmit={handleSubmit}>
          <TextField
            name="name"
            label="Company Name"
            value={formData.name}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <FormControl fullWidth margin="normal" required>
            <InputLabel>Company Type</InputLabel>
            <Select name="type" value={formData.type} onChange={handleChange}>
              {Object.entries(COMPANY_TYPES).map(([value, label]) => (
                <MenuItem key={value} value={value}>
                  {label}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <TextField
            name="industry"
            label="Industry"
            value={formData.industry}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            name="employeeCount"
            label="Number of Employees"
            type="number"
            value={formData.employeeCount}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            name="address"
            label="Address"
            value={formData.address}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            name="website"
            label="Website"
            value={formData.website}
            onChange={handleChange}
            fullWidth
            margin="normal"
          />
        </form>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={handleSubmit} color="primary">
          {company ? 'Update' : 'Create'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

CompanyForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  company: PropTypes.shape({
    id: PropTypes.string,
    name: PropTypes.string,
    type: PropTypes.string,
    industry: PropTypes.string,
    employeeCount: PropTypes.number,
    address: PropTypes.string,
    website: PropTypes.string,
  }),
};



export default CompanyForm;