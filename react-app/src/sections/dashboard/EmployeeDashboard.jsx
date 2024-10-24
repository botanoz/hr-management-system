import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import {
  Container,
  Grid,
  Typography,
  Card,
  CardContent,
  Box,
  CircularProgress,
  Avatar,
} from '@mui/material';
import {
  EventNote as EventNoteIcon,
  Receipt as ReceiptIcon,
  WatchLater as WatchLaterIcon,
  Notifications as NotificationsIcon,
  Event as EventIcon,
  BeachAccess as BeachAccessIcon,
  ErrorOutline as ErrorOutlineIcon,
  InfoOutlined as InfoOutlinedIcon,
} from '@mui/icons-material';
import { useTheme } from '@mui/material/styles';
import { fShortenNumber } from '../../utils/formatNumber';
import { fDate, fDateTime } from '../../utils/formatTime';
import useAuth from '../../hooks/useAuth';
import { getDashboardData } from '../../services/api';
import Scrollbar from '../../components/scrollbar';

const getColorStyles = (color) => {
  switch (color) {
    case 'warning':
      return { backgroundColor: '#FFECB3', avatarBgColor: 'orange' };
    case 'error':
      return { backgroundColor: '#FFCDD2', avatarBgColor: 'red' };
    case 'info':
      return { backgroundColor: '#BBDEFB', avatarBgColor: 'blue' };
    case 'success':
    default:
      return { backgroundColor: '#C8E6C9', avatarBgColor: 'green' };
  }
};

const AppWidgetSummary = ({ title, total, icon, color = 'primary' }) => {
  const { backgroundColor, avatarBgColor } = getColorStyles(color);

  return (
    <Card sx={{ height: '100%', backgroundColor }}>
      <CardContent>
        <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
          <Avatar sx={{ bgcolor: avatarBgColor, mr: 2 }}>
            {icon}
          </Avatar>
          <Box>
            <Typography variant="h3" sx={{ color: '#333', fontWeight: 'bold' }}>{fShortenNumber(total)}</Typography>
            <Typography variant="subtitle1" sx={{ color: 'text.secondary' }}>
              {title}
            </Typography>
          </Box>
        </Box>
      </CardContent>
    </Card>
  );
};

AppWidgetSummary.propTypes = {
  title: PropTypes.string.isRequired,
  total: PropTypes.number.isRequired,
  icon: PropTypes.node.isRequired,
  color: PropTypes.string,
};

const EmployeeDashboard = () => {
  const [dashboardData, setDashboardData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [fetchError, setFetchError] = useState(null);
  const theme = useTheme();
  const { user } = useAuth();

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        setLoading(true);
        setFetchError(null);

        const { data } = await getDashboardData();
        setDashboardData(data);
      } catch (err) {
        console.error('Error fetching dashboard data:', err);
        setFetchError('Failed to load dashboard data.');
      } finally {
        setLoading(false);
      }
    };

    fetchDashboardData();
  }, []);

  if (loading) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="100vh"
      >
        <CircularProgress />
      </Box>
    );
  }

  if (fetchError) {
    return <Typography color="error">{fetchError}</Typography>;
  }

  if (!dashboardData) {
    return <Typography>No data available</Typography>;
  }

  return (
    <Container maxWidth="xl">
      <Typography variant="h3" sx={{ mb: 5, color: theme.palette.primary.main, fontWeight: 'bold' }}>
        Hoş Geldiniz, {user.name}
      </Typography>

      <Grid container spacing={4}>
        <Grid item xs={12} sm={6} md={3}>
          <AppWidgetSummary
            title="Bekleyen İzin Talepleri"
            total={dashboardData.pendingLeaveRequests || 0}
            icon={<EventNoteIcon />}
            color="warning"
          />
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <AppWidgetSummary
            title="Bekleyen Gider Talepleri"
            total={dashboardData.pendingExpenseRequests || 0}
            icon={<ReceiptIcon />}
            color="error"
          />
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <AppWidgetSummary
            title="Kalan İzin Günleri | 20 Gün"
            total={dashboardData.remainingLeaveDays || 0}
            icon={<InfoOutlinedIcon />}
            color="info"
          />
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <AppWidgetSummary
            title="Yaklaşan Vardiyalar"
            total={dashboardData.upcomingShifts?.length || 0}
            icon={<WatchLaterIcon />}
            color="success"
          />
        </Grid>

        {/* Recent Notifications */}
        <Grid item xs={12} md={6} lg={4}>
          <Card sx={{ height: '100%' }}>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Son Bildirimler
              </Typography>
              <Scrollbar sx={{ height: 300 }}>
                {dashboardData.recentNotifications?.length > 0 ? (
                  dashboardData.recentNotifications.map((notification) => (
                    <Box
                      key={notification.id}
                      sx={{
                        display: 'flex',
                        alignItems: 'center',
                        mb: 2,
                        bgcolor: '#f0f0f0',
                        p: 2,
                        borderRadius: 1,
                      }}
                    >
                      <NotificationsIcon sx={{ mr: 2, color: theme.palette.info.main }} />
                      <Box>
                        <Typography variant="subtitle2">
                          {notification.message}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                          {fDateTime(notification.dateSent)}
                        </Typography>
                      </Box>
                    </Box>
                  ))
                ) : (
                  <Box display="flex" alignItems="center">
                    <ErrorOutlineIcon sx={{ mr: 1, color: theme.palette.warning.main }} />
                    <Typography variant="body2">No recent notifications.</Typography>
                  </Box>
                )}
              </Scrollbar>
            </CardContent>
          </Card>
        </Grid>

        {/* Upcoming Events */}
        <Grid item xs={12} md={6} lg={4}>
          <Card sx={{ height: '100%' }}>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Yaklaşan Etkinlikler
              </Typography>
              <Scrollbar sx={{ height: 300 }}>
                {dashboardData.upcomingEvents?.length > 0 ? (
                  dashboardData.upcomingEvents.map((event) => (
                    <Box
                      key={event.id}
                      sx={{
                        display: 'flex',
                        alignItems: 'center',
                        mb: 2,
                        bgcolor: '#f0f0f0',
                        p: 2,
                        borderRadius: 1,
                      }}
                    >
                      <EventIcon sx={{ mr: 2, color: theme.palette.success.main }} />
                      <Box>
                        <Typography variant="subtitle2">{event.title}</Typography>
                        <Typography variant="body2" color="text.secondary">
                          {fDate(event.eventDate)}
                        </Typography>
                      </Box>
                    </Box>
                  ))
                ) : (
                  <Box display="flex" alignItems="center">
                    <ErrorOutlineIcon sx={{ mr: 1, color: theme.palette.warning.main }} />
                    <Typography variant="body2">No upcoming events.</Typography>
                  </Box>
                )}
              </Scrollbar>
            </CardContent>
          </Card>
        </Grid>

        {/* Upcoming Holidays */}
        <Grid item xs={12} md={6} lg={4}>
          <Card sx={{ height: '100%' }}>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Yaklaşan Tatil Günleri
              </Typography>
              <Scrollbar sx={{ height: 300 }}>
                {dashboardData.upcomingHolidays?.length > 0 ? (
                  dashboardData.upcomingHolidays.map((holiday) => (
                    <Box
                      key={holiday.id}
                      sx={{
                        display: 'flex',
                        alignItems: 'center',
                        mb: 2,
                        bgcolor: '#f0f0f0',
                        p: 2,
                        borderRadius: 1,
                      }}
                    >
                      <BeachAccessIcon sx={{ mr: 2, color: theme.palette.primary.main }} />
                      <Box>
                        <Typography variant="subtitle2">{holiday.name}</Typography>
                        <Typography variant="body2" color="text.secondary">
                          {fDate(holiday.date)}
                        </Typography>
                      </Box>
                    </Box>
                  ))
                ) : (
                  <Box display="flex" alignItems="center">
                    <ErrorOutlineIcon sx={{ mr: 1, color: theme.palette.warning.main }} />
                    <Typography variant="body2">No upcoming holidays.</Typography>
                  </Box>
                )}
              </Scrollbar>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Container>
  );
};

export default EmployeeDashboard;
