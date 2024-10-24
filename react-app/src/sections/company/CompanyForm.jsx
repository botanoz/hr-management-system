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
    setFormData((prevData) => ({ ...prevData, [name]: value }));
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
      <DialogTitle>{company ? 'Edit Company' : 'New Company'}</DialogTitle>
      <form onSubmit={handleSubmit}>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            name="name"
            label="Company Name"
            type="text"
            fullWidth
            value={formData.name}
            onChange={handleChange}
            required
          />
          <FormControl fullWidth margin="dense">
            <InputLabel>Company Type</InputLabel>
            <Select
              name="type"
              value={formData.type}
              onChange={handleChange}
              required
            >
              {Object.entries(COMPANY_TYPES).map(([key, value]) => (
                <MenuItem key={key} value={key}>{value}</MenuItem>
              ))}
            </Select>
          </FormControl>
          <TextField
            margin="dense"
            name="industry"
            label="Industry"
            type="text"
            fullWidth
            value={formData.industry}
            onChange={handleChange}
            required
          />
          <TextField
            margin="dense"
            name="employeeCount"
            label="Number of Employees"
            type="number"
            fullWidth
            value={formData.employeeCount}
            onChange={handleChange}
            required
          />
          <TextField
            margin="dense"
            name="address"
            label="Address"
            type="text"
            fullWidth
            value={formData.address}
            onChange={handleChange}
          />
          <TextField
            margin="dense"
            name="website"
            label="Website"
            type="url"
            fullWidth
            value={formData.website}
            onChange={handleChange}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancel</Button>
          <Button type="submit" color="primary">
            {company ? 'Update' : 'Create'}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};

CompanyForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  company: PropTypes.shape({
    id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    type: PropTypes.string.isRequired,
    industry: PropTypes.string.isRequired,
    employeeCount: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
    address: PropTypes.string,
    website: PropTypes.string,
  }),
};



export default CompanyForm;