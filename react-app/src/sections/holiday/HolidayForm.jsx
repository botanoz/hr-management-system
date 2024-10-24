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
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { createHoliday, updateHoliday } from '../../services/api';
import useNotification from '../../hooks/useNotification';

const initialFormState = {
  name: '',
  date: null,
  type: '',
  description: '',
};

const HolidayForm = ({ open, onClose, holiday }) => {
  const [formData, setFormData] = useState(initialFormState);
  const { addNotification } = useNotification();

  useEffect(() => {
    if (holiday) {
      setFormData({
        ...holiday,
        date: holiday.date ? new Date(holiday.date) : null,
      });
    } else {
      setFormData(initialFormState);
    }
  }, [holiday]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handleDateChange = (date) => {
    setFormData((prevData) => ({ ...prevData, date }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (holiday) {
        await updateHoliday(holiday.id, formData);
        addNotification({ type: 'success', message: 'Holiday updated successfully' });
      } else {
        await createHoliday(formData);
        addNotification({ type: 'success', message: 'Holiday created successfully' });
      }
      onClose();
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to save holiday' });
    }
  };

  return (
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <Dialog open={open} onClose={onClose}>
        <DialogTitle>{holiday ? 'Edit Holiday' : 'New Holiday'}</DialogTitle>
        <form onSubmit={handleSubmit}>
          <DialogContent>
            <TextField
              autoFocus
              margin="dense"
              name="name"
              label="Holiday Name"
              type="text"
              fullWidth
              value={formData.name}
              onChange={handleChange}
              required
            />
            <DatePicker
              label="Holiday Date"
              value={formData.date}
              onChange={handleDateChange}
              renderInput={(params) => <TextField {...params} fullWidth margin="dense" required />}
            />
            <FormControl fullWidth margin="dense">
              <InputLabel>Holiday Type</InputLabel>
              <Select
                name="type"
                value={formData.type}
                onChange={handleChange}
                required
              >
                <MenuItem value="public">Public Holiday</MenuItem>
                <MenuItem value="company">Company Holiday</MenuItem>
                <MenuItem value="optional">Optional Holiday</MenuItem>
              </Select>
            </FormControl>
            <TextField
              margin="dense"
              name="description"
              label="Description"
              type="text"
              fullWidth
              multiline
              rows={4}
              value={formData.description}
              onChange={handleChange}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={onClose}>Cancel</Button>
            <Button type="submit" color="primary">
              {holiday ? 'Update' : 'Create'}
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </LocalizationProvider>
  );
};

HolidayForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  holiday: PropTypes.shape({
    id: PropTypes.string,
    name: PropTypes.string,
    date: PropTypes.oneOfType([PropTypes.string, PropTypes.instanceOf(Date)]),
    type: PropTypes.string,
    description: PropTypes.string,
  }),
};


export default HolidayForm;
