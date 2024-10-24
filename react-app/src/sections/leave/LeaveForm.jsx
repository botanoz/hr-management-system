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
import { createLeave, updateLeave } from '../../services/api';
import useNotification from '../../hooks/useNotification';
import useAuth from '../../hooks/useAuth';
import { LEAVE_TYPES } from '../../utils/constants';

const initialFormState = {
  type: '',
  startDate: null,
  endDate: null,
  reason: '',
};

const LeaveForm = ({ open, onClose, leave }) => {
  const [formData, setFormData] = useState(initialFormState);
  const { addNotification } = useNotification();
  const { user } = useAuth();

  useEffect(() => {
    if (leave) {
      setFormData({
        ...leave,
        startDate: leave.startDate ? new Date(leave.startDate) : null,
        endDate: leave.endDate ? new Date(leave.endDate) : null,
      });
    } else {
      setFormData(initialFormState);
    }
  }, [leave]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handleDateChange = (name) => (date) => {
    setFormData((prevData) => ({ ...prevData, [name]: date }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const leaveData = {
        ...formData,
        employeeId: user.id,
        status: 'pending',
      };

      if (leave) {
        await updateLeave(leave.id, leaveData);
        addNotification({ type: 'success', message: 'Leave request updated successfully' });
      } else {
        await createLeave(leaveData);
        addNotification({ type: 'success', message: 'Leave request created successfully' });
      }
      onClose();
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to save leave request' });
    }
  };

  return (
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <Dialog open={open} onClose={onClose}>
        <DialogTitle>{leave ? 'Edit Leave Request' : 'New Leave Request'}</DialogTitle>
        <form onSubmit={handleSubmit}>
          <DialogContent>
            <FormControl fullWidth margin="dense">
              <InputLabel>Leave Type</InputLabel>
              <Select
                name="type"
                value={formData.type}
                onChange={handleChange}
                required
              >
                {Object.entries(LEAVE_TYPES).map(([key, value]) => (
                  <MenuItem key={key} value={key}>{value}</MenuItem>
                ))}
              </Select>
            </FormControl>
            <DatePicker
              label="Start Date"
              value={formData.startDate}
              onChange={handleDateChange('startDate')}
              renderInput={(params) => <TextField {...params} fullWidth margin="dense" required />}
            />
            <DatePicker
              label="End Date"
              value={formData.endDate}
              onChange={handleDateChange('endDate')}
              renderInput={(params) => <TextField {...params} fullWidth margin="dense" required />}
            />
            <TextField
              margin="dense"
              name="reason"
              label="Reason"
              type="text"
              fullWidth
              multiline
              rows={4}
              value={formData.reason}
              onChange={handleChange}
              required
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={onClose}>Cancel</Button>
            <Button type="submit" color="primary">
              {leave ? 'Update' : 'Submit'}
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </LocalizationProvider>
  );
};

LeaveForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  leave: PropTypes.shape({
    id: PropTypes.string,
    type: PropTypes.string,
    startDate: PropTypes.oneOfType([PropTypes.string, PropTypes.instanceOf(Date)]),
    endDate: PropTypes.oneOfType([PropTypes.string, PropTypes.instanceOf(Date)]),
    reason: PropTypes.string,
  }),
};



export default LeaveForm;
