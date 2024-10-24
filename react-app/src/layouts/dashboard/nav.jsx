import PropTypes from 'prop-types';
import { useEffect, useMemo, useState, useContext, useCallback } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { styled, alpha } from '@mui/material/styles';
import { 
  Box, Link, Drawer, Typography, Avatar, CircularProgress, Button,
  List, ListItem, ListItemIcon, ListItemText, Divider
} from '@mui/material';
import { 
  Work as WorkIcon, 
  Business as BusinessIcon, 
  Person as PersonIcon, 
  ExitToApp as LogoutIcon,
  Dashboard as DashboardIcon,
  People as PeopleIcon,
  EventNote as EventNoteIcon,
  AttachMoney as AttachMoneyIcon,
  Schedule as ScheduleIcon
} from '@mui/icons-material';
import useResponsive from '../../hooks/use-responsive';
import Logo from '../../components/logo';
import Scrollbar from '../../components/scrollbar';
import { NAV_WIDTH } from '../../config';
import navConfig from './config-navigation';
import { getProfile } from '../../services/api';
import AuthContext from '../../context/AuthContext';

const StyledRoot = styled('div')(({ theme }) => ({
  [theme.breakpoints.up('lg')]: {
    flexShrink: 0,
    width: NAV_WIDTH,
  },
}));

const StyledAccount = styled('div')(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
  padding: theme.spacing(2),
  borderRadius: theme.shape.borderRadius,
  backgroundColor: alpha(theme.palette.primary.main, 0.1),
  marginBottom: theme.spacing(3),
}));

const StyledAvatar = styled(Avatar)(({ theme }) => ({
  width: 80,
  height: 80,
  marginBottom: theme.spacing(2),
  border: `4px solid ${theme.palette.background.paper}`,
  boxShadow: theme.shadows[3],
}));

const StyledNavItem = styled(ListItem)(({ theme }) => ({
  borderRadius: theme.shape.borderRadius,
  '&:hover': {
    backgroundColor: alpha(theme.palette.primary.main, 0.1),
  },
  '&.Mui-selected': {
    backgroundColor: alpha(theme.palette.primary.main, 0.2),
    '&:hover': {
      backgroundColor: alpha(theme.palette.primary.main, 0.3),
    },
  },
}));

export default function Nav({ openNav, onCloseNav }) {
  const { pathname } = useLocation();
  const navigate = useNavigate();
  const isDesktop = useResponsive('up', 'lg');
  const { logout } = useContext(AuthContext);

  const [user, setUser] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [fetchError, setFetchError] = useState(null);

  const handleCloseNav = useCallback(() => {
    if (openNav) {
      onCloseNav();
    }
  }, [openNav, onCloseNav]);

  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        const response = await getProfile();
        setUser(response.data);
      } catch (err) {
        console.error('Failed to fetch user profile:', err);
        setFetchError('Failed to load user profile');
      } finally {
        setIsLoading(false);
      }
    };

    fetchUserProfile();
  }, []);

  useEffect(() => {
    if (isDesktop && openNav) {
      handleCloseNav();
    }
  }, [pathname, isDesktop, openNav, handleCloseNav]);

  const filteredNavConfig = useMemo(
    () => navConfig.filter((item) => item.roles.includes(user?.role || 'User')),
    [user?.role]
  );

  const handleLogout = useCallback(() => {
    logout();
    navigate('/login');
  }, [logout, navigate]);

  const renderUserInfo = () => {
    if (isLoading) {
      return <CircularProgress />;
    }

    if (fetchError) {
      return (
        <Typography variant="body2" color="error">
          {fetchError}
        </Typography>
      );
    }

    if (!user) {
      return (
        <Typography variant="body2" color="text.secondary">
          User not logged in
        </Typography>
      );
    }

    return (
      <StyledAccount>
        <StyledAvatar alt={user.fullName || 'User'}>
          {user.fullName ? user.fullName.charAt(0).toUpperCase() : 'U'}
        </StyledAvatar>
        <Typography variant="h6" sx={{ mb: 0.5 }}>
          {user.fullName || 'N/A'}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {user.role || 'User'}
        </Typography>
        <Typography variant="caption" color="text.secondary" sx={{ mt: 0.5 }}>
          {user.companyName || 'No Company'}
        </Typography>
      </StyledAccount>
    );
  };

  const renderNavItems = useCallback((items) => (
    <List>
      {items.map((item) => (
        <StyledNavItem
          key={item.title}
          component={Link}
          href={item.path}
          selected={pathname === item.path}
          onClick={() => navigate(item.path)}
        >
          <ListItemIcon>
            {item.title === 'Dashboard' && <DashboardIcon />}
            {item.title === 'User' && <PeopleIcon />}
            {item.title === 'Product' && <AttachMoneyIcon />}
            {item.title === 'Blog' && <EventNoteIcon />}
            {item.title === 'Login' && <PersonIcon />}
            {item.title === 'Not found' && <ScheduleIcon />}
          </ListItemIcon>
          <ListItemText primary={item.title} />
        </StyledNavItem>
      ))}
    </List>
  ), [pathname, navigate]);

  const renderContent = (
    <Scrollbar
      sx={{
        height: 1,
        '& .simplebar-content': { height: 1, display: 'flex', flexDirection: 'column' },
      }}
    >
      <Box sx={{ px: 2.5, py: 3, display: 'inline-flex' }}>
        <Logo />
      </Box>

      {renderUserInfo()}

      {user && renderNavItems(filteredNavConfig)}

      <Box sx={{ flexGrow: 1 }} />

      <Divider sx={{ my: 2 }} />

      <Box sx={{ px: 2.5, pb: 3, mt: 10 }}>
        <Button
          fullWidth
          color="inherit"
          variant="outlined"
          onClick={handleLogout}
          startIcon={<LogoutIcon />}
        >
          Logout
        </Button>
      </Box>
    </Scrollbar>
  );

  return (
    <StyledRoot>
      {!isDesktop && (
        <Drawer
          open={openNav}
          onClose={handleCloseNav}
          PaperProps={{
            sx: { width: NAV_WIDTH },
          }}
        >
          {renderContent}
        </Drawer>
      )}

      {isDesktop && (
        <Drawer
          open
          variant="permanent"
          PaperProps={{
            sx: {
              width: NAV_WIDTH,
              bgcolor: 'background.default',
              borderRightStyle: 'dashed',
            },
          }}
        >
          {renderContent}
        </Drawer>
      )}
    </StyledRoot>
  );
}

Nav.propTypes = {
  openNav: PropTypes.bool.isRequired,
  onCloseNav: PropTypes.func.isRequired,
};