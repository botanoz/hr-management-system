import { lazy, Suspense } from 'react';
import { Outlet, Navigate, useRoutes } from 'react-router-dom';

import DashboardLayout from '../layouts/dashboard';
import AuthGuard from '../guards/AuthGuard';

const Loadable = (Component) => (props) => (
  <Suspense fallback={<div>Loading...</div>}>
    <Component {...props} />
  </Suspense>
);

// Dashboard
const DashboardPage = Loadable(lazy(() => import('../pages/DashboardPage')));
const EmployeesPage = Loadable(lazy(() => import('../pages/EmployeesPage')));
const LeavesPage = Loadable(lazy(() => import('../pages/LeavesPage')));
const ExpensesPage = Loadable(lazy(() => import('../pages/ExpensesPage')));
const ShiftsPage = Loadable(lazy(() => import('../pages/ShiftsPage')));
const CompaniesPage = Loadable(lazy(() => import('../pages/CompaniesPage')));
const ProfilePage = Loadable(lazy(() => import('../pages/ProfilePage')));
const SettingsPage = Loadable(lazy(() => import('../pages/SettingsPage')));
const HolidaysPage = Loadable(lazy(() => import('../pages/HolidaysPage')));

// Auth
const LoginPage = Loadable(lazy(() => import('../pages/login')));

// Error
const Page404 = Loadable(lazy(() => import('../pages/page-not-found')));

export default function Router() {
  return useRoutes([
    {
      path: '/',
      element: (
        <AuthGuard>
          <DashboardLayout>
            <Outlet />
          </DashboardLayout>
        </AuthGuard>
      ),
      children: [
        { element: <Navigate to="/dashboard" replace />, index: true },
        { path: 'dashboard', element: <DashboardPage /> },
        { path: 'employees', element: <EmployeesPage /> },
        { path: 'leaves', element: <LeavesPage /> },
        { path: 'expenses', element: <ExpensesPage /> },
        { path: 'shifts', element: <ShiftsPage /> },
        { path: 'companies', element: <CompaniesPage /> },
        { path: 'profile', element: <ProfilePage /> },
        { path: 'settings', element: <SettingsPage /> },
        { path: 'holidays', element: <HolidaysPage /> },
      ],
    },
    {
      path: 'login',
      element: <LoginPage />,
    },
    {
      path: '404',
      element: <Page404 />,
    },
    {
      path: '*',
      element: <Navigate to="/404" replace />,
    },
  ]);
}