import React, { useState, useEffect, useCallback } from 'react';
import {
  Button,
  Chip,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from '@mui/material';
import { Delete as DeleteIcon, Edit as EditIcon } from '@mui/icons-material';
import { deleteShift, getShifts } from '../../services/api';
import useAuth from '../../hooks/useAuth';
import  useNotification  from '../../hooks/useNotification';
import { canManageShifts } from '../../utils/permissions';
import { fDate, fTime } from '../../utils/formatTime';
import { SHIFT_TYPES } from '../../utils/constants';
import ShiftForm from './ShiftForm';

const ShiftList = () => {
  const [shifts, setShifts] = useState([]);
  const [selectedShift, setSelectedShift] = useState(null);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const { user } = useAuth();
  const { addNotification } = useNotification();

  const fetchShifts = useCallback(async () => {
    try {
      const response = await getShifts();
      setShifts(response.data);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch shifts' });
    }
  }, [addNotification]);

  useEffect(() => {
    fetchShifts();
  }, [fetchShifts]);

  const handleEdit = (shift) => {
    setSelectedShift(shift);
    setIsFormOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this shift?')) {
      try {
        await deleteShift(id);
        addNotification({ type: 'success', message: 'Shift deleted successfully' });
        fetchShifts();
      } catch (error) {
        addNotification({ type: 'error', message: 'Failed to delete shift' });
      }
    }
  };

  const handleFormClose = () => {
    setSelectedShift(null);
    setIsFormOpen(false);
    fetchShifts();
  };

  const getShiftTypeColor = (type) => {
    switch (type) {
      case 'morning':
        return 'primary';
      case 'afternoon':
        return 'secondary';
      case 'night':
        return 'error';
      default:
        return 'default';
    }
  };

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Shifts
      </Typography>
      {canManageShifts(user.role) && (
        <Button variant="contained" color="primary" onClick={() => setIsFormOpen(true)} sx={{ mb: 2 }}>
          New Shift
        </Button>
      )}
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Date</TableCell>
              <TableCell>Employee</TableCell>
              <TableCell>Shift Type</TableCell>
              <TableCell>Start Time</TableCell>
              <TableCell>End Time</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {shifts.map((shift) => (
              <TableRow key={shift.id}>
                <TableCell>{fDate(shift.date)}</TableCell>
                <TableCell>{shift.employeeName}</TableCell>
                <TableCell>
                  <Chip 
                    label={SHIFT_TYPES[shift.type]} 
                    color={getShiftTypeColor(shift.type)}
                  />
                </TableCell>
                <TableCell>{fTime(shift.startTime)}</TableCell>
                <TableCell>{fTime(shift.endTime)}</TableCell>
                <TableCell>
                  {canManageShifts(user.role) && (
                    <>
                      <IconButton onClick={() => handleEdit(shift)}>
                        <EditIcon />
                      </IconButton>
                      <IconButton onClick={() => handleDelete(shift.id)}>
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
      <ShiftForm
        open={isFormOpen}
        onClose={handleFormClose}
        shift={selectedShift}
      />
    </div>
  );
};

export default ShiftList;