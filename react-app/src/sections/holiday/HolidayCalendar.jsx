import React, { useState, useEffect, useCallback } from 'react';
import { Calendar, momentLocalizer } from 'react-big-calendar';
import moment from 'moment';
import { Paper, Typography } from '@mui/material';
import { getHolidays } from '../../services/api';
import useNotification from '../../hooks/useNotification';
import 'react-big-calendar/lib/css/react-big-calendar.css';

moment.locale('en-GB');
const localizer = momentLocalizer(moment);

const HolidayCalendar = () => {
  const [events, setEvents] = useState([]);
  const { addNotification } = useNotification();

  const fetchHolidays = useCallback(async () => {
    try {
      const response = await getHolidays();
      const formattedEvents = response.data.map(holiday => ({
        id: holiday.id,
        title: holiday.name,
        start: new Date(holiday.date),
        end: new Date(holiday.date),
        allDay: true,
        resource: holiday,
      }));
      setEvents(formattedEvents);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch holidays' });
    }
  }, [addNotification]);

  useEffect(() => {
    fetchHolidays();
  }, [fetchHolidays]);

  const eventStyleGetter = (event) => {
    let backgroundColor = '#3174ad';
    switch (event.resource.type) {
      case 'public':
        backgroundColor = '#4caf50';
        break;
      case 'company':
        backgroundColor = '#ff9800';
        break;
      case 'optional':
        backgroundColor = '#2196f3';
        break;
      default:
        break;
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
        Holiday Calendar
      </Typography>
      <div style={{ height: 600 }}>
        <Calendar
          localizer={localizer}
          events={events}
          startAccessor="start"
          endAccessor="end"
          style={{ height: '100%' }}
          eventPropGetter={eventStyleGetter}
          views={['month', 'agenda']}
        />
      </div>
    </Paper>
  );
};

export default HolidayCalendar;
