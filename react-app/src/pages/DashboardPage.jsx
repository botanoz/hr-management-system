import React from 'react';
import { Container, Grid, Box } from '@mui/material';
import useAuth from '../hooks/useAuth';
import AdminDashboard from '../sections/dashboard/AdminDashboard';
import ManagerDashboard from '../sections/dashboard/ManagerDashboard';
import EmployeeDashboard from '../sections/dashboard/EmployeeDashboard';

const DashboardPage = () => {
  const { user } = useAuth();

  const renderDashboard = () => {
    switch (user.role) {
      case 'Admin':
        return <AdminDashboard />;
      case 'Manager':
        return <ManagerDashboard />;
      case 'Employee':
        return <EmployeeDashboard />;
      default:
        return <div>Unauthorized</div>;
    }
  };

  return (

      <Container
        maxWidth="xlg"
        sx={{
          backgroundColor: '#f9fbfc',
          borderRadius: 2,
          boxShadow: '0px 4px 4px rgba(0, 0, 0.1, 0.1)',
          padding: 5,
          minHeight: '100vh', // Ekranın tamamını kaplaması için minimum yükseklik
        }}
      >
        <Grid container spacing={3}>
          {renderDashboard()}
        </Grid>
      </Container>
    
  );
};

export default DashboardPage;
