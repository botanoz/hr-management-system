import React from 'react';
import { Helmet } from 'react-helmet-async';
import { Container, Typography } from '@mui/material';
import HolidayList from '../sections/holiday/HolidayList';
import HolidayCalendar from '../sections/holiday/HolidayCalendar';

const HolidaysPage = () => (
  <>
    <Helmet>
      <title> Holidays | HRMS </title>
    </Helmet>

    <Container maxWidth="xl">
      <Typography variant="h4" sx={{ mb: 5 }}>
        Holidays
      </Typography>

      <HolidayList />
      
      <Typography variant="h5" sx={{ mt: 5, mb: 3 }}>
        Holiday Calendar
      </Typography>
      
      <HolidayCalendar />
    </Container>
  </>
);

export default HolidaysPage;
