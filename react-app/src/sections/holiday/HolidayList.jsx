import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
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
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { getHolidays, deleteHoliday } from '../../services/api';
import useAuth from '../../hooks/useAuth';
import useNotification from '../../hooks/useNotification';
import { canManageHolidays } from '../../utils/permissions';
import { fDate } from '../../utils/formatTime';
import HolidayForm from './HolidayForm';

const HolidayList = () => {
  const [holidays, setHolidays] = useState([]);
  const [selectedHoliday, setSelectedHoliday] = useState(null);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const { user } = useAuth();
  const { addNotification } = useNotification();

  const fetchHolidays = useCallback(async () => {
    try {
      const response = await getHolidays();
      setHolidays(response.data);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch holidays' });
    }
  }, [addNotification]);

  useEffect(() => {
    fetchHolidays();
  }, [fetchHolidays]);

  const handleEdit = (holiday) => {
    setSelectedHoliday(holiday);
    setIsFormOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this holiday?')) {
      try {
        await deleteHoliday(id);
        addNotification({ type: 'success', message: 'Holiday deleted successfully' });
        fetchHolidays();
      } catch (error) {
        addNotification({ type: 'error', message: 'Failed to delete holiday' });
      }
    }
  };

  const handleFormClose = () => {
    setSelectedHoliday(null);
    setIsFormOpen(false);
    fetchHolidays();
  };

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Holidays
      </Typography>
      {canManageHolidays(user.role) && (
        <Button variant="contained" color="primary" onClick={() => setIsFormOpen(true)} sx={{ mb: 2 }}>
          Add New Holiday
        </Button>
      )}
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Date</TableCell>
              <TableCell>Type</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {holidays.map((holiday) => (
              <TableRow key={holiday.id}>
                <TableCell>{holiday.name}</TableCell>
                <TableCell>{fDate(holiday.date)}</TableCell>
                <TableCell>{holiday.type}</TableCell>
                <TableCell>
                  {canManageHolidays(user.role) && (
                    <>
                      <IconButton onClick={() => handleEdit(holiday)}>
                        <EditIcon />
                      </IconButton>
                      <IconButton onClick={() => handleDelete(holiday.id)}>
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
      <HolidayForm
        open={isFormOpen}
        onClose={handleFormClose}
        holiday={selectedHoliday}
      />
    </div>
  );
};

HolidayList.propTypes = {
  // Eğer `HolidayList` bileşeni için props tanımlamaları yapacaksanız buraya ekleyebilirsiniz.
};

export default HolidayList;
