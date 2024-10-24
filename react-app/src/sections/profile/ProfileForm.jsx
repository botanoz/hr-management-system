import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Grid,
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { updateProfile } from '../../services/api';
import useNotification from '../../hooks/useNotification';

const initialFormState = {
  name: '',
  email: '',
  phone: '',
  dateOfBirth: null,
  address: '',
  bio: '',
};

const ProfileForm = ({ open, onClose, profile }) => {
  const [formData, setFormData] = useState(initialFormState);
  const { addNotification } = useNotification();

  useEffect(() => {
    if (profile) {
      setFormData({
        ...profile,
        dateOfBirth: profile.dateOfBirth ? new Date(profile.dateOfBirth) : null,
      });
    } else {
      setFormData(initialFormState);
    }
  }, [profile]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handleDateChange = (date) => {
    setFormData((prevData) => ({ ...prevData, dateOfBirth: date }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await updateProfile(formData);
      addNotification({ type: 'success', message: 'Profile updated successfully' });
      onClose();
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to update profile' });
    }
  };

  return (
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <Dialog open={open} onClose={onClose}>
        <DialogTitle>Edit Profile</DialogTitle>
        <form onSubmit={handleSubmit}>
          <DialogContent>
            <Grid container spacing={2}>
              <Grid item xs={12}>
                <TextField
                  autoFocus
                  margin="dense"
                  name="name"
                  label="Full Name"
                  type="text"
                  fullWidth
                  value={formData.name}
                  onChange={handleChange}
                  required
                />
              </Grid>
              <Grid item xs={12} sm={6}>
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
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  margin="dense"
                  name="phone"
                  label="Phone"
                  type="tel"
                  fullWidth
                  value={formData.phone}
                  onChange={handleChange}
                />
              </Grid>
              <Grid item xs={12}>
                <DatePicker
                  label="Date of Birth"
                  value={formData.dateOfBirth}
                  onChange={handleDateChange}
                  renderInput={(params) => <TextField {...params} fullWidth margin="dense" />}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  margin="dense"
                  name="address"
                  label="Address"
                  type="text"
                  fullWidth
                  value={formData.address}
                  onChange={handleChange}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  margin="dense"
                  name="bio"
                  label="Bio"
                  type="text"
                  fullWidth
                  multiline
                  rows={4}
                  value={formData.bio}
                  onChange={handleChange}
                />
              </Grid>
            </Grid>
          </DialogContent>
          <DialogActions>
            <Button onClick={onClose}>Cancel</Button>
            <Button type="submit" color="primary">
              Update
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </LocalizationProvider>
  );
};

ProfileForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  profile: PropTypes.shape({
    name: PropTypes.string,
    email: PropTypes.string,
    phone: PropTypes.string,
    dateOfBirth: PropTypes.oneOfType([PropTypes.string, PropTypes.instanceOf(Date)]),
    address: PropTypes.string,
    bio: PropTypes.string,
  }),
};

ProfileForm.defaultProps = {
  profile: null,
};

export default ProfileForm;