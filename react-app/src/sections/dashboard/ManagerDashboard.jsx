import React, { useState, useEffect, useCallback } from 'react';
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
  LinearProgress,
  Paper,
} from '@mui/material';
import {
  People,
  DateRange,
  AccountBalanceWallet,
  Schedule,
  Celebration,
  Announcement,
  Today,
  Warning,
} from '@mui/icons-material';
import { useTheme } from '@mui/material/styles';
import { useNavigate } from 'react-router-dom';
import { fShortenNumber } from '../../utils/formatNumber';
import { fDate, fDateTime } from '../../utils/formatTime';
import useAuth from '../../hooks/useAuth';
import { getDashboardData } from '../../services/api';
import Scrollbar from '../../components/scrollbar';

const StatCard = ({ title, value, icon, color, progress }) => {
  const theme = useTheme();
  return (
    <Paper sx={{ height: '100%', bgcolor: theme.palette.background.paper, borderRadius: '12px', boxShadow: 3, p: 2 }}>
      <CardContent>
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', mb: 2 }}>
          <Avatar sx={{ bgcolor: color, mb: 1 }}>{icon}</Avatar>
          <Typography variant="h6" component="div" sx={{ color: theme.palette.text.primary, fontWeight: 'bold', textAlign: 'center' }}>
            {title}
          </Typography>
        </Box>
        <Typography variant="h4" component="div" sx={{ mb: 2, color: theme.palette.primary.dark, textAlign: 'center' }}>
          {fShortenNumber(value)}
        </Typography>
        <LinearProgress variant="determinate" value={progress} sx={{ height: 8, borderRadius: 4, bgcolor: color }} />
      </CardContent>
    </Paper>
  );
};

StatCard.propTypes = {
  title: PropTypes.string.isRequired,
  value: PropTypes.number.isRequired,
  icon: PropTypes.node.isRequired,
  color: PropTypes.string.isRequired,
  progress: PropTypes.number.isRequired,
};

const ManagerDashboard = () => {
  const [dashboardData, setDashboardData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [fetchError, setFetchError] = useState(null);
  const { user } = useAuth();
  const theme = useTheme();
  const navigate = useNavigate();

  const fetchDashboardData = useCallback(async () => {
    try {
      setLoading(true);
      setFetchError(null);
      const response = await getDashboardData();
      setDashboardData(response.data);
    } catch (err) {
      console.error('Error fetching dashboard data:', err);
      setFetchError('Gösterge tablosu yüklenemedi.');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchDashboardData();
  }, [fetchDashboardData]);

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="100vh" bgcolor={theme.palette.grey[100]}>
        <CircularProgress color="secondary" />
      </Box>
    );
  }

  if (fetchError) {
    return <Typography color="error" align="center" sx={{ mt: 4 }}>{fetchError}</Typography>;
  }

  if (!dashboardData) {
    return <Typography align="center" variant="h6" sx={{ mt: 4 }}>Veri bulunamadı</Typography>;
  }

  return (
    <Container maxWidth="lg" sx={{ py: 4, bgcolor: theme.palette.background.default }}>
      <Typography variant="h3" sx={{ mb: 4, color: theme.palette.secondary.main, textAlign: 'center', fontWeight: 'bold' }}>
        Hoş Geldiniz {user.name}
      </Typography>

      <Grid container spacing={4}>
        <Grid item xs={12} sm={6} md={3}>
          <StatCard
            title="Çalışan Sayısı"
            value={dashboardData.totalEmployees || 0}
            icon={<People />}
            color={theme.palette.success.main}
            progress={75}
          />
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <StatCard
            title="Bekleyen İzinler"
            value={dashboardData.pendingLeaveRequests || 0}
            icon={<DateRange />}
            color={theme.palette.info.main}
            progress={50}
          />
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <StatCard
            title="Bekleyen Harcamalar"
            value={dashboardData.pendingExpenseRequests || 0}
            icon={<AccountBalanceWallet />}
            color={theme.palette.warning.main}
            progress={30}
          />
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <StatCard
            title="Aktif Vardiyalar"
            value={dashboardData.upcomingShifts?.length || 0}
            icon={<Schedule />}
            color={theme.palette.error.main}
            progress={90}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <Card sx={{ borderRadius: '16px', boxShadow: 4, height: '100%' }}>
            <CardContent>
              <Typography variant="h5" gutterBottom align="center">Yaklaşan Doğum Günleri</Typography>
              <Scrollbar style={{ maxHeight: 250 }}>
                {dashboardData.upcomingBirthdays?.length > 0 ? (
                  dashboardData.upcomingBirthdays.map((employee) => (
                    <Box key={employee.id} sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                      <Avatar sx={{ bgcolor: theme.palette.secondary.main, mr: 2 }}><Celebration /></Avatar>
                      <Box sx={{ flexGrow: 1 }}>
                        <Typography variant="subtitle2">{employee.fullName}</Typography>
                        <Typography variant="body2" color="text.secondary">{fDate(employee.birthdate)}</Typography>
                      </Box>
                    </Box>
                  ))
                ) : (
                  <Box display="flex" justifyContent="center" alignItems="center">
                    <Warning sx={{ mr: 1, color: theme.palette.warning.main }} />
                    <Typography variant="body2">Yaklaşan doğum günü yok</Typography>
                  </Box>
                )}
              </Scrollbar>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={4}>
          <Card sx={{ borderRadius: '16px', boxShadow: 4, height: '100%' }}>
            <CardContent>
              <Typography variant="h5" gutterBottom align="center">Son Bildirimler</Typography>
              <Scrollbar style={{ maxHeight: 250 }}>
                {dashboardData.recentNotifications?.length > 0 ? (
                  dashboardData.recentNotifications.map((notification) => (
                    <Box key={notification.id} sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                      <Avatar sx={{ bgcolor: theme.palette.info.main, mr: 2 }}><Announcement /></Avatar>
                      <Box sx={{ flexGrow: 1 }}>
                        <Typography variant="subtitle2">{notification.message}</Typography>
                        <Typography variant="body2" color="text.secondary">{fDateTime(notification.dateSent)}</Typography>
                      </Box>
                    </Box>
                  ))
                ) : (
                  <Box display="flex" justifyContent="center" alignItems="center">
                    <Warning sx={{ mr: 1, color: theme.palette.warning.main }} />
                    <Typography variant="body2">Son bildirim yok</Typography>
                  </Box>
                )}
              </Scrollbar>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={4}>
          <Card sx={{ borderRadius: '16px', boxShadow: 4, height: '100%' }}>
            <CardContent>
              <Typography variant="h5" gutterBottom align="center">Yaklaşan Etkinlikler</Typography>
              <Scrollbar style={{ maxHeight: 250 }}>
                {dashboardData.upcomingEvents?.length > 0 ? (
                  dashboardData.upcomingEvents.map((event) => (
                    <Box key={event.id} sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                      <Avatar sx={{ bgcolor: theme.palette.success.main, mr: 2 }}><Today /></Avatar>
                      <Box sx={{ flexGrow: 1 }}>
                        <Typography variant="subtitle2">{event.title}</Typography>
                        <Typography variant="body2" color="text.secondary">{fDate(event.eventDate)}</Typography>
                      </Box>
                    </Box>
                  ))
                ) : (
                  <Box display="flex" justifyContent="center" alignItems="center">
                    <Warning sx={{ mr: 1, color: theme.palette.warning.main }} />
                    <Typography variant="body2">Yaklaşan etkinlik yok</Typography>
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

export default ManagerDashboard;
