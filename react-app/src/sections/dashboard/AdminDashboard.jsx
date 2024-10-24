import React, { useState, useEffect, useMemo } from 'react';
import PropTypes from 'prop-types';
import { Container, Grid, Typography, Card, CardContent, Box } from '@mui/material';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, PieChart, Pie, Cell, ResponsiveContainer } from 'recharts';
import { BusinessCenter, Assignment, People, Subscriptions } from '@mui/icons-material';
import { getDashboardData } from '../../services/api';

const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#8884D8'];

const DashboardCard = ({ title, value, icon, color }) => (
  <Card elevation={3} sx={{ height: '100%' }}>
    <CardContent sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
      <Box>
        <Typography variant="h4" component="div">
          {value}
        </Typography>
        <Typography variant="subtitle1" color="text.secondary">
          {title}
        </Typography>
      </Box>
      <Box sx={{ color, fontSize: 40 }}>{icon}</Box>
    </CardContent>
  </Card>
);

DashboardCard.propTypes = {
  title: PropTypes.string.isRequired,
  value: PropTypes.number.isRequired,
  icon: PropTypes.element.isRequired,
  color: PropTypes.string.isRequired,
};

const AdminDashboard = () => {
  const [dashboardData, setDashboardData] = useState({
    totalCompanies: 0,
    pendingCompanyApprovals: 0,
    totalUsers: 0,
    activeSubscriptions: 0,
    recentlyRegisteredCompanies: [],
    upcomingSubscriptionExpirations: [],
    recentNotifications: [],
    upcomingEvents: [],
  });

  const [loading, setLoading] = useState(true);
  const [fetchError, setFetchError] = useState(null);

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        setLoading(true);
        setFetchError(null);
        const response = await getDashboardData();
        setDashboardData(response.data || {});
      } catch (error) {
        console.error('Error fetching dashboard data:', error);
        setFetchError('Failed to load dashboard data.');
      } finally {
        setLoading(false);
      }
    };

    fetchDashboardData();
  }, []);

  const recentCompaniesChart = useMemo(() => (
    dashboardData.recentlyRegisteredCompanies?.map(company => ({
      name: new Date(company.registrationDate).toLocaleDateString(),
      Employees: company.employeeCount,
    })) || []
  ), [dashboardData.recentlyRegisteredCompanies]);

  const subscriptionExpirationChart = useMemo(() => (
    dashboardData.upcomingSubscriptionExpirations?.map(company => ({
      name: company.companyName,
      value: company.employeeCount,
    })) || []
  ), [dashboardData.upcomingSubscriptionExpirations]);

  if (loading) {
    return <Typography variant="h4">Loading...</Typography>;
  }

  if (fetchError) {
    return <Typography variant="h4" color="error">{fetchError}</Typography>;
  }

  return (
    <Container maxWidth="xl">
      <Typography variant="h3" sx={{ mb: 5 }}>
        Admin Dashboard
      </Typography>

      <Grid container spacing={3}>
        <Grid item xs={12} sm={6} md={3}>
          <DashboardCard
            title="Total Companies"
            value={dashboardData.totalCompanies}
            icon={<BusinessCenter />}
            color="#0088FE"
          />
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <DashboardCard
            title="Pending Approvals"
            value={dashboardData.pendingCompanyApprovals}
            icon={<Assignment />}
            color="#00C49F"
          />
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <DashboardCard
            title="Total Users"
            value={dashboardData.totalUsers}
            icon={<People />}
            color="#FFBB28"
          />
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <DashboardCard
            title="Active Subscriptions"
            value={dashboardData.activeSubscriptions}
            icon={<Subscriptions />}
            color="#FF8042"
          />
        </Grid>

        <Grid item xs={12} md={8}>
          <Card elevation={3}>
            <CardContent>
              <Typography variant="h6" gutterBottom>Recently Registered Companies</Typography>
              <ResponsiveContainer width="100%" height={300}>
                <BarChart data={recentCompaniesChart}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="name" />
                  <YAxis />
                  <Tooltip />
                  <Legend />
                  <Bar dataKey="Employees" fill="#8884d8" />
                </BarChart>
              </ResponsiveContainer>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={4}>
          <Card elevation={3}>
            <CardContent>
              <Typography variant="h6" gutterBottom>Subscription Expiration</Typography>
              <ResponsiveContainer width="100%" height={300}>
                <PieChart>
                  <Pie
                    data={subscriptionExpirationChart}
                    cx="50%"
                    cy="50%"
                    labelLine={false}
                    outerRadius={80}
                    fill="#8884d8"
                    dataKey="value"
                  >
                    {subscriptionExpirationChart.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                    ))}
                  </Pie>
                  <Tooltip />
                  <Legend />
                </PieChart>
              </ResponsiveContainer>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={6}>
          <Card elevation={3}>
            <CardContent>
              <Typography variant="h6" gutterBottom>Recent Notifications</Typography>
              {dashboardData.recentNotifications?.map((notification) => (
                <Box key={notification.id} sx={{ mt: 2 }}>
                  <Typography variant="body1">{notification.message}</Typography>
                  <Typography variant="body2" color="text.secondary">
                    {new Date(notification.dateSent).toLocaleString()}
                  </Typography>
                </Box>
              ))}
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={6}>
          <Card elevation={3}>
            <CardContent>
              <Typography variant="h6" gutterBottom>Upcoming Events</Typography>
              {dashboardData.upcomingEvents?.map((event) => (
                <Box key={event.id} sx={{ mt: 2 }}>
                  <Typography variant="body1">{event.title}</Typography>
                  <Typography variant="body2" color="text.secondary">
                    {new Date(event.eventDate).toLocaleString()}
                  </Typography>
                </Box>
              ))}
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Container>
  );
};

export default AdminDashboard;