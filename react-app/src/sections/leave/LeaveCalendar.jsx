import React, { useState, useEffect, useCallback } from 'react';
import { Calendar, momentLocalizer } from 'react-big-calendar';
import moment from 'moment';
import { Paper, Typography } from '@mui/material';
import { getLeaves } from '../../services/api';
import useNotification from '../../hooks/useNotification';
import 'react-big-calendar/lib/css/react-big-calendar.css';

moment.locale('en-GB');
const localizer = momentLocalizer(moment);

const LeaveCalendar = () => {
  const [events, setEvents] = useState([]);
  const { addNotification } = useNotification();

  const fetchLeaves = useCallback(async () => {
    try {
      const response = await getLeaves();
      const formattedEvents = response.data.map((leave) => ({
        id: leave.id,
        title: `${leave.employeeName} - ${leave.type}`,
        start: new Date(leave.startDate),
        end: new Date(leave.endDate),
        allDay: true,
        resource: leave,
      }));
      setEvents(formattedEvents);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch leaves' });
    }
  }, [addNotification]);

  useEffect(() => {
    fetchLeaves();
  }, [fetchLeaves]);

  const eventStyleGetter = (event) => {
    let backgroundColor = '#3174ad';
    if (event.resource.status === 'approved') {
      backgroundColor = '#5cb85c';
    } else if (event.resource.status === 'rejected') {
      backgroundColor = '#d9534f';
    }

    return {
      style: {
        backgroundColor,
        borderRadius: '0px',
        opacity: 0.8,
        color: 'white',
        border: '0px',
        display: 'block',
      },
    };
  };

  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="h4" gutterBottom>
        Leave Calendar
      </Typography>
      <div style={{ height: 600 }}>
        <Calendar
          localizer={localizer}
          events={events}
          startAccessor="start"
          endAccessor="end"
          style={{ height: '100%' }}
          eventPropGetter={eventStyleGetter}
        />
      </div>
    </Paper>
  );
};

export default LeaveCalendar;
