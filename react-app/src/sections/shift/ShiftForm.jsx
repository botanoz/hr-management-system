import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  TextField,
} from '@mui/material';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { TimePicker } from '@mui/x-date-pickers/TimePicker';
import { createShift, getEmployees, updateShift } from '../../services/api';
import { SHIFT_TYPES } from '../../utils/constants';
import useNotification from '../../hooks/useNotification';

const initialFormState = {
  employeeId: '',
  date: null,
  type: '',
  startTime: null,
  endTime: null,
};

const ShiftForm = ({ open, onClose, shift }) => {
  const [formData, setFormData] = useState(initialFormState);
  const [employees, setEmployees] = useState([]);
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

  useEffect(() => {
    if (shift) {
      setFormData({
        ...shift,
        date: new Date(shift.date),
        startTime: new Date(shift.startTime),
        endTime: new Date(shift.endTime),
      });
    } else {
      setFormData(initialFormState);
    }
  }, [shift]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handleDateChange = (date) => {
    setFormData((prevData) => ({ ...prevData, date }));
  };

  const handleTimeChange = (name) => (time) => {
    setFormData((prevData) => ({ ...prevData, [name]: time }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (shift) {
        await updateShift(shift.id, formData);
        addNotification({ type: 'success', message: 'Shift updated successfully' });
      } else {
        await createShift(formData);
        addNotification({ type: 'success', message: 'Shift created successfully' });
      }
      onClose();
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to save shift' });
    }
  };

  return (
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <Dialog open={open} onClose={onClose}>
        <DialogTitle>{shift ? 'Edit Shift' : 'New Shift'}</DialogTitle>
        <form onSubmit={handleSubmit}>
          <DialogContent>
            <FormControl fullWidth margin="dense">
              <InputLabel>Employee</InputLabel>
              <Select
                name="employeeId"
                value={formData.employeeId}
                onChange={handleChange}
                required
              >
                {employees.map((employee) => (
                  <MenuItem key={employee.id} value={employee.id}>{employee.name}</MenuItem>
                ))}
              </Select>
            </FormControl>
            <DatePicker
              label="Date"
              value={formData.date}
              onChange={handleDateChange}
              renderInput={(params) => <TextField {...params} fullWidth margin="dense" required />}
            />
            <FormControl fullWidth margin="dense">
              <InputLabel>Shift Type</InputLabel>
              <Select
                name="type"
                value={formData.type}
                onChange={handleChange}
                required
              >
                {Object.entries(SHIFT_TYPES).map(([key, value]) => (
                  <MenuItem key={key} value={key}>{value}</MenuItem>
                ))}
              </Select>
            </FormControl>
            <TimePicker
              label="Start Time"
              value={formData.startTime}
              onChange={handleTimeChange('startTime')}
              renderInput={(params) => <TextField {...params} fullWidth margin="dense" required />}
            />
            <TimePicker
              label="End Time"
              value={formData.endTime}
              onChange={handleTimeChange('endTime')}
              renderInput={(params) => <TextField {...params} fullWidth margin="dense" required />}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={onClose}>Cancel</Button>
            <Button type="submit" color="primary">
              {shift ? 'Update' : 'Create'}
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </LocalizationProvider>
  );
};

ShiftForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  shift: PropTypes.shape({
    id: PropTypes.string,
    date: PropTypes.string,
    startTime: PropTypes.string,
    endTime: PropTypes.string,
  }),
};



export default ShiftForm;
