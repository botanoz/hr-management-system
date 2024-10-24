import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import { useParams } from 'react-router-dom';
import {
  Card,
  CardContent,
  Typography,
  Grid,
  Avatar,
  Chip,
} from '@mui/material';
import { getEmployee } from '../../services/api';
import useNotification from '../../hooks/useNotification';
import { fDate } from '../../utils/formatTime';

const EmployeeDetails = () => {
  const { id } = useParams();
  const [employee, setEmployee] = useState(null);
  const { addNotification } = useNotification();

  const fetchEmployeeDetails = useCallback(async () => {
    try {
      const response = await getEmployee(id);
      setEmployee(response.data);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch employee details' });
    }
  }, [id, addNotification]);

  useEffect(() => {
    fetchEmployeeDetails();
  }, [fetchEmployeeDetails]);

  if (!employee) {
    return <Typography>Loading...</Typography>;
  }

  return (
    <Card>
      <CardContent>
        <Grid container spacing={3}>
          <Grid item xs={12} sm={4} md={3}>
            <Avatar
              alt={employee.name}
              src={employee.avatar}
              sx={{ width: 150, height: 150, margin: 'auto' }}
            />
          </Grid>
          <Grid item xs={12} sm={8} md={9}>
            <Typography variant="h4" gutterBottom>
              {employee.name}
            </Typography>
            <Typography variant="subtitle1" color="textSecondary" gutterBottom>
              {employee.position}
            </Typography>
            <Chip label={employee.department} color="primary" sx={{ mt: 1, mb: 2 }} />
            <Typography variant="body1" paragraph>
              <strong>Email:</strong> {employee.email}
            </Typography>
            <Typography variant="body1" paragraph>
              <strong>Phone:</strong> {employee.phone}
            </Typography>
            <Typography variant="body1" paragraph>
              <strong>Hire Date:</strong> {fDate(employee.hireDate)}
            </Typography>
            <Typography variant="body1" paragraph>
              <strong>Address:</strong> {employee.address}
            </Typography>
            {employee.bio && (
              <Typography variant="body1" paragraph>
                <strong>Bio:</strong> {employee.bio}
              </Typography>
            )}
          </Grid>
        </Grid>
      </CardContent>
    </Card>
  );
};

export default EmployeeDetails;

// EmployeeForm component
const EmployeeForm = ({ open, onClose, employee }) => {
  // ... form implementation
};

EmployeeForm.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  employee: PropTypes.shape({
    id: PropTypes.string,
    name: PropTypes.string,
    position: PropTypes.string,
    department: PropTypes.string,
    email: PropTypes.string,
    phone: PropTypes.string,
    hireDate: PropTypes.string,
    address: PropTypes.string,
    bio: PropTypes.string,
    avatar: PropTypes.string,
  }),
};

