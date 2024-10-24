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
  Chip,
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { getLeaves, deleteLeave } from '../../services/api';
import useAuth from '../../hooks/useAuth';
import useNotification from '../../hooks/useNotification';
import { canManageLeaves } from '../../utils/permissions';
import { fDate } from '../../utils/formatTime';
import LeaveForm from './LeaveForm';

const LeaveList = () => {
  const [leaves, setLeaves] = useState([]);
  const [selectedLeave, setSelectedLeave] = useState(null);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const { user } = useAuth();
  const { addNotification } = useNotification();

  const fetchLeaves = useCallback(async () => {
    try {
      const response = await getLeaves();
      setLeaves(response.data);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch leaves' });
    }
  }, [addNotification]);

  useEffect(() => {
    fetchLeaves();
  }, [fetchLeaves]);

  const handleEdit = (leave) => {
    setSelectedLeave(leave);
    setIsFormOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this leave request?')) {
      try {
        await deleteLeave(id);
        addNotification({ type: 'success', message: 'Leave request deleted successfully' });
        fetchLeaves();
      } catch (error) {
        addNotification({ type: 'error', message: 'Failed to delete leave request' });
      }
    }
  };

  const handleFormClose = () => {
    setSelectedLeave(null);
    setIsFormOpen(false);
    fetchLeaves();
  };

  const getStatusColor = (status) => {
    switch (status) {
      case 'approved':
        return 'success';
      case 'rejected':
        return 'error';
      case 'pending':
        return 'warning';
      default:
        return 'default';
    }
  };

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Leave Requests
      </Typography>
      <Button variant="contained" color="primary" onClick={() => setIsFormOpen(true)} sx={{ mb: 2 }}>
        New Leave Request
      </Button>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Employee</TableCell>
              <TableCell>Type</TableCell>
              <TableCell>Start Date</TableCell>
              <TableCell>End Date</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {leaves.map((leave) => (
              <TableRow key={leave.id}>
                <TableCell>{leave.employeeName}</TableCell>
                <TableCell>{leave.type}</TableCell>
                <TableCell>{fDate(leave.startDate)}</TableCell>
                <TableCell>{fDate(leave.endDate)}</TableCell>
                <TableCell>
                  <Chip label={leave.status} color={getStatusColor(leave.status)} />
                </TableCell>
                <TableCell>
                  {(canManageLeaves(user.role) || user.id === leave.employeeId) && (
                    <>
                      <IconButton onClick={() => handleEdit(leave)}>
                        <EditIcon />
                      </IconButton>
                      <IconButton onClick={() => handleDelete(leave.id)}>
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
      <LeaveForm
        open={isFormOpen}
        onClose={handleFormClose}
        leave={selectedLeave}
      />
    </div>
  );
};

export default LeaveList;
